using System.Data.Common;
using System.Data.Entity;
using TddBuddy.SpeedyLocalDb.EF.Example.Audit.DateTime;
using TddBuddy.SpeedyLocalDb.EF.Example.Audit.Entities;

namespace TddBuddy.SpeedyLocalDb.EF.Example.Audit.Context
{
    public class SqlConnection : DbContext, IDateTimeProvider
    {
        private readonly IDateTimeProvider _dateTimeProvider;

        public System.DateTime Now => _dateTimeProvider.Now;

        public SqlConnection() : this(new DateTimeProvider())
        {
            Database.SetInitializer<SqlConnection>(null);
        }

        public SqlConnection(IDateTimeProvider dateTimeProvider) : base("AuditingContext")
        {
            _dateTimeProvider = dateTimeProvider;
            Database.SetInitializer<SqlConnection>(null);
        }

        public SqlConnection(DbConnection connection, IDateTimeProvider dateTimeProvider) : base(connection, false)
        {
            _dateTimeProvider = dateTimeProvider;
            //Database.SetInitializer<AuditingDbContext>(null);
        }

        public IDbSet<AuditEntry> AuditEntries { get; set; }
    }
}
