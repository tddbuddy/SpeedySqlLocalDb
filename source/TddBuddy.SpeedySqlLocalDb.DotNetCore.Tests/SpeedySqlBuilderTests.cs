using System.Threading.Tasks;
using NUnit.Framework;
using TddBuddy.SpeedyLocalDb.DotNetCore.Construction;

namespace TddBuddy.SpeedySqlLocalDb.DotNetCore.Tests
{
    [TestFixture]
    public class SpeedySqlBuilderTests
    {
        [Test]
        public void Build_WhenConstructedWithMigrationAction_ShouldExecuteMigrationAction()
        {
            Task.Run(() =>
            {
                //---------------Set up test pack-------------------
                var result = string.Empty;
                var builder = new SpeedySqlBuilder();
                //---------------Execute Test ----------------------
                builder.WithMigrationAction(() => { result = "transaction ran"; });
                builder.BuildWrapper();
                //---------------Test Result -----------------------
                var expected = "transaction ran";
                Assert.AreEqual(expected, result);
            });

        }

        [Test]
        public void Build_WhenConstructedNoWithMigrationAction_ShouldNotThrowException()
        {
            Task.Run(() =>
            {

                //---------------Set up test pack-------------------
                var builder = new SpeedySqlBuilder();
                //---------------Execute Test ----------------------
                //---------------Test Result -----------------------
                Assert.DoesNotThrow(() => builder.BuildWrapper());
            });
            
        }
        
        [Test]
        public void Build_WhenConstructedTwiceWithMigrationAction_ShouldExecuteMigrationActionOnce()
        {
            Task.Run(() =>
            {
                //---------------Set up test pack-------------------
                var result = string.Empty;
                var counter = 1;
                var builder = new SpeedySqlBuilder();
                //---------------Execute Test ----------------------
                builder.WithMigrationAction(() =>
                {
                    result = $"transaction ran {counter} time(s)";
                    counter++;
                });
                builder.BuildWrapper();
                builder.BuildWrapper();
                //---------------Test Result -----------------------
                var expected = "transaction ran 1 time(s)";
                Assert.AreEqual(expected, result);
            });
        }
    }
}