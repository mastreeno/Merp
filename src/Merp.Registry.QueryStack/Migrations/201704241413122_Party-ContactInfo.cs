namespace Merp.Registry.QueryStack.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PartyContactInfo : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Registry_Party", "ContactInfo_PhoneNumber", c => c.String());
            AddColumn("dbo.Registry_Party", "ContactInfo_FaxNumber", c => c.String());
            AddColumn("dbo.Registry_Party", "ContactInfo_WebsiteAddress", c => c.String());
            AddColumn("dbo.Registry_Party", "ContactInfo_EmailAddress", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Registry_Party", "ContactInfo_EmailAddress");
            DropColumn("dbo.Registry_Party", "ContactInfo_WebsiteAddress");
            DropColumn("dbo.Registry_Party", "ContactInfo_FaxNumber");
            DropColumn("dbo.Registry_Party", "ContactInfo_PhoneNumber");
        }
    }
}
