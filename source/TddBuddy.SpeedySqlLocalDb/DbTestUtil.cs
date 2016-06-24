using System;
using System.Text;

namespace TddBuddy.SpeedySqlLocalDb
{
    public static class DbTestUtil
    {
        private static string _dbName;

        public static string GetRandomTestDbNameForTestRun()
        {
            if (_dbName != null) return _dbName;

            var dbName = Guid.NewGuid().ToString();
            var plainTextBytes = Encoding.UTF8.GetBytes(dbName);
            _dbName = Convert.ToBase64String(plainTextBytes);

            return _dbName;
        }        
    }
}