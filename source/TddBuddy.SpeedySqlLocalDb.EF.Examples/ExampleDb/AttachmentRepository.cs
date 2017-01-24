using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace TddBuddy.SpeedySqlLocalDb.EF.Examples.ExampleDb
{
    public class AttachmentRepository : IAttachmentRepository
    {
        private readonly AttachmentDbContext _dbContext;

        public AttachmentRepository(AttachmentDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Attachment Find(Guid id)
        {
            var attachment = _dbContext.Attachments.FirstOrDefault(r => r.Id == id);
            return attachment;
        }

        public List<Attachment> FindAll()
        {
            return _dbContext.Attachments.ToList();
        }

        public void Create(Attachment attachment)
        {
            _dbContext.Attachments.Add(attachment);
        }

        public void Update(Attachment attachment)
        {
            _dbContext.Entry(attachment).State = EntityState.Modified;
        }

        public void Save()
        {
            _dbContext.SaveChanges();
        }
    }
}