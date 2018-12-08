using Microsoft.EntityFrameworkCore.Migrations;

namespace Merp.Accountancy.QueryStack.Migrations
{
    public partial class RemoveVatIncludedFromInvoiceLineItems : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "VatIncluded",
                table: "InvoiceLineItems");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "VatIncluded",
                table: "InvoiceLineItems",
                nullable: false,
                defaultValue: false);
        }
    }
}
