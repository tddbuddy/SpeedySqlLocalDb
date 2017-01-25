using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Text;

namespace TddBuddy.SpeedySqlLocalDb.Attribute
{
    public sealed class SharedSpeedyLocalDb : System.Attribute, IDisposable
    {        
        private readonly Type _dbContextType;
        private readonly Type[] _dbContextTypeArgs;
        private readonly ISpeedySqlLocalDb _speedyInstance;
        private readonly ContextVariables _contextVariables;

        public SharedSpeedyLocalDb(Type dbContextType) : this(dbContextType, new Type[0])
        {
        }

        public SharedSpeedyLocalDb(Type dbContextType, params Type[] dbContextTypeArgs)
        {
            if (!dbContextType.IsSubclassOf(typeof(DbContext)))
            {
                throw new Exception("Type must be Subclass of DbContext");
            }

            _dbContextType = dbContextType;
            _dbContextTypeArgs = dbContextTypeArgs;
            _contextVariables = new ContextVariables();
            _speedyInstance = new SpeedySqlLocalDb(_contextVariables);
            BootstrapDatabaseForEfMigrations();
            CleanUpOldDatabases();
        }

        public void Dispose()
        {
            DetachDatabase();
        }
        
        private DbContext CreateDbContext(DbConnection connection)
        {
            // todo : if _dbContextTypeArgs not null use them to make instance
            if (_dbContextTypeArgs.Length == 0)
            {
                return (DbContext) Activator.CreateInstance(_dbContextType, connection);
            }

            return BuildDbContextWithArguments(connection);
        }

        private DbContext BuildDbContextWithArguments(DbConnection connection)
        {
            var argValues = new List<object>
            {
                connection
            };

            foreach (var value in _dbContextTypeArgs)
            {
                argValues.Add(Activator.CreateInstance(value));
            }

            return (DbContext) Activator.CreateInstance(_dbContextType, argValues.ToArray());
        }

        // todo : TEST THIS
        private void CleanUpOldDatabases()
        {
            var appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            var masterLocalDb = @"Microsoft\Microsoft SQL Server Local DB\Instances\MSSQLLocalDb\master.mdf";
            var connectionString = $"Data Source={_contextVariables.LocalDbName};AttachDBFileName={appDataPath}\\{masterLocalDb};Initial Catalog=master;Integrated Security=True;";
            using (var connection = new SqlConnection(connectionString))
            {
                var cleanCmd = CreateCleanupCommand();

                using (var cmd = connection.CreateCommand())
                {
                    try
                    {
                        connection.Open();
                        cmd.CommandText = cleanCmd;
                        cmd.ExecuteNonQuery();
                    }
                    catch (Exception e)
                    {
                        // could be a pathing issue to DB or other sillyness
                        Console.Write($"Failed attempting to clean up old connections - {e.Message}");
                    }
                }
            }
        }

        private string CreateCleanupCommand()
        {
            var cleanCmd = new StringBuilder();
            cleanCmd.AppendLine("declare @tmp table(cmd nvarchar(1024));");
            cleanCmd.AppendLine(
                $"insert into @tmp(cmd) select 'drop database ' + name from sys.databases where name like '{_contextVariables.Prefix}%' and is_cleanly_shutdown = 1;");
            cleanCmd.AppendLine("declare @cmd nvarchar(1024);");
            cleanCmd.AppendLine("while exists(select * from @tmp)");
            cleanCmd.AppendLine("begin");
            cleanCmd.AppendLine("select top 1 @cmd = cmd from @tmp;");
            cleanCmd.AppendLine("delete from @tmp where cmd = @cmd;");
            cleanCmd.AppendLine("begin try");
            cleanCmd.AppendLine("exec(@cmd);");
            cleanCmd.AppendLine("end try");
            cleanCmd.AppendLine("begin catch");
            cleanCmd.AppendLine("end catch");
            cleanCmd.AppendLine("end");
            return cleanCmd.ToString();
        }

        private void BootstrapDatabaseForEfMigrations()
        {
            using (var connectionWrapper = _speedyInstance.CreateSpeedyLocalDbWrapper())
            {
                var repositoryDbContext = CreateDbContext(connectionWrapper.Connection);
                // force EF migrations to run and build db
                repositoryDbContext.Database.ExecuteSqlCommand("select 1");
                connectionWrapper.CompleteTransaction();
            }
        }

        private void DetachDatabase()
        {
            try
            {
                var connectionString = $"Data Source={_contextVariables.LocalDbName};Initial Catalog=master;Integrated Security=True";
                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    var closeConnectionsCommand = connection.CreateCommand();
                    closeConnectionsCommand.CommandText = $"ALTER DATABASE {_contextVariables.DbName} SET SINGLE_USER WITH ROLLBACK IMMEDIATE";
                    closeConnectionsCommand.ExecuteNonQuery();

                    var cmd = connection.CreateCommand();
                    cmd.CommandText = $"exec sp_detach_db '{_contextVariables.DbName}'";
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Couldn't detatch database {_contextVariables.DbName} - {ex.Message}");
            }
        }
    }
}
