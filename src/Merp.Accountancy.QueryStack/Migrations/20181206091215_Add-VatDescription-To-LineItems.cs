using Microsoft.EntityFrameworkCore.Migrations;

namespace Merp.Accountancy.QueryStack.Migrations
{
    public partial class AddVatDescriptionToLineItems : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "VatDescription",
                table: "InvoiceLineItems",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "VatDescription",
                table: "InvoiceLineItems");
        }
    }
}
