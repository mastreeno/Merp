using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Merp.Registry.QueryStack.Migrations
{
    public partial class InitialMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Companies",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AdministrativeContact = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CompanyName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MainContact = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NationalIdentificationNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OriginalId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    VatIndex = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Companies", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Parties",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    DisplayName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    EmailAddress = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FaxNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    InstantMessaging = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MobileNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NationalIdentificationNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OriginalId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Type = table.Column<int>(type: "int", nullable: false),
                    VatIndex = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    WebsiteAddress = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LegalAddress_Address = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LegalAddress_City = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LegalAddress_Country = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LegalAddress_PostalCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LegalAddress_Province = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Parties", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "People",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    DisplayName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NationalIdentificationNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OriginalId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    VatIndex = table.Column<string>(type: "nvarchar(max)", nullable: true)
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
                name: "IX_Parties_DisplayName",
                table: "Parties",
                column: "DisplayName");

            migrationBuilder.CreateIndex(
                name: "IX_Parties_OriginalId",
                table: "Parties",
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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Companies");

            migrationBuilder.DropTable(
                name: "Parties");

            migrationBuilder.DropTable(
                name: "People");
        }
    }
}
