using Microsoft.EntityFrameworkCore.Migrations;

namespace Merp.Accountancy.Drafts.Migrations
{
    public partial class AddDraftProvidenceFund : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "ProvidenceFund_Amount",
                table: "InvoiceDraft",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProvidenceFund_Description",
                table: "InvoiceDraft",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "ProvidenceFund_Rate",
                table: "InvoiceDraft",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProvidenceFund_Amount",
                table: "InvoiceDraft");

            migrationBuilder.DropColumn(
                name: "ProvidenceFund_Description",
                table: "InvoiceDraft");

            migrationBuilder.DropColumn(
                name: "ProvidenceFund_Rate",
                table: "InvoiceDraft");
        }
    }
}
