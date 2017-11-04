namespace PhotoGallery.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddNewValidtionsToEvents : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Events", "location", c => c.String(nullable: true));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Events", "location", c => c.String(nullable: false));
        }
    }
}
