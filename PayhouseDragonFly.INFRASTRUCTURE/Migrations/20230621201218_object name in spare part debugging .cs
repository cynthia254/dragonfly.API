//using Microsoft.EntityFrameworkCore.Migrations;

//#nullable disable

//namespace PayhouseDragonFly.INFRASTRUCTURE.Migrations
//{
//    /// <inheritdoc />
//    public partial class objectnameinsparepartdebugging : Migration
//    {
//        /// <inheritdoc />
//        protected override void Up(MigrationBuilder migrationBuilder)
//        {
//            migrationBuilder.DropPrimaryKey(
//                name: "PK_SparePart",
//                table: "SparePart");

//            migrationBuilder.RenameTable(
//                name: "SparePart",
//                newName: "AddPart");

//            migrationBuilder.AddPrimaryKey(
//                name: "PK_AddPart",
//                table: "AddPart",
//                column: "PartID");
//        }

//        /// <inheritdoc />
//        protected override void Down(MigrationBuilder migrationBuilder)
//        {
//            migrationBuilder.DropPrimaryKey(
//                name: "PK_AddPart",
//                table: "AddPart");

//            migrationBuilder.RenameTable(
//                name: "AddPart",
//                newName: "SparePart");

//            migrationBuilder.AddPrimaryKey(
//                name: "PK_SparePart",
//                table: "SparePart",
//                column: "PartID");
//        }
//    }
//}
