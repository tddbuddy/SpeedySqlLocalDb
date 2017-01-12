using System;
using System.IO;
using System.Reflection;
using System.Text;

namespace TddBuddy.SpeedySqlLocalDb
{
    public class ContextVariables
    {
        public string LocalDbName => "(LocalDB)\\MSSQLLocalDb";
        public string Prefix => "SpeedyDb_";

        public string DbPath { get; }
        public string DbLogPath { get; }
        public string OutputFolder { get; }
        public string DbName { get; }
        
        public ContextVariables()
        {
            var executingAssembly = Assembly.GetExecutingAssembly();
            DbName = GetRandomTestDbNameForTestRun();

            var tempDbDirectoryPath = Path.GetDirectoryName(executingAssembly.Location) ?? Path.GetTempPath();
            
            OutputFolder = Path.Combine(tempDbDirectoryPath, "Data");

            DbPath = Path.Combine(OutputFolder, $"{DbName}.mdf");
            DbLogPath = Path.Combine(OutputFolder, $"{DbName}_log.ldf");
        }

        private string GetRandomTestDbNameForTestRun()
        {
            if (DbName != null) return DbName;

            var dbName = Guid.NewGuid().ToString();
            var plainTextBytes = Encoding.UTF8.GetBytes(dbName);
            var base64PostName = Convert.ToBase64String(plainTextBytes);
            return $"{Prefix}{base64PostName}";
        }
    }
}
