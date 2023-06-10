using Microsoft.AspNetCore.Mvc;
using PayhouseDragonFly.CORE.ConnectorClasses.Response.BseResponse;
using PayhouseDragonFly.CORE.ConnectorClasses.Response.StockResponse;
using PayhouseDragonFly.CORE.DTOs.Stock;
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
            return await _stockServices.UpdateStockQuantity(itemid,quantityadded);

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
    }
}
