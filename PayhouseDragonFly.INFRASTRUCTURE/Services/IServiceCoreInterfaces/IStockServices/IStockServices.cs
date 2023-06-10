using PayhouseDragonFly.CORE.ConnectorClasses.Response.BseResponse;
using PayhouseDragonFly.CORE.ConnectorClasses.Response.StockResponse;
using PayhouseDragonFly.CORE.DTOs.Stock;
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



    }
}
