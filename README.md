# SpeedySqlLocalDb

**Please note: This package only works with NUnit**

A super quick localDB testing framework for use with Entity Framework. It uses a localDB instance for the entire test run's life. Each test's execution is wrapped into transaction to avoid test interfering with each other. This means the cost of setting up the DB is only paid once. That is when the first test executes and the EF migrations are run to initialize the DB to the correct state.

This framework was sliced out of from a production project where the test runs where nearing 40 minutes on the CI server. The bulk of the time was take up by 200 repository integration test. This due to Entity Framework's need to run migrations for each test being executed because each test needed its own DB to allow for proper isolation. After implementing the transactional scoping logic as per this package the total test run time was reduced to 8 minutes. A full 5x speed improvement.

Have a look at **SpeedySqlLocalDb/source/TddBuddy.SpeedySqlLocalDb.EF.Examples/AttachmentRepositoryTests.cs** for example of how to use it.

An example using it with EF:

    [TestFixture]
    [SharedSpeedyLocalDb(typeof(AttachmentDbContext))]
    public class AttachmentRepositoryTests 
    {
        [Test]
        public void Create_GivenOneAttachment_ShouldStoreInDatabase()
        {
            //---------------Set up test pack-------------------
            var attachment = new AttachmentBuilder()
                    .WithFileName("A.JPG")
                    .WithContent(CreateRandomByteArray(100))
                    .Build();

            using (var wrapper = new SpeedySqlFactory().CreateWrapper())
            {
                var repositoryDbContext = CreateDbContext(wrapper.Connection);
                var assertDbContext = CreateDbContext(wrapper.Connection);
                var attachmentsRepository = CreateRepository(repositoryDbContext);
                //---------------Execute Test ----------------------
                attachmentsRepository.Create(attachment);
                attachmentsRepository.Save();
                //---------------Test Result -----------------------
                Assert.AreEqual(1, assertDbContext.Attachments.Count());
                var actualAttachment = assertDbContext.Attachments.First();
                AssertIsEqual(attachment, actualAttachment);
            }
        }
     }
     
To make use of the utilty with a non-EF migration framework you will need to adjust how you use the SpeedySqlBuilder. It must take a WithMigrationAction as per below.
Then use an empty annotation on the class [SharedSpeedyLocalDb], this causes it to not try and run EF migrations.
Intead the migrations action specified in the builder is run once like the EF migrations.
Becuase EF migrations provide a lot of out of the box magic this was the best of all the bad solutions. 

    return new SpeedySqlBuilder()
               .WithTransactionTimeout(5)
               .WithMigrationAction(() =>
                {
                    /* Add migration action here */
                })
                .BuildWrapper();
