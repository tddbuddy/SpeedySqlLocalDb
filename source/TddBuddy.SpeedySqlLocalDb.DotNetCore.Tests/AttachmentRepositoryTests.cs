using System;
using System.Data.Common;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using TddBuddy.SpeedyLocalDb.DotNetCore.Attribute;
using TddBuddy.SpeedyLocalDb.DotNetCore.Construction;
using TddBuddy.SpeedyLocalDb.EF.Example.Attachment.DotNetCore.Builders;
using TddBuddy.SpeedyLocalDb.EF.Example.Attachment.DotNetCore.Context;
using TddBuddy.SpeedyLocalDb.EF.Example.Attachment.DotNetCore.Entities;
using TddBuddy.SpeedyLocalDb.EF.Example.Attachment.DotNetCore.Repositories;

namespace TddBuddy.SpeedySqlLocalDb.DotNetCore.Tests
{
    [TestFixture, SharedSpeedyLocalDb(typeof(AttachmentDbContext))]
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

            using (var wrapper = SpeedySqlFactory.CreateWrapper())
            {
                var dbContext = CreateDbContext(wrapper.Connection);
                var attachmentsRepository = CreateRepository(dbContext);
                //---------------Execute Test ----------------------
                attachmentsRepository.Create(attachment);
                attachmentsRepository.Save();
                //---------------Test Result -----------------------
                Assert.AreEqual(1, dbContext.Attachments.Count());
                var actualAttachment = dbContext.Attachments.First();
                AssertIsEqual(attachment, actualAttachment);
            }
        }

        [Test]
        public void Create_GivenExistingAttachment_ShouldThrowExceptionWhenSaving()
        {
            //---------------Set up test pack-------------------
            var attachment = new AttachmentBuilder()
                .WithFileName("A.JPG")
                .WithContent(CreateRandomByteArray(100))
                .Build();

            using (var wrapper = SpeedySqlFactory.CreateWrapper())
            {
                var dbContext = CreateDbContext(wrapper.Connection);
                var attachmentsRepository = CreateRepository(dbContext);

                dbContext.Attachments.Add(attachment);
                dbContext.SaveChanges();
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

            using (var wrapper = SpeedySqlFactory.CreateWrapper())
            {
                var dbContext = CreateDbContext(wrapper.Connection);
                var attachmentsRepository = CreateRepository(dbContext);
                //---------------Execute Test ----------------------
                attachmentsRepository.Create(attachment1);
                attachmentsRepository.Create(attachment2);
                attachmentsRepository.Create(attachment3);
                attachmentsRepository.Save();
                //---------------Test Result -----------------------
                Assert.AreEqual(3, dbContext.Attachments.Count());
                var actualAttachment1 = dbContext.Attachments.First(r => r.Id == attachment1.Id);
                var actualAttachment2 = dbContext.Attachments.First(r => r.Id == attachment2.Id);
                var actualAttachment3 = dbContext.Attachments.First(r => r.Id == attachment3.Id);

                AssertIsEqual(attachment1, actualAttachment1);
                AssertIsEqual(attachment2, actualAttachment2);
                AssertIsEqual(attachment3, actualAttachment3);
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
            using (var wrapper = SpeedySqlFactory.CreateWrapper())
            {
                var dbContext = CreateDbContext(wrapper.Connection);
                var attachmentsRepository = CreateRepository(dbContext);

                dbContext.Attachments.Add(attachment);
                dbContext.SaveChanges();
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

            using (var wrapper = SpeedySqlFactory.CreateWrapper())
            {
                var dbContext = CreateDbContext(wrapper.Connection);
                var attachmentsRepository = CreateRepository(dbContext);

                dbContext.Attachments.Add(attachment);
                dbContext.SaveChanges();
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

            using (var wrapper = SpeedySqlFactory.CreateWrapper())
            {
                var dbContext = CreateDbContext(wrapper.Connection);
                var attachmentsRepository = CreateRepository(dbContext);

                dbContext.Attachments.Add(attachment);
                dbContext.SaveChanges();

                var updatedAttachment = dbContext.Attachments.First(r => r.Id == attachment.Id);
                updatedAttachment.FileName = "Update-A.jpg";
                //---------------Execute Test ----------------------
                attachmentsRepository.Update(updatedAttachment);
                attachmentsRepository.Save();
                //---------------Test Result -----------------------
                var actualAttachment = dbContext.Attachments.First();
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

            using (var wrapper = SpeedySqlFactory.CreateWrapper())
            {
                var dbContext = CreateDbContext(wrapper.Connection);
                var attachmentsRepository = CreateRepository(dbContext);

                dbContext.Attachments.Add(attachmentBeingUpdated);
                dbContext.Attachments.Add(attachmentNotBeingUpdated);
                dbContext.SaveChanges();

                var updatedAttachment = dbContext.Attachments.First(r => r.Id == attachmentBeingUpdated.Id);
                updatedAttachment.FileName = "Update-A.jpg";
                //---------------Execute Test ----------------------
                attachmentsRepository.Update(updatedAttachment);
                attachmentsRepository.Save();
                //---------------Test Result -----------------------
                var actualAttachmentBeingUpdated =
                    dbContext.Attachments.First(r => r.Id == attachmentBeingUpdated.Id);
                var actualAttachmentNotBeingUpdated =
                    dbContext.Attachments.First(r => r.Id == attachmentNotBeingUpdated.Id);
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

            using (var wrapper = SpeedySqlFactory.CreateWrapper())
            {
                var dbContext = CreateDbContext(wrapper.Connection);
                var attachmentsRepository = CreateRepository(dbContext);

                dbContext.Attachments.Add(attachment1);
                dbContext.Attachments.Add(attachment2);
                dbContext.Attachments.Add(attachment3);
                dbContext.SaveChanges();

                var updatedAttachment1 = dbContext.Attachments.First(r => r.Id == attachment1.Id);
                updatedAttachment1.FileName = "Update-A.jpg";

                var updatedAttachment2 = dbContext.Attachments.First(r => r.Id == attachment2.Id);
                updatedAttachment2.FileName = "Update-B.jpg";

                var updatedAttachment3 = dbContext.Attachments.First(r => r.Id == attachment3.Id);
                updatedAttachment3.FileName = "Update-C.jpg";
                //---------------Execute Test ----------------------
                attachmentsRepository.Update(updatedAttachment1);
                attachmentsRepository.Update(updatedAttachment2);
                attachmentsRepository.Update(updatedAttachment3);
                attachmentsRepository.Save();
                //---------------Test Result -----------------------
                var actualAttachment1 = dbContext.Attachments.First(r => r.Id == attachment1.Id);
                var actualAttachment2 = dbContext.Attachments.First(r => r.Id == attachment2.Id);
                var actualAttachment3 = dbContext.Attachments.First(r => r.Id == attachment3.Id);
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
            using (var wrapper = SpeedySqlFactory.CreateWrapper())
            {
                var dbContext = CreateDbContext(wrapper.Connection);
                var attachmentsRepository = CreateRepository(dbContext);
                //---------------Execute Test ----------------------
                attachmentsRepository.Update(attachment);
                var exception = Assert.Throws<DbUpdateConcurrencyException>(() => attachmentsRepository.Save());
                //---------------Test Result -----------------------
                StringAssert.Contains("Database operation expected to affect 1 row(s) but actually affected 0 row(s). Data may have been modified or deleted since entities were loaded. See http://go.microsoft.com/fwlink/?LinkId=527962 for information on understanding and handling optimistic concurrency exceptions.",
                    exception.Message);
            }
        }

        private void AssertIsEqual(Attachment expectedAttachment, Attachment actualAttachment)
        {
            Assert.AreEqual(expectedAttachment.Id, actualAttachment.Id);
            Assert.AreEqual(expectedAttachment.FileName, actualAttachment.FileName);
        }

        private AttachmentRepository CreateRepository(AttachmentDbContext writeDbContext)
        {
            return new AttachmentRepository(writeDbContext);
        }

        private AttachmentDbContext CreateDbContext(DbConnection connection)
        {
            var connectionString = connection.ConnectionString;

            var builder = new DbContextOptionsBuilder<AttachmentDbContext>();
            builder
                .UseSqlServer(connectionString)
                .EnableSensitiveDataLogging(true);

            return new AttachmentDbContext(builder.Options);
        }

        private static byte[] CreateRandomByteArray(int size)
        {
            var bytes = new byte[size];
            new Random().NextBytes(bytes);
            return bytes;
        }
    }

}