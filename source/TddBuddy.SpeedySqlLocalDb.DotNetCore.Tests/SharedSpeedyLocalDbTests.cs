using System;
using System.Data.Common;
using NUnit.Framework;
using TddBuddy.SpeedyLocalDb.DotNetCore.Attribute;
using TddBuddy.SpeedyLocalDb.EF.Example.Attachment.DotNetCore.Context;

namespace TddBuddy.SpeedySqlLocalDb.DotNetCore.Tests
{
    [TestFixture]
    public class SharedSpeedyLocalDbTests
    {
        [Test]
        public void Ctor_WhenDbContext_ShouldNotThrowException()
        {
            //---------------Set up test pack-------------------
            //---------------Execute Test ----------------------
            //---------------Test Result -----------------------
            Assert.DoesNotThrow(() => new SharedSpeedyLocalDb(typeof(AttachmentDbContext)));
        }

        [Test]
        public void Ctor_WhenDbConnection_ShouldNotThrowException()
        {
            //---------------Set up test pack-------------------
            //---------------Execute Test ----------------------
            //---------------Test Result -----------------------
            Assert.DoesNotThrow(() =>
            {
                var dbContextType = typeof(DbConnection);
                new SharedSpeedyLocalDb(dbContextType);
            });
        }

        [Test]
        public void Ctor_WhenNotDbContext_ShouldThrowException()
        {
            //---------------Set up test pack-------------------
            //---------------Execute Test ----------------------
            var result = Assert.Throws<Exception>(() => new SharedSpeedyLocalDb(typeof(string)));
            //---------------Test Result -----------------------
            Assert.AreEqual("dbContextType must be a subclass of DbContext or DbConnection", result.Message);
        }

        [Test]
        public void Ctor_WhenDefault_ShouldNotThrowException()
        {
            //---------------Set up test pack-------------------
            //---------------Execute Test ----------------------
            //---------------Test Result -----------------------
            Assert.DoesNotThrow(() => new SharedSpeedyLocalDb());
        }
    }
}
