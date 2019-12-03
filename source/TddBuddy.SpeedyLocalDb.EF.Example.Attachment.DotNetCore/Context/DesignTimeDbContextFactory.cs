using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace TddBuddy.SpeedyLocalDb.EF.Example.Attachment.DotNetCore.Context
{
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<AttachmentDbContext>
    {
        public AttachmentDbContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<AttachmentDbContext>();

            var connectionString = $"Server=(localdb)\\mssqllocaldb;Database=__migrations__;Trusted_Connection=True;MultipleActiveResultSets=true";

            builder.UseSqlServer(connectionString);

            return new AttachmentDbContext(builder.Options);
        }
    }
}
