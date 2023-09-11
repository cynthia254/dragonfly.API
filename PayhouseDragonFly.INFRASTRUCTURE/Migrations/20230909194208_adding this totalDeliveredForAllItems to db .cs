using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PayhouseDragonFly.INFRASTRUCTURE.Migrations
{
    /// <inheritdoc />
    public partial class addingthistotalDeliveredForAllItemstodb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "totalDeliveredForAllItems",
                table: "UploadPOFile",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "totalDeliveredForAllItems",
                table: "AddDeliveryNote",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "totalDeliveredForAllItems",
                table: "UploadPOFile");

            migrationBuilder.DropColumn(
                name: "totalDeliveredForAllItems",
                table: "AddDeliveryNote");
        }
    }
}
