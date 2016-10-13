namespace Merp.Web.UI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Accountancy : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Accountancy_IncomingInvoice",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Supplier_OriginalId = c.Guid(nullable: false),
                        Supplier_Name = c.String(),
                        Supplier_StreetName = c.String(),
                        Supplier_PostalCode = c.String(),
                        Supplier_City = c.String(),
                        Supplier_Country = c.String(),
                        Supplier_VatIndex = c.String(),
                        Supplier_NationalIdentificationNumber = c.String(),
                        OriginalId = c.Guid(nullable: false),
                        Number = c.String(),
                        Date = c.DateTime(nullable: false),
                        JobOrderId = c.Guid(),
                        Amount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Taxes = c.Decimal(nullable: false, precision: 18, scale: 2),
                        TotalPrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                        PurchaseOrderNumber = c.String(),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Accountancy_JobOrder",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        OriginalId = c.Guid(nullable: false),
                        CustomerId = c.Guid(nullable: false),
                        CustomerName = c.String(),
                        ManagerId = c.Guid(nullable: false),
                        ManagerName = c.String(),
                        DateOfStart = c.DateTime(nullable: false),
                        DateOfCompletion = c.DateTime(),
                        Name = c.String(),
                        IsCompleted = c.Boolean(nullable: false),
                        IsFixedPrice = c.Boolean(nullable: false),
                        IsTimeAndMaterial = c.Boolean(nullable: false),
                        Description = c.String(),
                        Number = c.String(),
                        PurchaseOrderNumber = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Accountancy_OutgoingInvoice",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Customer_OriginalId = c.Guid(nullable: false),
                        Customer_Name = c.String(),
                        Customer_StreetName = c.String(),
                        Customer_PostalCode = c.String(),
                        Customer_City = c.String(),
                        Customer_Country = c.String(),
                        Customer_VatIndex = c.String(),
                        Customer_NationalIdentificationNumber = c.String(),
                        OriginalId = c.Guid(nullable: false),
                        Number = c.String(),
                        Date = c.DateTime(nullable: false),
                        JobOrderId = c.Guid(),
                        Amount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Taxes = c.Decimal(nullable: false, precision: 18, scale: 2),
                        TotalPrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                        PurchaseOrderNumber = c.String(),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Accountancy_FixedPriceJobOrder",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        Price = c.Decimal(nullable: false, precision: 18, scale: 2),
                        DueDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Accountancy_JobOrder", t => t.Id)
                .Index(t => t.Id);
            
            CreateTable(
                "dbo.Accountancy_TimeAndMaterialJobOrder",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        Value = c.Decimal(precision: 18, scale: 2),
                        DateOfExpiration = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Accountancy_JobOrder", t => t.Id)
                .Index(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Accountancy_TimeAndMaterialJobOrder", "Id", "dbo.Accountancy_JobOrder");
            DropForeignKey("dbo.Accountancy_FixedPriceJobOrder", "Id", "dbo.Accountancy_JobOrder");
            DropIndex("dbo.Accountancy_TimeAndMaterialJobOrder", new[] { "Id" });
            DropIndex("dbo.Accountancy_FixedPriceJobOrder", new[] { "Id" });
            DropTable("dbo.Accountancy_TimeAndMaterialJobOrder");
            DropTable("dbo.Accountancy_FixedPriceJobOrder");
            DropTable("dbo.Accountancy_OutgoingInvoice");
            DropTable("dbo.Accountancy_JobOrder");
            DropTable("dbo.Accountancy_IncomingInvoice");
        }
    }
}
