using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Merp.Accountancy.QueryStack.Migrations
{
    public partial class InitialMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Invoice",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Discriminator = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DueDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsOverdue = table.Column<bool>(type: "bit", nullable: false),
                    IsPaid = table.Column<bool>(type: "bit", nullable: false),
                    JobOrderId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Number = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OriginalId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PaymentDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PurchaseOrderNumber = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    TaxableAmount = table.Column<decimal>(type: "decimal(18, 2)", nullable: false),
                    Taxes = table.Column<decimal>(type: "decimal(18, 2)", nullable: false),
                    TotalPrice = table.Column<decimal>(type: "decimal(18, 2)", nullable: false),
                    Customer_City = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Customer_Country = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Customer_Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Customer_NationalIdentificationNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Customer_OriginalId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Customer_PostalCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Customer_StreetName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Customer_VatIndex = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Supplier_City = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Supplier_Country = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Supplier_Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Supplier_NationalIdentificationNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Supplier_OriginalId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Supplier_PostalCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Supplier_StreetName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Supplier_VatIndex = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Invoice", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "JobOrders",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Currency = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: false),
                    CustomerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CustomerName = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    DateOfCompletion = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DateOfRegistration = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateOfStart = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DueDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsCompleted = table.Column<bool>(type: "bit", nullable: false),
                    IsTimeAndMaterial = table.Column<bool>(type: "bit", nullable: false),
                    ManagerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ManagerName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Number = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OriginalId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18, 2)", nullable: true),
                    PurchaseOrderNumber = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JobOrders", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Invoice_Date",
                table: "Invoice",
                column: "Date");

            migrationBuilder.CreateIndex(
                name: "IX_Invoice_DueDate",
                table: "Invoice",
                column: "DueDate");

            migrationBuilder.CreateIndex(
                name: "IX_Invoice_IsOverdue",
                table: "Invoice",
                column: "IsOverdue");

            migrationBuilder.CreateIndex(
                name: "IX_Invoice_IsPaid",
                table: "Invoice",
                column: "IsPaid");

            migrationBuilder.CreateIndex(
                name: "IX_Invoice_JobOrderId",
                table: "Invoice",
                column: "JobOrderId");

            migrationBuilder.CreateIndex(
                name: "IX_Invoice_PurchaseOrderNumber",
                table: "Invoice",
                column: "PurchaseOrderNumber");

            migrationBuilder.CreateIndex(
                name: "IX_Invoice_Customer_Name",
                table: "Invoice",
                column: "Customer_Name");

            migrationBuilder.CreateIndex(
                name: "IX_Invoice_Supplier_Name",
                table: "Invoice",
                column: "Supplier_Name");

            migrationBuilder.CreateIndex(
                name: "IX_JobOrders_CustomerName",
                table: "JobOrders",
                column: "CustomerName");

            migrationBuilder.CreateIndex(
                name: "IX_JobOrders_IsCompleted",
                table: "JobOrders",
                column: "IsCompleted");

            migrationBuilder.CreateIndex(
                name: "IX_JobOrders_Name",
                table: "JobOrders",
                column: "Name");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Invoice");

            migrationBuilder.DropTable(
                name: "JobOrders");
        }
    }
}
