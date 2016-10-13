namespace Merp.Registry.QueryStack.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Registry : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Registry_Party",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        OriginalId = c.Guid(nullable: false),
                        DisplayName = c.String(maxLength: 200),
                        VatIndex = c.String(),
                        NationalIdentificationNumber = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.DisplayName);
            
            CreateTable(
                "dbo.Registry_Company",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        CompanyName = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Registry_Party", t => t.Id)
                .Index(t => t.Id);
            
            CreateTable(
                "dbo.Registry_Person",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        FirstName = c.String(),
                        LastName = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Registry_Party", t => t.Id)
                .Index(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Registry_Person", "Id", "dbo.Registry_Party");
            DropForeignKey("dbo.Registry_Company", "Id", "dbo.Registry_Party");
            DropIndex("dbo.Registry_Person", new[] { "Id" });
            DropIndex("dbo.Registry_Company", new[] { "Id" });
            DropIndex("dbo.Registry_Party", new[] { "DisplayName" });
            DropTable("dbo.Registry_Person");
            DropTable("dbo.Registry_Company");
            DropTable("dbo.Registry_Party");
        }
    }
}
