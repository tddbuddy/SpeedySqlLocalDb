﻿using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TddBuddy.SpeedyLocalDb.EF.Example.Audit.DotNetCore.Entities
{
    [Table("Entries", Schema = "aud")]
    public class AuditEntry
    {
        [Key]
        public Guid Id { get; set; }

        public string System { get; set; }
        public string User { get; set; }
        public string LogDetail { get; set; }
        public System.DateTime CreateTimestamp { get; set; }
    }
}