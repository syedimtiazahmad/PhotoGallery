namespace PhotoGallery.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddIndexAndForiegnKeyToEventIdInImages : DbMigration
    {
        public override void Up()
        {
            CreateIndex("dbo.Images", "EventId");
            AddForeignKey("dbo.Images", "EventId", "dbo.Events", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Images", "EventId", "dbo.Events");
            DropIndex("dbo.Images", new[] { "EventId" });
        }
    }
}
