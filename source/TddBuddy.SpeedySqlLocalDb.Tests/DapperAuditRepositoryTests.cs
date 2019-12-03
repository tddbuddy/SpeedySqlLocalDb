using System;
using System.Linq;
using Dapper;
using FluentAssertions;
using NUnit.Framework;
using TddBuddy.SpeedyLocalDb.EF.Example.Audit.Entities;
using TddBuddy.SpeedySqlLocalDb.Attribute;
using TddBuddy.SpeedySqlLocalDb.Construction;

namespace TddBuddy.SpeedySqlLocalDb.Tests
{
    [TestFixture, SharedSpeedyLocalDb]
    public class DapperAuditRepositoryTests
    {
        [Test]
        [Ignore("wip")]
        public void Create_GivenAuditEntry_ShouldStoreInDatabase()
        {
            //---------------Set up test pack-------------------
            var auditEntry = new AuditEntry
            {
                Id = Guid.NewGuid(),
                System = "System-A",
                User = "Foo",
                LogDetail = "Bar One"
            };

            using (var wrapper = CreateWrapper())
            {
                var connection = wrapper.Connection;
                connection.Execute($"INSERT INTO AuditEntry(Id, System, [User], LogDetail) VALUES" +
                                   $"({auditEntry.Id},{auditEntry.System},{auditEntry.User},{auditEntry.LogDetail})");
                //---------------Execute Test ----------------------
                var actual = connection.Query<AuditEntry>("SELECT * from AuditEntry")
                                       .FirstOrDefault();
                //---------------Test Result -----------------------
                actual.Should().BeEquivalentTo(auditEntry);
            }
        }

        private ISpeedySqlLocalDbWrapper CreateWrapper()
        {
            return new SpeedySqlBuilder()
                       .WithTransactionTimeout(5)
                        .WithMigrationAction(() =>
                        {
                            // todo : run mirations to build DB
                        })
                        .BuildWrapper();
        }

        private void AssertIsEqual(AuditEntry expectedAttachment, AuditEntry actualAttachment)
        {
            Assert.AreEqual(expectedAttachment.Id, actualAttachment.Id);
            Assert.AreEqual(expectedAttachment.System, actualAttachment.System);
            Assert.AreEqual(expectedAttachment.User, actualAttachment.User);
        }

    }
}