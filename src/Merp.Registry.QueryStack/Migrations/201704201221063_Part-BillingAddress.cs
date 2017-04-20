namespace Merp.Registry.QueryStack.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PartBillingAddress : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Registry_Party", "BillingAddress_Address", c => c.String());
            AddColumn("dbo.Registry_Party", "BillingAddress_City", c => c.String());
            AddColumn("dbo.Registry_Party", "BillingAddress_PostalCode", c => c.String());
            AddColumn("dbo.Registry_Party", "BillingAddress_Province", c => c.String());
            AddColumn("dbo.Registry_Party", "BillingAddress_Country", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Registry_Party", "BillingAddress_Country");
            DropColumn("dbo.Registry_Party", "BillingAddress_Province");
            DropColumn("dbo.Registry_Party", "BillingAddress_PostalCode");
            DropColumn("dbo.Registry_Party", "BillingAddress_City");
            DropColumn("dbo.Registry_Party", "BillingAddress_Address");
        }
    }
}
