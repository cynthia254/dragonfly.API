using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PayhouseDragonFly.INFRASTRUCTURE.Migrations
{
    /// <inheritdoc />
    public partial class addingapproverssideofthebatch : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ApprovalStatus",
                table: "AddDeliveryNote",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ApprovedBy",
                table: "AddDeliveryNote",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "AprrovedDate",
                table: "AddDeliveryNote",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "RejectedReason",
                table: "AddDeliveryNote",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "selectedOption",
                table: "AddDeliveryNote",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ApprovalStatus",
                table: "AddDeliveryNote");

            migrationBuilder.DropColumn(
                name: "ApprovedBy",
                table: "AddDeliveryNote");

            migrationBuilder.DropColumn(
                name: "AprrovedDate",
                table: "AddDeliveryNote");

            migrationBuilder.DropColumn(
                name: "RejectedReason",
                table: "AddDeliveryNote");

            migrationBuilder.DropColumn(
                name: "selectedOption",
                table: "AddDeliveryNote");
        }
    }
}
