//using System;
//using Microsoft.EntityFrameworkCore.Migrations;

//#nullable disable

//namespace PayhouseDragonFly.INFRASTRUCTURE.Migrations
//{
//    /// <inheritdoc />
//    public partial class addingchangestopurchaseOrdersvm : Migration
//    {
//        /// <inheritdoc />
//        protected override void Up(MigrationBuilder migrationBuilder)
//        {
//            migrationBuilder.CreateTable(
//                name: "PurchaseOrderss",
//                columns: table => new
//                {
//                    PurchaseOrderID = table.Column<int>(type: "int", nullable: false)
//                        .Annotation("SqlServer:Identity", "1, 1"),
//                    PONumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
//                    PODate = table.Column<DateTime>(type: "datetime2", nullable: false),
//                    Vendor = table.Column<string>(type: "nvarchar(max)", nullable: false)
//                },
//                constraints: table =>
//                {
//                    table.PrimaryKey("PK_PurchaseOrderss", x => x.PurchaseOrderID);
//                });
//        }

//        /// <inheritdoc />
//        protected override void Down(MigrationBuilder migrationBuilder)
//        {
//            migrationBuilder.DropTable(
//                name: "PurchaseOrderss");
//        }
//    }
//}
