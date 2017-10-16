using System;
using System.ComponentModel.DataAnnotations;

namespace TddBuddy.SpeedyLocalDb.EF.Examples.Attachment.Entities
{
    public class Attachment
    {
        [Key]
        public Guid Id { get; set; }

        public string FileName { get; set; }
        public string ContentType { get; set; }
        public byte[] Content { get; set; }
    }
}