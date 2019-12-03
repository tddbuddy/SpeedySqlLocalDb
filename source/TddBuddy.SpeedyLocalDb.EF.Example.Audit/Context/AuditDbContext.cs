using System.Data.Common;
using System.Data.Entity;
using TddBuddy.SpeedyLocalDb.EF.Example.Audit.DateTime;
using TddBuddy.SpeedyLocalDb.EF.Example.Audit.Entities;

namespace TddBuddy.SpeedyLocalDb.EF.Example.Audit.Context
{
    public class AuditDbContext : DbContext, IDateTimeProvider
    {
        private readonly IDateTimeProvider _dateTimeProvider;

        public System.DateTime Now => _dateTimeProvider.Now;

        public AuditDbContext() : this(new DateTimeProvider())
        {
            Database.SetInitializer<AuditDbContext>(null);
        }

        public AuditDbContext(IDateTimeProvider dateTimeProvider) : base("AuditingContext")
        {
            _dateTimeProvider = dateTimeProvider;
            Database.SetInitializer<AuditDbContext>(null);
        }

        public AuditDbContext(DbConnection connection, IDateTimeProvider dateTimeProvider) : base(connection, false)
        {
            _dateTimeProvider = dateTimeProvider;
            //Database.SetInitializer<AuditingDbContext>(null);
        }

        public IDbSet<AuditEntry> AuditEntries { get; set; }
    }
}
