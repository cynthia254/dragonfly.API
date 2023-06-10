using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PayhouseDragonFly.INFRASTRUCTURE.Migrations
{
    /// <inheritdoc />
    public partial class updatingsalesservice : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PurchaseId",
                table: "PurchaseStatusTable",
                newName: "purchaseId");

            migrationBuilder.AddColumn<DateTime>(
                name: "DateAdded",
                table: "Sales",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "ReasonForSalesStatus",
                table: "Sales",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "SalesStatus",
                table: "Sales",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DateAdded",
                table: "Sales");

            migrationBuilder.DropColumn(
                name: "ReasonForSalesStatus",
                table: "Sales");

            migrationBuilder.DropColumn(
                name: "SalesStatus",
                table: "Sales");

            migrationBuilder.RenameColumn(
                name: "purchaseId",
                table: "PurchaseStatusTable",
                newName: "PurchaseId");
        }
    }
}
