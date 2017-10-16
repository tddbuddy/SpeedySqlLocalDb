using System;
using System.Data.Common;
using System.Linq;
using NUnit.Framework;
using TddBuddy.SpeedyLocalDb.EF.Example.Audit;
using TddBuddy.SpeedyLocalDb.EF.Example.Audit.Context;
using TddBuddy.SpeedyLocalDb.EF.Example.Audit.DateTime;
using TddBuddy.SpeedyLocalDb.EF.Example.Audit.Entities;
using TddBuddy.SpeedySqlLocalDb.Attribute;
using TddBuddy.SpeedySqlLocalDb.Construction;
using TddBuddy.SpeedySqlLocalDb.EF.Examples.Context;
using TddBuddy.SpeedySqlLocalDb.EF.Examples.Entities;
using TddBuddy.SpeedySqlLocalDb.EF.Examples.Repositories;

namespace TddBuddy.SpeedySqlLocalDb.Tests
{
    [TestFixture, SharedSpeedyLocalDb(typeof(AuditingDbContext), typeof(DateTimeProvider))]
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
                var repositoryDbContext = CreateDbContext(wrapper.Connection);
                var assertDbContext = CreateDbContext(wrapper.Connection);
                var auditingRepository = CreateRepository(repositoryDbContext);
                //---------------Execute Test ----------------------
                auditingRepository.Create(entry);
                auditingRepository.Save();
                //---------------Test Result -----------------------
                Assert.AreEqual(1, assertDbContext.AuditEntries.Count());
                var actualEntry = assertDbContext.AuditEntries.First();
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

        private AuditingDbContext CreateDbContext(DbConnection connection)
        {
            return new AuditingDbContext(connection, new DateTimeProvider());
        }

        private AuditRepository CreateRepository(AuditingDbContext writeDbContext)
        {
            return new AuditRepository(writeDbContext);
        }
    }
}