using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TddBuddy.SpeedyLocalDb.EF.Example.Audit.DotNetCore.Migrations
{
    public partial class Init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AuditEntries",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    System = table.Column<string>(nullable: true),
                    User = table.Column<string>(nullable: true),
                    LogDetail = table.Column<string>(nullable: true),
                    CreateTimestamp = table.Column<System.DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuditEntries", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AuditEntries");
        }
    }
}
