﻿//using Microsoft.EntityFrameworkCore.Migrations;

//#nullable disable

//namespace PayhouseDragonFly.INFRASTRUCTURE.Migrations
//{
//    /// <inheritdoc />
//    public partial class addingstatusandreferencenumberofuploadpofilevmssinthesystemstockses : Migration
//    {
//        /// <inheritdoc />
//        protected override void Up(MigrationBuilder migrationBuilder)
//        {
//            migrationBuilder.DropColumn(
//                name: "Reference_Number",
//                table: "UploadPOFile");

//            migrationBuilder.DropColumn(
//                name: "status",
//                table: "UploadPOFile");
//        }

//        /// <inheritdoc />
//        protected override void Down(MigrationBuilder migrationBuilder)
//        {
//            migrationBuilder.AddColumn<string>(
//                name: "Reference_Number",
//                table: "UploadPOFile",
//                type: "nvarchar(max)",
//                nullable: false,
//                defaultValue: "");

//            migrationBuilder.AddColumn<string>(
//                name: "status",
//                table: "UploadPOFile",
//                type: "nvarchar(max)",
//                nullable: false,
//                defaultValue: "");
//        }
//    }
//}
