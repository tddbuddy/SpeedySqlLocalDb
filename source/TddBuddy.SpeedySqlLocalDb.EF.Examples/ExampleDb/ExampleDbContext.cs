using System.Data.Common;
using System.Data.Entity;

namespace TddBuddy.SpeedySqlLocalDb.EF.Examples.ExampleDb
{ 

    public class ExampleDbContext : DbContext
    {
        public IDbSet<Attachment> Attachments { get; set; }

        public ExampleDbContext() : base("SomeDbConnectionStringReference")
        {
            Database.SetInitializer<ExampleDbContext>(null);
        }

        public ExampleDbContext(DbConnection dbConnection) : base(dbConnection, false)
        {
        }
        
    }
}