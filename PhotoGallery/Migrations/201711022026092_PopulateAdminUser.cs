namespace PhotoGallery.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PopulateAdminUser : DbMigration
    {
        public override void Up()
        {
            Sql("INSERT INTO users (FirstName, LastName, age, Email, Password, RoleId, Permission) VALUES ('Imtiaz', 'Ahmad', 0, 'imtiaz.ahmad@gmail.com', 'password', 1, 1)");
        }
        
        public override void Down()
        {
        }
    }
}
