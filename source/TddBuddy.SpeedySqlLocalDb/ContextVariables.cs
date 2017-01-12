using System;
using System.IO;
using System.Text;

namespace TddBuddy.SpeedySqlLocalDb
{
    public class ContextVariables
    {
        public string LocalDbName => "(LocalDB)\\MSSQLLocalDb";

        public string DbPath { get; private set; }
        public string DbLogPath { get; private set; }
        public string OutputFolder { get; }

        public string DbName { get; }


        public ContextVariables()
        {
            DbName = GetRandomTestDbNameForTestRun();

            var mdfFilename = DbName + ".mdf";
            var tempDbDirectoryPath = Path.GetTempPath();

            OutputFolder = Path.Combine(tempDbDirectoryPath, "Data");
            DbPath = Path.Combine(OutputFolder, mdfFilename);
            DbLogPath = Path.Combine(OutputFolder, $"{DbName}_log.ldf");
        }

        private string GetRandomTestDbNameForTestRun()
        {
            if (DbName != null) return DbName;

            var dbName = Guid.NewGuid().ToString();
            var plainTextBytes = Encoding.UTF8.GetBytes(dbName);
            return Convert.ToBase64String(plainTextBytes);
        }
    }
}
