using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PayhouseDragonFly.INFRASTRUCTURE.Migrations
{
    /// <inheritdoc />
    public partial class Changesonregistrationtable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PartnerName",
                table: "PayhouseDragonFlyUsers",
                newName: "Site");

            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "PayhouseDragonFlyUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ClientName",
                table: "PayhouseDragonFlyUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "County",
                table: "PayhouseDragonFlyUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "DepartmentName",
                table: "PayhouseDragonFlyUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Address",
                table: "PayhouseDragonFlyUsers");

            migrationBuilder.DropColumn(
                name: "ClientName",
                table: "PayhouseDragonFlyUsers");

            migrationBuilder.DropColumn(
                name: "County",
                table: "PayhouseDragonFlyUsers");

            migrationBuilder.DropColumn(
                name: "DepartmentName",
                table: "PayhouseDragonFlyUsers");

            migrationBuilder.RenameColumn(
                name: "Site",
                table: "PayhouseDragonFlyUsers",
                newName: "PartnerName");
        }
    }
}
