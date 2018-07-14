using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Merp.ProjectManagement.QueryStack.Migrations
{
    public partial class Base : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Projects",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CustomerId = table.Column<Guid>(nullable: false),
                    ContactPersonId = table.Column<Guid>(nullable: true),
                    ManagerId = table.Column<Guid>(nullable: false),
                    DateOfRegistration = table.Column<DateTime>(nullable: false),
                    DateOfStart = table.Column<DateTime>(nullable: true),
                    DueDate = table.Column<DateTime>(nullable: true),
                    DateOfCompletion = table.Column<DateTime>(nullable: true),
                    IsCompleted = table.Column<bool>(nullable: false),
                    IsTimeAndMaterial = table.Column<bool>(nullable: false),
                    Number = table.Column<string>(nullable: false),
                    CustomerPurchaseOrderNumber = table.Column<string>(nullable: true),
                    Price = table.Column<decimal>(nullable: true),
                    Currency = table.Column<string>(maxLength: 3, nullable: false),
                    Name = table.Column<string>(nullable: false),
                    Description = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Projects", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Projects_CustomerId",
                table: "Projects",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_Projects_IsCompleted",
                table: "Projects",
                column: "IsCompleted");

            migrationBuilder.CreateIndex(
                name: "IX_Projects_ManagerId",
                table: "Projects",
                column: "ManagerId");

            migrationBuilder.CreateIndex(
                name: "IX_Projects_Name",
                table: "Projects",
                column: "Name");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Projects");
        }
    }
}
