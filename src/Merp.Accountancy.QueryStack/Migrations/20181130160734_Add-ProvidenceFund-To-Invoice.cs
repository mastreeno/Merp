using Microsoft.EntityFrameworkCore.Migrations;

namespace Merp.Accountancy.QueryStack.Migrations
{
    public partial class AddProvidenceFundToInvoice : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "ProvidenceFund_Amount",
                table: "Invoice",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "ProvidenceFund_Description",
                table: "Invoice",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "ProvidenceFund_Rate",
                table: "Invoice",
                nullable: false,
                defaultValue: 0m);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProvidenceFund_Amount",
                table: "Invoice");

            migrationBuilder.DropColumn(
                name: "ProvidenceFund_Description",
                table: "Invoice");

            migrationBuilder.DropColumn(
                name: "ProvidenceFund_Rate",
                table: "Invoice");
        }
    }
}
