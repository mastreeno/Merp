using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Merp.Accountancy.Settings.Migrations
{
    public partial class AddVat : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Vats",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Rate = table.Column<decimal>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    Unlisted = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vats", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Vats_Description",
                table: "Vats",
                column: "Description");

            migrationBuilder.CreateIndex(
                name: "IX_Vats_Rate",
                table: "Vats",
                column: "Rate");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Vats");
        }
    }
}
