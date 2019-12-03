using System;
using System.Data.SqlClient;
using System.IO;
using System.Transactions;

namespace TddBuddy.SpeedyLocalDb.DotNetCore
{
    public class SpeedySqlLocalDb : ISpeedySqlLocalDb
    {
        private readonly ContextVariables _contextVariables;

        internal SpeedySqlLocalDb(ContextVariables contextVariables)
        {
            _contextVariables = contextVariables;
        }

        public ISpeedySqlLocalDbWrapper CreateSpeedyLocalDbWrapper()
        {
            CreateDirectoryIfNotExist(_contextVariables.OutputFolder);

            if (DoesDbFileExist())
            {
                return CreateDbWrapper();
            }

            CreateDatabase();
            RunMigrations();

            return CreateDbWrapper();
        }

        private void RunMigrations()
        {
            _contextVariables.MigrationAction();
        }

        private bool DoesDbFileExist()
        {
            return File.Exists(_contextVariables.DbPath);
        }


        private ISpeedySqlLocalDbWrapper CreateDbWrapper()
        {
            var connection = CreateConnection();

            var scopeTimeout = new TimeSpan(0, 0, _contextVariables.TransactionTimeoutMinutes, 0);
            var transactionScope = new TransactionScope(TransactionScopeOption.Required, scopeTimeout);

            return new SpeedySqlLocalDbWrapper(connection, transactionScope);
        }


        private SqlConnection CreateConnection()
        {
            var connectionString =
                $"Data Source={_contextVariables.LocalDbName};AttachDBFileName={_contextVariables.DbPath};Initial Catalog={_contextVariables.DbName};Integrated Security=True;";
            var connection = new SqlConnection(connectionString);
            return connection;
        }

        private void CreateDatabase()
        {
            var connectionString = $"Data Source={_contextVariables.LocalDbName};Initial Catalog=master;Integrated Security=True";
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                var cmd = connection.CreateCommand();

                cmd.CommandText = $"CREATE DATABASE {_contextVariables.DbName} ON (NAME = N'{_contextVariables.DbName}', FILENAME = '{_contextVariables.DbPath}')";
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