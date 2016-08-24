using System;
using System.Data.Entity;
using System.Data.SqlClient;
using System.IO;
using System.Transactions;

namespace TddBuddy.SpeedySqlLocalDb
{
    public class SpeedySqlLocalDb : ISpeedySqlLocalDb
    {
        public string DbPath { get; private set; }
        public string DbLogPath { get; private set; }

        private readonly string _dbName;
        private readonly string _localDbName = "(LocalDB)\\MSSQLLocalDb";
        private const string DbDirectory = "Data";

        public SpeedySqlLocalDb()
        {
            _dbName = DbTestUtil.GetRandomTestDbNameForTestRun();
        }

        //todo : add second bootstrap to use fluent migrations as Func<DbContext, bool>

        public void BootstrapDatabaseForEfMigrations(Func<SqlConnection, DbContext> createDbContextFunc)
        {
            // todo : as part of the boot strap look for old instances and detach them
            using (var connectionWrapper = new SpeedySqlLocalDb().CreateTempLocalDbWrapper())
            {
                var repositoryDbContext = createDbContextFunc(connectionWrapper.Connection);
                if (repositoryDbContext == null) throw new ArgumentException(nameof(createDbContextFunc));
                // force EF migrations to run and build db
                repositoryDbContext.Database.ExecuteSqlCommand("select 1"); 
                connectionWrapper.CompleteTransaction();
            }
        }

        public ISpeedySqlLocalDbWrapper CreateTempLocalDbWrapper()
        {
            var tempDbDirectoryPath = Path.GetTempPath();
            var outputFolder = Path.Combine(tempDbDirectoryPath, DbDirectory);
            
            var mdfFilename = _dbName + ".mdf";
            DbPath = Path.Combine(outputFolder, mdfFilename);
            DbLogPath = Path.Combine(outputFolder, $"{_dbName}_log.ldf");

            CreateDirectoryIfNotExist(outputFolder);

            if (!File.Exists(DbPath))
            {
                CreateDatabase(DbPath);
            }

            return CreateTempLocalDbWrapper(DbPath);
        }

        public void DetachDatabase()
        {
            try
            {
                var connectionString = $"Data Source={_localDbName};Initial Catalog=master;Integrated Security=True";
                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    var closeConnectionsCommand = connection.CreateCommand();
                    closeConnectionsCommand.CommandText = $"ALTER DATABASE {_dbName} SET SINGLE_USER WITH ROLLBACK IMMEDIATE";
                    closeConnectionsCommand.ExecuteNonQuery();

                    var cmd = connection.CreateCommand();
                    cmd.CommandText = $"exec sp_detach_db '{_dbName}'";
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Couldn't detatch database {_dbName} - {ex.Message}");
            }
        }
        
        private SpeedySqlLocalDbWrapper CreateTempLocalDbWrapper(string dbFileName)
        {
            var connectionString =
                $"Data Source={_localDbName};AttachDBFileName={dbFileName};Initial Catalog={_dbName};Integrated Security=True;";
            var connection = new SqlConnection(connectionString);

            var transactionScope = new TransactionScope();
            return new SpeedySqlLocalDbWrapper(connection, transactionScope);
        }
        
        private void CreateDatabase(string dbFileName)
        {
            var connectionString = $"Data Source={_localDbName};Initial Catalog=master;Integrated Security=True";
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                var cmd = connection.CreateCommand();

                cmd.CommandText = $"CREATE DATABASE {_dbName} ON (NAME = N'{_dbName}', FILENAME = '{dbFileName}')";
                cmd.ExecuteNonQuery();
            }
        }

        private void CreateDirectoryIfNotExist(string outputFolder)
        {
            if (!Directory.Exists(outputFolder))
            {
                Directory.CreateDirectory(outputFolder);
            }
        }
    }
}