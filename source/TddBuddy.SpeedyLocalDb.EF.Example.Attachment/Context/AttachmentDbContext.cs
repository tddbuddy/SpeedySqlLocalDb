using System.Data.Common;
using System.Data.Entity;

namespace TddBuddy.SpeedyLocalDb.EF.Example.Attachment.Context
{
    public class AttachmentDbContext : DbContext
    {
        public AttachmentDbContext(DbConnection connection) : base(connection, false){ }

        public AttachmentDbContext() : base("AttachmentContext")
        {
            Database.SetInitializer<AttachmentDbContext>(null);
        }

        public IDbSet<Entities.Attachment> Attachments { get; set; }

    }
}