namespace PhotoGallery.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddValidtionsToEvents : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Events", "Name", c => c.String(nullable: false));
            AlterColumn("dbo.Events", "Description", c => c.String(maxLength: 1000));
            AlterColumn("dbo.Events", "location", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Events", "location", c => c.String());
            AlterColumn("dbo.Events", "Description", c => c.String());
            AlterColumn("dbo.Events", "Name", c => c.String());
        }
    }
}
