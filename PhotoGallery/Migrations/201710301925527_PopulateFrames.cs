namespace PhotoGallery.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PopulateFrames : DbMigration
    {
        public override void Up()
        {
            Sql("INSERT INTO frames (Code, Name) VALUES (1, 'Purple')");
            Sql("INSERT INTO frames (Code, Name) VALUES (2, 'BurlyWood')");
            Sql("INSERT INTO frames (Code, Name) VALUES (3, 'BlueGreen')");
            Sql("INSERT INTO frames (Code, Name) VALUES (4, 'RedOrangeGreen')");
        }
        
        public override void Down()
        {
            Sql("DELETE FROM frames WHERE Code = 1");
            Sql("DELETE FROM frames WHERE Code = 2");
            Sql("DELETE FROM frames WHERE Code = 3");
            Sql("DELETE FROM frames WHERE Code = 4");
        }
    }
}
