using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using TddBuddy.SpeedyLocalDb.EF.Example.Attachment.Context;

namespace TddBuddy.SpeedyLocalDb.EF.Example.Attachment.Repositories
{
    public class AttachmentRepository 
    {
        private readonly AttachmentDbContext _dbContext;

        public AttachmentRepository(AttachmentDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Entities.Attachment Find(Guid id)
        {
            var attachment = _dbContext.Attachments.FirstOrDefault(r => r.Id == id);
            return attachment;
        }

        public List<Entities.Attachment> FindAll()
        {
            return _dbContext.Attachments.ToList();
        }

        public void Create(Entities.Attachment attachment)
        {
            _dbContext.Attachments.Add(attachment);
        }

        public void Update(Entities.Attachment attachment)
        {
            _dbContext.Entry(attachment).State = EntityState.Modified;
        }

        public void Save()
        {
            _dbContext.SaveChanges();
        }
    }
}