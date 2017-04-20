namespace Merp.Registry.QueryStack.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ShippingAddressConstraintsRemoved : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Registry_Party", "ShippingAddress_Address", c => c.String());
            AlterColumn("dbo.Registry_Party", "ShippingAddress_City", c => c.String());
            AlterColumn("dbo.Registry_Party", "ShippingAddress_Country", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Registry_Party", "ShippingAddress_Country", c => c.String(nullable: false));
            AlterColumn("dbo.Registry_Party", "ShippingAddress_City", c => c.String(nullable: false));
            AlterColumn("dbo.Registry_Party", "ShippingAddress_Address", c => c.String(nullable: false));
        }
    }
}
