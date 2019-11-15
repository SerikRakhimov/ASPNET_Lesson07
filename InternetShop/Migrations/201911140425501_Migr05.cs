namespace InternetShop.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Migr05 : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Products", "CategoryName");
            DropColumn("dbo.Products", "Discriminator");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Products", "Discriminator", c => c.String(nullable: false, maxLength: 128));
            AddColumn("dbo.Products", "CategoryName", c => c.String());
        }
    }
}
