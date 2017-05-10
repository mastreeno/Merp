namespace Merp.Registry.QueryStack.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CompanyMailContact : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Registry_Company", "MailContact_Id", c => c.Int());
            CreateIndex("dbo.Registry_Company", "MailContact_Id");
            AddForeignKey("dbo.Registry_Company", "MailContact_Id", "dbo.Registry_Person", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Registry_Company", "MailContact_Id", "dbo.Registry_Person");
            DropIndex("dbo.Registry_Company", new[] { "MailContact_Id" });
            DropColumn("dbo.Registry_Company", "MailContact_Id");
        }
    }
}
