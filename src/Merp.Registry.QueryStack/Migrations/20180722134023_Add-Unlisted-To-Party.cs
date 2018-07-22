using Microsoft.EntityFrameworkCore.Migrations;

namespace Merp.Registry.QueryStack.Migrations
{
    public partial class AddUnlistedToParty : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Unlisted",
                table: "Parties",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Unlisted",
                table: "Parties");
        }
    }
}
