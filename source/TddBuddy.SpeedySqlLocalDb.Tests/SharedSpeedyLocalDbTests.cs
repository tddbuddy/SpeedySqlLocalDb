using System;
using NUnit.Framework;
using TddBuddy.SpeedyLocalDb.EF.Example.Attachment.Context;
using TddBuddy.SpeedySqlLocalDb.Attribute;

namespace TddBuddy.SpeedySqlLocalDb.Tests
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
        public void Ctor_WhenNotDbContext_ShouldThrowException()
        {
            //---------------Set up test pack-------------------
            //---------------Execute Test ----------------------
            var result = Assert.Throws<Exception>(() => new SharedSpeedyLocalDb(typeof(string)));
            //---------------Test Result -----------------------
            Assert.AreEqual("dbContextType must be a subclass of DbContext", result.Message);
        }
    }
}
