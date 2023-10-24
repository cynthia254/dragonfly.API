using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PayhouseDragonFly.INFRASTRUCTURE.Migrations
{
    /// <inheritdoc />
    public partial class addinjgreturneddetAILSTOTHESYSTEM : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "BrandName",
                table: "ReturnedItem",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ItemName",
                table: "ReturnedItem",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "ApprovalRetun",
                columns: table => new
                {
                    AprrovedID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Id = table.Column<int>(type: "int", nullable: false),
                    ApprovalStatus = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    selectedOption = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AprrovedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RejectedReason = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ApprovedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ReturnedQuantity = table.Column<int>(type: "int", nullable: false),
                    ReturnedStatus = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SerialNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ItemName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BrandName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TotalReturnedQuantity = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApprovalRetun", x => x.AprrovedID);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ApprovalRetun");

            migrationBuilder.DropColumn(
                name: "BrandName",
                table: "ReturnedItem");

            migrationBuilder.DropColumn(
                name: "ItemName",
                table: "ReturnedItem");
        }
    }
}
