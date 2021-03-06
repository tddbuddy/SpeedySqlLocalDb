﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using TddBuddy.SpeedyLocalDb.EF.Example.Attachment.DotNetCore.Context;

namespace TddBuddy.SpeedyLocalDb.EF.Example.Attachment.DotNetCore.Migrations
{
    [DbContext(typeof(AttachmentDbContext))]
    partial class AttachmentDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.14-servicing-32113")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("TddBuddy.SpeedyLocalDb.EF.Example.Attachment.DotNetCore.Entities.Attachment", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<byte[]>("Content");

                    b.Property<string>("ContentType");

                    b.Property<string>("FileName");

                    b.HasKey("Id");

                    b.ToTable("Attachments");
                });
#pragma warning restore 612, 618
        }
    }
}
