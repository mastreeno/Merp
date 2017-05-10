namespace Merp.Registry.QueryStack.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CompanyMailContactrenametoMainContact : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.Registry_Company", name: "MailContact_Id", newName: "MainContact_Id");
            RenameIndex(table: "dbo.Registry_Company", name: "IX_MailContact_Id", newName: "IX_MainContact_Id");
        }
        
        public override void Down()
        {
            RenameIndex(table: "dbo.Registry_Company", name: "IX_MainContact_Id", newName: "IX_MailContact_Id");
            RenameColumn(table: "dbo.Registry_Company", name: "MainContact_Id", newName: "MailContact_Id");
        }
    }
}
