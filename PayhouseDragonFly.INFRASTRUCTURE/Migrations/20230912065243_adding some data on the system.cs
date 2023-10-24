using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PayhouseDragonFly.INFRASTRUCTURE.Migrations
{
    /// <inheritdoc />
    public partial class addingsomedataonthesystem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "OrderNumber",
                table: "ApplyRequistionForm",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "OrderNo",
                columns: table => new
                {
                    OrderKey = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderNumber = table.Column<int>(type: "int", nullable: false),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderNo", x => x.OrderKey);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OrderNo");

            migrationBuilder.DropColumn(
                name: "OrderNumber",
                table: "ApplyRequistionForm");
        }
    }
}
