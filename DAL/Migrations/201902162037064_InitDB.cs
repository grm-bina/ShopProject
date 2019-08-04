namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitDB : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Products",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 30),
                        PublicationDate = c.DateTime(nullable: false),
                        Description = c.String(nullable: false, maxLength: 100),
                        Price = c.Decimal(nullable: false, precision: 18, scale: 2),
                        UploadedByUser_Username = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Users", t => t.UploadedByUser_Username, cascadeDelete: true)
                .Index(t => t.UploadedByUser_Username);
            
            CreateTable(
                "dbo.Pictures",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Image = c.Binary(nullable: false),
                        IsMainPicture = c.Boolean(nullable: false),
                        ProductID_ID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Products", t => t.ProductID_ID, cascadeDelete: true)
                .Index(t => t.ProductID_ID);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        Username = c.String(nullable: false, maxLength: 128),
                        Email = c.String(),
                        Password = c.String(),
                    })
                .PrimaryKey(t => t.Username);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Products", "UploadedByUser_Username", "dbo.Users");
            DropForeignKey("dbo.Pictures", "ProductID_ID", "dbo.Products");
            DropIndex("dbo.Pictures", new[] { "ProductID_ID" });
            DropIndex("dbo.Products", new[] { "UploadedByUser_Username" });
            DropTable("dbo.Users");
            DropTable("dbo.Pictures");
            DropTable("dbo.Products");
        }
    }
}
