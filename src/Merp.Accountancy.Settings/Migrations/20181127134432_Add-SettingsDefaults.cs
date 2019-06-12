using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Merp.Accountancy.Settings.Migrations
{
    public partial class AddSettingsDefaults : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SettingsDefaults",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    SubscriptionId = table.Column<Guid>(nullable: false),
                    MinimumTaxPayerRegime = table.Column<bool>(nullable: false),
                    ElectronicInvoiceEnabled = table.Column<bool>(nullable: false),
                    SplitPaymentApplied = table.Column<bool>(nullable: false),
                    VatId = table.Column<Guid>(nullable: true),
                    WithholdingTaxId = table.Column<Guid>(nullable: true),
                    ProvidenceFundId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SettingsDefaults", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SettingsDefaults_ProvidenceFunds_ProvidenceFundId",
                        column: x => x.ProvidenceFundId,
                        principalTable: "ProvidenceFunds",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SettingsDefaults_Vats_VatId",
                        column: x => x.VatId,
                        principalTable: "Vats",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SettingsDefaults_WithholdingTaxes_WithholdingTaxId",
                        column: x => x.WithholdingTaxId,
                        principalTable: "WithholdingTaxes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SettingsDefaults_ProvidenceFundId",
                table: "SettingsDefaults",
                column: "ProvidenceFundId");

            migrationBuilder.CreateIndex(
                name: "IX_SettingsDefaults_SubscriptionId",
                table: "SettingsDefaults",
                column: "SubscriptionId");

            migrationBuilder.CreateIndex(
                name: "IX_SettingsDefaults_VatId",
                table: "SettingsDefaults",
                column: "VatId");

            migrationBuilder.CreateIndex(
                name: "IX_SettingsDefaults_WithholdingTaxId",
                table: "SettingsDefaults",
                column: "WithholdingTaxId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SettingsDefaults");
        }
    }
}
