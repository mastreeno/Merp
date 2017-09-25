namespace Merp.Accountancy.QueryStack.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Invoices : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Accountancy_IncomingInvoice", "TaxableAmount", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.Accountancy_IncomingInvoice", "IsOverdue", c => c.Boolean(nullable: false));
            AddColumn("dbo.Accountancy_IncomingInvoice", "Customer_OriginalId", c => c.Guid(nullable: false));
            AddColumn("dbo.Accountancy_IncomingInvoice", "Customer_Name", c => c.String(maxLength: 100));
            AddColumn("dbo.Accountancy_IncomingInvoice", "Customer_StreetName", c => c.String());
            AddColumn("dbo.Accountancy_IncomingInvoice", "Customer_PostalCode", c => c.String());
            AddColumn("dbo.Accountancy_IncomingInvoice", "Customer_City", c => c.String());
            AddColumn("dbo.Accountancy_IncomingInvoice", "Customer_Country", c => c.String());
            AddColumn("dbo.Accountancy_IncomingInvoice", "Customer_VatIndex", c => c.String());
            AddColumn("dbo.Accountancy_IncomingInvoice", "Customer_NationalIdentificationNumber", c => c.String());
            AddColumn("dbo.Accountancy_OutgoingInvoice", "TaxableAmount", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.Accountancy_OutgoingInvoice", "IsOverdue", c => c.Boolean(nullable: false));
            AddColumn("dbo.Accountancy_OutgoingInvoice", "Supplier_OriginalId", c => c.Guid(nullable: false));
            AddColumn("dbo.Accountancy_OutgoingInvoice", "Supplier_Name", c => c.String(maxLength: 100));
            AddColumn("dbo.Accountancy_OutgoingInvoice", "Supplier_StreetName", c => c.String());
            AddColumn("dbo.Accountancy_OutgoingInvoice", "Supplier_PostalCode", c => c.String());
            AddColumn("dbo.Accountancy_OutgoingInvoice", "Supplier_City", c => c.String());
            AddColumn("dbo.Accountancy_OutgoingInvoice", "Supplier_Country", c => c.String());
            AddColumn("dbo.Accountancy_OutgoingInvoice", "Supplier_VatIndex", c => c.String());
            AddColumn("dbo.Accountancy_OutgoingInvoice", "Supplier_NationalIdentificationNumber", c => c.String());
            AlterColumn("dbo.Accountancy_IncomingInvoice", "Supplier_Name", c => c.String(maxLength: 100));
            AlterColumn("dbo.Accountancy_OutgoingInvoice", "Customer_Name", c => c.String(maxLength: 100));
            CreateIndex("dbo.Accountancy_IncomingInvoice", "Date");
            CreateIndex("dbo.Accountancy_IncomingInvoice", "DueDate");
            CreateIndex("dbo.Accountancy_IncomingInvoice", "JobOrderId");
            CreateIndex("dbo.Accountancy_IncomingInvoice", "IsPaid");
            CreateIndex("dbo.Accountancy_IncomingInvoice", "IsOverdue");
            CreateIndex("dbo.Accountancy_IncomingInvoice", "Supplier_Name");
            CreateIndex("dbo.Accountancy_IncomingInvoice", "Customer_Name");
            CreateIndex("dbo.Accountancy_OutgoingInvoice", "Date");
            CreateIndex("dbo.Accountancy_OutgoingInvoice", "DueDate");
            CreateIndex("dbo.Accountancy_OutgoingInvoice", "JobOrderId");
            CreateIndex("dbo.Accountancy_OutgoingInvoice", "IsPaid");
            CreateIndex("dbo.Accountancy_OutgoingInvoice", "IsOverdue");
            CreateIndex("dbo.Accountancy_OutgoingInvoice", "Supplier_Name");
            CreateIndex("dbo.Accountancy_OutgoingInvoice", "Customer_Name");
            DropColumn("dbo.Accountancy_IncomingInvoice", "Amount");
            DropColumn("dbo.Accountancy_IncomingInvoice", "IsExpired");
            DropColumn("dbo.Accountancy_OutgoingInvoice", "Amount");
            DropColumn("dbo.Accountancy_OutgoingInvoice", "IsExpired");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Accountancy_OutgoingInvoice", "IsExpired", c => c.Boolean(nullable: false));
            AddColumn("dbo.Accountancy_OutgoingInvoice", "Amount", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.Accountancy_IncomingInvoice", "IsExpired", c => c.Boolean(nullable: false));
            AddColumn("dbo.Accountancy_IncomingInvoice", "Amount", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            DropIndex("dbo.Accountancy_OutgoingInvoice", new[] { "Customer_Name" });
            DropIndex("dbo.Accountancy_OutgoingInvoice", new[] { "Supplier_Name" });
            DropIndex("dbo.Accountancy_OutgoingInvoice", new[] { "IsOverdue" });
            DropIndex("dbo.Accountancy_OutgoingInvoice", new[] { "IsPaid" });
            DropIndex("dbo.Accountancy_OutgoingInvoice", new[] { "JobOrderId" });
            DropIndex("dbo.Accountancy_OutgoingInvoice", new[] { "DueDate" });
            DropIndex("dbo.Accountancy_OutgoingInvoice", new[] { "Date" });
            DropIndex("dbo.Accountancy_IncomingInvoice", new[] { "Customer_Name" });
            DropIndex("dbo.Accountancy_IncomingInvoice", new[] { "Supplier_Name" });
            DropIndex("dbo.Accountancy_IncomingInvoice", new[] { "IsOverdue" });
            DropIndex("dbo.Accountancy_IncomingInvoice", new[] { "IsPaid" });
            DropIndex("dbo.Accountancy_IncomingInvoice", new[] { "JobOrderId" });
            DropIndex("dbo.Accountancy_IncomingInvoice", new[] { "DueDate" });
            DropIndex("dbo.Accountancy_IncomingInvoice", new[] { "Date" });
            AlterColumn("dbo.Accountancy_OutgoingInvoice", "Customer_Name", c => c.String());
            AlterColumn("dbo.Accountancy_IncomingInvoice", "Supplier_Name", c => c.String());
            DropColumn("dbo.Accountancy_OutgoingInvoice", "Supplier_NationalIdentificationNumber");
            DropColumn("dbo.Accountancy_OutgoingInvoice", "Supplier_VatIndex");
            DropColumn("dbo.Accountancy_OutgoingInvoice", "Supplier_Country");
            DropColumn("dbo.Accountancy_OutgoingInvoice", "Supplier_City");
            DropColumn("dbo.Accountancy_OutgoingInvoice", "Supplier_PostalCode");
            DropColumn("dbo.Accountancy_OutgoingInvoice", "Supplier_StreetName");
            DropColumn("dbo.Accountancy_OutgoingInvoice", "Supplier_Name");
            DropColumn("dbo.Accountancy_OutgoingInvoice", "Supplier_OriginalId");
            DropColumn("dbo.Accountancy_OutgoingInvoice", "IsOverdue");
            DropColumn("dbo.Accountancy_OutgoingInvoice", "TaxableAmount");
            DropColumn("dbo.Accountancy_IncomingInvoice", "Customer_NationalIdentificationNumber");
            DropColumn("dbo.Accountancy_IncomingInvoice", "Customer_VatIndex");
            DropColumn("dbo.Accountancy_IncomingInvoice", "Customer_Country");
            DropColumn("dbo.Accountancy_IncomingInvoice", "Customer_City");
            DropColumn("dbo.Accountancy_IncomingInvoice", "Customer_PostalCode");
            DropColumn("dbo.Accountancy_IncomingInvoice", "Customer_StreetName");
            DropColumn("dbo.Accountancy_IncomingInvoice", "Customer_Name");
            DropColumn("dbo.Accountancy_IncomingInvoice", "Customer_OriginalId");
            DropColumn("dbo.Accountancy_IncomingInvoice", "IsOverdue");
            DropColumn("dbo.Accountancy_IncomingInvoice", "TaxableAmount");
        }
    }
}
