namespace Merp.Web.UI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InvoicePaymentAndExpirationDates : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Accountancy_IncomingInvoice", "IsPaid", c => c.Boolean(nullable: false));
            AddColumn("dbo.Accountancy_IncomingInvoice", "PaymentDate", c => c.DateTime());
            AddColumn("dbo.Accountancy_IncomingInvoice", "IsExpired", c => c.Boolean(nullable: false));
            AddColumn("dbo.Accountancy_IncomingInvoice", "DueDate", c => c.DateTime());
            AddColumn("dbo.Accountancy_OutgoingInvoice", "IsPaid", c => c.Boolean(nullable: false));
            AddColumn("dbo.Accountancy_OutgoingInvoice", "PaymentDate", c => c.DateTime());
            AddColumn("dbo.Accountancy_OutgoingInvoice", "IsExpired", c => c.Boolean(nullable: false));
            AddColumn("dbo.Accountancy_OutgoingInvoice", "DueDate", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Accountancy_OutgoingInvoice", "DueDate");
            DropColumn("dbo.Accountancy_OutgoingInvoice", "IsExpired");
            DropColumn("dbo.Accountancy_OutgoingInvoice", "PaymentDate");
            DropColumn("dbo.Accountancy_OutgoingInvoice", "IsPaid");
            DropColumn("dbo.Accountancy_IncomingInvoice", "DueDate");
            DropColumn("dbo.Accountancy_IncomingInvoice", "IsExpired");
            DropColumn("dbo.Accountancy_IncomingInvoice", "PaymentDate");
            DropColumn("dbo.Accountancy_IncomingInvoice", "IsPaid");
        }
    }
}
