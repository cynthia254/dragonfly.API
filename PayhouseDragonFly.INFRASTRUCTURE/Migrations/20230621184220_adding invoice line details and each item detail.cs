//using Microsoft.EntityFrameworkCore.Migrations;

//#nullable disable

//namespace PayhouseDragonFly.INFRASTRUCTURE.Migrations
//{
//    /// <inheritdoc />
//    public partial class addinginvoicelinedetailsandeachitemdetail : Migration
//    {
//        /// <inheritdoc />
//        protected override void Up(MigrationBuilder migrationBuilder)
//        {
//            migrationBuilder.DropColumn(
//                name: "ItemDescription",
//                table: "AddItem");

//            migrationBuilder.CreateTable(
//                name: "Invoice_Item_Quantity",
//                columns: table => new
//                {
//                    Invoic_item_quantity_id = table.Column<int>(type: "int", nullable: false)
//                        .Annotation("SqlServer:Identity", "1, 1"),
//                    Invoce_No = table.Column<string>(type: "nvarchar(max)", nullable: false),
//                    Quantity = table.Column<int>(type: "int", nullable: false),
//                    Type = table.Column<string>(type: "nvarchar(max)", nullable: false),
//                    Invoice_quantity_Unique_id = table.Column<string>(type: "nvarchar(max)", nullable: false)
//                },
//                constraints: table =>
//                {
//                    table.PrimaryKey("PK_Invoice_Item_Quantity", x => x.Invoic_item_quantity_id);
//                });

//            migrationBuilder.CreateTable(
//                name: "Item_Numbering_Stock",
//                columns: table => new
//                {
//                    invoice_numbering_id = table.Column<int>(type: "int", nullable: false)
//                        .Annotation("SqlServer:Identity", "1, 1"),
//                    Product_No = table.Column<int>(type: "int", nullable: false),
//                    Invoice_quantity_Id = table.Column<string>(type: "nvarchar(max)", nullable: false),
//                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false)
//                },
//                constraints: table =>
//                {
//                    table.PrimaryKey("PK_Item_Numbering_Stock", x => x.invoice_numbering_id);
//                });
//        }

//        /// <inheritdoc />
//        protected override void Down(MigrationBuilder migrationBuilder)
//        {
//            migrationBuilder.DropTable(
//                name: "Invoice_Item_Quantity");

//            migrationBuilder.DropTable(
//                name: "Item_Numbering_Stock");

//            migrationBuilder.AddColumn<string>(
//                name: "ItemDescription",
//                table: "AddItem",
//                type: "nvarchar(max)",
//                nullable: false,
//                defaultValue: "");
//        }
//    }
//}
