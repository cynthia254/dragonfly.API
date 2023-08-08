//using System;
//using Microsoft.EntityFrameworkCore.Migrations;

//#nullable disable

//namespace PayhouseDragonFly.INFRASTRUCTURE.Migrations
//{
//    /// <inheritdoc />
//    public partial class changingthewholeuploadfilevm : Migration
//    {
//        /// <inheritdoc />
//        protected override void Up(MigrationBuilder migrationBuilder)
//        {
//            migrationBuilder.DropColumn(
//                name: "PODate",
//                table: "UploadPOFile");

//            migrationBuilder.DropColumn(
//                name: "Vendor",
//                table: "UploadPOFile");
//        }

//        /// <inheritdoc />
//        protected override void Down(MigrationBuilder migrationBuilder)
//        {
//            migrationBuilder.AddColumn<DateTime>(
//                name: "PODate",
//                table: "UploadPOFile",
//                type: "datetime2",
//                nullable: false,
//                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

//            migrationBuilder.AddColumn<string>(
//                name: "Vendor",
//                table: "UploadPOFile",
//                type: "nvarchar(max)",
//                nullable: false,
//                defaultValue: "");
//        }
//    }
//}
