//using Microsoft.EntityFrameworkCore.Migrations;

//#nullable disable

//namespace PayhouseDragonFly.INFRASTRUCTURE.Migrations
//{
//    /// <inheritdoc />
//    public partial class rectifyingstatusandreferencenumberofuploadpofilevmssinthesystemstocksesystemmanagementinthesystemwebitingSSo : Migration
//    {
//        /// <inheritdoc />
//        protected override void Up(MigrationBuilder migrationBuilder)
//        {
//            migrationBuilder.DropColumn(
//                name: "Reference_Number",
//                table: "UploadPOFile");

//            migrationBuilder.DropColumn(
//                name: "Status",
//                table: "UploadPOFile");

//            migrationBuilder.CreateTable(
//                name: "UploadPOFiles",
//                columns: table => new
//                {
//                    ID = table.Column<int>(type: "int", nullable: false)
//                        .Annotation("SqlServer:Identity", "1, 1")
//                },
//                constraints: table =>
//                {
//                    table.PrimaryKey("PK_UploadPOFiles", x => x.ID);
//                });
//        }

//        /// <inheritdoc />
//        protected override void Down(MigrationBuilder migrationBuilder)
//        {
//            migrationBuilder.DropTable(
//                name: "UploadPOFiles");

//            migrationBuilder.AddColumn<string>(
//                name: "Reference_Number",
//                table: "UploadPOFile",
//                type: "nvarchar(max)",
//                nullable: false,
//                defaultValue: "");

//            migrationBuilder.AddColumn<string>(
//                name: "Status",
//                table: "UploadPOFile",
//                type: "nvarchar(max)",
//                nullable: false,
//                defaultValue: "");
//        }
//    }
//}
