using System;

namespace TddBuddy.SpeedySqlLocalDb.EF.Examples.Entities
{
    public interface IDateTimeProvider  
    {
        DateTime Now { get; }
    }
}