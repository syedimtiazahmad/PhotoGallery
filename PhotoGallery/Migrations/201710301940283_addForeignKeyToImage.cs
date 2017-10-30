namespace PhotoGallery.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addForeignKeyToImage : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Images", "FrameId", c => c.Int(nullable: false, defaultValue: 0));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Frames", "Name");
        }
    }
}
