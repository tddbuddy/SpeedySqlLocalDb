using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace TddBuddy.SpeedyLocalDb.DotNetCore.MigrationRunner
{
    public class EntityFrameworkMigrationRunner
    {
        public static readonly Type[] NullTypeArgs = new Type[0];
        
        public void RunMigrations(ISpeedySqlLocalDbWrapper speedyInstance, Type dbContextType, Type[] contextArgs)
        {
            try
            {
                var connectionString = string.Empty;
                using (var connectionWrapper = speedyInstance)
                {
                    connectionString = connectionWrapper.Connection.ConnectionString;
                    connectionWrapper.CompleteTransaction();
                }

                var repositoryDbContext = CreateDbContext(connectionString, dbContextType, contextArgs);
                repositoryDbContext.Database.Migrate();
            }
            catch (Exception e)
            {
                Console.WriteLine($"Bootstrap Exception: {e.Message}");
            }
        }

        private DbContext CreateDbContext(string connectionString, Type dbContextType, Type[] dbContextTypeArgs)
        {
            var builder = new DbContextOptionsBuilder();
            builder.UseSqlServer(connectionString);

            if (dbContextTypeArgs == NullTypeArgs)
            {
                return (DbContext)Activator.CreateInstance(dbContextType, builder.Options);
            }

            return BuildDbContextWithArguments(builder, dbContextType, dbContextTypeArgs);
        }

        private DbContext BuildDbContextWithArguments(DbContextOptionsBuilder builder, Type dbContextType, Type[] dbContextTypeArgs)
        {
            var argValues = new List<object> { builder.Options };

            foreach (var value in dbContextTypeArgs)
            {
                argValues.Add(Activator.CreateInstance(value));
            }

            return (DbContext)Activator.CreateInstance(dbContextType, argValues.ToArray());
        }
    }
}