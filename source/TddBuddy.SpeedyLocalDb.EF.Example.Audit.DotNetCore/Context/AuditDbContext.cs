using Microsoft.EntityFrameworkCore;
using TddBuddy.SpeedyLocalDb.EF.Example.Audit.DotNetCore.DateTime;
using TddBuddy.SpeedyLocalDb.EF.Example.Audit.DotNetCore.Entities;

namespace TddBuddy.SpeedyLocalDb.EF.Example.Audit.DotNetCore.Context
{
    public class AuditDbContext : DbContext, IDateTimeProvider
    {
        private readonly IDateTimeProvider _dateTimeProvider;

        public System.DateTime Now => _dateTimeProvider.Now;

        public DbSet<AuditEntry> AuditEntries { get; set; }

        public AuditDbContext(DbContextOptions options, 
                              IDateTimeProvider dateTimeProvider) : base(options)
        {
            _dateTimeProvider = dateTimeProvider;
        }
    }
}
