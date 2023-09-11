using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PayhouseDragonFly.INFRASTRUCTURE.Migrations
{
    /// <inheritdoc />
    public partial class addingtheDETAILSTOISSUEANDSELECTSERIALtoproductdetails : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "BrandName",
                table: "SelectSerial",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "IMEI2",
                table: "SelectSerial",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "IMEII1",
                table: "SelectSerial",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ItemName",
                table: "SelectSerial",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Requisitioner",
                table: "SelectSerial",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "StockNeed",
                table: "SelectSerial",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "clientName",
                table: "SelectSerial",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "BrandName",
                table: "IssueProcess",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ClientName",
                table: "IssueProcess",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Requisitiioner",
                table: "IssueProcess",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "StockNeed",
                table: "IssueProcess",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BrandName",
                table: "SelectSerial");

            migrationBuilder.DropColumn(
                name: "IMEI2",
                table: "SelectSerial");

            migrationBuilder.DropColumn(
                name: "IMEII1",
                table: "SelectSerial");

            migrationBuilder.DropColumn(
                name: "ItemName",
                table: "SelectSerial");

            migrationBuilder.DropColumn(
                name: "Requisitioner",
                table: "SelectSerial");

            migrationBuilder.DropColumn(
                name: "StockNeed",
                table: "SelectSerial");

            migrationBuilder.DropColumn(
                name: "clientName",
                table: "SelectSerial");

            migrationBuilder.DropColumn(
                name: "BrandName",
                table: "IssueProcess");

            migrationBuilder.DropColumn(
                name: "ClientName",
                table: "IssueProcess");

            migrationBuilder.DropColumn(
                name: "Requisitiioner",
                table: "IssueProcess");

            migrationBuilder.DropColumn(
                name: "StockNeed",
                table: "IssueProcess");
        }
    }
}
