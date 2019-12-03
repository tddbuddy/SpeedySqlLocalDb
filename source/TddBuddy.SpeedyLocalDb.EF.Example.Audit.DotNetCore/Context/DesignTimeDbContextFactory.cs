using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using TddBuddy.SpeedyLocalDb.EF.Example.Audit.DotNetCore.DateTime;

namespace TddBuddy.SpeedyLocalDb.EF.Example.Audit.DotNetCore.Context
{
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<AuditDbContext>
    {
        public AuditDbContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<AuditDbContext>();

            var connectionString = $"Server=(localdb)\\mssqllocaldb;Database=__migrations__;Trusted_Connection=True;MultipleActiveResultSets=true";

            builder.UseSqlServer(connectionString);

            return new AuditDbContext(builder.Options, new DateTimeProvider());
        }
    }
}
