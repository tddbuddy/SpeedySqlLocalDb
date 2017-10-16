using System;
using System.Data.Common;
using System.Data.Entity;
using TddBuddy.SpeedySqlLocalDb.EF.Examples.Entities;

namespace TddBuddy.SpeedySqlLocalDb.EF.Examples.Context
{
    public class AuditingDbContext : DbContext, IDateTimeProvider
    {
        private readonly IDateTimeProvider _dateTimeProvider;

        public DateTime Now => _dateTimeProvider.Now;

        public AuditingDbContext(DbConnection connection, IDateTimeProvider dateTimeProvider) : base(connection, false)
        {
            _dateTimeProvider = dateTimeProvider;
        }

        public IDbSet<AuditEntry> AuditEntries { get; set; }
    }
}
