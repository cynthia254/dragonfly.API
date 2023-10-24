//using Microsoft.EntityFrameworkCore.Migrations;

//#nullable disable

//namespace PayhouseDragonFly.INFRASTRUCTURE.Migrations
//{
//    /// <inheritdoc />
//    public partial class addingtheexcessquantitytothestocksystem : Migration
//    {
//        /// <inheritdoc />
//        protected override void Up(MigrationBuilder migrationBuilder)
//        {
//            migrationBuilder.DropColumn(
//                name: "TotalQuantityDispatchedForAnId",
//                table: "SelectSerial");

//            migrationBuilder.AddColumn<int>(
//                name: "ExcessQuantity",
//                table: "AddDeliveryNote",
//                type: "int",
//                nullable: false,
//                defaultValue: 0);
//        }

//        /// <inheritdoc />
//        protected override void Down(MigrationBuilder migrationBuilder)
//        {
//            migrationBuilder.DropColumn(
//                name: "ExcessQuantity",
//                table: "AddDeliveryNote");

//            migrationBuilder.AddColumn<int>(
//                name: "TotalQuantityDispatchedForAnId",
//                table: "SelectSerial",
//                type: "int",
//                nullable: false,
//                defaultValue: 0);
//        }
//    }
//}
