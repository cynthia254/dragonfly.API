//using Microsoft.EntityFrameworkCore.Migrations;

//#nullable disable

//namespace PayhouseDragonFly.INFRASTRUCTURE.Migrations
//{
//    /// <inheritdoc />
//    public partial class rectifyingeverythign : Migration
//    {
//        /// <inheritdoc />
//        protected override void Up(MigrationBuilder migrationBuilder)
//        {
//            migrationBuilder.DropColumn(
//                name: "OrderNumber",
//                table: "SelectSerial");

//            migrationBuilder.RenameColumn(
//                name: "orderNumber",
//                table: "SelectSerial",
//                newName: "OrderNumbers");
//        }

//        /// <inheritdoc />
//        protected override void Down(MigrationBuilder migrationBuilder)
//        {
//            migrationBuilder.RenameColumn(
//                name: "OrderNumbers",
//                table: "SelectSerial",
//                newName: "orderNumber");

//            migrationBuilder.AddColumn<string>(
//                name: "OrderNumber",
//                table: "SelectSerial",
//                type: "nvarchar(max)",
//                nullable: false,
//                defaultValue: "");
//        }
//    }
//}
