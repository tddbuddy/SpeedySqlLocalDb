namespace TddBuddy.SpeedyLocalDb.EF.Example.Audit.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Audit_Init : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AuditEntries",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        System = c.String(),
                        User = c.String(),
                        LogDetail = c.String(),
                        CreateTimestamp = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.AuditEntries");
        }
    }
}
