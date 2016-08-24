using System;
using System.Data.Entity;
using System.Data.SqlClient;

namespace TddBuddy.SpeedySqlLocalDb
{
    public interface ISpeedySqlLocalDb
    {
        void BootstrapDatabaseForEfMigrations(Func<SqlConnection, DbContext> createDbContextFunc);
        ISpeedySqlLocalDbWrapper CreateTempLocalDbWrapper();
        void DetachDatabase();
    }
}