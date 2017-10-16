using System.Data.Entity.Migrations;
using TddBuddy.SpeedyLocalDb.EF.Example.Audit.Context;

namespace TddBuddy.SpeedyLocalDb.EF.Example.Audit.Migrations
{
    internal sealed class Configuration : DbMigrationsConfiguration<AuditingDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }
    }
}
