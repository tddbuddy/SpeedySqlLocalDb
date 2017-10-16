using System.Data.Common;
using System.Data.Entity;

namespace TddBuddy.SpeedyLocalDb.EF.Example.Attachment.Context
{ 

    public class AttachmentDbContext : DbContext
    {
        // NOTE: This would be a production contructor, here for illustration 
        public AttachmentDbContext() : base("AttachmentContext")
        {
            Database.SetInitializer<AttachmentDbContext>(null);
        }

        public AttachmentDbContext(DbConnection connection) : base(connection, false){ }

        public IDbSet<Entities.Attachment> Attachments { get; set; }

    }
}