using Microsoft.EntityFrameworkCore.Migrations;

namespace TddBuddy.SpeedyLocalDb.EF.Example.Audit.DotNetCore.Migrations
{
    public partial class Added_Schema : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_AuditEntries",
                table: "AuditEntries");

            migrationBuilder.EnsureSchema(
                name: "aud");

            migrationBuilder.RenameTable(
                name: "AuditEntries",
                newName: "Entries",
                newSchema: "aud");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Entries",
                schema: "aud",
                table: "Entries",
                column: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Entries",
                schema: "aud",
                table: "Entries");

            migrationBuilder.RenameTable(
                name: "Entries",
                schema: "aud",
                newName: "AuditEntries");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AuditEntries",
                table: "AuditEntries",
                column: "Id");
        }
    }
}
