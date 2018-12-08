using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Merp.Accountancy.Drafts.Migrations
{
    public partial class AddOutgoingInvoiceDraft : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "OutgoingInvoiceDrafts",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Date = table.Column<DateTime>(nullable: true),
                    Currency = table.Column<string>(nullable: true),
                    TaxableAmount = table.Column<decimal>(nullable: false),
                    Taxes = table.Column<decimal>(nullable: false),
                    TotalPrice = table.Column<decimal>(nullable: false),
                    PurchaseOrderNumber = table.Column<string>(nullable: true),
                    PaymentTerms = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    PricesAreVatIncluded = table.Column<bool>(nullable: false),
                    Customer_OriginalId = table.Column<Guid>(nullable: false),
                    Customer_Name = table.Column<string>(maxLength: 100, nullable: true),
                    Customer_StreetName = table.Column<string>(nullable: true),
                    Customer_PostalCode = table.Column<string>(nullable: true),
                    Customer_City = table.Column<string>(nullable: true),
                    Customer_Country = table.Column<string>(nullable: true),
                    Customer_VatIndex = table.Column<string>(nullable: true),
                    Customer_NationalIdentificationNumber = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OutgoingInvoiceDrafts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DraftLineItems",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Code = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    Quantity = table.Column<int>(nullable: false),
                    UnitPrice = table.Column<decimal>(nullable: false),
                    TotalPrice = table.Column<decimal>(nullable: false),
                    Vat = table.Column<decimal>(nullable: false),
                    OutgoingInvoiceDraftId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DraftLineItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DraftLineItems_OutgoingInvoiceDrafts_OutgoingInvoiceDraftId",
                        column: x => x.OutgoingInvoiceDraftId,
                        principalTable: "OutgoingInvoiceDrafts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "DraftNonTaxableItems",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Description = table.Column<string>(nullable: true),
                    Amount = table.Column<decimal>(nullable: false),
                    OutgoingInvoiceDraftId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DraftNonTaxableItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DraftNonTaxableItems_OutgoingInvoiceDrafts_OutgoingInvoiceDraftId",
                        column: x => x.OutgoingInvoiceDraftId,
                        principalTable: "OutgoingInvoiceDrafts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "DraftPricesByVat",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    TaxableAmount = table.Column<decimal>(nullable: false),
                    VatRate = table.Column<decimal>(nullable: false),
                    VatAmount = table.Column<decimal>(nullable: false),
                    TotalPrice = table.Column<decimal>(nullable: false),
                    OutgoingInvoiceDraftId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DraftPricesByVat", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DraftPricesByVat_OutgoingInvoiceDrafts_OutgoingInvoiceDraftId",
                        column: x => x.OutgoingInvoiceDraftId,
                        principalTable: "OutgoingInvoiceDrafts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DraftLineItems_OutgoingInvoiceDraftId",
                table: "DraftLineItems",
                column: "OutgoingInvoiceDraftId");

            migrationBuilder.CreateIndex(
                name: "IX_DraftNonTaxableItems_OutgoingInvoiceDraftId",
                table: "DraftNonTaxableItems",
                column: "OutgoingInvoiceDraftId");

            migrationBuilder.CreateIndex(
                name: "IX_DraftPricesByVat_OutgoingInvoiceDraftId",
                table: "DraftPricesByVat",
                column: "OutgoingInvoiceDraftId");

            migrationBuilder.CreateIndex(
                name: "IX_OutgoingInvoiceDrafts_Date",
                table: "OutgoingInvoiceDrafts",
                column: "Date");

            migrationBuilder.CreateIndex(
                name: "IX_OutgoingInvoiceDrafts_PurchaseOrderNumber",
                table: "OutgoingInvoiceDrafts",
                column: "PurchaseOrderNumber");

            migrationBuilder.CreateIndex(
                name: "IX_OutgoingInvoiceDrafts_Customer_Name",
                table: "OutgoingInvoiceDrafts",
                column: "Customer_Name");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DraftLineItems");

            migrationBuilder.DropTable(
                name: "DraftNonTaxableItems");

            migrationBuilder.DropTable(
                name: "DraftPricesByVat");

            migrationBuilder.DropTable(
                name: "OutgoingInvoiceDrafts");
        }
    }
}
