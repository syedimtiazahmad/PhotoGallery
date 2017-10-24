namespace PhotoGallery.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddColumnsToEvents : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Events", "location", c => c.String());
            AddColumn("dbo.Events", "CreatedAt", c => c.DateTime());
            AddColumn("dbo.Events", "UpdatedAt", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Events", "UpdatedAt");
            DropColumn("dbo.Events", "CreatedAt");
            DropColumn("dbo.Events", "location");
        }
    }
}
