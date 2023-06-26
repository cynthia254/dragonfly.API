//using System;
//using Microsoft.EntityFrameworkCore.Migrations;

//#nullable disable

//namespace PayhouseDragonFly.INFRASTRUCTURE.Migrations
//{
//    /// <inheritdoc />
//    public partial class addinginvoicelinedetailsandeachitemdetail2 : Migration
//    {
//        /// <inheritdoc />
//        protected override void Up(MigrationBuilder migrationBuilder)
//        {
//            migrationBuilder.CreateTable(
//                name: "AddBatchDetail",
//                columns: table => new
//                {
//                    ItemID = table.Column<int>(type: "int", nullable: false)
//                        .Annotation("SqlServer:Identity", "1, 1"),
//                    SerialNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
//                    BrandName = table.Column<string>(type: "nvarchar(max)", nullable: false),
//                    ItemName = table.Column<string>(type: "nvarchar(max)", nullable: false),
//                    BatchID = table.Column<int>(type: "int", nullable: false),
//                    IMEI1 = table.Column<int>(type: "int", nullable: false),
//                    IMEI2 = table.Column<int>(type: "int", nullable: false),
//                    WarrantyStartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
//                    WarrantyEndDate = table.Column<DateTime>(type: "datetime2", nullable: false)
//                },
//                constraints: table =>
//                {
//                    table.PrimaryKey("PK_AddBatchDetail", x => x.ItemID);
//                });

//            migrationBuilder.CreateTable(
//                name: "AddBrand",
//                columns: table => new
//                {
//                    BrandId = table.Column<int>(type: "int", nullable: false)
//                        .Annotation("SqlServer:Identity", "1, 1"),
//                    BrandName = table.Column<string>(type: "nvarchar(max)", nullable: false)
//                },
//                constraints: table =>
//                {
//                    table.PrimaryKey("PK_AddBrand", x => x.BrandId);
//                });

//            migrationBuilder.CreateTable(
//                name: "AddItem",
//                columns: table => new
//                {
//                    ItemID = table.Column<int>(type: "int", nullable: false)
//                        .Annotation("SqlServer:Identity", "1, 1"),
//                    ItemName = table.Column<string>(type: "nvarchar(max)", nullable: false),
//                    Category = table.Column<string>(type: "nvarchar(max)", nullable: false),
//                    BrandName = table.Column<string>(type: "nvarchar(max)", nullable: false),
//                    ReOrderLevel = table.Column<int>(type: "int", nullable: false),
//                    Currency = table.Column<string>(type: "nvarchar(max)", nullable: false),
//                    IndicativePrice = table.Column<int>(type: "int", nullable: false)
//                },
//                constraints: table =>
//                {
//                    table.PrimaryKey("PK_AddItem", x => x.ItemID);
//                });

//            migrationBuilder.CreateTable(
//                name: "AddProductDetails",
//                columns: table => new
//                {
//                    BatchID = table.Column<int>(type: "int", nullable: false)
//                        .Annotation("SqlServer:Identity", "1, 1"),
//                    ItemID = table.Column<int>(type: "int", nullable: false),
//                    SerialNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
//                    BrandName = table.Column<string>(type: "nvarchar(max)", nullable: false),
//                    ItemName = table.Column<string>(type: "nvarchar(max)", nullable: false),
//                    IMEI1 = table.Column<int>(type: "int", nullable: false),
//                    IMEI2 = table.Column<int>(type: "int", nullable: false),
//                    WarrantyStartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
//                    WarrantyEndDate = table.Column<DateTime>(type: "datetime2", nullable: false)
//                },
//                constraints: table =>
//                {
//                    table.PrimaryKey("PK_AddProductDetails", x => x.BatchID);
//                });

//            migrationBuilder.CreateTable(
//                name: "AddStock",
//                columns: table => new
//                {
//                    StockId = table.Column<int>(type: "int", nullable: false)
//                        .Annotation("SqlServer:Identity", "1, 1"),
//                    BrandName = table.Column<string>(type: "nvarchar(max)", nullable: false),
//                    ItemName = table.Column<string>(type: "nvarchar(max)", nullable: false),
//                    Quantity = table.Column<int>(type: "int", nullable: false),
//                    ReOrderLevel = table.Column<int>(type: "int", nullable: false),
//                    BuyingPrice = table.Column<int>(type: "int", nullable: false),
//                    SellingPrice = table.Column<int>(type: "int", nullable: false),
//                    AvailableStock = table.Column<int>(type: "int", nullable: false),
//                    DateAdded = table.Column<DateTime>(type: "datetime2", nullable: false),
//                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
//                    Currency = table.Column<string>(type: "nvarchar(max)", nullable: false),
//                    StockInTransit = table.Column<int>(type: "int", nullable: false),
//                    SalesCurrency = table.Column<string>(type: "nvarchar(max)", nullable: false),
//                    OpeningStock = table.Column<int>(type: "int", nullable: false),
//                    ReorderRequired = table.Column<string>(type: "nvarchar(max)", nullable: false),
//                    StockOut = table.Column<int>(type: "int", nullable: false),
//                    TotalReturnedStock = table.Column<int>(type: "int", nullable: false),
//                    StockIn = table.Column<int>(type: "int", nullable: false)
//                },
//                constraints: table =>
//                {
//                    table.PrimaryKey("PK_AddStock", x => x.StockId);
//                });

//            migrationBuilder.CreateTable(
//                name: "AspNetRoles",
//                columns: table => new
//                {
//                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
//                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
//                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
//                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
//                },
//                constraints: table =>
//                {
//                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
//                });

//            migrationBuilder.CreateTable(
//                name: "Category",
//                columns: table => new
//                {
//                    CategoryID = table.Column<int>(type: "int", nullable: false)
//                        .Annotation("SqlServer:Identity", "1, 1"),
//                    CategoryName = table.Column<string>(type: "nvarchar(max)", nullable: false),
//                    CategoryDesc = table.Column<string>(type: "nvarchar(max)", nullable: false)
//                },
//                constraints: table =>
//                {
//                    table.PrimaryKey("PK_Category", x => x.CategoryID);
//                });

//            migrationBuilder.CreateTable(
//                name: "Claim_Role_Map",
//                columns: table => new
//                {
//                    ClaimRoleId = table.Column<int>(type: "int", nullable: false)
//                        .Annotation("SqlServer:Identity", "1, 1"),
//                    RoleId = table.Column<int>(type: "int", nullable: false),
//                    ClaimId = table.Column<int>(type: "int", nullable: false)
//                },
//                constraints: table =>
//                {
//                    table.PrimaryKey("PK_Claim_Role_Map", x => x.ClaimRoleId);
//                });

//            migrationBuilder.CreateTable(
//                name: "Customer",
//                columns: table => new
//                {
//                    CustomerId = table.Column<int>(type: "int", nullable: false)
//                        .Annotation("SqlServer:Identity", "1, 1"),
//                    CustomerName = table.Column<string>(type: "nvarchar(max)", nullable: false),
//                    CompanyName = table.Column<string>(type: "nvarchar(max)", nullable: false),
//                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
//                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: false)
//                },
//                constraints: table =>
//                {
//                    table.PrimaryKey("PK_Customer", x => x.CustomerId);
//                });

//            migrationBuilder.CreateTable(
//                name: "Departments",
//                columns: table => new
//                {
//                    Departnmentid = table.Column<int>(type: "int", nullable: false)
//                        .Annotation("SqlServer:Identity", "1, 1"),
//                    DepartmentName = table.Column<string>(type: "nvarchar(max)", nullable: false),
//                    DepartmentDescription = table.Column<string>(type: "nvarchar(max)", nullable: false)
//                },
//                constraints: table =>
//                {
//                    table.PrimaryKey("PK_Departments", x => x.Departnmentid);
//                });

//            migrationBuilder.CreateTable(
//                name: "Designation",
//                columns: table => new
//                {
//                    PostionId = table.Column<int>(type: "int", nullable: false)
//                        .Annotation("SqlServer:Identity", "1, 1"),
//                    PositionName = table.Column<string>(type: "nvarchar(max)", nullable: false),
//                    PositionDescription = table.Column<string>(type: "nvarchar(max)", nullable: false)
//                },
//                constraints: table =>
//                {
//                    table.PrimaryKey("PK_Designation", x => x.PostionId);
//                });

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
//                name: "InvoiceLines",
//                columns: table => new
//                {
//                    BatchID = table.Column<int>(type: "int", nullable: false)
//                        .Annotation("SqlServer:Identity", "1, 1"),
//                    BrandName = table.Column<string>(type: "nvarchar(max)", nullable: false),
//                    CategoryName = table.Column<string>(type: "nvarchar(max)", nullable: false),
//                    ItemName = table.Column<string>(type: "nvarchar(max)", nullable: false),
//                    Quantity = table.Column<int>(type: "int", nullable: false),
//                    UnitPrice = table.Column<int>(type: "int", nullable: false),
//                    Currency = table.Column<string>(type: "nvarchar(max)", nullable: false),
//                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
//                    UpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
//                    Warranty = table.Column<int>(type: "int", nullable: false),
//                    WarrantyStartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
//                    WarrantyEndDate = table.Column<DateTime>(type: "datetime2", nullable: false)
//                },
//                constraints: table =>
//                {
//                    table.PrimaryKey("PK_InvoiceLines", x => x.BatchID);
//                });

//            migrationBuilder.CreateTable(
//                name: "InvoiceLinesDetails",
//                columns: table => new
//                {
//                    InvoiceLineId = table.Column<int>(type: "int", nullable: false)
//                        .Annotation("SqlServer:Identity", "1, 1"),
//                    BrandName = table.Column<string>(type: "nvarchar(max)", nullable: false),
//                    CategoryName = table.Column<string>(type: "nvarchar(max)", nullable: false),
//                    ItemName = table.Column<string>(type: "nvarchar(max)", nullable: false),
//                    Quantity = table.Column<int>(type: "int", nullable: false),
//                    UnitPrice = table.Column<int>(type: "int", nullable: false),
//                    Currency = table.Column<string>(type: "nvarchar(max)", nullable: false),
//                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
//                    UpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
//                    Warranty = table.Column<int>(type: "int", nullable: false),
//                    WarrantyStartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
//                    WarrantyEndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
//                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false)
//                },
//                constraints: table =>
//                {
//                    table.PrimaryKey("PK_InvoiceLinesDetails", x => x.InvoiceLineId);
//                });

//            migrationBuilder.CreateTable(
//                name: "InvoiceNo",
//                columns: table => new
//                {
//                    InvoiceKey = table.Column<int>(type: "int", nullable: false)
//                        .Annotation("SqlServer:Identity", "1, 1"),
//                    InvoieNo = table.Column<int>(type: "int", nullable: false),
//                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false)
//                },
//                constraints: table =>
//                {
//                    table.PrimaryKey("PK_InvoiceNo", x => x.InvoiceKey);
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

//            migrationBuilder.CreateTable(
//                name: "LPO_No",
//                columns: table => new
//                {
//                    LpoKey = table.Column<int>(type: "int", nullable: false)
//                        .Annotation("SqlServer:Identity", "1, 1"),
//                    LpoNo = table.Column<int>(type: "int", nullable: false),
//                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false)
//                },
//                constraints: table =>
//                {
//                    table.PrimaryKey("PK_LPO_No", x => x.LpoKey);
//                });

//            migrationBuilder.CreateTable(
//                name: "OtherRoles",
//                columns: table => new
//                {
//                    otherRolesID = table.Column<int>(type: "int", nullable: false)
//                        .Annotation("SqlServer:Identity", "1, 1"),
//                    RoleID = table.Column<int>(type: "int", nullable: false),
//                    userID = table.Column<string>(type: "nvarchar(max)", nullable: false)
//                },
//                constraints: table =>
//                {
//                    table.PrimaryKey("PK_OtherRoles", x => x.otherRolesID);
//                });

//            migrationBuilder.CreateTable(
//                name: "PayhouseDragonFlyUsers",
//                columns: table => new
//                {
//                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
//                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
//                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
//                    Salutation = table.Column<string>(type: "nvarchar(max)", nullable: false),
//                    AdditionalInformation = table.Column<string>(type: "nvarchar(max)", nullable: false),
//                    BusinessUnit = table.Column<string>(type: "nvarchar(max)", nullable: false),
//                    Site = table.Column<string>(type: "nvarchar(max)", nullable: false),
//                    County = table.Column<string>(type: "nvarchar(max)", nullable: false),
//                    DepartmentName = table.Column<string>(type: "nvarchar(max)", nullable: false),
//                    Address = table.Column<string>(type: "nvarchar(max)", nullable: false),
//                    ClientName = table.Column<string>(type: "nvarchar(max)", nullable: false),
//                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
//                    VerificationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
//                    VerificationToken = table.Column<string>(type: "nvarchar(max)", nullable: false),
//                    AnyMessage = table.Column<string>(type: "nvarchar(max)", nullable: false),
//                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false),
//                    RoleId = table.Column<int>(type: "int", nullable: false),
//                    Position = table.Column<string>(type: "nvarchar(max)", nullable: false),
//                    DepartmentDescription = table.Column<string>(type: "nvarchar(max)", nullable: false),
//                    UserStatus = table.Column<string>(type: "nvarchar(max)", nullable: false),
//                    StatusReason = table.Column<string>(type: "nvarchar(max)", nullable: false),
//                    PositionName = table.Column<string>(type: "nvarchar(max)", nullable: false),
//                    PostionDescription = table.Column<string>(type: "nvarchar(max)", nullable: false),
//                    UserActive = table.Column<bool>(type: "bit", nullable: false),
//                    StatusDescription = table.Column<string>(type: "nvarchar(max)", nullable: true),
//                    ReasonforStatus = table.Column<string>(type: "nvarchar(max)", nullable: true),
//                    ForgetpasswordVerificationToken = table.Column<string>(type: "nvarchar(max)", nullable: true),
//                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
//                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
//                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
//                    NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
//                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
//                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
//                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
//                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
//                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
//                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
//                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
//                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
//                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
//                },
//                constraints: table =>
//                {
//                    table.PrimaryKey("PK_PayhouseDragonFlyUsers", x => x.Id);
//                });

//            migrationBuilder.CreateTable(
//                name: "Purchases",
//                columns: table => new
//                {
//                    PurchaseId = table.Column<int>(type: "int", nullable: false)
//                        .Annotation("SqlServer:Identity", "1, 1"),
//                    BrandName = table.Column<string>(type: "nvarchar(max)", nullable: false),
//                    ItemName = table.Column<string>(type: "nvarchar(max)", nullable: false),
//                    Quantity = table.Column<int>(type: "int", nullable: false),
//                    BuyingPrice = table.Column<int>(type: "int", nullable: false),
//                    TotalPurchase = table.Column<int>(type: "int", nullable: false),
//                    SupplierName = table.Column<string>(type: "nvarchar(max)", nullable: false),
//                    DeliveryDate = table.Column<DateTime>(type: "datetime2", nullable: false),
//                    PurchaseDate = table.Column<DateTime>(type: "datetime2", nullable: false),
//                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
//                    PurchaseStatus = table.Column<string>(type: "nvarchar(max)", nullable: false),
//                    ReasonforStatus = table.Column<string>(type: "nvarchar(max)", nullable: false),
//                    DateAdded = table.Column<DateTime>(type: "datetime2", nullable: false),
//                    Currency = table.Column<string>(type: "nvarchar(max)", nullable: false)
//                },
//                constraints: table =>
//                {
//                    table.PrimaryKey("PK_Purchases", x => x.PurchaseId);
//                });

//            migrationBuilder.CreateTable(
//                name: "PurchaseStatusTable",
//                columns: table => new
//                {
//                    purchaseId = table.Column<int>(type: "int", nullable: false)
//                        .Annotation("SqlServer:Identity", "1, 1"),
//                    PurchaseStatus = table.Column<string>(type: "nvarchar(max)", nullable: false),
//                    ReasonforStatus = table.Column<string>(type: "nvarchar(max)", nullable: false),
//                    DateAdded = table.Column<DateTime>(type: "datetime2", nullable: false)
//                },
//                constraints: table =>
//                {
//                    table.PrimaryKey("PK_PurchaseStatusTable", x => x.purchaseId);
//                });

//            migrationBuilder.CreateTable(
//                name: "ReturnedStatusTable",
//                columns: table => new
//                {
//                    ReturnedID = table.Column<int>(type: "int", nullable: false)
//                        .Annotation("SqlServer:Identity", "1, 1"),
//                    ReturnedStatus = table.Column<string>(type: "nvarchar(max)", nullable: false),
//                    ReturnedDescription = table.Column<string>(type: "nvarchar(max)", nullable: false)
//                },
//                constraints: table =>
//                {
//                    table.PrimaryKey("PK_ReturnedStatusTable", x => x.ReturnedID);
//                });

//            migrationBuilder.CreateTable(
//                name: "ReturnedStock",
//                columns: table => new
//                {
//                    ReturnedId = table.Column<int>(type: "int", nullable: false)
//                        .Annotation("SqlServer:Identity", "1, 1"),
//                    ReturnedStatus = table.Column<string>(type: "nvarchar(max)", nullable: false),
//                    ReturnReason = table.Column<string>(type: "nvarchar(max)", nullable: false),
//                    ReturnedQuantity = table.Column<int>(type: "int", nullable: false),
//                    BrandName = table.Column<string>(type: "nvarchar(max)", nullable: false),
//                    ItemName = table.Column<string>(type: "nvarchar(max)", nullable: false),
//                    DateReturned = table.Column<DateTime>(type: "datetime2", nullable: false)
//                },
//                constraints: table =>
//                {
//                    table.PrimaryKey("PK_ReturnedStock", x => x.ReturnedId);
//                });

//            migrationBuilder.CreateTable(
//                name: "RoleClaimsTable",
//                columns: table => new
//                {
//                    RolesClaimsTableId = table.Column<int>(type: "int", nullable: false)
//                        .Annotation("SqlServer:Identity", "1, 1"),
//                    ClaimName = table.Column<string>(type: "nvarchar(max)", nullable: false)
//                },
//                constraints: table =>
//                {
//                    table.PrimaryKey("PK_RoleClaimsTable", x => x.RolesClaimsTableId);
//                });

//            migrationBuilder.CreateTable(
//                name: "RolesTable",
//                columns: table => new
//                {
//                    RolesID = table.Column<int>(type: "int", nullable: false)
//                        .Annotation("SqlServer:Identity", "1, 1"),
//                    RoleName = table.Column<string>(type: "nvarchar(max)", nullable: false)
//                },
//                constraints: table =>
//                {
//                    table.PrimaryKey("PK_RolesTable", x => x.RolesID);
//                });

//            migrationBuilder.CreateTable(
//                name: "Sales",
//                columns: table => new
//                {
//                    SalesId = table.Column<int>(type: "int", nullable: false)
//                        .Annotation("SqlServer:Identity", "1, 1"),
//                    BrandName = table.Column<string>(type: "nvarchar(max)", nullable: false),
//                    ItemName = table.Column<string>(type: "nvarchar(max)", nullable: false),
//                    Quantity = table.Column<int>(type: "int", nullable: false),
//                    CustomerName = table.Column<string>(type: "nvarchar(max)", nullable: false),
//                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
//                    SellingPrice = table.Column<int>(type: "int", nullable: false),
//                    TotalSales = table.Column<int>(type: "int", nullable: false),
//                    SalesStatus = table.Column<string>(type: "nvarchar(max)", nullable: false),
//                    ReasonForSalesStatus = table.Column<string>(type: "nvarchar(max)", nullable: false),
//                    DateAdded = table.Column<DateTime>(type: "datetime2", nullable: false),
//                    Comments = table.Column<string>(type: "nvarchar(max)", nullable: false),
//                    Department = table.Column<string>(type: "nvarchar(max)", nullable: false)
//                },
//                constraints: table =>
//                {
//                    table.PrimaryKey("PK_Sales", x => x.SalesId);
//                });

//            migrationBuilder.CreateTable(
//                name: "SparePart",
//                columns: table => new
//                {
//                    PartID = table.Column<int>(type: "int", nullable: false)
//                        .Annotation("SqlServer:Identity", "1, 1"),
//                    PartName = table.Column<string>(type: "nvarchar(max)", nullable: false),
//                    PartDescription = table.Column<string>(type: "nvarchar(max)", nullable: false)
//                },
//                constraints: table =>
//                {
//                    table.PrimaryKey("PK_SparePart", x => x.PartID);
//                });

//            migrationBuilder.CreateTable(
//                name: "StockIn",
//                columns: table => new
//                {
//                    StockID = table.Column<int>(type: "int", nullable: false)
//                        .Annotation("SqlServer:Identity", "1, 1"),
//                    InvoiceNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
//                    InvoiceDate = table.Column<DateTime>(type: "datetime2", nullable: false),
//                    LPONumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
//                    LPODate = table.Column<DateTime>(type: "datetime2", nullable: false),
//                    StockInDate = table.Column<DateTime>(type: "datetime2", nullable: false),
//                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
//                    SupplierName = table.Column<string>(type: "nvarchar(max)", nullable: false)
//                },
//                constraints: table =>
//                {
//                    table.PrimaryKey("PK_StockIn", x => x.StockID);
//                });

//            migrationBuilder.CreateTable(
//                name: "Suppliers",
//                columns: table => new
//                {
//                    SupplierId = table.Column<int>(type: "int", nullable: false)
//                        .Annotation("SqlServer:Identity", "1, 1"),
//                    SupplierName = table.Column<string>(type: "nvarchar(max)", nullable: false),
//                    CompanyName = table.Column<string>(type: "nvarchar(max)", nullable: false),
//                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
//                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: false)
//                },
//                constraints: table =>
//                {
//                    table.PrimaryKey("PK_Suppliers", x => x.SupplierId);
//                });

//            migrationBuilder.CreateTable(
//                name: "Tickets",
//                columns: table => new
//                {
//                    Id = table.Column<int>(type: "int", nullable: false)
//                        .Annotation("SqlServer:Identity", "1, 1"),
//                    ServiceName = table.Column<string>(type: "nvarchar(max)", nullable: false),
//                    itemName = table.Column<string>(type: "nvarchar(max)", nullable: false),
//                    siteArea = table.Column<string>(type: "nvarchar(max)", nullable: false),
//                    clientLocation = table.Column<string>(type: "nvarchar(max)", nullable: false),
//                    description = table.Column<string>(type: "nvarchar(max)", nullable: false),
//                    subject = table.Column<string>(type: "nvarchar(max)", nullable: false),
//                    priorityName = table.Column<string>(type: "nvarchar(max)", nullable: false),
//                    TicketTitle = table.Column<string>(type: "nvarchar(max)", nullable: false),
//                    ServiceIssue = table.Column<string>(type: "nvarchar(max)", nullable: false),
//                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false),
//                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
//                    CreatorEmail = table.Column<string>(type: "nvarchar(max)", nullable: false),
//                    DateAsigned = table.Column<DateTime>(type: "datetime2", nullable: false),
//                    assignedtoNames = table.Column<string>(type: "nvarchar(max)", nullable: false),
//                    ClientName = table.Column<string>(type: "nvarchar(max)", nullable: false),
//                    Resolution = table.Column<string>(type: "nvarchar(max)", nullable: false),
//                    Escalation = table.Column<string>(type: "nvarchar(max)", nullable: false)
//                },
//                constraints: table =>
//                {
//                    table.PrimaryKey("PK_Tickets", x => x.Id);
//                });

//            migrationBuilder.CreateTable(
//                name: "UserStatusTable",
//                columns: table => new
//                {
//                    userstatusId = table.Column<int>(type: "int", nullable: false)
//                        .Annotation("SqlServer:Identity", "1, 1"),
//                    userId = table.Column<string>(type: "nvarchar(max)", nullable: false),
//                    UsertActive = table.Column<bool>(type: "bit", nullable: false),
//                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
//                    StatusDescription = table.Column<string>(type: "nvarchar(max)", nullable: false),
//                    ReasonforStatus = table.Column<string>(type: "nvarchar(max)", nullable: false),
//                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
//                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
//                    Totaldays = table.Column<int>(type: "int", nullable: false)
//                },
//                constraints: table =>
//                {
//                    table.PrimaryKey("PK_UserStatusTable", x => x.userstatusId);
//                });

//            migrationBuilder.CreateTable(
//                name: "AspNetRoleClaims",
//                columns: table => new
//                {
//                    Id = table.Column<int>(type: "int", nullable: false)
//                        .Annotation("SqlServer:Identity", "1, 1"),
//                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false),
//                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
//                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
//                },
//                constraints: table =>
//                {
//                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
//                    table.ForeignKey(
//                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
//                        column: x => x.RoleId,
//                        principalTable: "AspNetRoles",
//                        principalColumn: "Id",
//                        onDelete: ReferentialAction.Cascade);
//                });

//            migrationBuilder.CreateTable(
//                name: "AspNetUserClaims",
//                columns: table => new
//                {
//                    Id = table.Column<int>(type: "int", nullable: false)
//                        .Annotation("SqlServer:Identity", "1, 1"),
//                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
//                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
//                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
//                },
//                constraints: table =>
//                {
//                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
//                    table.ForeignKey(
//                        name: "FK_AspNetUserClaims_PayhouseDragonFlyUsers_UserId",
//                        column: x => x.UserId,
//                        principalTable: "PayhouseDragonFlyUsers",
//                        principalColumn: "Id",
//                        onDelete: ReferentialAction.Cascade);
//                });

//            migrationBuilder.CreateTable(
//                name: "AspNetUserLogins",
//                columns: table => new
//                {
//                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
//                    ProviderKey = table.Column<string>(type: "nvarchar(450)", nullable: false),
//                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
//                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
//                },
//                constraints: table =>
//                {
//                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
//                    table.ForeignKey(
//                        name: "FK_AspNetUserLogins_PayhouseDragonFlyUsers_UserId",
//                        column: x => x.UserId,
//                        principalTable: "PayhouseDragonFlyUsers",
//                        principalColumn: "Id",
//                        onDelete: ReferentialAction.Cascade);
//                });

//            migrationBuilder.CreateTable(
//                name: "AspNetUserRoles",
//                columns: table => new
//                {
//                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
//                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false)
//                },
//                constraints: table =>
//                {
//                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
//                    table.ForeignKey(
//                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
//                        column: x => x.RoleId,
//                        principalTable: "AspNetRoles",
//                        principalColumn: "Id",
//                        onDelete: ReferentialAction.Cascade);
//                    table.ForeignKey(
//                        name: "FK_AspNetUserRoles_PayhouseDragonFlyUsers_UserId",
//                        column: x => x.UserId,
//                        principalTable: "PayhouseDragonFlyUsers",
//                        principalColumn: "Id",
//                        onDelete: ReferentialAction.Cascade);
//                });

//            migrationBuilder.CreateTable(
//                name: "AspNetUserTokens",
//                columns: table => new
//                {
//                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
//                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
//                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
//                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
//                },
//                constraints: table =>
//                {
//                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
//                    table.ForeignKey(
//                        name: "FK_AspNetUserTokens_PayhouseDragonFlyUsers_UserId",
//                        column: x => x.UserId,
//                        principalTable: "PayhouseDragonFlyUsers",
//                        principalColumn: "Id",
//                        onDelete: ReferentialAction.Cascade);
//                });

//            migrationBuilder.CreateIndex(
//                name: "IX_AspNetRoleClaims_RoleId",
//                table: "AspNetRoleClaims",
//                column: "RoleId");

//            migrationBuilder.CreateIndex(
//                name: "RoleNameIndex",
//                table: "AspNetRoles",
//                column: "NormalizedName",
//                unique: true,
//                filter: "[NormalizedName] IS NOT NULL");

//            migrationBuilder.CreateIndex(
//                name: "IX_AspNetUserClaims_UserId",
//                table: "AspNetUserClaims",
//                column: "UserId");

//            migrationBuilder.CreateIndex(
//                name: "IX_AspNetUserLogins_UserId",
//                table: "AspNetUserLogins",
//                column: "UserId");

//            migrationBuilder.CreateIndex(
//                name: "IX_AspNetUserRoles_RoleId",
//                table: "AspNetUserRoles",
//                column: "RoleId");

//            migrationBuilder.CreateIndex(
//                name: "EmailIndex",
//                table: "PayhouseDragonFlyUsers",
//                column: "NormalizedEmail");

//            migrationBuilder.CreateIndex(
//                name: "UserNameIndex",
//                table: "PayhouseDragonFlyUsers",
//                column: "NormalizedUserName",
//                unique: true,
//                filter: "[NormalizedUserName] IS NOT NULL");
//        }

//        /// <inheritdoc />
//        protected override void Down(MigrationBuilder migrationBuilder)
//        {
//            migrationBuilder.DropTable(
//                name: "AddBatchDetail");

//            migrationBuilder.DropTable(
//                name: "AddBrand");

//            migrationBuilder.DropTable(
//                name: "AddItem");

//            migrationBuilder.DropTable(
//                name: "AddProductDetails");

//            migrationBuilder.DropTable(
//                name: "AddStock");

//            migrationBuilder.DropTable(
//                name: "AspNetRoleClaims");

//            migrationBuilder.DropTable(
//                name: "AspNetUserClaims");

//            migrationBuilder.DropTable(
//                name: "AspNetUserLogins");

//            migrationBuilder.DropTable(
//                name: "AspNetUserRoles");

//            migrationBuilder.DropTable(
//                name: "AspNetUserTokens");

//            migrationBuilder.DropTable(
//                name: "Category");

//            migrationBuilder.DropTable(
//                name: "Claim_Role_Map");

//            migrationBuilder.DropTable(
//                name: "Customer");

//            migrationBuilder.DropTable(
//                name: "Departments");

//            migrationBuilder.DropTable(
//                name: "Designation");

//            migrationBuilder.DropTable(
//                name: "Invoice_Item_Quantity");

//            migrationBuilder.DropTable(
//                name: "InvoiceLines");

//            migrationBuilder.DropTable(
//                name: "InvoiceLinesDetails");

//            migrationBuilder.DropTable(
//                name: "InvoiceNo");

//            migrationBuilder.DropTable(
//                name: "Item_Numbering_Stock");

//            migrationBuilder.DropTable(
//                name: "LPO_No");

//            migrationBuilder.DropTable(
//                name: "OtherRoles");

//            migrationBuilder.DropTable(
//                name: "Purchases");

//            migrationBuilder.DropTable(
//                name: "PurchaseStatusTable");

//            migrationBuilder.DropTable(
//                name: "ReturnedStatusTable");

//            migrationBuilder.DropTable(
//                name: "ReturnedStock");

//            migrationBuilder.DropTable(
//                name: "RoleClaimsTable");

//            migrationBuilder.DropTable(
//                name: "RolesTable");

//            migrationBuilder.DropTable(
//                name: "Sales");

//            migrationBuilder.DropTable(
//                name: "SparePart");

//            migrationBuilder.DropTable(
//                name: "StockIn");

//            migrationBuilder.DropTable(
//                name: "Suppliers");

//            migrationBuilder.DropTable(
//                name: "Tickets");

//            migrationBuilder.DropTable(
//                name: "UserStatusTable");

//            migrationBuilder.DropTable(
//                name: "AspNetRoles");

//            migrationBuilder.DropTable(
//                name: "PayhouseDragonFlyUsers");
//        }
//    }
//}
