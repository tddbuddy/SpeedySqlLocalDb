using System;
using System.Data.Common;
using System.Data.Entity.Infrastructure;
using System.Linq;
using NUnit.Framework;
using TddBuddy.SpeedySqlLocalDb.EF.Examples.ExampleDb;

namespace TddBuddy.SpeedySqlLocalDb.EF.Examples
{
    [TestFixture]
    public class AttachmentRepositoryTests
    {
        // NOTE: The one and only time it is acceptable to use Setup, 
        // TearDown and class level variables in test is for DB Intergration test
        // this is for a large performance boost
        private readonly SpeedySqlLocalDb _testDb = new SpeedySqlLocalDb();
        
        [TestFixtureSetUp]
        public void Setup()
        {
            // pass through lamda to create your db context ;)
            _testDb.BootstrapDatabase(connection => new ExampleDbContext(connection) );
            // add your own migration logic here if not using EF's
            // https://github.com/lfreneda/EasyLocalDb - fluent migrator example
        }

        [TestFixtureTearDown]
        public void TearDown()
        {
            _testDb.DetachDatabase();
        }

        [Test]
        public void Create_GivenExistingAttachment_ShouldThrowExceptionWhenSaving()
        {
            //---------------Set up test pack-------------------
            var attachment = new AttachmentBuilder()
                .WithFileName("A.JPG")
                .WithContent(CreateRandomByteArray(100))
                .Build();

            using (var wrapper = _testDb.CreateTempLocalDbWrapper())
            {
                var repositoryDbContext = CreateDbContext(wrapper.Connection);
                var createDbContext = CreateDbContext(wrapper.Connection);
                var attachmentsRepository = CreateRepository(repositoryDbContext);

                createDbContext.Attachments.Add(attachment);
                createDbContext.SaveChanges();
                //---------------Execute Test ----------------------
                attachmentsRepository.Create(attachment);
                var exception = Assert.Throws<DbUpdateException>(() => attachmentsRepository.Save());
                //---------------Test Result -----------------------
                StringAssert.Contains("An error occurred while updating the entries", exception.Message);
            }
            
        }

        [Test]
        public void Create_GivenManyAttachments_ShouldStoreAllInDatabase()
        {
            //---------------Set up test pack-------------------
            var attachment1 = new AttachmentBuilder()
                    .WithFileName("A.JPG")
                    .WithContent(CreateRandomByteArray(100))
                    .Build();

            var attachment2 = new AttachmentBuilder()
                .WithId(new Guid())
                .WithFileName("B.JPG")
                .WithContent(CreateRandomByteArray(100))
                .Build();

            var attachment3 = new AttachmentBuilder()
                .WithFileName("C.JPG")
                .WithContent(CreateRandomByteArray(100))
                .Build();

            using (var wrapper = _testDb.CreateTempLocalDbWrapper())
            {
                var repositoryDbContext = CreateDbContext(wrapper.Connection);
                var assertDbContext = CreateDbContext(wrapper.Connection);
                var attachmentsRepository = CreateRepository(repositoryDbContext);
                //---------------Execute Test ----------------------
                attachmentsRepository.Create(attachment1);
                attachmentsRepository.Create(attachment2);
                attachmentsRepository.Create(attachment3);
                attachmentsRepository.Save();
                //---------------Test Result -----------------------
                Assert.AreEqual(3, assertDbContext.Attachments.Count());
                var actualAttachment1 = assertDbContext.Attachments.First(r => r.Id == attachment1.Id);
                var actualAttachment2 = assertDbContext.Attachments.First(r => r.Id == attachment2.Id);
                var actualAttachment3 = assertDbContext.Attachments.First(r => r.Id == attachment3.Id);

                AssertIsEqual(attachment1, actualAttachment1);
                AssertIsEqual(attachment2, actualAttachment2);
                AssertIsEqual(attachment3, actualAttachment3);
            }

        }

        [Test]
        public void Create_GivenOneAttachment_ShouldStoreInDatabase()
        {
            //---------------Set up test pack-------------------
            var attachment = new AttachmentBuilder()
                    .WithFileName("A.JPG")
                    .WithContent(CreateRandomByteArray(100))
                    .Build();

            using (var wrapper = _testDb.CreateTempLocalDbWrapper())
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

        [Test]
        public void Find_GivenExistingAttachment_ShouldReturnAttachment()
        {
            //---------------Set up test pack-------------------
            var id = Guid.NewGuid();
            var attachment = new AttachmentBuilder()
                .WithId(id)
                .WithFileName("A.JPG")
                .WithContent(CreateRandomByteArray(100))
                .Build();
            using (var wrapper = _testDb.CreateTempLocalDbWrapper())
            {
                var repositoryDbContext = CreateDbContext(wrapper.Connection);
                var createDbContext = CreateDbContext(wrapper.Connection);
                var attachmentsRepository = CreateRepository(repositoryDbContext);

                createDbContext.Attachments.Add(attachment);
                createDbContext.SaveChanges();
                //---------------Execute Test ----------------------
                var actualAttachment = attachmentsRepository.Find(id);
                //---------------Test Result -----------------------
                Assert.IsNotNull(actualAttachment);
                AssertIsEqual(attachment, actualAttachment);
            }
        }

        [Test]
        public void Find_GivenAttachmentDoesNotExist_ShouldReturnNull()
        {
            //---------------Set up test pack-------------------
            var nonExistantId = Guid.NewGuid();
            var attachment = new AttachmentBuilder()
                .WithId(Guid.NewGuid())
                .Build();

            using (var wrapper = _testDb.CreateTempLocalDbWrapper())
            {
                var repositoryDbContext = CreateDbContext(wrapper.Connection);
                var createDbContext = CreateDbContext(wrapper.Connection);
                var attachmentsRepository = CreateRepository(repositoryDbContext);

                createDbContext.Attachments.Add(attachment);
                createDbContext.SaveChanges();
                //---------------Execute Test ----------------------
                var actualAttachment = attachmentsRepository.Find(nonExistantId);
                //---------------Test Result -----------------------
                Assert.Null(actualAttachment);
            }
        }

        [Test]
        public void Update_GivenExistingAttachment_ShouldUpdateInDatabase()
        {
            //---------------Set up test pack-------------------
            var attachment = new AttachmentBuilder()
                    .WithFileName("A.JPG")
                    .WithContent(CreateRandomByteArray(100))
                    .Build();

            using (var wrapper = _testDb.CreateTempLocalDbWrapper())
            {
                var repositoryDbContext = CreateDbContext(wrapper.Connection);
                var createDbContext = CreateDbContext(wrapper.Connection);
                var assertDbContext = CreateDbContext(wrapper.Connection);
                var readDbContext = CreateDbContext(wrapper.Connection);
                var attachmentsRepository = CreateRepository(repositoryDbContext);

                createDbContext.Attachments.Add(attachment);
                createDbContext.SaveChanges();

                var updatedAttachment = readDbContext.Attachments.First(r => r.Id == attachment.Id);
                updatedAttachment.FileName = "Update-A.jpg";
                //---------------Execute Test ----------------------
                attachmentsRepository.Update(updatedAttachment);
                attachmentsRepository.Save();
                //---------------Test Result -----------------------
                var actualAttachment = assertDbContext.Attachments.First();
                AssertIsEqual(updatedAttachment, actualAttachment);
            }
        }

        [Test]
        public void Update_GivenExistingAttachments_ShouldOnlyUpdateAttachmentWithGivenId()
        {
            //---------------Set up test pack-------------------
            var attachmentBeingUpdated = new AttachmentBuilder()
                    .WithFileName("A.JPG")
                    .WithContent(CreateRandomByteArray(100))
                    .Build();

            var attachmentNotBeingUpdated = new AttachmentBuilder()
                .WithFileName("B.JPG")
                .WithContent(CreateRandomByteArray(100))
                .Build();

            using (var wrapper = _testDb.CreateTempLocalDbWrapper())
            {
                var repositoryDbContext = CreateDbContext(wrapper.Connection);
                var createDbContext = CreateDbContext(wrapper.Connection);
                var assertDbContext = CreateDbContext(wrapper.Connection);
                var readDbContext = CreateDbContext(wrapper.Connection);
                var attachmentsRepository = CreateRepository(repositoryDbContext);

                createDbContext.Attachments.Add(attachmentBeingUpdated);
                createDbContext.Attachments.Add(attachmentNotBeingUpdated);
                createDbContext.SaveChanges();

                var updatedAttachment = readDbContext.Attachments.First(r => r.Id == attachmentBeingUpdated.Id);
                updatedAttachment.FileName = "Update-A.jpg";
                //---------------Execute Test ----------------------
                attachmentsRepository.Update(updatedAttachment);
                attachmentsRepository.Save();
                //---------------Test Result -----------------------
                var actualAttachmentBeingUpdated =
                    assertDbContext.Attachments.First(r => r.Id == attachmentBeingUpdated.Id);
                var actualAttachmentNotBeingUpdated =
                    assertDbContext.Attachments.First(r => r.Id == attachmentNotBeingUpdated.Id);
                AssertIsEqual(updatedAttachment, actualAttachmentBeingUpdated);
                AssertIsEqual(attachmentNotBeingUpdated, actualAttachmentNotBeingUpdated);
            }
        }

        [Test]
        public void Update_GivenManyExistingAttachments_ShouldUpdateInDatabase()
        {
            //---------------Set up test pack-------------------
            var attachment1 = new AttachmentBuilder()
                    .WithFileName("A.JPG")
                    .WithContent(CreateRandomByteArray(100))
                    .Build();

            var attachment2 = new AttachmentBuilder()
                .WithFileName("B.JPG")
                .WithContent(CreateRandomByteArray(100))
                .Build();

            var attachment3 = new AttachmentBuilder()
                .WithFileName("C.JPG")
                .WithContent(CreateRandomByteArray(100))
                .Build();

            using (var wrapper = _testDb.CreateTempLocalDbWrapper())
            {
                var repositoryDbContext = CreateDbContext(wrapper.Connection);
                var createDbContext = CreateDbContext(wrapper.Connection);
                var assertDbContext = CreateDbContext(wrapper.Connection);
                var readDbContext = CreateDbContext(wrapper.Connection);
                var attachmentsRepository = CreateRepository(repositoryDbContext);

                createDbContext.Attachments.Add(attachment1);
                createDbContext.Attachments.Add(attachment2);
                createDbContext.Attachments.Add(attachment3);
                createDbContext.SaveChanges();

                var updatedAttachment1 = readDbContext.Attachments.First(r => r.Id == attachment1.Id);
                updatedAttachment1.FileName = "Update-A.jpg";

                var updatedAttachment2 = readDbContext.Attachments.First(r => r.Id == attachment2.Id);
                updatedAttachment2.FileName = "Update-B.jpg";

                var updatedAttachment3 = readDbContext.Attachments.First(r => r.Id == attachment3.Id);
                updatedAttachment3.FileName = "Update-C.jpg";
                //---------------Execute Test ----------------------
                attachmentsRepository.Update(updatedAttachment1);
                attachmentsRepository.Update(updatedAttachment2);
                attachmentsRepository.Update(updatedAttachment3);
                attachmentsRepository.Save();
                //---------------Test Result -----------------------
                var actualAttachment1 = assertDbContext.Attachments.First(r => r.Id == attachment1.Id);
                var actualAttachment2 = assertDbContext.Attachments.First(r => r.Id == attachment2.Id);
                var actualAttachment3 = assertDbContext.Attachments.First(r => r.Id == attachment3.Id);
                AssertIsEqual(updatedAttachment1, actualAttachment1);
                AssertIsEqual(updatedAttachment2, actualAttachment2);
                AssertIsEqual(updatedAttachment3, actualAttachment3);
            }
        }

        [Test]
        public void Update_GivenAttachmentDoesNotExist_ShouldThrowExceptionWhenSaving()
        {
            //---------------Set up test pack-------------------
            var attachment = new AttachmentBuilder()
                    .WithFileName("A.JPG")
                    .WithContent(CreateRandomByteArray(100))
                    .Build();
            using (var wrapper = _testDb.CreateTempLocalDbWrapper())
            {
                var repositoryDbContext = CreateDbContext(wrapper.Connection);
                var attachmentsRepository = CreateRepository(repositoryDbContext);
                //---------------Execute Test ----------------------
                attachmentsRepository.Update(attachment);
                var exception = Assert.Throws<DbUpdateConcurrencyException>(() => attachmentsRepository.Save());
                //---------------Test Result -----------------------
                StringAssert.Contains("Entities may have been modified or deleted since entities were loaded",
                    exception.Message);
            }
        }

        private void AssertIsEqual(Attachment expectedAttachment, Attachment actualAttachment)
        {
            Assert.AreEqual(expectedAttachment.Id, actualAttachment.Id);
            Assert.AreEqual(expectedAttachment.FileName, actualAttachment.FileName);
        }

        private AttachmentRepository CreateRepository(ExampleDbContext writeDbContext)
        {
            return new AttachmentRepository(writeDbContext);
        }

        private ExampleDbContext CreateDbContext(DbConnection connection)
        {
            return new ExampleDbContext(connection);
        }

        private static byte[] CreateRandomByteArray(int size)
        {
            var bytes = new byte[size];
            new Random().NextBytes(bytes);
            return bytes;
        }
    }
}