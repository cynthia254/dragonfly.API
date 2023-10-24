using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PayhouseDragonFly.INFRASTRUCTURE.Migrations
{
    /// <inheritdoc />
    public partial class datatobereturnedrecorddtoandclassmodel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ReturnedItem",
                columns: table => new
                {
                    ReturnID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ReasonForReturn = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ReturnedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ReturnedCondition = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ReturnedQuantity = table.Column<int>(type: "int", nullable: false),
                    SerialNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IMEI1 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IMEI2 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IssuedId = table.Column<int>(type: "int", nullable: false),
                    DateReturned = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ReturnedStatus = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReturnedItem", x => x.ReturnID);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ReturnedItem");
        }
    }
}
