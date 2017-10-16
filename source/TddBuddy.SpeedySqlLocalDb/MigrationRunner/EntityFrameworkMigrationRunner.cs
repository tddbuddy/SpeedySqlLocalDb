using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.Entity;

namespace TddBuddy.SpeedySqlLocalDb.MigrationRunner
{
    public class EntityFrameworkMigrationRunner
    {
        public static readonly Type[] NullTypeArgs = new Type[0];
        
        public void RunMigrations(ISpeedySqlLocalDbWrapper speedyInstance, Type dbContextType, Type[] contextArgs)
        {
            try
            {
                using (var connectionWrapper = speedyInstance)
                {
                    var repositoryDbContext = CreateDbContext(connectionWrapper.Connection, dbContextType, contextArgs);
                    // force EF migrations to run and build db
                    repositoryDbContext.Database.ExecuteSqlCommand("select 1");
                    connectionWrapper.CompleteTransaction();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Bootstrap Exception: {e.Message}");
            }
        }


        private DbContext CreateDbContext(DbConnection connection, Type dbContextType, Type[] dbContextTypeArgs)
        {
            if (dbContextTypeArgs == NullTypeArgs)
            {
                return (DbContext)Activator.CreateInstance(dbContextType, connection);
            }

            return BuildDbContextWithArguments(connection, dbContextType, dbContextTypeArgs);
        }

        private DbContext BuildDbContextWithArguments(DbConnection connection, Type dbContextType, Type[] dbContextTypeArgs)
        {
            var argValues = new List<object> { connection };

            foreach (var value in dbContextTypeArgs)
            {
                argValues.Add(Activator.CreateInstance(value));
            }

            return (DbContext)Activator.CreateInstance(dbContextType, argValues.ToArray());
        }
    }
}