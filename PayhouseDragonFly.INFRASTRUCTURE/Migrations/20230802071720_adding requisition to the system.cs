//using System;
//using Microsoft.EntityFrameworkCore.Migrations;

//#nullable disable

//namespace PayhouseDragonFly.INFRASTRUCTURE.Migrations
//{
//    /// <inheritdoc />
//    public partial class addingrequisitiontothesystem : Migration
//    {
//        /// <inheritdoc />
//        protected override void Up(MigrationBuilder migrationBuilder)
//        {
//            migrationBuilder.CreateTable(
//                name: "RequisitionForm",
//                columns: table => new
//                {
//                    Id = table.Column<int>(type: "int", nullable: false)
//                        .Annotation("SqlServer:Identity", "1, 1"),
//                    itemName = table.Column<string>(type: "nvarchar(max)", nullable: false),
//                    BrandName = table.Column<string>(type: "nvarchar(max)", nullable: false),
//                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
//                    stockNeed = table.Column<string>(type: "nvarchar(max)", nullable: false),
//                    Quantity = table.Column<int>(type: "int", nullable: false),
//                    DeviceBeingRepaired = table.Column<string>(type: "nvarchar(max)", nullable: false),
//                    clientName = table.Column<string>(type: "nvarchar(max)", nullable: false),
//                    Department = table.Column<string>(type: "nvarchar(max)", nullable: false),
//                    Purpose = table.Column<string>(type: "nvarchar(max)", nullable: false),
//                    Requisitioner = table.Column<string>(type: "nvarchar(max)", nullable: false),
//                    IssuedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
//                    IssuedByDate = table.Column<DateTime>(type: "datetime2", nullable: false),
//                    ApprovedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
//                    ApprovedStatus = table.Column<string>(type: "nvarchar(max)", nullable: false)
//                },
//                constraints: table =>
//                {
//                    table.PrimaryKey("PK_RequisitionForm", x => x.Id);
//                });
//        }

//        /// <inheritdoc />
//        protected override void Down(MigrationBuilder migrationBuilder)
//        {
//            migrationBuilder.DropTable(
//                name: "RequisitionForm");
//        }
//    }
//}
