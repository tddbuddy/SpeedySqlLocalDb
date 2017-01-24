namespace TddBuddy.SpeedySqlLocalDb.EF.Examples.ExampleDb
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