using Microsoft.EntityFrameworkCore;

namespace TddBuddy.SpeedyLocalDb.EF.Example.Attachment.DotNetCore.Context
{
    public class AttachmentDbContext : DbContext
    {
        public DbSet<Entities.Attachment> Attachments { get; set; }

        public AttachmentDbContext(DbContextOptions options) : base(options){ }

    }
}