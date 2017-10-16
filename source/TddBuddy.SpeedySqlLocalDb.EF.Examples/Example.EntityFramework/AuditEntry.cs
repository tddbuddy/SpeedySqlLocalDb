using System;
using System.ComponentModel.DataAnnotations;

namespace TddBuddy.SpeedySqlLocalDb.EF.Examples.Example.EntityFramework
{
    public class AuditEntry
    {
        [Key]
        public Guid Id { get; set; }

        public string System { get; set; }
        public string User { get; set; }
        public string LogDetail { get; set; }
        public DateTime CreateTimestamp { get; set; }
    }
}