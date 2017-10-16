using System.Data.Entity.Migrations;
using TddBuddy.SpeedyLocalDb.EF.Example.Attachment.Context;

namespace TddBuddy.SpeedyLocalDb.EF.Example.Attachment.Migrations
{
    internal sealed class Configuration : DbMigrationsConfiguration<AttachmentDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }
    }
}
