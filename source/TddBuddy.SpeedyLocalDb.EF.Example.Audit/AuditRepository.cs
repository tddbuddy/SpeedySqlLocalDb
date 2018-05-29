using TddBuddy.SpeedyLocalDb.EF.Example.Audit.Context;
using TddBuddy.SpeedyLocalDb.EF.Example.Audit.Entities;

namespace TddBuddy.SpeedyLocalDb.EF.Example.Audit
{
    public class AuditRepository
    {
        private readonly SqlConnection _dbContext;

        public AuditRepository(SqlConnection dbContext)
        {
            _dbContext = dbContext;
        }

        public void Create(AuditEntry entry)
        {
            entry.CreateTimestamp = _dbContext.Now;
            _dbContext.AuditEntries.Add(entry);
        }

        public void Save()
        {
            _dbContext.SaveChanges();
        }
    }
}