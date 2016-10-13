namespace Merp.Registry.QueryStack.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Person_DateOfBirth : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Registry_Person", "DateOfBirth", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Registry_Person", "DateOfBirth");
        }
    }
}
