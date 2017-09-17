namespace Merp.Registry.QueryStack.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DefaultSchema : DbMigration
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
                        LegalAddress_Address = c.String(),
                        LegalAddress_City = c.String(),
                        LegalAddress_PostalCode = c.String(),
                        LegalAddress_Province = c.String(),
                        LegalAddress_Country = c.String(),
                        ShippingAddress_Address = c.String(),
                        ShippingAddress_City = c.String(),
                        ShippingAddress_PostalCode = c.String(),
                        ShippingAddress_Province = c.String(),
                        ShippingAddress_Country = c.String(),
                        BillingAddress_Address = c.String(),
                        BillingAddress_City = c.String(),
                        BillingAddress_PostalCode = c.String(),
                        BillingAddress_Province = c.String(),
                        BillingAddress_Country = c.String(),
                        ContactInfo_PhoneNumber = c.String(),
                        ContactInfo_MobileNumber = c.String(),
                        ContactInfo_FaxNumber = c.String(),
                        ContactInfo_WebsiteAddress = c.String(),
                        ContactInfo_EmailAddress = c.String(),
                        ContactInfo_InstantMessaging = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.OriginalId)
                .Index(t => t.DisplayName);
            
            CreateTable(
                "dbo.Registry_Company",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        AdministrativeContact_Id = c.Int(),
                        MainContact_Id = c.Int(),
                        CompanyName = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Registry_Party", t => t.Id)
                .ForeignKey("dbo.Registry_Person", t => t.AdministrativeContact_Id)
                .ForeignKey("dbo.Registry_Person", t => t.MainContact_Id)
                .Index(t => t.Id)
                .Index(t => t.AdministrativeContact_Id)
                .Index(t => t.MainContact_Id);
            
            CreateTable(
                "dbo.Registry_Person",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        FirstName = c.String(nullable: false),
                        LastName = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Registry_Party", t => t.Id)
                .Index(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Registry_Person", "Id", "dbo.Registry_Party");
            DropForeignKey("dbo.Registry_Company", "MainContact_Id", "dbo.Registry_Person");
            DropForeignKey("dbo.Registry_Company", "AdministrativeContact_Id", "dbo.Registry_Person");
            DropForeignKey("dbo.Registry_Company", "Id", "dbo.Registry_Party");
            DropIndex("dbo.Registry_Person", new[] { "Id" });
            DropIndex("dbo.Registry_Company", new[] { "MainContact_Id" });
            DropIndex("dbo.Registry_Company", new[] { "AdministrativeContact_Id" });
            DropIndex("dbo.Registry_Company", new[] { "Id" });
            DropIndex("dbo.Registry_Party", new[] { "DisplayName" });
            DropIndex("dbo.Registry_Party", new[] { "OriginalId" });
            DropTable("dbo.Registry_Person");
            DropTable("dbo.Registry_Company");
            DropTable("dbo.Registry_Party");
        }
    }
}
