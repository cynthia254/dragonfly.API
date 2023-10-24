//using Microsoft.EntityFrameworkCore.Migrations;

//#nullable disable

//namespace PayhouseDragonFly.INFRASTRUCTURE.Migrations
//{
//    /// <inheritdoc />
//    public partial class addingordernumbertotheissuepart : Migration
//    {
//        /// <inheritdoc />
//        protected override void Up(MigrationBuilder migrationBuilder)
//        {
//            migrationBuilder.DropColumn(
//                name: "ExcessQuantity",
//                table: "AddDeliveryNote");

//            migrationBuilder.AddColumn<string>(
//                name: "OrderNumber",
//                table: "SelectSerial",
//                type: "nvarchar(max)",
//                nullable: false,
//                defaultValue: "");
//        }

//        /// <inheritdoc />
//        protected override void Down(MigrationBuilder migrationBuilder)
//        {
//            migrationBuilder.DropColumn(
//                name: "OrderNumber",
//                table: "SelectSerial");

//            migrationBuilder.AddColumn<int>(
//                name: "ExcessQuantity",
//                table: "AddDeliveryNote",
//                type: "int",
//                nullable: false,
//                defaultValue: 0);
//        }
//    }
//}
