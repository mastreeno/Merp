namespace Merp.Registry.QueryStack.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PartyLegalAddress : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Registry_Party", "LegalAddress_Address", c => c.String());
            AddColumn("dbo.Registry_Party", "LegalAddress_City", c => c.String());
            AddColumn("dbo.Registry_Party", "LegalAddress_PostalCode", c => c.String());
            AddColumn("dbo.Registry_Party", "LegalAddress_Province", c => c.String());
            AddColumn("dbo.Registry_Party", "LegalAddress_Country", c => c.String());
            AlterColumn("dbo.Registry_Party", "NationalIdentificationNumber", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Registry_Party", "NationalIdentificationNumber", c => c.String(nullable: false));
            DropColumn("dbo.Registry_Party", "LegalAddress_Country");
            DropColumn("dbo.Registry_Party", "LegalAddress_Province");
            DropColumn("dbo.Registry_Party", "LegalAddress_PostalCode");
            DropColumn("dbo.Registry_Party", "LegalAddress_City");
            DropColumn("dbo.Registry_Party", "LegalAddress_Address");
        }
    }
}
