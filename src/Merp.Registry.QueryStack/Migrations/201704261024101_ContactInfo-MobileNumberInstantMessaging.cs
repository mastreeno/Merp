namespace Merp.Registry.QueryStack.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ContactInfoMobileNumberInstantMessaging : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Registry_Party", "ContactInfo_MobileNumber", c => c.String());
            AddColumn("dbo.Registry_Party", "ContactInfo_InstantMessaging", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Registry_Party", "ContactInfo_InstantMessaging");
            DropColumn("dbo.Registry_Party", "ContactInfo_MobileNumber");
        }
    }
}
