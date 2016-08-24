using System;
using System.Data.SqlClient;

namespace TddBuddy.SpeedySqlLocalDb
{
    public interface ISpeedySqlLocalDbWrapper : IDisposable
    {
        SqlConnection Connection { get; }
        void CompleteTransaction();
    }
}