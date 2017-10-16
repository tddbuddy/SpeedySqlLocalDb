using System.Data.Entity.Migrations;
using TddBuddy.SpeedyLocalDb.EF.Examples.Attachment.Context;

namespace TddBuddy.SpeedyLocalDb.EF.Examples.Attachment.Migrations
{
    internal sealed class Configuration : DbMigrationsConfiguration<AttachmentDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }
    }
}
