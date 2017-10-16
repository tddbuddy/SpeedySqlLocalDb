using System;

namespace TddBuddy.SpeedySqlLocalDb.EF.Examples.Example.EntityFramework
{
    public interface IDateTimeProvider  
    {
        DateTime Now { get; }
    }
}