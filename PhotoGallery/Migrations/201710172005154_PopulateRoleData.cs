namespace PhotoGallery.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PopulateRoleData : DbMigration
    {
        public override void Up()
        {
            Sql("INSERT INTO Roles (Name) VALUES ('Admin')");
            Sql("INSERT INTO Roles (Name) VALUES ('User')");
        }
        
        public override void Down()
        {
            Sql("DELETE FROM Roles WHERE Name='Admin'");
            Sql("DELETE FROM Roles WHERE Name='User'");
        }
    }
}
