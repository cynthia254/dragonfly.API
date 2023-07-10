using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PayhouseDragonFly.CORE.ConnectorClasses.Response.BseResponse;
using PayhouseDragonFly.CORE.ConnectorClasses.Response.StockResponse;
using PayhouseDragonFly.CORE.DTOs.Stock;
using PayhouseDragonFly.CORE.DTOs.Stock.Invoicing_vm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayhouseDragonFly.INFRASTRUCTURE.Services.IServiceCoreInterfaces.IStockServices
{
    public interface IStockServices
    {
        Task<StockResponse> AddBrand(AddBrandvm addBrandvm);
        Task<StockResponse> GetAllBrand();
        Task<StockResponse> AddItem(AddItemvm addItemvm);
        Task<StockResponse> GetAllItems();
        Task<StockResponse> AddStock(AddStockvm addStockvm);
        Task<StockResponse> AddCustomer(AddCustomervm addCustomervm);
        Task<StockResponse> GetAllCustomers();
        Task<StockResponse> GetAllStock();
        Task<StockResponse> UpdateStockQuantity(int itemid, int quantityadded);
        Task<StockResponse> AddSupplier(AddSupplierVm addSupplierVm);
        Task<StockResponse> GetAllPurchases();
         Task<StockResponse> GetAllSuppliers();
        Task<StockResponse> AddSales(AddSalesOrdersVm addSalesOrdersVm);
         Task<StockResponse> GetAllSales();
        Task<StockResponse> AddPurchase(AddPurchaseOrderVm addPurchaseOrderVm);
        Task<StockResponse> GetItemByName(string itemname);
        Task<StockResponse> GetItemById(int itemid);
        Task<StockResponse> GetPurchaseById(int purchaseId);
        Task<StockResponse> ChangePurchaseStatus(PurchaseStatusvm purchaseStatusvm);
        Task<StockResponse> GetSalesbyId(int salesId);
        Task<StockResponse> ChangeSalesStatus(ChangeSalesStatusvm changeSalesStatusvm);
        Task<StockResponse> AddReturnedStatus(AddReturnedStatusvm addReturnedStatusvm);
        Task<StockResponse> GetAllReturnedStatus();
         Task<StockResponse> AddReturnedStock(AddReturnedStockvm addReturnedStockvm);
        Task<StockResponse> GetAllReturnedStock();
        Task<StockResponse> SearchForStock(string search_query);
       Task<StockResponse> SearchForStockIn(string search_query);
       Task<StockResponse> SearchForStockOut(string search_query);
        Task<StockResponse> SearchForCustomer(string search_query);
        Task<StockResponse> SearchForSupplier(string search_query);
        Task<StockResponse> EditSales(editSalesvm salesvm);
        Task<StockResponse> DeleteStockOut(int salesId);
        Task<StockResponse> EditCustomer(EditCustomervm editCustomervm);
         Task<StockResponse> GetCustomerById(int customerId);
         Task<StockResponse> GetSupplierById(int supplierId);
        Task<StockResponse> EditSupplier(editSuppliervm suppliervm);
         Task<StockResponse> GetBrandById(int BrandId);
     Task<StockResponse> GetItemsById(int ItemId);
        Task<StockResponse> EditBrand(EditBrandvm editBrandvm);
         Task<StockResponse> EditItem(EditItemvm editItemvm);
        Task<StockResponse> EditStockIn(EditPurchasevm editPurchasevm);
        Task<StockResponse> GenerateExcel();
        Task<StockResponse> AddCategory(AddCategoryvm addCategoryvm);
        Task<StockResponse> GetAllCategory();
         Task<StockResponse> AddInvoiceDetails(StockInvm stockInvm);
        Task<StockResponse> GetInvoiceDetails();
       Task<StockResponse> AddBatchDetails(AddBatchDetailsvm addBatchDetailsvm);
         Task<StockResponse> GetInvoiceLines();

         Task<StockResponse> AddProductDetails(AddProductDetailvm addProductDetailvm);
         Task<StockResponse> GetProductDetails();
         Task<StockResponse> GetInvoiceByNumber(string InvoiceNumber);
        Task<StockResponse> AddSparePart(AddPartvm addPartvm);
       Task<StockResponse> GetAllParts();
        Task<StockResponse> AddPartsSpare(AddSparePartvm addSparePartvm);
        Task<StockResponse> GetAllSpareParts();
         Task<StockResponse> Add_Invoice_Item_Quantity(invoice_item_quantity_vm vm);
        Task<string> GetGeneratedref();
        Task<StockResponse> GetInvoiceLinByNumber(string invoiceNumber);
         Task<StockResponse> GetInvoiceItemByID(int invoicelineId);
        Task<StockResponse> GetProductDetailsbyid(int itemID);
        Task<StockResponse> GetProduct_Numbers_ByReference(string reference);
        Task<StockResponse> GetProduvctLineyId(int product_line_id);
        Task<StockResponse> SearchForInvoice(string search_query);
         Task<StockResponse> SearchForItem(string search_query);
         Task<StockResponse> GetItemsbyBrandName(string BrandName);
        Task<StockResponse> SearchForInvoiceLines(string search_query);
        Task<StockResponse> UploadData([FromBody] List<uploadDatavm> data);
         Task<StockResponse> UploadingData(IFormFile file);
    }
}
