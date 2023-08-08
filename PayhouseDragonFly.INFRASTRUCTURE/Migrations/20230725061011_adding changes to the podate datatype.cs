//using System;
//using Microsoft.EntityFrameworkCore.Migrations;

//#nullable disable

//namespace PayhouseDragonFly.INFRASTRUCTURE.Migrations
//{
//    /// <inheritdoc />
//    public partial class addingchangestothepodatedatatype : Migration
//    {
//        /// <inheritdoc />
//        protected override void Up(MigrationBuilder migrationBuilder)
//        {
//            migrationBuilder.AlterColumn<DateTime>(
//                name: "PODate",
//                table: "UploadPOFile",
//                type: "datetime2",
//                nullable: false,
//                oldClrType: typeof(string),
//                oldType: "nvarchar(max)");
//        }

//        /// <inheritdoc />
//        protected override void Down(MigrationBuilder migrationBuilder)
//        {
//            migrationBuilder.AlterColumn<string>(
//                name: "PODate",
//                table: "UploadPOFile",
//                type: "nvarchar(max)",
//                nullable: false,
//                oldClrType: typeof(DateTime),
//                oldType: "datetime2");
//        }
//    }
//}
