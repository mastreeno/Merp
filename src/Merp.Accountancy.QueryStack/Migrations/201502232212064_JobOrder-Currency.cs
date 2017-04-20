namespace Merp.Accountancy.QueryStack.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class JobOrderCurrency : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Accountancy_FixedPriceJobOrder", "Currency", c => c.String(nullable: false, maxLength: 3));
            AddColumn("dbo.Accountancy_TimeAndMaterialJobOrder", "Currency", c => c.String(nullable: false, maxLength: 3));
            AlterColumn("dbo.Accountancy_JobOrder", "CustomerName", c => c.String(nullable: false));
            AlterColumn("dbo.Accountancy_JobOrder", "ManagerName", c => c.String(nullable: false));
            AlterColumn("dbo.Accountancy_JobOrder", "Name", c => c.String(nullable: false));
            AlterColumn("dbo.Accountancy_JobOrder", "Number", c => c.String(nullable: false));
            AlterColumn("dbo.Accountancy_TimeAndMaterialJobOrder", "Value", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Accountancy_TimeAndMaterialJobOrder", "Value", c => c.Decimal(precision: 18, scale: 2));
            AlterColumn("dbo.Accountancy_JobOrder", "Number", c => c.String());
            AlterColumn("dbo.Accountancy_JobOrder", "Name", c => c.String());
            AlterColumn("dbo.Accountancy_JobOrder", "ManagerName", c => c.String());
            AlterColumn("dbo.Accountancy_JobOrder", "CustomerName", c => c.String());
            DropColumn("dbo.Accountancy_TimeAndMaterialJobOrder", "Currency");
            DropColumn("dbo.Accountancy_FixedPriceJobOrder", "Currency");
        }
    }
}
