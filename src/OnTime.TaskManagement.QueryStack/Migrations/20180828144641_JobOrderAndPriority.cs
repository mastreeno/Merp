using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace OnTime.TaskManagement.QueryStack.Migrations
{
    public partial class JobOrderAndPriority : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "JobOrderId",
                table: "Tasks",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Priority",
                table: "Tasks",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "JobOrderId",
                table: "Tasks");

            migrationBuilder.DropColumn(
                name: "Priority",
                table: "Tasks");
        }
    }
}
