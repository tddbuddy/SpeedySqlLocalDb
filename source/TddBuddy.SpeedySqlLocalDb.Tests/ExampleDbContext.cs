using System.Data.Common;
using System.Data.Entity;

namespace TddBuddy.SpeedySqlLocalDb.Tests
{ 
    public class ExampleDbContext : DbContext
    {
        // NOTE: This would be a production contructor, here for illustration 
        public ExampleDbContext() : base("SomeDbConnectionStringReference")
        {
            Database.SetInitializer<ExampleDbContext>(null);
        }

        // NOTE: This is a required test contructor, here so we can to reflective attribute magic
        public ExampleDbContext(DbConnection dbConnection) : base(dbConnection, false)
        {
        }
    }
}