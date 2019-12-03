using System;
using System.Data.SqlClient;

namespace TddBuddy.SpeedyLocalDb.DotNetCore
{
    public interface ISpeedySqlLocalDbWrapper : IDisposable
    {
        SqlConnection Connection { get; }
        void CompleteTransaction();
    }
}