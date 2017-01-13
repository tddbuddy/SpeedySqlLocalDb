using System;
using System.Data.Common;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Text;

namespace TddBuddy.SpeedySqlLocalDb
{
    public sealed class SharedSpeedyLocalDb : Attribute, IDisposable
    {        
        private readonly Type _dbContextType;
        private readonly ISpeedySqlLocalDb _speedyInstance;
        private readonly ContextVariables _contextVariables;

        public SharedSpeedyLocalDb(Type dbContextType)
        {
            if (!dbContextType.IsSubclassOf(typeof(DbContext)))
            {
                throw new Exception("Type must be Subclass of DbContext");
            }

            _dbContextType = dbContextType;
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
            return (DbContext)Activator.CreateInstance(_dbContextType, connection);
        }

        // todo : TEST THIS
        private void CleanUpOldDatabases()
        {
            var appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            var masterLocalDb = @"Microsoft\Microsoft SQL Server Local DB\Instances\MSSQLLocalDb\master.mdf";
            var connectionString = $"Data Source={_contextVariables.LocalDbName};AttachDBFileName={appDataPath}\\{masterLocalDb};Initial Catalog=master;Integrated Security=True;";
            using (var connection = new SqlConnection(connectionString))
            {
                var cleanCmd = new StringBuilder();
                cleanCmd.AppendLine("declare @tmp table(cmd nvarchar(1024));");
                cleanCmd.AppendLine($"insert into @tmp(cmd) select 'drop database ' + name from sys.databases where name like '{_contextVariables.Prefix}%' and is_cleanly_shutdown = 1;");
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

                using (var cmd = connection.CreateCommand())
                {
                    try
                    {
                        connection.Open();
                        cmd.CommandText = cleanCmd.ToString();
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
