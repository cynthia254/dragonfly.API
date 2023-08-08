//using System;
//using Microsoft.EntityFrameworkCore.Migrations;

//#nullable disable

//namespace PayhouseDragonFly.INFRASTRUCTURE.Migrations
//{
//    /// <inheritdoc />
//    public partial class rectifyingstatusandreferencenumberofuploadpofilevmssinthesystemstocksesystemmanagementinthesystemwebitingSS : Migration
//    {
//        /// <inheritdoc />
//        protected override void Up(MigrationBuilder migrationBuilder)
//        {
//            migrationBuilder.DropTable(
//                name: "UploadPOFile");
//        }

//        /// <inheritdoc />
//        protected override void Down(MigrationBuilder migrationBuilder)
//        {
//            migrationBuilder.CreateTable(
//                name: "UploadPOFile",
//                columns: table => new
//                {
//                    ID = table.Column<int>(type: "int", nullable: false)
//                        .Annotation("SqlServer:Identity", "1, 1"),
//                    AjustedQuantity = table.Column<int>(type: "int", nullable: false),
//                    Amount = table.Column<string>(type: "nvarchar(max)", nullable: false),
//                    AvailableStock = table.Column<int>(type: "int", nullable: false),
//                    CategoryName = table.Column<string>(type: "nvarchar(max)", nullable: false),
//                    DateAdded = table.Column<DateTime>(type: "datetime2", nullable: false),
//                    ItemName = table.Column<string>(type: "nvarchar(max)", nullable: false),
//                    OpeningStock = table.Column<int>(type: "int", nullable: false),
//                    PONumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
//                    Quantity = table.Column<int>(type: "int", nullable: false),
//                    Rate = table.Column<string>(type: "nvarchar(max)", nullable: false),
//                    Warranty = table.Column<int>(type: "int", nullable: false),
//                    WarrantyEndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
//                    WarrantyStartDate = table.Column<DateTime>(type: "datetime2", nullable: false)
//                },
//                constraints: table =>
//                {
//                    table.PrimaryKey("PK_UploadPOFile", x => x.ID);
//                });
//        }
//    }
//}
