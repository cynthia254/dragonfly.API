using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PayhouseDragonFly.INFRASTRUCTURE.Migrations
{
    /// <inheritdoc />
    public partial class modifyingregistrationmodel2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ClosedDate",
                table: "Tickets");

            migrationBuilder.DropColumn(
                name: "DateUpdated",
                table: "Tickets");

            migrationBuilder.DropColumn(
                name: "DueDate",
                table: "Tickets");

            migrationBuilder.RenameColumn(
                name: "TicketType",
                table: "Tickets",
                newName: "subject");

            migrationBuilder.RenameColumn(
                name: "TicketDescriptiom",
                table: "Tickets",
                newName: "siteArea");

            migrationBuilder.RenameColumn(
                name: "Priority",
                table: "Tickets",
                newName: "priorityName");

            migrationBuilder.RenameColumn(
                name: "CreatedBy",
                table: "Tickets",
                newName: "itemName");

            migrationBuilder.RenameColumn(
                name: "AssignedTo",
                table: "Tickets",
                newName: "description");

            migrationBuilder.RenameColumn(
                name: "AssignedFrom",
                table: "Tickets",
                newName: "clientLocation");

            migrationBuilder.AddColumn<string>(
                name: "CreatorEmail",
                table: "Tickets",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ServiceIssue",
                table: "Tickets",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ServiceName",
                table: "Tickets",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatorEmail",
                table: "Tickets");

            migrationBuilder.DropColumn(
                name: "ServiceIssue",
                table: "Tickets");

            migrationBuilder.DropColumn(
                name: "ServiceName",
                table: "Tickets");

            migrationBuilder.RenameColumn(
                name: "subject",
                table: "Tickets",
                newName: "TicketType");

            migrationBuilder.RenameColumn(
                name: "siteArea",
                table: "Tickets",
                newName: "TicketDescriptiom");

            migrationBuilder.RenameColumn(
                name: "priorityName",
                table: "Tickets",
                newName: "Priority");

            migrationBuilder.RenameColumn(
                name: "itemName",
                table: "Tickets",
                newName: "CreatedBy");

            migrationBuilder.RenameColumn(
                name: "description",
                table: "Tickets",
                newName: "AssignedTo");

            migrationBuilder.RenameColumn(
                name: "clientLocation",
                table: "Tickets",
                newName: "AssignedFrom");

            migrationBuilder.AddColumn<DateTime>(
                name: "ClosedDate",
                table: "Tickets",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "DateUpdated",
                table: "Tickets",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "DueDate",
                table: "Tickets",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }
    }
}
