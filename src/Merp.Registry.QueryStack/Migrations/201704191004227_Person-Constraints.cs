namespace Merp.Registry.QueryStack.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PersonConstraints : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Registry_Person", "FirstName", c => c.String(nullable: false));
            AlterColumn("dbo.Registry_Person", "LastName", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Registry_Person", "LastName", c => c.String());
            AlterColumn("dbo.Registry_Person", "FirstName", c => c.String());
        }
    }
}
