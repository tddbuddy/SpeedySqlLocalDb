using System;
using System.Collections.Generic;

namespace TddBuddy.SpeedySqlLocalDb.EF.Examples.Example.EntityFramework
{
    public interface IAttachmentRepository
    {
        Attachment Find(Guid id);
        List<Attachment> FindAll();

        void Create(Attachment attachment);
        void Update(Attachment attachment);
        void Save();
    }
}