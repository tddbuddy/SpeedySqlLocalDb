using System;
using System.Data.Common;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Text;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using TddBuddy.SpeedySqlLocalDb.MigrationRunner;

namespace TddBuddy.SpeedySqlLocalDb.Attribute
{
    public sealed class SharedSpeedyLocalDb : System.Attribute, IDisposable, ITestAction
    {
        private readonly ContextVariables _contextVariables;

        public SharedSpeedyLocalDb(Type dbContextType) : this(dbContextType, EntityFrameworkMigrationRunner.NullTypeArgs) { }

        public SharedSpeedyLocalDb(Type dbContextType, params Type[] dbContextTypeArgs) : this()
        {
            var isEntityFrameworkContext = dbContextType.IsSubclassOf(typeof(DbContext));
            var isDbConnection = dbContextType.IsAssignableFrom(typeof(DbConnection));
            if (!isEntityFrameworkContext && !isDbConnection)
            {
                throw new Exception($"{nameof(dbContextType)} must be a subclass of DbContext or DbConnection");
            }

            if (isEntityFrameworkContext)
            {
                RunEntityFrameworkMigrations(dbContextType, dbContextTypeArgs);
            }
        }

        public SharedSpeedyLocalDb()
        {
            _contextVariables = new ContextVariables();
            CleanUpOldDatabases();
        }

        private void RunEntityFrameworkMigrations(Type dbContextType, Type[] dbContextTypeArgs)
        {
            var speedySqlLocalDbWrapper = new SpeedySqlLocalDb(_contextVariables).CreateSpeedyLocalDbWrapper();
            var migrationRunner = new EntityFrameworkMigrationRunner();
            migrationRunner.RunMigrations(speedySqlLocalDbWrapper, dbContextType, dbContextTypeArgs);
        }

        public void Dispose()
        {
            DetachDatabase();
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
                    RemoveDatabases(connection, cleanCmd, cmd);
                }
            }
        }

        private void RemoveDatabases(SqlConnection connection, string cleanCmd, SqlCommand cmd)
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

        private string CreateCleanupCommand()
        {
            var cleanCmd = new StringBuilder();
            cleanCmd.AppendLine("declare @tmp table(cmd nvarchar(1024));");
            cleanCmd.AppendLine(
                $"insert into @tmp(cmd) select 'drop database ' + name from sys.databases where name like '{ContextVariables.Prefix}%' and is_cleanly_shutdown = 1;");
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

        public void BeforeTest(ITest test)
        {
            // do nothing
        }

        public void AfterTest(ITest test)
        {
            // do nothing
        }

        public ActionTargets Targets => ActionTargets.Suite;
    }
}
