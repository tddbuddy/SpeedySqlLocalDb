using System;

namespace TddBuddy.SpeedySqlLocalDb.EF.Examples.ExampleDb
{
    public interface IDateTimeProvider  
    {
        DateTime Now { get; }
    }
}