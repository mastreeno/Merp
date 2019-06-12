using Microsoft.EntityFrameworkCore.Migrations;

namespace Merp.Accountancy.Drafts.Migrations
{
    public partial class AddWithholdingTaxToDrafts : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "TotalToPay",
                table: "InvoiceDraft",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "WithholdingTax_Amount",
                table: "InvoiceDraft",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "WithholdingTax_Description",
                table: "InvoiceDraft",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "WithholdingTax_Rate",
                table: "InvoiceDraft",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "WithholdingTax_TaxableAmountRate",
                table: "InvoiceDraft",
                nullable: false,
                defaultValue: 0m);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TotalToPay",
                table: "InvoiceDraft");

            migrationBuilder.DropColumn(
                name: "WithholdingTax_Amount",
                table: "InvoiceDraft");

            migrationBuilder.DropColumn(
                name: "WithholdingTax_Description",
                table: "InvoiceDraft");

            migrationBuilder.DropColumn(
                name: "WithholdingTax_Rate",
                table: "InvoiceDraft");

            migrationBuilder.DropColumn(
                name: "WithholdingTax_TaxableAmountRate",
                table: "InvoiceDraft");
        }
    }
}
