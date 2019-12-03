using System;
using System.IO;
using System.Reflection;
using System.Text;

namespace TddBuddy.SpeedyLocalDb.DotNetCore
{
    public class ContextVariables
    {
        public const string Prefix = "SpeedyDb_";

        private static readonly string _dbName;
        public string DbName => _dbName;

        public string LocalDbName => "(LocalDB)\\MSSQLLocalDb";

        public string DbPath { get; }
        public string DbLogPath { get; }
        public string OutputFolder { get; }

        public Action MigrationAction { get; set; }

        public int TransactionTimeoutMinutes { get; set; }

        static ContextVariables()
        {
            _dbName = GetRandomTestDbNameForTestRun();
        }

        public ContextVariables()
        {
            var executingAssembly = Assembly.GetExecutingAssembly();
           

            var tempDbDirectoryPath = Path.GetDirectoryName(executingAssembly.Location) ?? Path.GetTempPath();
            
            OutputFolder = Path.Combine(tempDbDirectoryPath, "Data");

            DbPath = Path.Combine(OutputFolder, $"{DbName}.mdf");
            DbLogPath = Path.Combine(OutputFolder, $"{DbName}_log.ldf");

            TransactionTimeoutMinutes = 1;
            MigrationAction = () => { };
        }

        private static string GetRandomTestDbNameForTestRun()
        {
            if (_dbName != null) return _dbName;

            var dbName = Guid.NewGuid().ToString();
            var plainTextBytes = Encoding.UTF8.GetBytes(dbName);
            var base64PostName = Convert.ToBase64String(plainTextBytes);
            return $"{Prefix}{base64PostName}";
        }
    }
}
