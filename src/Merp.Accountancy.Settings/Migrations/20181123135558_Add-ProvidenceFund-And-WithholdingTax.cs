using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Merp.Accountancy.Settings.Migrations
{
    public partial class AddProvidenceFundAndWithholdingTax : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ProvidenceFunds",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Country = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    Rate = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProvidenceFunds", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WithholdingTaxes",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Country = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    Rate = table.Column<decimal>(nullable: false),
                    TaxableAmountRate = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WithholdingTaxes", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProvidenceFunds_Description",
                table: "ProvidenceFunds",
                column: "Description");

            migrationBuilder.CreateIndex(
                name: "IX_ProvidenceFunds_Rate",
                table: "ProvidenceFunds",
                column: "Rate");

            migrationBuilder.CreateIndex(
                name: "IX_WithholdingTaxes_Description",
                table: "WithholdingTaxes",
                column: "Description");

            migrationBuilder.CreateIndex(
                name: "IX_WithholdingTaxes_Rate",
                table: "WithholdingTaxes",
                column: "Rate");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProvidenceFunds");

            migrationBuilder.DropTable(
                name: "WithholdingTaxes");
        }
    }
}
