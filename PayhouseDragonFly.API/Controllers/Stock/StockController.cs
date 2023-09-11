using DocumentFormat.OpenXml.Office2010.Excel;
using EnumsNET;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PayhouseDragonFly.CORE.ConnectorClasses.Response;
using PayhouseDragonFly.CORE.ConnectorClasses.Response.BseResponse;
using PayhouseDragonFly.CORE.ConnectorClasses.Response.StockResponse;
using PayhouseDragonFly.CORE.DTOs.Stock;
using PayhouseDragonFly.CORE.DTOs.Stock.Invoicing_vm;
using PayhouseDragonFly.CORE.Models.Stock;
using PayhouseDragonFly.INFRASTRUCTURE.Services.ExtraServices.RoleChecker;
using PayhouseDragonFly.INFRASTRUCTURE.Services.IServiceCoreInterfaces.IExtraServices;
using PayhouseDragonFly.INFRASTRUCTURE.Services.IServiceCoreInterfaces.IStockServices;
using PayhouseDragonFly.INFRASTRUCTURE.Services.RoleServices;

namespace PayhouseDragonFly.API.Controllers.Stock
{
    [Route("api/[controller]", Name = "Stock")]
    [ApiController]
    public class StockController : ControllerBase
    {
        private readonly IStockServices _stockServices;

        private readonly ILoggeinUserServices _loggeinuser;

        private readonly IRoleChecker _rolechecker;
        private readonly IRoleServices _roleservices;

        public StockController(IStockServices stockServices, ILoggeinUserServices loggeinuser,
            IRoleChecker rolechecker,
            IRoleServices roleservices)
        {
            _stockServices = stockServices;

            _rolechecker = rolechecker;
            _roleservices = roleservices;
            _loggeinuser = loggeinuser;
        }
        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpPost]
        [Route("AddBrand")]
        public async Task<StockResponse> AddBrand(AddBrandvm addBrandvm)
        {
            var roleclaimname = "CanAddStock";
            var loggedinuser = _loggeinuser.LoggedInUser().Result;
           var master_rolechecker_response= await _roleservices.MasterRoleChecker(loggedinuser.Id, roleclaimname);

            if (master_rolechecker_response)
            {

                return await _stockServices.AddBrand(addBrandvm);

            }
            else
            {
                return new StockResponse(false, "You have no permission to access this", null);
            }
              
             
                
           
            

        }
        [Authorize(AuthenticationSchemes = "Bearer")]

        [HttpGet]
        [Route("GetAllBrands")]
        public async Task<StockResponse> GetAllBrand()
        {
            return await _stockServices.GetAllBrand();
        }
        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpPost]
        [Route("AddItem")]
        public async Task<StockResponse> AddItem(AddItemvm addItemvm)
        {
            return await _stockServices.AddItem(addItemvm);
        }
        [Authorize(AuthenticationSchemes = "Bearer")]
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
        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpPost]
        [Route("AddCustomer")]
        public async Task<StockResponse> AddCustomer(AddCustomervm addCustomervm)
        {
            return await _stockServices.AddCustomer(addCustomervm);

        }
        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpGet]
        [Route("GetAllCustomers")]
        public async Task<StockResponse> GetAllCustomers()
        {
            return await _stockServices.GetAllCustomers();

        }
        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpGet]
        [Route("GetAllStock")]
        public async Task<StockResponse> GetAllStock()
        {
            return await _stockServices.GetAllStock();

        }
        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpPost]
        [Route("AddSupplier")]
        public async Task<StockResponse> AddSupplier(AddSupplierVm addSupplierVm)
        {
            return await _stockServices.AddSupplier(addSupplierVm);

        }
        [Authorize(AuthenticationSchemes = "Bearer")]
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
        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpGet]
        [Route("GetAllReturnedStatus")]
        public async Task<StockResponse> GetAllReturnedStatus()
        {
            return await _stockServices.GetAllReturnedStatus();
        }
        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpPost]
        [Route("AddReturnedStock")]
        public async Task<StockResponse> AddReturnedStock(AddReturnedStockvm addReturnedStockvm)
        {
            return await _stockServices.AddReturnedStock(addReturnedStockvm);
        }
        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpGet]
        [Route("GetAllReturnedStock")]
        public async Task<StockResponse> GetAllReturnedStock()
        {
            return await _stockServices.GetAllReturnedStock();
        }
        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpPost]
        [Route("SearchStock")]
        public async Task<StockResponse> SearchForStock(string search_query)
        {
            return await _stockServices.SearchForStock(search_query);
        }
        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpPost]
        [Route("SearchStockIn")]
        public async Task<StockResponse> SearchForStockIn(string search_query)
        {
            return await _stockServices.SearchForStockIn(search_query);
        }
        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpPost]
        [Route("SearchForPO")]
        public async Task<StockResponse> SearchForPO(string search_query)
        {
            return await _stockServices.SearchForPO(search_query);
        }
        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpPost]
        [Route("SearchStockOut")]
        public async Task<StockResponse> SearchForStockOut(string search_query)
        {
            return await _stockServices.SearchForStockOut(search_query);
        }
        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpPost]
        [Route("SearchCustomer")]
        public async Task<StockResponse> SearchForCustomer(string search_query)
        {
            return await _stockServices.SearchForCustomer(search_query);
        }
        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpPost]
        [Route("SearchSupplier")]
        public async Task<StockResponse> SearchForSupplier(string search_query)
        {
            return await _stockServices.SearchForSupplier(search_query);
        }
        [Authorize(AuthenticationSchemes = "Bearer")]
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
        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpPost]
        [Route("EditCustomer")]
        public async Task<StockResponse> EditCustomer(EditCustomervm editCustomervm)
        {
            return await _stockServices.EditCustomer(editCustomervm);
        }
        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpPost]
        [Route("GetCustomerByID")]
        public async Task<StockResponse> GetCustomerById(int customerId)
        {
            return await _stockServices.GetCustomerById(customerId);
        }
        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpPost]
        [Route("GetSupplierbyId")]
        public async Task<StockResponse> GetSupplierById(int supplierId)
        {
            return await _stockServices.GetSupplierById(supplierId);
        }
        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpPost]
        [Route("EditSupplier")]
        public async Task<StockResponse> EditSupplier(editSuppliervm suppliervm)
        {
            return await _stockServices.EditSupplier(suppliervm);
        }
        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpPost]
        [Route("GetBrandById")]
        public async Task<StockResponse> GetBrandById(int BrandId)
        {
            return await _stockServices.GetBrandById(BrandId);
        }
        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpPost]
        [Route("GetItemById")]
        public async Task<StockResponse> GetItemsById(int ItemId)
        {
            return await _stockServices.GetItemsById(ItemId);
        }
        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpPost]
        [Route("EditBrand")]
        public async Task<StockResponse> EditBrand(EditBrandvm editBrandvm)
        {
            return await _stockServices.EditBrand(editBrandvm);
        }
        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpPost]
        [Route("EditItem")]
        public async Task<StockResponse> EditItem(EditItemvm editItemvm)
        {
            return await _stockServices.EditItem(editItemvm);
        }
        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpPost]
        [Route("EditStockIn")]
        public async Task<StockResponse> EditStockIn(EditPurchasevm editPurchasevm)
        {
            return await _stockServices.EditStockIn(editPurchasevm);
        }
        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpGet]
        [Route("GenerateExcel")]
        public async Task<StockResponse> GenerateExcel()
        {
            return await _stockServices.GenerateExcel();
        }
        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpPost]
        [Route("AddCategory")]
        public async Task<StockResponse> AddCategory(AddCategoryvm addCategoryvm)
        {
            return await _stockServices.AddCategory(addCategoryvm);

        }
        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpGet]
        [Route("GetAllCategory")]
        public async Task<StockResponse> GetAllCategory()
        {
            return await _stockServices.GetAllCategory();
        }
        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpPost]
        [Route("AddInvoiceDetails")]
        public async Task<StockResponse> AddInvoiceDetails(StockInvm stockInvm)
        {
            return await _stockServices.AddInvoiceDetails(stockInvm);

        }
        [Authorize(AuthenticationSchemes = "Bearer")]
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
        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpGet]
        [Route("GetInvoiceLines")]
        public async Task<StockResponse> GetInvoiceLines()
        {
            return await _stockServices.GetInvoiceLines();
        }
        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpPost]
        [Route("AddProductDetails")]
        public async Task<StockResponse> AddProductDetails(AddProductDetailvm addProductDetailvm)
        {
            return await _stockServices.AddProductDetails(addProductDetailvm);

        }
        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpGet]
        [Route("GetProductDetails")]
        public async Task<StockResponse> GetProductDetails()
        {
            return await _stockServices.GetProductDetails();
        }
        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpPost]
        [Route("GetInvoiceByNumber")]
        public async Task<StockResponse> GetInvoiceByNumber(string InvoiceNumber)
        {
            return await _stockServices.GetInvoiceByNumber(InvoiceNumber);
        }
        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpPost]
        [Route("AddParts")]
        public async Task<StockResponse> AddSparePart(AddPartvm addPartvm)
        {
            return await _stockServices.AddSparePart(addPartvm);

        }
        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpGet]
        [Route("GetAllParts")]
        public async Task<StockResponse> GetAllParts()
        {
            return await _stockServices.GetAllParts();
        }
        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpPost]
        [Route("AddSpareParts")]
        public async Task<StockResponse> AddPartsSpare(AddSparePartvm addSparePartvm)
        {
            return await _stockServices.AddPartsSpare(addSparePartvm);

        }
        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpGet]
        [Route("GetAllSpareParts")]
        public async Task<StockResponse> GetAllSpareParts()
        {
            return await _stockServices.GetAllSpareParts();
        }
        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpPost]
        [Route("AddInvoiceQuantity")]
        public async Task<StockResponse> Add_Invoice_Item_Quantity(invoice_item_quantity_vm vm)
        {
            return await _stockServices.Add_Invoice_Item_Quantity(vm);

        }
        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpGet]
        [Route("GetGeneratedref")]
        public async Task<string> GetGeneratedref()
        {
            return await _stockServices.GetGeneratedref();
        }
        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpPost]
        [Route("GetInvoiceLinesByInvoiceNumber")]
        public async Task<StockResponse> GetInvoiceLinByNumber(string invoiceNumber)
        {
            return await _stockServices.GetInvoiceLinByNumber(invoiceNumber);
        }
        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpPost]
        [Route("GetInvoiceItemByID")]
        public async Task<StockResponse> GetInvoiceItemByID(int invoicelineId)
        {
            return await _stockServices.GetInvoiceItemByID(invoicelineId);
        }
        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpPost]
        [Route("GetProductDetailsbyid")]
        public async Task<StockResponse> GetProductDetailsbyid(string BatchNumber)
        {
            return await _stockServices.GetProductDetailsbyid(BatchNumber);
        }
        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpPost]
        [Route("GetBatchByPO")]
        public async Task<StockResponse> GetBatchByPONumber(string poNumber)
        {
            return await _stockServices.GetBatchByPONumber(poNumber);
        }
        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpPost]
        [Route("GetProductByBatch")]
        public async Task<StockResponse> GetProductDetailsByBatchNumber(string BatchNumber)
        {
            return await _stockServices.GetProductDetailsByBatchNumber(BatchNumber);
        }
        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpPost]
        [Route("GetProductNumbering")]
        public async Task<StockResponse> GetProduct_Numbers_ByReference(string reference)
        {
            return await _stockServices.GetProduct_Numbers_ByReference(reference);
        }
        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpPost]
        [Route("GetProductlinebyid")]
        public async Task<StockResponse> GetProduvctLineyId(int product_line_id)
        {
            return await _stockServices.GetProduvctLineyId(product_line_id);
        }
        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpPost]
        [Route("SearchInvoice")]
        public async Task<StockResponse> SearchForInvoice(string search_query)
        {
            return await _stockServices.SearchForInvoice(search_query);
        }
        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpPost]
        [Route("SearchItem")]
        public async Task<StockResponse> SearchForItem(string search_query)
        {
            return await _stockServices.SearchForItem(search_query);
        }
        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpPost]
        [Route("GettingItemByBrandName")]
        public async Task<StockResponse> GetItemsbyBrandName(string BrandName)
        {
            return await _stockServices.GetItemsbyBrandName(BrandName);
        }
        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpPost]
        [Route("SearchInvoiceLines")]
        public async Task<StockResponse> SearchForInvoiceLines(string search_query)
        {
            return await _stockServices.SearchForInvoiceLines(search_query);
        }
        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpPost]
        [Route("upload_bulk")]
        public async Task<StockResponse> UploadData([FromBody] List<uploadDatavm> data)
        {
            return await _stockServices.UploadData(data);
        }
        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpPost]
        [Route("uploading")]
        public async Task<StockResponse> UploadingData(IFormFile file,string BatchNumber, int batchID)
        {
            return await _stockServices.UploadingData(file,BatchNumber,batchID);
        }
        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpPost]
        [Route("EditSerialNumber")]
        public async Task<StockResponse> EditSerialNumber(EditSerialNumbervm editSerialNumbervm)
        {
            return await _stockServices.EditSerialNumber(editSerialNumbervm);
        }
        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpPost]
        [Route("GetSerialNumber")]
        public async Task<StockResponse> GetSerialNumberbyid(int itemID)
        {
            return await _stockServices.GetSerialNumberbyid(itemID);
        }
        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpPost]
        [Route("ScannedData")]
        public async Task<StockResponse> PostScannedData([FromBody] ScannedDataModel data)
        {
            return await _stockServices.PostScannedData(data);
        }
        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpPost]
        [Route("UploadPDFFile")]
        public async Task<StockResponse> Upload([FromBody] PODetailsvm pODetailsvm)
        {
            return await _stockServices.Upload(pODetailsvm);
        }
        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpPost]
        [Route("UploadPODetails")]
        public async Task<StockResponse> UploadingPODetails(PODetailsvm pODetailsvm)
        {
            return await _stockServices.UploadingPODetails(pODetailsvm);

        }
        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpGet]
        [Route("GetAllPOS")]
        public async Task<StockResponse> GetAllPOs()
        {
            return await _stockServices.GetAllPOs();
        }
        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpPost]
        [Route("UploadPOItems")]
        public async Task<StockResponse> UploadingPOItems([FromBody] DataWrapper dataWrapper)
        {
            return await _stockServices.UploadingPOItems(dataWrapper);

        }
        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpPost]
        [Route("Uploading>>>")]
        public async Task UploadingItemsPO(IFormFile file, string PONumber)
        {
            await _stockServices.UploadingItemsPO(file, PONumber);

        }
        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpPost]
        [Route("GetItemsByPO")]
        public async Task<StockResponse> GetItemsByPO(string PONumber)
        {
            return await _stockServices.GetItemsByPO(PONumber);

        }
        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpPost]
        [Route("UploadingPO>>>>>")]
        public async Task<StockResponse> UploadingPO(IFormFile file)
        {
            return await _stockServices.UploadingPO(file);

        }
        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpGet]
        [Route("GetAllPOSDetails")]
        public async Task<StockResponse> GetAllPOSDetails()
        {
            return await _stockServices.GetAllPOSDetails();
        }
        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpPost]
        [Route("GettingItemInPO")]
        public async Task<StockResponse> GetItemsByPOS(string PONumber)
        {
            return await _stockServices.GetItemsByPOS(PONumber);

        }
        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpPost]
        [Route("AddingPurchaseOrdersDetails")]
        public async Task<StockResponse> AddingPurchaseOrdersDetails(PurchaseOrderssvm purchaseOrderssvm)
        {
            return await _stockServices.AddPurchaseOrdersDetails(purchaseOrderssvm);

        }
        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpPost]
        [Route("AddingPOItemLines")]
        public async Task<StockResponse> AddPOItemLines(AddPOItemLinesvm addBatchDetailsvm)
        {
            return await _stockServices.AddPOItemLines(addBatchDetailsvm);

        }
        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpPost]
        [Route("MarkingPOComplete")]
        public async Task<StockResponse> MarkPOComplete(string PONumber)
        {
            return await _stockServices.MarkPOComplete(PONumber);

        }
        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpPost]
        [Route("MarkingPOLinesComplete")]
        public async Task<StockResponse> MarkPOLinesAsComplete(string PONumber)
        {
            return await _stockServices.MarkPOLinesAsComplete(PONumber);

        }
        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpGet]
        [Route("GetAllPurchaseOrderss")]
        public async Task<StockResponse> GetAllPurchaseOrderDetails()
        {
            return await _stockServices.GetAllPurchaseOrderDetails();
        }
        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpPost]
        [Route("AdjustStock")]
        public async Task<StockResponse> AdjustStock(AdjustStockvm adjustStockvm)
        {
            return await _stockServices.AdjustStock(adjustStockvm);

        }
        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpPost]
        [Route("GetPOItemsbyID")]
        public async Task<StockResponse> GetPOItemsByID(int ItemId)
        {
            return await _stockServices.GetPOItemsByID(ItemId);

        }
        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpPost]
        [Route("GetAdjustedStockByID")]
        public async Task<StockResponse> GetAdjustedStockById(string batchNumber)
        {
            return await _stockServices.GetAdjustedStockById(batchNumber);

        }
        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpGet]
        [Route("GetAllItemsStock")]
        public async Task<StockResponse> GetAllItemStock()
        {
            return await _stockServices.GetAllItemStock();
        }
        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpPost]
        [Route("ApplyRequisition")]
        public async Task<StockResponse> ApplyRequisition(AddRequisition addRequisition)
        {
            return await _stockServices.ApplyRequisition(addRequisition);
        }
        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpPost]
        [Route("ApplyingRequisitionForm")]
        public async Task<StockResponse> ApplyRequisitionForm(ApplyRequistionvm addRequisition)
        {
            return await _stockServices.ApplyRequisitionForm(addRequisition);
        }
        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpPost]
        [Route("ApplicationStatus")]
        public async Task<StockResponse> ApplicationStatus(ApprovalProcessvm approvalProcessvm)
        {
            return await _stockServices.ApplicationStatus(approvalProcessvm);
        }
        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpGet]
        [Route("GetAllRequisition")]
        public async Task<StockResponse> GetAllRequisitionApplication()
        {
            return await _stockServices.GetAllRequisitionApplication();
        }
        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpPost]
        [Route("GetFormByID")]
        public async Task<StockResponse> GetRequisitionbyId(int Id)
        {
            return await _stockServices.GetRequisitionbyId(Id);
        }
        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpPost]
        [Route("IssueProcess")]
        public async Task<StockResponse> IssueProcess(int id)
        {
            var roleclaimname = "CanAllowIssueStock";
          
            var loggedinuser = await _loggeinuser.LoggedInUser();
            var master_rolechecker_response = await _roleservices.MasterRoleChecker(loggedinuser.Id, roleclaimname);

            if (master_rolechecker_response)
            {

                return await _stockServices.IssueProcess(id);

            }
            else
            {
                return new StockResponse(false, "You have no permission to access this", null);
            }

        }
        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpPost]
        [Route("GetFormbyUserEmail")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<StockResponse> GetRequisitionByEmail()
        {
            return await _stockServices.GetRequisitionByEmail();
        }
        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpGet]
        [Route("GetFormWithStatusPending")]
        public async Task<StockResponse> GetFormByStatusPending()
        {
            var loggedinuser = await _loggeinuser.LoggedInUser();

            if (loggedinuser.Checker == true)
            {

                return await _stockServices.GetFormByStatusPending();

            }
            else
            {
                return new StockResponse(false, "You have no permission to access this", null);
            }

        }
        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpGet]
        [Route("GetFormStatusWithApproved")]

        public async Task<StockResponse> GetFormStatusApproved()
        {
            var loggedinuser = await _loggeinuser.LoggedInUser();

            if (loggedinuser.Checker == true)
            {

                return await _stockServices.GetFormStatusApproved();

            }
            else
            {
                return new StockResponse(false, "You have no permission to access this", null);
            }

        }
        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpPost]
        [Route("SelectSerialNumber")]
        public async Task<StockResponse> SelectSerialNumber(SelectSerialvm selectSerialvm)
        {
            return await _stockServices.SelectSerialNumber(selectSerialvm);
        }
        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpPost]
        [Route("GetSelectedByIssueID")]
        public async Task<StockResponse> GetSelectedSerials(int issueID)
        {
            return await _stockServices.GetSelectedSerials(issueID);
        }
        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpGet]
        [Route("GetNotIssuedSerial")]
        public async Task<StockResponse> GetSerialByIssued()
        {
            return await _stockServices.GetSerialByIssued();
        }
        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpGet]
        [Route("GetStatusIssued")]
        public async Task<StockResponse> GetFormByStatusIssued()
        {
            return await _stockServices.GetFormByStatusIssued();
        }
        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpPost]
        [Route("AddDeliveryNote")]
        public async Task<StockResponse> AddDeliveryNote(AddDeliveryNotevm addDeliveryNotevm)
        {
            return await _stockServices.AddDeliveryNote(addDeliveryNotevm);

        }
        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpPost]
        [Route("GetDeliveryByBatchNumber")]
        public async Task<StockResponse> GetBatchByItems(int itemId)
        {
            return await _stockServices.GetBatchByItems(itemId);

        }
        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpPost]
        [Route("GetPOLinesbyid")]
        public async Task<StockResponse> GetPOLinesNByID(int itemId)
        {
            return await _stockServices.GetPOLinesNByID(itemId);

        }
        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpPost]
        [Route("GetProductbyBatchNumber")]
        public async Task<StockResponse> GetProductbyBatchid(string BatchNumber)
        {
            return await _stockServices.GetProductbyBatchid(BatchNumber);
        }
        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpPost]
        [Route("StockAdjustment")]
        public async Task<StockResponse> StockAdjustment([FromBody] StockAdjustvm adjustStockvm)
        {
            return await _stockServices.StockAdjustment(adjustStockvm);
        }
        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpGet]
        [Route("GetBatchWithStatusComplete")]

        public async Task<StockResponse> GetBatchCompleteStatus()
        {
            //var roleclaimname = "CanViewBatchWithComplete";
            var loggedinuser = await _loggeinuser.LoggedInUser();
           
            if (loggedinuser.Checker==true)
            {

                return await _stockServices.GetBatchCompleteStatus();

            }
            else
            {
                return new StockResponse(false, "You have no permission to access this", null);
            }

        }
        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpPost]
        [Route("ApproversReview")]
        public async Task<StockResponse> ApprovalsReview(ApproveBatchvm approvalProcessvm)
        {
            return await _stockServices.ApprovalsReview(approvalProcessvm);
        }
        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpPost]
        [Route("GetItemByBatchNumber")]
        public async Task<StockResponse> GetBatchByBatchNumber(string BatchNumber)
        {
            return await _stockServices.GetBatchByBatchNumber(BatchNumber);
        }
        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpPost]
        [Route("ApprovalofPO")]
        public async Task<StockResponse> POApprovalReview(POApprovalvm pOApprovalvm)
        {
            return await _stockServices.POApprovalReview(pOApprovalvm);

        }
        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpGet]
        [Route("GetPOWithPendingStatus")]
        public async Task<StockResponse> GetPOByStatusPending()
        {
            var loggedinuser = await _loggeinuser.LoggedInUser();

            if (loggedinuser.Checker == true)
            {

                return await _stockServices.GetPOByStatusPending();

            }
            else
            {
                return new StockResponse(false, "You have no permission to access this", null);
            }

        }
        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpGet]
        [Route("GetPOWithStatusComplete")]
        public async Task<StockResponse> GetAllPOSWithStatusComplete()
        {
            return await _stockServices.GetAllPOSWithStatusComplete();
        }
        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpPost]
        [Route("GetPONumberbyNumber")]
        public async Task<StockResponse> GetPONumberbyNumber(string POnumber)
        {
            return await _stockServices.GetPONumberbyNumber(POnumber);

        }
        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpPost]
        [Route("MarkBatchAsComplete")]
        public async Task<StockResponse> MarkBatchCompplete(string BatchNumber)
        {
            return await _stockServices.MarkBatchCompplete(BatchNumber);

        }
        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpGet]
        [Route("GetAllPOSWithStatusPending")]
        public async Task<StockResponse> GetAllPOSWithStatusPending()
        {
            var loggedinuser = await _loggeinuser.LoggedInUser();

            if (loggedinuser.Checker == true)
            {

                return await _stockServices.GetAllPOSWithStatusPending();

            }
            else
            {
                return new StockResponse(false, "You have no permission to access this", null);
            }

        }
        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpPost]
        [Route("PODeliveryReview")]
        public async Task<StockResponse> PODeliveryReview(ApprovalPODeliveryvm pOApprovalvm)
        {
            return await _stockServices.PODeliveryReview(pOApprovalvm);
        }
        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpPost]
        [Route("GetItemByClient")]
        public async Task<StockResponse> GetItemByClient(string ClientName)
        {
           

                return await _stockServices.GetItemByClient(ClientName);
            
         
        }
        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpGet]
        [Route("GetItemsToBeReordered")]
        public async Task<StockResponse> GetAllItemsReorder()
        {
            return await _stockServices.GetAllItemsReorder();
        }
    }
}
