namespace PhotoGallery.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddColumnUserIdToEvents : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Events", "UserId", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Events", "UserId");
        }
    }
}
