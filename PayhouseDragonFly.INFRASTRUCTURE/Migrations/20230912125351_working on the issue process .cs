using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PayhouseDragonFly.INFRASTRUCTURE.Migrations
{
    /// <inheritdoc />
    public partial class workingontheissueprocess : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Quantity",
                table: "SelectSerial",
                newName: "QuantityOrdered");

            migrationBuilder.AddColumn<int>(
                name: "OutStandingBalance",
                table: "SelectSerial",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "QuantityDispatched",
                table: "SelectSerial",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Reason",
                table: "SelectSerial",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ClientName",
                table: "OrderNo",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OutStandingBalance",
                table: "SelectSerial");

            migrationBuilder.DropColumn(
                name: "QuantityDispatched",
                table: "SelectSerial");

            migrationBuilder.DropColumn(
                name: "Reason",
                table: "SelectSerial");

            migrationBuilder.DropColumn(
                name: "ClientName",
                table: "OrderNo");

            migrationBuilder.RenameColumn(
                name: "QuantityOrdered",
                table: "SelectSerial",
                newName: "Quantity");
        }
    }
}
