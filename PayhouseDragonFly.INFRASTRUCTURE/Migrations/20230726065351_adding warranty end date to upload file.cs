//using System;
//using Microsoft.EntityFrameworkCore.Migrations;

//#nullable disable

//namespace PayhouseDragonFly.INFRASTRUCTURE.Migrations
//{
//    /// <inheritdoc />
//    public partial class addingwarrantyenddatetouploadfile : Migration
//    {
//        /// <inheritdoc />
//        protected override void Up(MigrationBuilder migrationBuilder)
//        {
//            migrationBuilder.AlterColumn<DateTime>(
//                name: "WarrantyStartDate",
//                table: "UploadPOFile",
//                type: "datetime2",
//                nullable: false,
//                oldClrType: typeof(string),
//                oldType: "nvarchar(max)");

//            migrationBuilder.AlterColumn<int>(
//                name: "Warranty",
//                table: "UploadPOFile",
//                type: "int",
//                nullable: false,
//                oldClrType: typeof(string),
//                oldType: "nvarchar(max)");

//            migrationBuilder.AddColumn<DateTime>(
//                name: "WarrantyEndDate",
//                table: "UploadPOFile",
//                type: "datetime2",
//                nullable: false,
//                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
//        }

//        /// <inheritdoc />
//        protected override void Down(MigrationBuilder migrationBuilder)
//        {
//            migrationBuilder.DropColumn(
//                name: "WarrantyEndDate",
//                table: "UploadPOFile");

//            migrationBuilder.AlterColumn<string>(
//                name: "WarrantyStartDate",
//                table: "UploadPOFile",
//                type: "nvarchar(max)",
//                nullable: false,
//                oldClrType: typeof(DateTime),
//                oldType: "datetime2");

//            migrationBuilder.AlterColumn<string>(
//                name: "Warranty",
//                table: "UploadPOFile",
//                type: "nvarchar(max)",
//                nullable: false,
//                oldClrType: typeof(int),
//                oldType: "int");
//        }
//    }
//}
