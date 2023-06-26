using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PayhouseDragonFly.CORE.ConnectorClasses.Response.BseResponse;
using PayhouseDragonFly.CORE.ConnectorClasses.Response.StockResponse;
using PayhouseDragonFly.CORE.DTOs.Stock;
using PayhouseDragonFly.CORE.DTOs.Stock.Invoicing_vm;
using PayhouseDragonFly.CORE.Models.Stock;
using PayhouseDragonFly.INFRASTRUCTURE.Services.IServiceCoreInterfaces.IStockServices;

namespace PayhouseDragonFly.API.Controllers.Stock
{
    [Route("api/[controller]", Name = "Stock")]
    [ApiController]
    public class StockController : ControllerBase
    {
        private readonly IStockServices _stockServices;
        public StockController(IStockServices stockServices)
        {
            _stockServices = stockServices;
        }

        [HttpPost]
        [Route("AddBrand")]
        public async Task<StockResponse> AddBrand(AddBrandvm addBrandvm)
        {
            return await _stockServices.AddBrand(addBrandvm);

        }
        [HttpGet]
        [Route("GetAllBrands")]
        public async Task<StockResponse> GetAllBrand()
        {
            return await _stockServices.GetAllBrand();
        }
        [HttpPost]
        [Route("AddItem")]
        public async Task<StockResponse> AddItem(AddItemvm addItemvm)
        {
            return await _stockServices.AddItem(addItemvm);
        }
        [HttpGet]
        [Route("GetAllItems")]
        public async Task<StockResponse> GetAllItems()
        {
            return await _stockServices.GetAllItems();

        }

        [HttpPost]
        [Route("AddStock")]
        public async Task<StockResponse> AddStock(AddStockvm addStockvm)
        {
            return await _stockServices.AddStock(addStockvm);

        }
        [HttpPost]
        [Route("AddCustomer")]
        public async Task<StockResponse> AddCustomer(AddCustomervm addCustomervm)
        {
            return await _stockServices.AddCustomer(addCustomervm);

        }
        [HttpGet]
        [Route("GetAllCustomers")]
        public async Task<StockResponse> GetAllCustomers()
        {
            return await _stockServices.GetAllCustomers();

        }
        [HttpGet]
        [Route("GetAllStock")]
        public async Task<StockResponse> GetAllStock()
        {
            return await _stockServices.GetAllStock();

        }
        [HttpPost]
        [Route("AddSupplier")]
        public async Task<StockResponse> AddSupplier(AddSupplierVm addSupplierVm)
        {
            return await _stockServices.AddSupplier(addSupplierVm);

        }

        [HttpGet]
        [Route("GetAllSuppliers")]
        public async Task<StockResponse> GetAllSuppliers()
        {
            return await _stockServices.GetAllSuppliers();

        }
        [HttpPost]
        [Route("AddSales")]
        public async Task<StockResponse> AddSales(AddSalesOrdersVm addSalesOrdersVm)
        {
            return await _stockServices.AddSales(addSalesOrdersVm);

        }

        [HttpGet]
        [Route("GetAllSales")]
        public async Task<StockResponse> GetAllSales()
        {
            return await _stockServices.GetAllSales();

        }

        [HttpPost]
        [Route("AddPurchases")]
        public async Task<StockResponse> AddPurchase(AddPurchaseOrderVm addPurchaseOrderVm)
        {
            return await _stockServices.AddPurchase(addPurchaseOrderVm);
        }

        [HttpGet]
        [Route("GetAllPurchases")]
        public async Task<StockResponse> GetAllPurchases()
        {
            return await _stockServices.GetAllPurchases();
        }

        [HttpPost]
        [Route("UpdateStockQuantity")]
        public async Task<StockResponse> UpdateStockQuantity(int itemid, int quantityadded)
        {
            return await _stockServices.UpdateStockQuantity(itemid, quantityadded);

        }
        [HttpPost]
        [Route("GetStockByName")]
        public async Task<StockResponse> GetItemByName(string itemname)
        {
            return await _stockServices.GetItemByName(itemname);
        }

        [HttpPost]
        [Route("GetStockById")]
        public async Task<StockResponse> GetItemById(int itemid)
        {
            return await _stockServices.GetItemById(itemid);
        }
        [HttpPost]
        [Route("GetPurchaseById")]
        public async Task<StockResponse> GetPurchaseById(int purchaseId)
        {
            return await _stockServices.GetPurchaseById(purchaseId);
        }
        [HttpPost]
        [Route("ChangePurchaseStatus")]
        public async Task<StockResponse> ChangePurchaseStatus(PurchaseStatusvm purchaseStatusvm)
        {
            return await _stockServices.ChangePurchaseStatus(purchaseStatusvm);

        }
        [HttpPost]
        [Route("GetSalesbyId")]
        public async Task<StockResponse> GetSalesbyId(int salesId)
        {
            return await _stockServices.GetSalesbyId(salesId);
        }
        [HttpPost]
        [Route("ChangeSalesStatus")]
        public async Task<StockResponse> ChangeSalesStatus(ChangeSalesStatusvm changeSalesStatusvm)
        {
            return await _stockServices.ChangeSalesStatus(changeSalesStatusvm);

        }

        [HttpPost]
        [Route("AddReturnedStatus")]
        public async Task<StockResponse> AddReturnedStatus(AddReturnedStatusvm addReturnedStatusvm)
        {
            return await _stockServices.AddReturnedStatus(addReturnedStatusvm);
        }
        [HttpGet]
        [Route("GetAllReturnedStatus")]
        public async Task<StockResponse> GetAllReturnedStatus()
        {
            return await _stockServices.GetAllReturnedStatus();
        }
        [HttpPost]
        [Route("AddReturnedStock")]
        public async Task<StockResponse> AddReturnedStock(AddReturnedStockvm addReturnedStockvm)
        {
            return await _stockServices.AddReturnedStock(addReturnedStockvm);
        }
        [HttpGet]
        [Route("GetAllReturnedStock")]
        public async Task<StockResponse> GetAllReturnedStock()
        {
            return await _stockServices.GetAllReturnedStock();
        }

        [HttpPost]
        [Route("SearchStock")]
        public async Task<StockResponse> SearchForStock(string search_query)
        {
            return await _stockServices.SearchForStock(search_query);
        }
        [HttpPost]
        [Route("SearchStockIn")]
        public async Task<StockResponse> SearchForStockIn(string search_query)
        {
            return await _stockServices.SearchForStockIn(search_query);
        }
        [HttpPost]
        [Route("SearchStockOut")]
        public async Task<StockResponse> SearchForStockOut(string search_query)
        {
            return await _stockServices.SearchForStockOut(search_query);
        }
        [HttpPost]
        [Route("SearchCustomer")]
        public async Task<StockResponse> SearchForCustomer(string search_query)
        {
            return await _stockServices.SearchForCustomer(search_query);
        }
        [HttpPost]
        [Route("SearchSupplier")]
        public async Task<StockResponse> SearchForSupplier(string search_query)
        {
            return await _stockServices.SearchForSupplier(search_query);
        }

        [HttpPost]
        [Route("DeleteStockOut")]
        public async Task<StockResponse> DeleteStockOut(int salesId)
        {
            return await _stockServices.DeleteStockOut(salesId);
        }

        [HttpPost]
        [Route("EditStockOut")]
        public async Task<StockResponse> EditSales(editSalesvm salesvm)
        {
            return await _stockServices.EditSales(salesvm);
        }
        [HttpPost]
        [Route("EditCustomer")]
        public async Task<StockResponse> EditCustomer(EditCustomervm editCustomervm)
        {
            return await _stockServices.EditCustomer(editCustomervm);
        }
        [HttpPost]
        [Route("GetCustomerByID")]
        public async Task<StockResponse> GetCustomerById(int customerId)
        {
            return await _stockServices.GetCustomerById(customerId);
        }
        [HttpPost]
        [Route("GetSupplierbyId")]
        public async Task<StockResponse> GetSupplierById(int supplierId)
        {
            return await _stockServices.GetSupplierById(supplierId);
        }
        [HttpPost]
        [Route("EditSupplier")]
        public async Task<StockResponse> EditSupplier(editSuppliervm suppliervm)
        {
            return await _stockServices.EditSupplier(suppliervm);
        }
        [HttpPost]
        [Route("GetBrandById")]
        public async Task<StockResponse> GetBrandById(int BrandId)
        {
            return await _stockServices.GetBrandById(BrandId);
        }
        [HttpPost]
        [Route("GetItemById")]
        public async Task<StockResponse> GetItemsById(int ItemId)
        {
            return await _stockServices.GetItemsById(ItemId);
        }

        [HttpPost]
        [Route("EditBrand")]
        public async Task<StockResponse> EditBrand(EditBrandvm editBrandvm)
        {
            return await _stockServices.EditBrand(editBrandvm);
        }
        [HttpPost]
        [Route("EditItem")]
        public async Task<StockResponse> EditItem(EditItemvm editItemvm)
        {
            return await _stockServices.EditItem(editItemvm);
        }
        [HttpPost]
        [Route("EditStockIn")]
        public async Task<StockResponse> EditStockIn(EditPurchasevm editPurchasevm)
        {
            return await _stockServices.EditStockIn(editPurchasevm);
        }
        [HttpGet]
        [Route("GenerateExcel")]
        public async Task<StockResponse> GenerateExcel()
        {
            return await _stockServices.GenerateExcel();
        }
        [HttpPost]
        [Route("AddCategory")]
        public async Task<StockResponse> AddCategory(AddCategoryvm addCategoryvm)
        {
            return await _stockServices.AddCategory(addCategoryvm);

        }
        [HttpGet]
        [Route("GetAllCategory")]
        public async Task<StockResponse> GetAllCategory()
        {
            return await _stockServices.GetAllCategory();
        }
        [HttpPost]
        [Route("AddInvoiceDetails")]
        public async Task<StockResponse> AddInvoiceDetails(StockInvm stockInvm)
        {
            return await _stockServices.AddInvoiceDetails(stockInvm);

        }
        [HttpGet]
        [Route("GetInvoiceDetails")]
        public async Task<StockResponse> GetInvoiceDetails()
        {
            return await _stockServices.GetInvoiceDetails();
        }
        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpPost]
        [Route("AddBatchDetails")]
        public async Task<StockResponse> AddBatchDetails(AddBatchDetailsvm addBatchDetailsvm)
        {
            return await _stockServices.AddBatchDetails(addBatchDetailsvm);

        }
        [HttpGet]
        [Route("GetInvoiceLines")]
        public async Task<StockResponse> GetInvoiceLines()
        {
            return await _stockServices.GetInvoiceLines();
        }
        [HttpPost]
        [Route("AddProductDetails")]
        public async Task<StockResponse> AddProductDetails(AddProductDetailvm addProductDetailvm)
        {
            return await _stockServices.AddProductDetails(addProductDetailvm);

        }
        [HttpGet]
        [Route("GetProductDetails")]
        public async Task<StockResponse> GetProductDetails()
        {
            return await _stockServices.GetProductDetails();
        }
        [HttpPost]
        [Route("GetInvoiceByNumber")]
        public async Task<StockResponse> GetInvoiceByNumber(string InvoiceNumber)
        {
            return await _stockServices.GetInvoiceByNumber(InvoiceNumber);
        }
        [HttpPost]
        [Route("AddParts")]
        public async Task<StockResponse> AddSparePart(AddPartvm addPartvm)
        {
            return await _stockServices.AddSparePart(addPartvm);

        }
        [HttpGet]
        [Route("GetAllParts")]
        public async Task<StockResponse> GetAllParts()
        {
            return await _stockServices.GetAllParts();
        }
        [HttpPost]
        [Route("AddSpareParts")]
        public async Task<StockResponse> AddPartsSpare(AddSparePartvm addSparePartvm)
        {
            return await _stockServices.AddPartsSpare(addSparePartvm);

        }
        [HttpGet]
        [Route("GetAllSpareParts")]
        public async Task<StockResponse> GetAllSpareParts()
        {
            return await _stockServices.GetAllSpareParts();
        }
        [HttpPost]
        [Route("AddInvoiceQuantity")]
        public async Task<StockResponse> Add_Invoice_Item_Quantity(invoice_item_quantity_vm vm)
        {
            return await _stockServices.Add_Invoice_Item_Quantity(vm);

        }
        [HttpGet]
        [Route("GetGeneratedref")]
        public async Task<string> GetGeneratedref()
        {
            return await _stockServices.GetGeneratedref();
        }
        [HttpPost]
        [Route("GetInvoiceLinesByInvoiceNumber")]
        public async Task<StockResponse> GetInvoiceLinByNumber(string invoiceNumber)
        {
            return await _stockServices.GetInvoiceLinByNumber(invoiceNumber);
        }
        [HttpPost]
        [Route("GetInvoiceItemByID")]
        public async Task<StockResponse> GetInvoiceItemByID(int invoicelineId)
        {
            return await _stockServices.GetInvoiceItemByID(invoicelineId);
        }
        [HttpPost]
        [Route("GetProductDetailsbyid")]
        public async Task<StockResponse> GetProductDetailsbyid(int itemID)
        {
            return await _stockServices.GetProductDetailsbyid(itemID);
        }
        [HttpPost]
        [Route("GetProductNumbering")]
        public async Task<StockResponse> GetProduct_Numbers_ByReference(string reference)
        {
            return await _stockServices.GetProduct_Numbers_ByReference(reference);
        }
        [HttpPost]
        [Route("GetProductlinebyid")]
        public async Task<StockResponse> GetProduvctLineyId(int product_line_id)
        {
            return await _stockServices.GetProduvctLineyId(product_line_id);
        }


    }
}
