using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PayhouseDragonFly.INFRASTRUCTURE.Migrations
{
    /// <inheritdoc />
    public partial class addingnametousetoselectserialandapplyingrequisitonform : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "NameToUse",
                table: "SelectSerial",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "NameToUse",
                table: "ApplyRequistionForm",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NameToUse",
                table: "SelectSerial");

            migrationBuilder.DropColumn(
                name: "NameToUse",
                table: "ApplyRequistionForm");
        }
    }
}
