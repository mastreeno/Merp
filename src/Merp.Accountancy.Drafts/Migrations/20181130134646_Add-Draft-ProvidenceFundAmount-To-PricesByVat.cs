using Microsoft.EntityFrameworkCore.Migrations;

namespace Merp.Accountancy.Drafts.Migrations
{
    public partial class AddDraftProvidenceFundAmountToPricesByVat : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "ProvidenceFundAmount",
                table: "DraftPricesByVat",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProvidenceFundAmount",
                table: "DraftPricesByVat");
        }
    }
}
