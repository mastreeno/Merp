namespace Merp.Registry.QueryStack.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CompanyAdministrativeContact : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Registry_Company", "AdministrativeContact_Id", c => c.Int());
            CreateIndex("dbo.Registry_Company", "AdministrativeContact_Id");
            AddForeignKey("dbo.Registry_Company", "AdministrativeContact_Id", "dbo.Registry_Person", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Registry_Company", "AdministrativeContact_Id", "dbo.Registry_Person");
            DropIndex("dbo.Registry_Company", new[] { "AdministrativeContact_Id" });
            DropColumn("dbo.Registry_Company", "AdministrativeContact_Id");
        }
    }
}
