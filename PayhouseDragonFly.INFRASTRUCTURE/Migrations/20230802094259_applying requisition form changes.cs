//using Microsoft.EntityFrameworkCore.Migrations;

//#nullable disable

//namespace PayhouseDragonFly.INFRASTRUCTURE.Migrations
//{
//    /// <inheritdoc />
//    public partial class applyingrequisitionformchanges : Migration
//    {
//        /// <inheritdoc />
//        protected override void Up(MigrationBuilder migrationBuilder)
//        {
//            migrationBuilder.DropPrimaryKey(
//                name: "PK_RequisitionForm",
//                table: "RequisitionForm");

//            migrationBuilder.RenameTable(
//                name: "RequisitionForm",
//                newName: "RequisitionForms");

//            migrationBuilder.AddPrimaryKey(
//                name: "PK_RequisitionForms",
//                table: "RequisitionForms",
//                column: "Id");
//        }

//        /// <inheritdoc />
//        protected override void Down(MigrationBuilder migrationBuilder)
//        {
//            migrationBuilder.DropPrimaryKey(
//                name: "PK_RequisitionForms",
//                table: "RequisitionForms");

//            migrationBuilder.RenameTable(
//                name: "RequisitionForms",
//                newName: "RequisitionForm");

//            migrationBuilder.AddPrimaryKey(
//                name: "PK_RequisitionForm",
//                table: "RequisitionForm",
//                column: "Id");
//        }
//    }
//}
