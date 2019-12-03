using System;

namespace TddBuddy.SpeedyLocalDb.DotNetCore.Construction
{
    public class SpeedySqlBuilder
    {
        private int _transactionTimeoutMinutes;
        private Action _migrationAction;

        public SpeedySqlBuilder()
        {
            _transactionTimeoutMinutes = 1;
            _migrationAction = () => { };
        }

        public SpeedySqlBuilder WithTransactionTimeout(int timeout)
        {
            _transactionTimeoutMinutes = timeout;
            return this;
        }

        public SpeedySqlBuilder WithMigrationAction(Action transactionAction)
        {
            _migrationAction = transactionAction;
            return this;
        }

        public ISpeedySqlLocalDbWrapper BuildWrapper()
        {
            var contextVariables = new ContextVariables
            {
                TransactionTimeoutMinutes = _transactionTimeoutMinutes,
                MigrationAction = _migrationAction
            };

            return new SpeedySqlLocalDb(contextVariables).CreateSpeedyLocalDbWrapper();
        }
    }
}
