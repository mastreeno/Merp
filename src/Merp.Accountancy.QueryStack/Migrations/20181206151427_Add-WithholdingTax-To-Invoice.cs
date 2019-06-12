using Microsoft.EntityFrameworkCore.Migrations;

namespace Merp.Accountancy.QueryStack.Migrations
{
    public partial class AddWithholdingTaxToInvoice : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "WithholdingTax_Amount",
                table: "Invoice",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "WithholdingTax_Description",
                table: "Invoice",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "WithholdingTax_Rate",
                table: "Invoice",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "WithholdingTax_TaxableAmountRate",
                table: "Invoice",
                nullable: false,
                defaultValue: 0m);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "WithholdingTax_Amount",
                table: "Invoice");

            migrationBuilder.DropColumn(
                name: "WithholdingTax_Description",
                table: "Invoice");

            migrationBuilder.DropColumn(
                name: "WithholdingTax_Rate",
                table: "Invoice");

            migrationBuilder.DropColumn(
                name: "WithholdingTax_TaxableAmountRate",
                table: "Invoice");
        }
    }
}
