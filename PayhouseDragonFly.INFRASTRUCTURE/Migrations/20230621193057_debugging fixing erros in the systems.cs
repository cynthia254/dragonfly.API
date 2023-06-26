//using System;
//using Microsoft.EntityFrameworkCore.Migrations;

//#nullable disable

//namespace PayhouseDragonFly.INFRASTRUCTURE.Migrations
//{
//    /// <inheritdoc />
//    public partial class debuggingfixingerrosinthesystems : Migration
//    {
//        /// <inheritdoc />
//        protected override void Up(MigrationBuilder migrationBuilder)
//        {
//            migrationBuilder.DropTable(
//                name: "AddBatchDetail");
//        }

//        /// <inheritdoc />
//        protected override void Down(MigrationBuilder migrationBuilder)
//        {
//            migrationBuilder.CreateTable(
//                name: "AddBatchDetail",
//                columns: table => new
//                {
//                    ItemID = table.Column<int>(type: "int", nullable: false)
//                        .Annotation("SqlServer:Identity", "1, 1"),
//                    BatchID = table.Column<int>(type: "int", nullable: false),
//                    BrandName = table.Column<string>(type: "nvarchar(max)", nullable: false),
//                    IMEI1 = table.Column<int>(type: "int", nullable: false),
//                    IMEI2 = table.Column<int>(type: "int", nullable: false),
//                    ItemName = table.Column<string>(type: "nvarchar(max)", nullable: false),
//                    SerialNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
//                    WarrantyEndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
//                    WarrantyStartDate = table.Column<DateTime>(type: "datetime2", nullable: false)
//                },
//                constraints: table =>
//                {
//                    table.PrimaryKey("PK_AddBatchDetail", x => x.ItemID);
//                });
//        }
//    }
//}
