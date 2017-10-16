using System.Data.Common;
using System.Data.Entity;

namespace TddBuddy.SpeedySqlLocalDb.EF.Examples.Example.EntityFramework
{ 

    public class AttachmentDbContext : DbContext
    {
        // NOTE: This would be a production contructor, here for illustration 
        public AttachmentDbContext() : base("SomeDbConnectionStringReference")
        {
            Database.SetInitializer<AttachmentDbContext>(null);
        }

        // NOTE: This is a required test contructor, here so we can to reflective attribute magic
        public AttachmentDbContext(DbConnection dbConnection) : base(dbConnection, false)
        {
        }

        public IDbSet<Attachment> Attachments { get; set; }

    }
}