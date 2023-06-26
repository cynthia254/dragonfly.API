//using Microsoft.EntityFrameworkCore.Migrations;

//#nullable disable

//namespace PayhouseDragonFly.INFRASTRUCTURE.Migrations
//{
//    /// <inheritdoc />
//    public partial class addingITEMIDchangestoproductdetailsinsystem : Migration
//    {
//        /// <inheritdoc />
//        protected override void Up(MigrationBuilder migrationBuilder)
//        {
//            migrationBuilder.DropPrimaryKey(
//                name: "PK_AddProductDetails",
//                table: "AddProductDetails");

//            migrationBuilder.AlterColumn<int>(
//                name: "ItemID",
//                table: "AddProductDetails",
//                type: "int",
//                nullable: false,
//                oldClrType: typeof(int),
//                oldType: "int")
//                .Annotation("SqlServer:Identity", "1, 1");

//            migrationBuilder.AlterColumn<int>(
//                name: "BatchID",
//                table: "AddProductDetails",
//                type: "int",
//                nullable: false,
//                oldClrType: typeof(int),
//                oldType: "int")
//                .OldAnnotation("SqlServer:Identity", "1, 1");

//            migrationBuilder.AddPrimaryKey(
//                name: "PK_AddProductDetails",
//                table: "AddProductDetails",
//                column: "ItemID");
//        }

//        /// <inheritdoc />
//        protected override void Down(MigrationBuilder migrationBuilder)
//        {
//            migrationBuilder.DropPrimaryKey(
//                name: "PK_AddProductDetails",
//                table: "AddProductDetails");

//            migrationBuilder.AlterColumn<int>(
//                name: "BatchID",
//                table: "AddProductDetails",
//                type: "int",
//                nullable: false,
//                oldClrType: typeof(int),
//                oldType: "int")
//                .Annotation("SqlServer:Identity", "1, 1");

//            migrationBuilder.AlterColumn<int>(
//                name: "ItemID",
//                table: "AddProductDetails",
//                type: "int",
//                nullable: false,
//                oldClrType: typeof(int),
//                oldType: "int")
//                .OldAnnotation("SqlServer:Identity", "1, 1");

//            migrationBuilder.AddPrimaryKey(
//                name: "PK_AddProductDetails",
//                table: "AddProductDetails",
//                column: "BatchID");
//        }
//    }
//}
