using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PayhouseDragonFly.INFRASTRUCTURE.Migrations
{
    /// <inheritdoc />
    public partial class addingchangestothesystemlistoutstandingbalance : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "OutStandingBalance",
                table: "ApplyRequistionForm",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "QuantityDispatched",
                table: "ApplyRequistionForm",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OutStandingBalance",
                table: "ApplyRequistionForm");

            migrationBuilder.DropColumn(
                name: "QuantityDispatched",
                table: "ApplyRequistionForm");
        }
    }
}
