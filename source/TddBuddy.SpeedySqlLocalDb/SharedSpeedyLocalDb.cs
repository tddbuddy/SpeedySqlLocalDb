using System;
using System.Data.Common;
using System.Data.Entity;
using System.Data.SqlClient;

namespace TddBuddy.SpeedySqlLocalDb
{
    // https://github.com/fluffynuts/PeanutButter/blob/master/source/TempDb/PeanutButter.TempDb.LocalDb/TempDBLocalDb.cs
    // https://github.com/fluffynuts/PeanutButter/blob/master/source/TempDb/PeanutButter.TempDb.Tests/TestTempDBLocalDb.cs

    public class SharedSpeedyLocalDb : Attribute, IDisposable
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
            // todo : Clean up existing old DB's
            BootstrapDatabaseForEfMigrations(CreateDbContext);
        }
        
        public void Dispose()
        {
            DetachDatabase();
        }

        public void DetachDatabase()
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

        private DbContext CreateDbContext(DbConnection connection)
        {
            return (DbContext)Activator.CreateInstance(_dbContextType, connection);
        }

        private void BootstrapDatabaseForEfMigrations(Func<SqlConnection, DbContext> createDbContextFunc)
        {
            // todo : as part of the boot strap look for old instances and detach them
            using (var connectionWrapper = _speedyInstance.CreateSpeedyLocalDbWrapper())
            {
                var repositoryDbContext = createDbContextFunc(connectionWrapper.Connection);
                if (repositoryDbContext == null) throw new ArgumentException(nameof(createDbContextFunc));
                // force EF migrations to run and build db
                repositoryDbContext.Database.ExecuteSqlCommand("select 1");
                connectionWrapper.CompleteTransaction();
            }
        }
    }
}
