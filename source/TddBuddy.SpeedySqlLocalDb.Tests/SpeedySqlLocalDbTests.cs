using NUnit.Framework;
using System;
using System.IO;
using TddBuddy.SpeedySqlLocalDb;
using TddBuddy.SpeedySqlLocalDb.Test;

namespace TempLocalDb.TestingFramework.Tests
{
    [TestFixture]
    public class SpeedyTestLocalDb
    {
        // https://github.com/fluffynuts/PeanutButter/blob/master/source/TempDb/PeanutButter.TempDb.LocalDb/TempDBLocalDb.cs
        // https://github.com/fluffynuts/PeanutButter/blob/master/source/TempDb/PeanutButter.TempDb.Tests/TestTempDBLocalDb.cs

        [Test]
        public void Ctor_WhenUsingParamaterlessConstructor_ShouldNotThrowException()
        {
            //---------------Set up test pack-------------------
            //---------------Execute Test ----------------------
            //---------------Test Result -----------------------
            Assert.DoesNotThrow(() => new SpeedySqlLocalDb());
        }

        [Test]
        public void BootstrapDatabase_WhenNullCreateDbContextFunc_ShouldThrowANE()
        {
            //---------------Set up test pack-------------------
            var expected = "createDbContextFunc";
            var sut = CreateSpeedyLocalDb();
            //---------------Execute Test ----------------------
            var result = Assert.Throws<ArgumentException>(() => sut.BootstrapDatabaseForEfMigrations(connection => null));
            //---------------Test Result -----------------------
            Assert.AreEqual(expected, result.Message);
        }

        [Test]
        public void BootstrapDatabase_WhenValidCreateDbContextFunc_ShouldNotThrowException()
        {
            //---------------Set up test pack-------------------
            var sut = CreateSpeedyLocalDb();
            //---------------Execute Test ----------------------
            //---------------Test Result -----------------------
            Assert.DoesNotThrow(() => sut.BootstrapDatabaseForEfMigrations(connection => new ExampleDbContext(connection)));
        }

        [Test]
        public void CreateTempLocalDbWrapper_WhenConfiguredCorrectly_ShouldHaveNotNullConnection()
        {
            //---------------Set up test pack-------------------
            var sut = CreateSpeedyLocalDb();
            sut.BootstrapDatabaseForEfMigrations(connection => new ExampleDbContext(connection));
            //---------------Execute Test ----------------------
            using (var wrapper = sut.CreateTempLocalDbWrapper())
            {
                //---------------Test Result -----------------------
                Assert.NotNull(wrapper.Connection);
            }
        }

        private static SpeedySqlLocalDb CreateSpeedyLocalDb()
        {
            return new SpeedySqlLocalDb();
        }
    }
}
