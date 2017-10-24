namespace PhotoGallery.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addColumnPermissionToUsers : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Users", "Permission", c => c.Boolean(nullable: false, defaultValue: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Users", "Permission");
        }
    }
}
