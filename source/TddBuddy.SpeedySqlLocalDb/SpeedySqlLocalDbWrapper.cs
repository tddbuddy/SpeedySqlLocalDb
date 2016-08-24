using System;
using System.Data.SqlClient;
using System.Transactions;

namespace TddBuddy.SpeedySqlLocalDb
{
    public class SpeedySqlLocalDbWrapper : ISpeedySqlLocalDbWrapper
    {
        public SqlConnection Connection { get; }
        private readonly TransactionScope _scope;

        public SpeedySqlLocalDbWrapper(SqlConnection connection, TransactionScope scope)
        {
            Connection = connection;
            _scope = scope;
        }

        public void CompleteTransaction()
        {
            _scope.Complete();
        }

        public void Dispose()
        {
            Connection?.Dispose();
            _scope?.Dispose();
        }
    }
}