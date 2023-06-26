//using System;
//using Microsoft.EntityFrameworkCore.Migrations;

//#nullable disable

//namespace PayhouseDragonFly.INFRASTRUCTURE.Migrations
//{
//    /// <inheritdoc />
//    public partial class addinginvoicelines : Migration
//    {
//        /// <inheritdoc />
//        protected override void Up(MigrationBuilder migrationBuilder)
//        {
//            migrationBuilder.CreateTable(
//                name: "InvoiceLines",
//                columns: table => new
//                {
//                    BatchID = table.Column<int>(type: "int", nullable: false)
//                        .Annotation("SqlServer:Identity", "1, 1"),
//                    BrandName = table.Column<string>(type: "nvarchar(max)", nullable: false),
//                    CategoryName = table.Column<string>(type: "nvarchar(max)", nullable: false),
//                    ItemName = table.Column<string>(type: "nvarchar(max)", nullable: false),
//                    Quantity = table.Column<int>(type: "int", nullable: false),
//                    UnitPrice = table.Column<int>(type: "int", nullable: false),
//                    Currency = table.Column<string>(type: "nvarchar(max)", nullable: false),
//                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
//                    UpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
//                    Warranty = table.Column<string>(type: "nvarchar(max)", nullable: false),
//                    WarrantyStartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
//                    WarrantyEndDate = table.Column<DateTime>(type: "datetime2", nullable: false)
//                },
//                constraints: table =>
//                {
//                    table.PrimaryKey("PK_InvoiceLines", x => x.BatchID);
//                });
//        }

//        /// <inheritdoc />
//        protected override void Down(MigrationBuilder migrationBuilder)
//        {
//            migrationBuilder.DropTable(
//                name: "InvoiceLines");
//        }
//    }
//}
