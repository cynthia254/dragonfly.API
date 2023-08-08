//using Microsoft.EntityFrameworkCore.Migrations;

//#nullable disable

//namespace PayhouseDragonFly.INFRASTRUCTURE.Migrations
//{
//    /// <inheritdoc />
//    public partial class addingponumberinthesystem : Migration
//    {
//        /// <inheritdoc />
//        protected override void Up(MigrationBuilder migrationBuilder)
//        {
//            migrationBuilder.AlterColumn<string>(
//                name: "Quantity",
//                table: "UploadPOItem",
//                type: "nvarchar(max)",
//                nullable: false,
//                oldClrType: typeof(int),
//                oldType: "int");

//            migrationBuilder.AddColumn<string>(
//                name: "PONumber",
//                table: "UploadPOItem",
//                type: "nvarchar(max)",
//                nullable: false,
//                defaultValue: "");
//        }

//        /// <inheritdoc />
//        protected override void Down(MigrationBuilder migrationBuilder)
//        {
//            migrationBuilder.DropColumn(
//                name: "PONumber",
//                table: "UploadPOItem");

//            migrationBuilder.AlterColumn<int>(
//                name: "Quantity",
//                table: "UploadPOItem",
//                type: "int",
//                nullable: false,
//                oldClrType: typeof(string),
//                oldType: "nvarchar(max)");
//        }
//    }
//}
