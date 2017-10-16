using System;

namespace TddBuddy.SpeedyLocalDb.EF.Examples.Attachment.Builders
{
    public class AttachmentBuilder
    {
        private byte[] _content;
        private string _contentType;
        private string _fileName;
        private Guid _id;

        public AttachmentBuilder()
        {
            _id = Guid.NewGuid();
            _fileName = "";
            _contentType = "";
            _content = null;
        }

        public AttachmentBuilder WithId(Guid id)
        {
            _id = id;
            return this;
        }

        public AttachmentBuilder WithFileName(string fileName)
        {
            _fileName = fileName;
            return this;
        }

        public AttachmentBuilder WithContentType(string contentType)
        {
            _contentType = contentType;
            return this;
        }

        public AttachmentBuilder WithContent(byte[] content)
        {
            _content = content;
            return this;
        }

        public Entities.Attachment Build()
        {
            return new Entities.Attachment
            {
                Id = _id,
                FileName = _fileName,
                ContentType = _contentType,
                Content = _content
            };
        }
    }
}