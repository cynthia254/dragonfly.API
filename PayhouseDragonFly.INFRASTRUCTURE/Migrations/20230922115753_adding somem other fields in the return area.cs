using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PayhouseDragonFly.INFRASTRUCTURE.Migrations
{
    /// <inheritdoc />
    public partial class addingsomemotherfieldsinthereturnarea : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FaultyDescription",
                table: "ReturnedItem",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "FaultyQuantity",
                table: "ReturnedItem",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "SerialFaulty",
                table: "ReturnedItem",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FaultyDescription",
                table: "ReturnedItem");

            migrationBuilder.DropColumn(
                name: "FaultyQuantity",
                table: "ReturnedItem");

            migrationBuilder.DropColumn(
                name: "SerialFaulty",
                table: "ReturnedItem");
        }
    }
}
