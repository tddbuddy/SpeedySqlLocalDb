using System.Data.Common;
using System.Data.Entity;
using TddBuddy.SpeedyLocalDb.EF.Example.Audit.DateTime;
using TddBuddy.SpeedyLocalDb.EF.Example.Audit.Entities;

namespace TddBuddy.SpeedyLocalDb.EF.Example.Audit.Context
{
    public class AuditingDbContext : DbContext, IDateTimeProvider
    {
        private readonly IDateTimeProvider _dateTimeProvider;

        public System.DateTime Now => _dateTimeProvider.Now;

        public AuditingDbContext(DbConnection connection, IDateTimeProvider dateTimeProvider) : base(connection, false)
        {
            _dateTimeProvider = dateTimeProvider;
        }

        public IDbSet<AuditEntry> AuditEntries { get; set; }
    }
}
