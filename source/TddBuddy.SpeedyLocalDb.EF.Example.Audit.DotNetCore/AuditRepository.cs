using TddBuddy.SpeedyLocalDb.EF.Example.Audit.DotNetCore.Context;
using TddBuddy.SpeedyLocalDb.EF.Example.Audit.DotNetCore.Entities;

namespace TddBuddy.SpeedyLocalDb.EF.Example.Audit.DotNetCore
{
    public class AuditRepository
    {
        private readonly AuditDbContext _dbContext;

        public AuditRepository(AuditDbContext dbContext)
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