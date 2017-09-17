namespace Merp.Accountancy.QueryStack.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class BaseSchema : DbMigration
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
                        IsPaid = c.Boolean(nullable: false),
                        PaymentDate = c.DateTime(),
                        IsExpired = c.Boolean(nullable: false),
                        DueDate = c.DateTime(),
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
                        DateOfRegistration = c.DateTime(nullable: false),
                        DateOfStart = c.DateTime(nullable: false),
                        DateOfCompletion = c.DateTime(),
                        Name = c.String(nullable: false),
                        IsCompleted = c.Boolean(nullable: false),
                        IsTimeAndMaterial = c.Boolean(nullable: false),
                        Description = c.String(),
                        Number = c.String(nullable: false),
                        PurchaseOrderNumber = c.String(),
                        Price = c.Decimal(precision: 18, scale: 2),
                        Currency = c.String(nullable: false, maxLength: 3),
                        DueDate = c.DateTime(nullable: false),
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
                        IsPaid = c.Boolean(nullable: false),
                        PaymentDate = c.DateTime(),
                        IsExpired = c.Boolean(nullable: false),
                        DueDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Accountancy_OutgoingInvoice");
            DropTable("dbo.Accountancy_JobOrder");
            DropTable("dbo.Accountancy_IncomingInvoice");
        }
    }
}
