using System;
using System.ComponentModel.DataAnnotations;

namespace TddBuddy.SpeedySqlLocalDb.EF.Examples.ExampleDb
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