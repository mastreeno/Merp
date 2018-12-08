using Microsoft.EntityFrameworkCore.Migrations;

namespace Merp.Accountancy.Drafts.Migrations
{
    public partial class AddCreditNoteDrafts : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DraftLineItems_OutgoingInvoiceDrafts_OutgoingInvoiceDraftId",
                table: "DraftLineItems");

            migrationBuilder.DropForeignKey(
                name: "FK_DraftNonTaxableItems_OutgoingInvoiceDrafts_OutgoingInvoiceDraftId",
                table: "DraftNonTaxableItems");

            migrationBuilder.DropForeignKey(
                name: "FK_DraftPricesByVat_OutgoingInvoiceDrafts_OutgoingInvoiceDraftId",
                table: "DraftPricesByVat");

            migrationBuilder.DropPrimaryKey(
                name: "PK_OutgoingInvoiceDrafts",
                table: "OutgoingInvoiceDrafts");

            migrationBuilder.RenameTable(
                name: "OutgoingInvoiceDrafts",
                newName: "InvoiceDraft");

            migrationBuilder.RenameColumn(
                name: "OutgoingInvoiceDraftId",
                table: "DraftPricesByVat",
                newName: "InvoiceDraftId");

            migrationBuilder.RenameIndex(
                name: "IX_DraftPricesByVat_OutgoingInvoiceDraftId",
                table: "DraftPricesByVat",
                newName: "IX_DraftPricesByVat_InvoiceDraftId");

            migrationBuilder.RenameColumn(
                name: "OutgoingInvoiceDraftId",
                table: "DraftNonTaxableItems",
                newName: "InvoiceDraftId");

            migrationBuilder.RenameIndex(
                name: "IX_DraftNonTaxableItems_OutgoingInvoiceDraftId",
                table: "DraftNonTaxableItems",
                newName: "IX_DraftNonTaxableItems_InvoiceDraftId");

            migrationBuilder.RenameColumn(
                name: "OutgoingInvoiceDraftId",
                table: "DraftLineItems",
                newName: "InvoiceDraftId");

            migrationBuilder.RenameIndex(
                name: "IX_DraftLineItems_OutgoingInvoiceDraftId",
                table: "DraftLineItems",
                newName: "IX_DraftLineItems_InvoiceDraftId");

            migrationBuilder.RenameIndex(
                name: "IX_OutgoingInvoiceDrafts_Customer_Name",
                table: "InvoiceDraft",
                newName: "IX_InvoiceDraft_Customer_Name");

            migrationBuilder.RenameIndex(
                name: "IX_OutgoingInvoiceDrafts_PurchaseOrderNumber",
                table: "InvoiceDraft",
                newName: "IX_InvoiceDraft_PurchaseOrderNumber");

            migrationBuilder.RenameIndex(
                name: "IX_OutgoingInvoiceDrafts_Date",
                table: "InvoiceDraft",
                newName: "IX_InvoiceDraft_Date");

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "InvoiceDraft",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_InvoiceDraft",
                table: "InvoiceDraft",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_DraftLineItems_InvoiceDraft_InvoiceDraftId",
                table: "DraftLineItems",
                column: "InvoiceDraftId",
                principalTable: "InvoiceDraft",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DraftNonTaxableItems_InvoiceDraft_InvoiceDraftId",
                table: "DraftNonTaxableItems",
                column: "InvoiceDraftId",
                principalTable: "InvoiceDraft",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DraftPricesByVat_InvoiceDraft_InvoiceDraftId",
                table: "DraftPricesByVat",
                column: "InvoiceDraftId",
                principalTable: "InvoiceDraft",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DraftLineItems_InvoiceDraft_InvoiceDraftId",
                table: "DraftLineItems");

            migrationBuilder.DropForeignKey(
                name: "FK_DraftNonTaxableItems_InvoiceDraft_InvoiceDraftId",
                table: "DraftNonTaxableItems");

            migrationBuilder.DropForeignKey(
                name: "FK_DraftPricesByVat_InvoiceDraft_InvoiceDraftId",
                table: "DraftPricesByVat");

            migrationBuilder.DropPrimaryKey(
                name: "PK_InvoiceDraft",
                table: "InvoiceDraft");

            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "InvoiceDraft");

            migrationBuilder.RenameTable(
                name: "InvoiceDraft",
                newName: "OutgoingInvoiceDrafts");

            migrationBuilder.RenameColumn(
                name: "InvoiceDraftId",
                table: "DraftPricesByVat",
                newName: "OutgoingInvoiceDraftId");

            migrationBuilder.RenameIndex(
                name: "IX_DraftPricesByVat_InvoiceDraftId",
                table: "DraftPricesByVat",
                newName: "IX_DraftPricesByVat_OutgoingInvoiceDraftId");

            migrationBuilder.RenameColumn(
                name: "InvoiceDraftId",
                table: "DraftNonTaxableItems",
                newName: "OutgoingInvoiceDraftId");

            migrationBuilder.RenameIndex(
                name: "IX_DraftNonTaxableItems_InvoiceDraftId",
                table: "DraftNonTaxableItems",
                newName: "IX_DraftNonTaxableItems_OutgoingInvoiceDraftId");

            migrationBuilder.RenameColumn(
                name: "InvoiceDraftId",
                table: "DraftLineItems",
                newName: "OutgoingInvoiceDraftId");

            migrationBuilder.RenameIndex(
                name: "IX_DraftLineItems_InvoiceDraftId",
                table: "DraftLineItems",
                newName: "IX_DraftLineItems_OutgoingInvoiceDraftId");

            migrationBuilder.RenameIndex(
                name: "IX_InvoiceDraft_Customer_Name",
                table: "OutgoingInvoiceDrafts",
                newName: "IX_OutgoingInvoiceDrafts_Customer_Name");

            migrationBuilder.RenameIndex(
                name: "IX_InvoiceDraft_PurchaseOrderNumber",
                table: "OutgoingInvoiceDrafts",
                newName: "IX_OutgoingInvoiceDrafts_PurchaseOrderNumber");

            migrationBuilder.RenameIndex(
                name: "IX_InvoiceDraft_Date",
                table: "OutgoingInvoiceDrafts",
                newName: "IX_OutgoingInvoiceDrafts_Date");

            migrationBuilder.AddPrimaryKey(
                name: "PK_OutgoingInvoiceDrafts",
                table: "OutgoingInvoiceDrafts",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_DraftLineItems_OutgoingInvoiceDrafts_OutgoingInvoiceDraftId",
                table: "DraftLineItems",
                column: "OutgoingInvoiceDraftId",
                principalTable: "OutgoingInvoiceDrafts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_DraftNonTaxableItems_OutgoingInvoiceDrafts_OutgoingInvoiceDraftId",
                table: "DraftNonTaxableItems",
                column: "OutgoingInvoiceDraftId",
                principalTable: "OutgoingInvoiceDrafts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_DraftPricesByVat_OutgoingInvoiceDrafts_OutgoingInvoiceDraftId",
                table: "DraftPricesByVat",
                column: "OutgoingInvoiceDraftId",
                principalTable: "OutgoingInvoiceDrafts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
