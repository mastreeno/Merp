using Microsoft.EntityFrameworkCore.Migrations;

namespace Merp.Accountancy.Settings.Migrations
{
    public partial class AddCountryToVat : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Country",
                table: "Vats",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Country",
                table: "Vats");
        }
    }
}
