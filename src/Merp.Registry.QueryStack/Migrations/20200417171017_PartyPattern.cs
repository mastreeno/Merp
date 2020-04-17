using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Merp.Registry.QueryStack.Migrations
{
    public partial class PartyPattern : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Companies");

            migrationBuilder.DropTable(
                name: "People");

            migrationBuilder.AddColumn<string>(
                name: "AdministrativeContactName",
                table: "Parties",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "AdministrativeContactUid",
                table: "Parties",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CompanyName",
                table: "Parties",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MainContactName",
                table: "Parties",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "MainContactUid",
                table: "Parties",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FirstName",
                table: "Parties",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastName",
                table: "Parties",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BillingAddress_Address",
                table: "Parties",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BillingAddress_City",
                table: "Parties",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BillingAddress_Country",
                table: "Parties",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BillingAddress_PostalCode",
                table: "Parties",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BillingAddress_Province",
                table: "Parties",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ShippingAddress_Address",
                table: "Parties",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ShippingAddress_City",
                table: "Parties",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ShippingAddress_Country",
                table: "Parties",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ShippingAddress_PostalCode",
                table: "Parties",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ShippingAddress_Province",
                table: "Parties",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AdministrativeContactName",
                table: "Parties");

            migrationBuilder.DropColumn(
                name: "AdministrativeContactUid",
                table: "Parties");

            migrationBuilder.DropColumn(
                name: "CompanyName",
                table: "Parties");

            migrationBuilder.DropColumn(
                name: "MainContactName",
                table: "Parties");

            migrationBuilder.DropColumn(
                name: "MainContactUid",
                table: "Parties");

            migrationBuilder.DropColumn(
                name: "FirstName",
                table: "Parties");

            migrationBuilder.DropColumn(
                name: "LastName",
                table: "Parties");

            migrationBuilder.DropColumn(
                name: "BillingAddress_Address",
                table: "Parties");

            migrationBuilder.DropColumn(
                name: "BillingAddress_City",
                table: "Parties");

            migrationBuilder.DropColumn(
                name: "BillingAddress_Country",
                table: "Parties");

            migrationBuilder.DropColumn(
                name: "BillingAddress_PostalCode",
                table: "Parties");

            migrationBuilder.DropColumn(
                name: "BillingAddress_Province",
                table: "Parties");

            migrationBuilder.DropColumn(
                name: "ShippingAddress_Address",
                table: "Parties");

            migrationBuilder.DropColumn(
                name: "ShippingAddress_City",
                table: "Parties");

            migrationBuilder.DropColumn(
                name: "ShippingAddress_Country",
                table: "Parties");

            migrationBuilder.DropColumn(
                name: "ShippingAddress_PostalCode",
                table: "Parties");

            migrationBuilder.DropColumn(
                name: "ShippingAddress_Province",
                table: "Parties");

            migrationBuilder.CreateTable(
                name: "Companies",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AdministrativeContact = table.Column<string>(nullable: true),
                    CompanyName = table.Column<string>(nullable: false),
                    MainContact = table.Column<string>(nullable: true),
                    NationalIdentificationNumber = table.Column<string>(nullable: true),
                    OriginalId = table.Column<Guid>(nullable: false),
                    VatIndex = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Companies", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "People",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DisplayName = table.Column<string>(maxLength: 200, nullable: true),
                    FirstName = table.Column<string>(nullable: false),
                    LastName = table.Column<string>(nullable: false),
                    NationalIdentificationNumber = table.Column<string>(nullable: true),
                    OriginalId = table.Column<Guid>(nullable: false),
                    VatIndex = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_People", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Companies_OriginalId",
                table: "Companies",
                column: "OriginalId");

            migrationBuilder.CreateIndex(
                name: "IX_People_DisplayName",
                table: "People",
                column: "DisplayName");

            migrationBuilder.CreateIndex(
                name: "IX_People_OriginalId",
                table: "People",
                column: "OriginalId");
        }
    }
}
