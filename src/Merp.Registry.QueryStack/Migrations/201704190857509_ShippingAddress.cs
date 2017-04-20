namespace Merp.Registry.QueryStack.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ShippingAddress : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Registry_Party", "ShippingAddress_Address", c => c.String(nullable: false));
            AddColumn("dbo.Registry_Party", "ShippingAddress_City", c => c.String(nullable: false));
            AddColumn("dbo.Registry_Party", "ShippingAddress_PostalCode", c => c.String());
            AddColumn("dbo.Registry_Party", "ShippingAddress_Province", c => c.String());
            AddColumn("dbo.Registry_Party", "ShippingAddress_Country", c => c.String(nullable: false));
            AlterColumn("dbo.Registry_Party", "NationalIdentificationNumber", c => c.String(nullable: false));
            CreateIndex("dbo.Registry_Party", "OriginalId");
            DropColumn("dbo.Registry_Person", "DateOfBirth");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Registry_Person", "DateOfBirth", c => c.DateTime());
            DropIndex("dbo.Registry_Party", new[] { "OriginalId" });
            AlterColumn("dbo.Registry_Party", "NationalIdentificationNumber", c => c.String());
            DropColumn("dbo.Registry_Party", "ShippingAddress_Country");
            DropColumn("dbo.Registry_Party", "ShippingAddress_Province");
            DropColumn("dbo.Registry_Party", "ShippingAddress_PostalCode");
            DropColumn("dbo.Registry_Party", "ShippingAddress_City");
            DropColumn("dbo.Registry_Party", "ShippingAddress_Address");
        }
    }
}
