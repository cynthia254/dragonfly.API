using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PayhouseDragonFly.INFRASTRUCTURE.Migrations
{
    /// <inheritdoc />
    public partial class modifyingregistrationmodel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AdditionalInformation",
                table: "PayhouseDragonFlyUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "BusinessUnit",
                table: "PayhouseDragonFlyUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Position",
                table: "PayhouseDragonFlyUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Salutation",
                table: "PayhouseDragonFlyUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AdditionalInformation",
                table: "PayhouseDragonFlyUsers");

            migrationBuilder.DropColumn(
                name: "BusinessUnit",
                table: "PayhouseDragonFlyUsers");

            migrationBuilder.DropColumn(
                name: "Position",
                table: "PayhouseDragonFlyUsers");

            migrationBuilder.DropColumn(
                name: "Salutation",
                table: "PayhouseDragonFlyUsers");
        }
    }
}
