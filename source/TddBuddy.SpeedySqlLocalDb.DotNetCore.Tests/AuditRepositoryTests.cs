using System;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using TddBuddy.SpeedyLocalDb.DotNetCore;
using TddBuddy.SpeedyLocalDb.DotNetCore.Attribute;
using TddBuddy.SpeedyLocalDb.DotNetCore.Construction;
using TddBuddy.SpeedyLocalDb.EF.Example.Audit.DotNetCore;
using TddBuddy.SpeedyLocalDb.EF.Example.Audit.DotNetCore.Context;
using TddBuddy.SpeedyLocalDb.EF.Example.Audit.DotNetCore.DateTime;
using TddBuddy.SpeedyLocalDb.EF.Example.Audit.DotNetCore.Entities;

namespace TddBuddy.SpeedySqlLocalDb.DotNetCore.Tests
{
    [TestFixture, SharedSpeedyLocalDb(typeof(AuditDbContext), typeof(DateTimeProvider))]
    public class AuditRepositoryTests
    {
        [Test]
        public void Create_GivenAuditEntry_ShouldStoreInDatabase()
        {
            //---------------Set up test pack-------------------
            var entry = new AuditEntry
            {
                Id = Guid.NewGuid(),
                System = "System-A",
                User = "Foo",
                LogDetail = "Bar One"
            };

            using (var wrapper = CreateWrapper())
            {
                var dbContext = CreateDbContext(wrapper.Connection);
                var auditingRepository = CreateRepository(dbContext);
                //---------------Execute Test ----------------------
                auditingRepository.Create(entry);
                auditingRepository.Save();
                //---------------Test Result -----------------------
                Assert.AreEqual(1, dbContext.AuditEntries.Count());
                var actualEntry = dbContext.AuditEntries.First();
                AssertIsEqual(entry, actualEntry);
            }
        }

        private ISpeedySqlLocalDbWrapper CreateWrapper()
        {
            return new SpeedySqlBuilder()
                       .WithTransactionTimeout(5)
                       .BuildWrapper();
        }

        private void AssertIsEqual(AuditEntry expectedAttachment, AuditEntry actualAttachment)
        {
            Assert.AreEqual(expectedAttachment.Id, actualAttachment.Id);
            Assert.AreEqual(expectedAttachment.System, actualAttachment.System);
            Assert.AreEqual(expectedAttachment.User, actualAttachment.User);
        }

        private AuditDbContext CreateDbContext(DbConnection connection)
        {
            var connectionString = connection.ConnectionString;

            var builder = new DbContextOptionsBuilder<AuditDbContext>();
            builder
                .UseSqlServer(connectionString)
                .EnableSensitiveDataLogging(true);

            return new AuditDbContext(builder.Options, new DateTimeProvider());
        }


        private AuditRepository CreateRepository(AuditDbContext writeDbContext)
        {
            return new AuditRepository(writeDbContext);
        }
    }
}