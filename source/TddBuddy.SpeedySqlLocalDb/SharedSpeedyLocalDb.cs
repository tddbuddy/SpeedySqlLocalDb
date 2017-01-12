using System;
using System.Data.Common;
using System.Data.Entity;
using System.Data.SqlClient;

namespace TddBuddy.SpeedySqlLocalDb
{
    public class SharedSpeedyLocalDb : Attribute, IDisposable
    {
        private readonly Type _dbContextType;
        private readonly ISpeedySqlLocalDb _speedyInstance;
        private readonly string _dbName;

        public SharedSpeedyLocalDb(Type dbContextType)
        {
            if (!dbContextType.IsSubclassOf(typeof(DbContext)))
            {
                throw new Exception("Type must be Subclass of DbContext");
            }

            _dbName = DbTestUtil.GetRandomTestDbNameForTestRun();

            _dbContextType = dbContextType;
            _speedyInstance = new SpeedySqlLocalDb();
            _speedyInstance.BootstrapDatabaseForEfMigrations(CreateDbContext);
        }
        
        public void Dispose()
        {
            _speedyInstance.DetachDatabase();
        }

        private DbContext CreateDbContext(DbConnection connection)
        {
            return (DbContext)Activator.CreateInstance(_dbContextType, connection);
        }

    }
}
