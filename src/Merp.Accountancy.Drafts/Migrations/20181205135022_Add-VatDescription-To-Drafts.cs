using Microsoft.EntityFrameworkCore.Migrations;

namespace Merp.Accountancy.Drafts.Migrations
{
    public partial class AddVatDescriptionToDrafts : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "VatDescription",
                table: "DraftLineItems",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "VatDescription",
                table: "DraftLineItems");
        }
    }
}
