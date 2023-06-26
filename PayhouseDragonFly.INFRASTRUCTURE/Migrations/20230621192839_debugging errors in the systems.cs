//using Microsoft.EntityFrameworkCore.Migrations;

//#nullable disable

//namespace PayhouseDragonFly.INFRASTRUCTURE.Migrations
//{
//    /// <inheritdoc />
//    public partial class debuggingerrorsinthesystems : Migration
//    {
//        /// <inheritdoc />
//        protected override void Up(MigrationBuilder migrationBuilder)
//        {
//            migrationBuilder.RenameColumn(
//                name: "sparePartID",
//                table: "SparePart",
//                newName: "PartID");

//            migrationBuilder.CreateTable(
//                name: "AddSparePart",
//                columns: table => new
//                {
//                    sparePartID = table.Column<int>(type: "int", nullable: false)
//                        .Annotation("SqlServer:Identity", "1, 1"),
//                    PartName = table.Column<string>(type: "nvarchar(max)", nullable: false),
//                    PartDescription = table.Column<string>(type: "nvarchar(max)", nullable: false)
//                },
//                constraints: table =>
//                {
//                    table.PrimaryKey("PK_AddSparePart", x => x.sparePartID);
//                });
//        }

//        /// <inheritdoc />
//        protected override void Down(MigrationBuilder migrationBuilder)
//        {
//            migrationBuilder.DropTable(
//                name: "AddSparePart");

//            migrationBuilder.RenameColumn(
//                name: "PartID",
//                table: "SparePart",
//                newName: "sparePartID");
//        }
//    }
//}
