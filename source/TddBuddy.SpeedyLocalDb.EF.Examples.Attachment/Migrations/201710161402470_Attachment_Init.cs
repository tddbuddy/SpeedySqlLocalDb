namespace TddBuddy.SpeedyLocalDb.EF.Examples.Attachment.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Attachment_Init : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Attachments",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        FileName = c.String(),
                        ContentType = c.String(),
                        Content = c.Binary(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Attachments");
        }
    }
}
