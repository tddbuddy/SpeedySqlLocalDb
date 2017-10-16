using TddBuddy.SpeedySqlLocalDb.EF.Examples.Context;
using TddBuddy.SpeedySqlLocalDb.EF.Examples.Entities;

namespace TddBuddy.SpeedySqlLocalDb.EF.Examples.Repositories
{
    public class AuditRepository
    {
        private readonly AuditingDbContext _dbContext;

        public AuditRepository(AuditingDbContext dbContext)
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