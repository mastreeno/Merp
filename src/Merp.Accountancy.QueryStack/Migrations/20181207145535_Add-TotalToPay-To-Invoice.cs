using Microsoft.EntityFrameworkCore.Migrations;

namespace Merp.Accountancy.QueryStack.Migrations
{
    public partial class AddTotalToPayToInvoice : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "TotalToPay",
                table: "Invoice",
                nullable: false,
                defaultValue: 0m);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TotalToPay",
                table: "Invoice");
        }
    }
}
