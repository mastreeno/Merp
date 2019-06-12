using Microsoft.EntityFrameworkCore.Migrations;

namespace Merp.Accountancy.Settings.Migrations
{
    public partial class AddAppliedForMinimumTaxpayerRegimeFlag : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "AppliedForMinimumTaxPayer",
                table: "Vats",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AppliedForMinimumTaxPayer",
                table: "Vats");
        }
    }
}
