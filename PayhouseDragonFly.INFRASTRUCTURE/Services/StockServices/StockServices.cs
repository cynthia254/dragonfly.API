using Azure.Core;
using ClosedXML.Excel;
using MathNet.Numerics.Distributions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using NPOI.SS.Formula.Functions;
using OfficeOpenXml;
using PayhouseDragonFly.CORE.ConnectorClasses.Response.StockResponse;
using PayhouseDragonFly.CORE.DTOs.Stock;
using PayhouseDragonFly.CORE.DTOs.Stock.Invoicing_vm;
using PayhouseDragonFly.CORE.Models.Stock;
using PayhouseDragonFly.CORE.Models.Stock.Invoicing;
using PayhouseDragonFly.INFRASTRUCTURE.DataContext;
using PayhouseDragonFly.INFRASTRUCTURE.Services.ExtraServices;
using PayhouseDragonFly.INFRASTRUCTURE.Services.IServiceCoreInterfaces.IStockServices;
using System.Drawing;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.AspNetCore.Hosting;
using System.Text;
using ZXing;
using ZXing.Common;
using ZXing.QrCode;
using PdfSharp.Pdf.IO;
using static PdfSharp.Pdf.AcroForms.PdfAcroField;
using PdfSharp.Pdf.AcroForms;
using NPOI;
using PdfReader = iTextSharp.text.pdf.PdfReader;
using iTextSharp.text.pdf.parser;
using Org.BouncyCastle.Asn1.Ocsp;
using Microsoft.Extensions.Logging;
using DocumentFormat.OpenXml.Spreadsheet;
using NPOI.HPSF;
using Path = System.IO.Path;
using NPOI.OpenXmlFormats.Dml;
using PayhouseDragonFly.CORE.ConnectorClasses.Response.BseResponse;
using PayhouseDragonFly.INFRASTRUCTURE.Services.IServiceCoreInterfaces.IEmailServices;
using PayhouseDragonFly.CORE.DTOs.EmaillDtos;

namespace PayhouseDragonFly.INFRASTRUCTURE.Services.StockServices
{
    public class StockServices : IStockServices
    {
        private readonly DragonFlyContext _dragonFlyContext;
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly IEExtraServices _extraServices;
        private readonly ILogger<IStockServices> _logger;
        private readonly IEmailServices _iemail_service;
        public StockServices(DragonFlyContext dragonFlyContext, IServiceScopeFactory serviceScopeFactory, IEmailServices iemail_service,
            IEExtraServices extraServices,
            ILogger<IStockServices> logger
            )
        {
            _dragonFlyContext = dragonFlyContext;
            _serviceScopeFactory = serviceScopeFactory;
            _extraServices = extraServices;
            _logger = logger;
            _iemail_service = iemail_service;

        }

        public async Task<StockResponse> AddBrand(AddBrandvm addBrandvm)
        {
            try
            {
                if (addBrandvm.BrandName == "")
                {

                    return new StockResponse(false, "Kindly provide a brand name to add brand", null);
                }
                using (var scope = _serviceScopeFactory.CreateScope())
                {
                    var scopedcontext = scope.ServiceProvider.GetRequiredService<DragonFlyContext>();

                    //check if role exists 

                    var brandexists = await scopedcontext.AddBrand.Where(x => x.BrandName == addBrandvm.BrandName).FirstOrDefaultAsync();

                    if (brandexists != null)
                    {
                        return new StockResponse(false, $" Brand  '{addBrandvm.BrandName}' already exist, if  must add a similar brand kindly change the " +
                             $"latter cases from lower to upper and vice versa depending on the existing  brand . The existsing role is '{brandexists}' with brand id {brandexists.BrandId} ", null);
                    }
                    var brandclass = new AddBrand
                    {
                        BrandName = addBrandvm.BrandName,
                    };
                    await scopedcontext.AddAsync(brandclass);
                    await scopedcontext.SaveChangesAsync();
                    return new StockResponse(true, $"Brand '{addBrandvm.BrandName}'  created successfully", null);

                }

            }
            catch (Exception ex)
            {
                return new StockResponse(false, ex.Message, null);

            }

        }
        public async Task<StockResponse> GetAllBrand()
        {

            try
            {

                using (var scope = _serviceScopeFactory.CreateScope())
                {
                    var scopedcontext = scope.ServiceProvider.GetRequiredService<DragonFlyContext>();
                    var allbrands = await scopedcontext.AddBrand.ToListAsync();

                    if (allbrands == null)
                    {
                        return new StockResponse(false, "Brand doesn't exist", null);
                    }
                    return new StockResponse(true, "Successfully queried", allbrands);

                }
            }
            catch (Exception ex)
            {
                return new StockResponse(false, ex.Message, null);
            }
        }
        public async Task<StockResponse> AddItem(AddItemvm addItemvm)
        {
            try
            {
                if (addItemvm.ItemName == "")
                {

                    return new StockResponse(false, "Kindly provide an item name to add item", null);
                }
                if (addItemvm.Category == "")
                {
                    return new StockResponse(false, "Kindly provide category", null);

                }
                if (addItemvm.BrandName == "")
                {
                    return new StockResponse(false, "Kindly provide brand name", null);

                }
                if (addItemvm.Currency == "")
                {
                    return new StockResponse(false, "Kindly provide currency", null);
                }
                if (addItemvm.IndicativePrice < 0)
                {
                    return new StockResponse(false, "Kindly provide indicative price", null);
                }
                if (addItemvm.ReOrderLevel < 0)
                {
                    return new StockResponse(false, "Kindly provide reorder level", null);
                }
                if (addItemvm.ItemDescription == null)
                {
                    return new StockResponse(false, "Kindly provide description", null);
                }
                using (var scope = _serviceScopeFactory.CreateScope())
                {
                    var scopedcontext = scope.ServiceProvider.GetRequiredService<DragonFlyContext>();



                    var itemexists = await scopedcontext.AddItem.Where(x => x.ItemName == addItemvm.ItemName && x.BrandName == addItemvm.BrandName).FirstOrDefaultAsync();

                    if (itemexists != null)
                    {
                        return new StockResponse(false, $" Item  '{addItemvm.BrandName}-{addItemvm.ItemName}' already exist", null);
                    }

                    var itemclass = new AddItem
                    {
                        ItemName = addItemvm.ItemName,
                        Category = addItemvm.Category,
                        Currency = addItemvm.Currency,
                        IndicativePrice = addItemvm.IndicativePrice,
                        ReOrderLevel = addItemvm.ReOrderLevel,
                        BrandName = addItemvm.BrandName,
                        ItemDescription = addItemvm.ItemDescription,

                    };

                    await scopedcontext.AddAsync(itemclass);
                    await scopedcontext.SaveChangesAsync();
                    return new StockResponse(true, $"Item '{addItemvm.ItemName}'  created successfully", null);

                }

            }
            catch (Exception ex)
            {
                return new StockResponse(false, ex.Message, null);

            }

        }
        public async Task<StockResponse> GetAllItems()
        {

            try
            {

                using (var scope = _serviceScopeFactory.CreateScope())
                {
                    var scopedcontext = scope.ServiceProvider.GetRequiredService<DragonFlyContext>();
                    var allitems = await scopedcontext.AddItem.ToListAsync();

                    if (allitems == null)
                    {
                        return new StockResponse(false, "Item doesn't exist", null);
                    }
                    return new StockResponse(true, "Successfully queried", allitems);

                }
            }
            catch (Exception ex)
            {
                return new StockResponse(false, ex.Message, null);
            }
        }
        public async Task<StockResponse> AddStock(AddStockvm addStockvm)
        {
            try
            {
                if (addStockvm.BrandName == "")
                {
                    return new StockResponse(false, "BrandName cannot be empty", null);
                }
                if (addStockvm.ItemName == "")
                {
                    return new StockResponse(false, "ItemName cannot be empty", null);
                }

                if (addStockvm.ReOrderLevel == 0)
                {
                    return new StockResponse(false, "ReorderLevel cannot be empty", null);
                }
                if (addStockvm.BuyingPrice < 0)
                {
                    return new StockResponse(false, "Buying price cannot be empty", null);
                }
                if (addStockvm.Quantity < 0)
                {
                    return new StockResponse(false, "Quantity cannot be empty", null);
                }


                using (var scope = _serviceScopeFactory.CreateScope())
                {
                    var scopedcontext = scope.ServiceProvider.GetRequiredService<DragonFlyContext>();




                    var itemclass = new AddStock
                    {
                        ItemName = addStockvm.ItemName,
                        BrandName = addStockvm.BrandName,
                        ReOrderLevel = addStockvm.ReOrderLevel,
                        BuyingPrice = addStockvm.BuyingPrice,
                        DateAdded = addStockvm.DateAdded,
                        Currency = addStockvm.Currency,
                        StockInTransit = addStockvm.StockInTransit,
                        Quantity = addStockvm.Quantity,
                        SalesCurrency = addStockvm.SalesCurrency,
                        ReorderRequired = addStockvm.ReorderRequired,


                    };


                    itemclass.OpeningStock = addStockvm.Quantity;
                    itemclass.AvailableStock = addStockvm.Quantity;

                    if (itemclass.AvailableStock > addStockvm.ReOrderLevel)
                    {
                        itemclass.Status = "Good";
                    }
                    else if (itemclass.AvailableStock < addStockvm.ReOrderLevel && itemclass.AvailableStock > 0 || itemclass.AvailableStock == addStockvm.ReOrderLevel)
                    {
                        itemclass.Status = "Low";
                    }
                    else
                    {
                        itemclass.Status = "Out";
                    }
                    if (itemclass.AvailableStock == addStockvm.ReOrderLevel || itemclass.AvailableStock < addStockvm.ReOrderLevel)
                    {
                        itemclass.ReorderRequired = "Yes";
                    }
                    else
                    {
                        itemclass.ReorderRequired = "No";
                    }


                    var checkitemexits = await scopedcontext.AddStock
                        .Where(y => y.ItemName == addStockvm.ItemName && y.BrandName == addStockvm.BrandName)
                        .FirstOrDefaultAsync();
                    if (checkitemexits != null)
                    {
                        return new StockResponse(false, "item already exists", null);
                    }
                    else
                    {
                        await scopedcontext.AddAsync(itemclass);
                        await scopedcontext.SaveChangesAsync();
                        return new StockResponse(true, "Stock added successfully", null);
                    }

                };

            }


            catch (Exception ex)
            {
                return new StockResponse(false, ex.Message, null);

            }


        }
        public async Task<StockResponse> UpdateStockQuantity(int itemid, int quantityadded)
        {

            try
            {
                using (var scope = _serviceScopeFactory.CreateScope())
                {
                    var scopedcontext = scope.ServiceProvider.GetRequiredService<DragonFlyContext>();
                    var lastupdate = await scopedcontext.AddStock.Where(y => y.StockId == itemid)
                        .FirstOrDefaultAsync();
                    if (lastupdate == null)
                    {
                        return new StockResponse(false, "stock not found", null);
                    }
                    lastupdate.Quantity = lastupdate.Quantity + quantityadded;
                    lastupdate.AvailableStock = quantityadded + lastupdate.AvailableStock;
                    if (lastupdate.AvailableStock > lastupdate.ReOrderLevel)
                    {
                        lastupdate.Status = "Good";
                    }
                    else if (lastupdate.AvailableStock < lastupdate.ReOrderLevel && lastupdate.AvailableStock > 0)
                    {
                        lastupdate.Status = "Low";
                    }
                    else
                    {
                        lastupdate.Status = "Out";
                    }
                    scopedcontext.Update(lastupdate);
                    await scopedcontext.SaveChangesAsync();
                    return new StockResponse(true, "Stock updated successfully", null);

                }
            }
            catch (Exception ex)
            {
                return new StockResponse(false, ex.Message, null);
            }
        }

        public async Task<StockResponse> AddCustomer(AddCustomervm addCustomervm)
        {
            try
            {
                if (addCustomervm.CustomerName == "")
                {

                    return new StockResponse(false, "Kindly provide  customer name to add customer", null);
                }
                if (addCustomervm.CompanyName == "")
                {

                    return new StockResponse(false, "Kindly provide  company name to add customer", null);
                }
                if (addCustomervm.Email == "")
                {

                    return new StockResponse(false, "Kindly provide an email to add customer", null);
                }
                if (addCustomervm.PhoneNumber == "")
                {

                    return new StockResponse(false, "Kindly provide phoneNumber to add customer", null);
                }
                using (var scope = _serviceScopeFactory.CreateScope())
                {
                    var scopedcontext = scope.ServiceProvider.GetRequiredService<DragonFlyContext>();



                    var customerexists = await scopedcontext.Customer.Where(x => x.Email == addCustomervm.Email).FirstOrDefaultAsync();

                    if (customerexists != null)
                    {
                        return new StockResponse(false, $" Customer  '{addCustomervm.Email}' already exists", null);
                    }
                    var itemclass = new AddCustomer
                    {
                        CustomerName = addCustomervm.CustomerName,
                        CompanyName = addCustomervm.CompanyName,
                        Email = addCustomervm.Email,
                        PhoneNumber = addCustomervm.PhoneNumber,
                    };
                    await scopedcontext.AddAsync(itemclass);
                    await scopedcontext.SaveChangesAsync();
                    return new StockResponse(true, $" '{addCustomervm.CustomerName}'  has been added successfully", null);

                }

            }
            catch (Exception ex)
            {
                return new StockResponse(false, ex.Message, null);

            }

        }

        public async Task<StockResponse> GetAllCustomers()
        {

            try
            {

                using (var scope = _serviceScopeFactory.CreateScope())
                {
                    var scopedcontext = scope.ServiceProvider.GetRequiredService<DragonFlyContext>();
                    var allcustomers = await scopedcontext.Customer.ToListAsync();

                    if (allcustomers == null)
                    {
                        return new StockResponse(false, "Customer doesn't exist", null);
                    }

                    return new StockResponse(true, "Successfully queried", allcustomers);

                }
            }
            catch (Exception ex)
            {
                return new StockResponse(false, ex.Message, null);
            }
        }
        public async Task<StockResponse> GetItemByName(string itemname)
        {
            try
            {
                using (var scope = _serviceScopeFactory.CreateScope())
                {
                    var scopedcontext = scope.ServiceProvider.GetRequiredService<DragonFlyContext>();

                    var itemexists = await scopedcontext.AddStock.Where(y => y.ItemName == itemname).FirstOrDefaultAsync();
                    if (itemexists == null)
                    {
                        return new StockResponse(false, "nothing to show ", null);
                    }
                    return new StockResponse(true, "Queried successfully", itemexists);


                }

            }
            catch (Exception ex)
            {
                return new StockResponse(false, ex.Message, null);
            }
        }
        public async Task<StockResponse> GetAllStock()
        {

            try
            {

                using (var scope = _serviceScopeFactory.CreateScope())
                {
                    var scopedcontext = scope.ServiceProvider.GetRequiredService<DragonFlyContext>();
                    var allstock = await scopedcontext.AddStock.OrderByDescending(x => x.DateAdded).ToListAsync();

                    if (allstock == null)
                    {
                        return new StockResponse(false, "Stock doesn't exist", null);
                    }
                    List<All_Stocks_List> stocklist = new List<All_Stocks_List>();

                    foreach (var stock in allstock)
                    {
                        var newstockfound = new All_Stocks_List
                        {
                            StockId = stock.StockId,
                            BrandName = stock.BrandName,
                            ItemName = stock.ItemName,
                            Quantity = stock.Quantity,
                            ReOrderLevel = stock.ReOrderLevel,
                            BuyingPrice = stock.BuyingPrice,
                            SellingPrice = stock.SellingPrice,
                            AvailableStock = stock.AvailableStock,
                            DateAdded = stock.DateAdded,
                            Status = stock.Status,
                            Currency = stock.Currency,
                            SalesCurrency = stock.SalesCurrency,
                            StockInTransit = stock.StockInTransit,
                            OpeningStock = stock.OpeningStock,
                            ReorderRequired = stock.ReorderRequired,
                            StockOut = stock.StockOut,
                            TotalReturnedStock = stock.TotalReturnedStock,
                            StockIn = stock.StockIn,
                        };
                        allstock.Sum(x => x.Quantity);
                        newstockfound.TotalBuyingPrice = stock.BuyingPrice * stock.AvailableStock;
                        newstockfound.TotalSellingPrice = stock.SellingPrice * stock.AvailableStock;
                        stocklist.Add(newstockfound);

                    }
                    return new StockResponse(true, "Successfully queried", stocklist);

                }

            }
            catch (Exception ex)
            {
                return new StockResponse(false, ex.Message, null);
            }
        }
        public async Task<StockResponse> AddSupplier(AddSupplierVm addSupplierVm)
        {
            try
            {
                if (addSupplierVm.SupplierName == "")
                {

                    return new StockResponse(false, "Kindly provide  supplier name to add supplier", null);
                }
                if (addSupplierVm.CompanyName == "")
                {

                    return new StockResponse(false, "Kindly provide  company name to add supplier", null);
                }
                if (addSupplierVm.Email == "")
                {

                    return new StockResponse(false, "Kindly provide an email to add supplier", null);
                }
                if (addSupplierVm.PhoneNumber == "")
                {

                    return new StockResponse(false, "Kindly provide phoneNumber to add supplier", null);
                }
                using (var scope = _serviceScopeFactory.CreateScope())
                {
                    var scopedcontext = scope.ServiceProvider.GetRequiredService<DragonFlyContext>();



                    var customerexists = await scopedcontext.Suppliers.Where(x => x.Email == addSupplierVm.Email).FirstOrDefaultAsync();

                    if (customerexists != null)
                    {
                        return new StockResponse(false, $" Supplier  '{addSupplierVm.Email}' already exists", null);
                    }
                    var itemclass = new AddSupplier
                    {
                        SupplierName = addSupplierVm.SupplierName,
                        CompanyName = addSupplierVm.CompanyName,
                        Email = addSupplierVm.Email,
                        PhoneNumber = addSupplierVm.PhoneNumber,
                    };
                    await scopedcontext.AddAsync(itemclass);
                    await scopedcontext.SaveChangesAsync();
                    return new StockResponse(true, $" '{addSupplierVm.SupplierName}'  has been added successfully", null);

                }

            }
            catch (Exception ex)
            {
                return new StockResponse(false, ex.Message, null);

            }

        }
        public async Task<StockResponse> GetAllSuppliers()
        {

            try
            {

                using (var scope = _serviceScopeFactory.CreateScope())
                {
                    var scopedcontext = scope.ServiceProvider.GetRequiredService<DragonFlyContext>();
                    var allsuppliers = await scopedcontext.Suppliers.ToListAsync();

                    if (allsuppliers == null)
                    {
                        return new StockResponse(false, "Supplier doesn't exist", null);
                    }
                    return new StockResponse(true, "Successfully queried", allsuppliers);

                }
            }
            catch (Exception ex)
            {
                return new StockResponse(false, ex.Message, null);
            }
        }
        public async Task<StockResponse> AddSales(AddSalesOrdersVm addSalesOrdersVm)
        {
            try
            {
                if (addSalesOrdersVm.BrandName == "")
                {

                    return new StockResponse(false, "Kindly provide  brand name to add sales", null);
                }
                if (addSalesOrdersVm.ItemName == "")
                {

                    return new StockResponse(false, "Kindly provide  item name to add sales", null);
                }
                if (addSalesOrdersVm.Quantity == 0)
                {

                    return new StockResponse(false, "Kindly provide quantity to add sales", null);
                }
                if (addSalesOrdersVm.CustomerName == "")
                {

                    return new StockResponse(false, "Kindly provide customer name to add sales", null);
                }
                if (addSalesOrdersVm.Comments == "")
                {
                    return new StockResponse(false, "Kindly provide comments to add stock out ", null);
                }
                if (addSalesOrdersVm.Department == "")
                {
                    return new StockResponse(false, "Kindly provide department details", null);
                }

                using (var scope = _serviceScopeFactory.CreateScope())
                {
                    var scopedcontext = scope.ServiceProvider.GetRequiredService<DragonFlyContext>();
                    var itemexists = await scopedcontext.AddStock
                    .Where(y => y.BrandName == addSalesOrdersVm.BrandName &&
                    y.ItemName == addSalesOrdersVm.ItemName).FirstOrDefaultAsync();
                    if (itemexists == null)
                    {
                        return new StockResponse(false, "Does not exist", null);
                    }



                    var itemclass = new AddSalesOrder
                    {
                        BrandName = addSalesOrdersVm.BrandName,
                        ItemName = addSalesOrdersVm.ItemName,
                        Quantity = addSalesOrdersVm.Quantity,
                        CustomerName = addSalesOrdersVm.CustomerName,
                        Comments = addSalesOrdersVm.Comments,
                        DateAdded = DateTime.Now,
                        Department = addSalesOrdersVm.Department,







                    };

                    itemexists.Quantity -= addSalesOrdersVm.Quantity;
                    itemexists.StockOut += addSalesOrdersVm.Quantity;
                    if (itemexists.AvailableStock == 0)
                    {
                        return new StockResponse(false, "No available stock please restock first....", null);
                    }
                    if (itemexists.AvailableStock < itemclass.Quantity)
                    {
                        return new StockResponse(false, $"Note:You can only stockOut from '{itemexists.AvailableStock}'!!! ", null);
                    }
                    itemexists.AvailableStock -= addSalesOrdersVm.Quantity;

                    if (itemexists.AvailableStock > itemexists.ReOrderLevel)
                    {
                        itemexists.Status = "Good";
                    }
                    else if (itemexists.AvailableStock < itemexists.ReOrderLevel && itemexists.AvailableStock > 0 || itemexists.AvailableStock == itemexists.ReOrderLevel)
                    {
                        itemexists.Status = "Low";
                    }
                    else
                    {
                        itemexists.Status = "Out";
                    }
                    if (itemexists.AvailableStock < itemexists.ReOrderLevel || itemexists.AvailableStock == itemexists.ReOrderLevel)
                    {
                        itemexists.ReorderRequired = "Yes";
                    }
                    else
                    {
                        itemexists.ReorderRequired = "No";
                    }



                    await scopedcontext.AddAsync(itemclass);
                    await scopedcontext.SaveChangesAsync();
                    return new StockResponse(true, "StockOut has been added successfully", null);

                }


            }
            catch (Exception ex)
            {
                return new StockResponse(false, ex.Message, null);

            }

        }
        public async Task<StockResponse> GetAllSales()
        {

            try
            {

                using (var scope = _serviceScopeFactory.CreateScope())
                {
                    var scopedcontext = scope.ServiceProvider.GetRequiredService<DragonFlyContext>();
                    var allsales = await scopedcontext.Sales.ToListAsync();

                    if (allsales == null)
                    {
                        return new StockResponse(false, "Sale doesn't exist", null);
                    }
                    return new StockResponse(true, "Successfully queried", allsales);

                }
            }
            catch (Exception ex)
            {
                return new StockResponse(false, ex.Message, null);
            }
        }
        public async Task<StockResponse> AddPurchase(AddPurchaseOrderVm addPurchaseOrderVm)
        {
            try
            {
                if (addPurchaseOrderVm.BrandName == "")
                {

                    return new StockResponse(false, "Kindly provide  brand name ", null);
                }
                if (addPurchaseOrderVm.ItemName == "")
                {

                    return new StockResponse(false, "Kindly provide  item name ", null);
                }
                if (addPurchaseOrderVm.Quantity == 0)
                {

                    return new StockResponse(false, "Kindly provide quantity", null);
                }
                if (addPurchaseOrderVm.SupplierName == "")
                {

                    return new StockResponse(false, "Kindly provide supplier name", null);
                }
                if (addPurchaseOrderVm.DeliveryDate == DateTime.MaxValue)
                {

                    return new StockResponse(false, "Kindly provide delivery date", null);
                }
                using (var scope = _serviceScopeFactory.CreateScope())
                {
                    var scopedcontext = scope.ServiceProvider.GetRequiredService<DragonFlyContext>();
                    var itemexists = await scopedcontext.AddStock
                        .Where(y => y.BrandName == addPurchaseOrderVm.BrandName &&
                        y.ItemName == addPurchaseOrderVm.ItemName).FirstOrDefaultAsync();
                    if (itemexists == null)
                    {
                        return new StockResponse(false, "Does not exist", null);
                    }


                    var itemclass = new AddPurchaseOrder
                    {
                        BrandName = addPurchaseOrderVm.BrandName,
                        ItemName = addPurchaseOrderVm.ItemName,
                        Quantity = addPurchaseOrderVm.Quantity,
                        SupplierName = addPurchaseOrderVm.SupplierName,
                        DeliveryDate = addPurchaseOrderVm.DeliveryDate,
                        PurchaseDate = addPurchaseOrderVm.PurchaseDate,
                        TotalPurchase = addPurchaseOrderVm.TotalPurchase,
                        Status = "Ordered",
                        ReasonforStatus = "New",
                        PurchaseStatus = "Ordered",
                        DateAdded = DateTime.Now,



                    };
                    itemclass.TotalPurchase = addPurchaseOrderVm.Quantity * itemexists.BuyingPrice;
                    itemexists.AvailableStock += addPurchaseOrderVm.Quantity;
                    itemexists.Quantity += addPurchaseOrderVm.Quantity;
                    itemexists.StockIn += addPurchaseOrderVm.Quantity;
                    itemclass.Currency = itemexists.Currency;
                    if (itemexists.AvailableStock > itemexists.ReOrderLevel)
                    {
                        itemexists.Status = "Good";
                    }
                    else if (itemexists.AvailableStock < itemexists.ReOrderLevel && itemexists.AvailableStock > 0 || itemexists.AvailableStock == itemexists.ReOrderLevel)
                    {
                        itemexists.Status = "Low";
                    }
                    else
                    {
                        itemexists.Status = "Out";
                    }
                    if (itemexists.AvailableStock == itemexists.ReOrderLevel || itemexists.AvailableStock < itemexists.ReOrderLevel)
                    {
                        itemexists.ReorderRequired = "Yes";
                    }
                    else
                    {
                        itemexists.ReorderRequired = "No";
                    }




                    await scopedcontext.AddAsync(itemclass);
                    await scopedcontext.SaveChangesAsync();
                    return new StockResponse(true, "Stock  has been added successfully", null);

                }

            }
            catch (Exception ex)
            {
                return new StockResponse(false, ex.Message, null);

            }

        }
        public async Task<StockResponse> GetAllPurchases()
        {

            try
            {

                using (var scope = _serviceScopeFactory.CreateScope())
                {
                    var scopedcontext = scope.ServiceProvider.GetRequiredService<DragonFlyContext>();
                    var allpurchases = await scopedcontext.Purchases.ToListAsync();

                    if (allpurchases == null)
                    {
                        return new StockResponse(false, "Purchase doesn't exist", null);
                    }
                    return new StockResponse(true, "Successfully queried", allpurchases);

                }
            }
            catch (Exception ex)
            {
                return new StockResponse(false, ex.Message, null);
            }
        }
        public async Task<StockResponse> GetItemById(int itemid)
        {
            try
            {
                using (var scope = _serviceScopeFactory.CreateScope())
                {

                    var scopedcontext = scope.ServiceProvider.GetRequiredService<DragonFlyContext>();
                    var itemexist = await scopedcontext.AddStock.Where(u => u.StockId == itemid).FirstOrDefaultAsync();
                    if (itemexist == null)
                    {
                        return new StockResponse(false, "not found", null);
                    }
                    return new StockResponse(true, "Queried successfully", itemexist);
                }
            }
            catch (Exception ex)
            {
                return new StockResponse(false, ex.Message, null);
            }
        }
        public async Task<StockResponse> GetPurchaseById(int purchaseId)
        {
            try
            {
                using (var scope = _serviceScopeFactory.CreateScope())
                {

                    var scopedcontext = scope.ServiceProvider.GetRequiredService<DragonFlyContext>();
                    var purchaseexists = await scopedcontext.Purchases.Where(u => u.PurchaseId == purchaseId).FirstOrDefaultAsync();
                    if (purchaseexists == null)
                    {
                        return new StockResponse(false, "not found", null);
                    }
                    return new StockResponse(true, "Queried successfully", purchaseexists);
                }
            }
            catch (Exception ex)
            {
                return new StockResponse(false, ex.Message, null);
            }
        }
        public async Task<StockResponse> ChangePurchaseStatus(PurchaseStatusvm purchaseStatusvm)
        {

            try
            {

                using (var scope = _serviceScopeFactory.CreateScope())
                {
                    var scopedcontext = scope.ServiceProvider.GetRequiredService<DragonFlyContext>();


                    var purchaseexists = await scopedcontext.Purchases.Where(u => u.PurchaseId == purchaseStatusvm.PurchaseId).FirstOrDefaultAsync();
                    if (purchaseexists == null)
                    {

                        return new StockResponse(false, "purchase does not exist", null);
                    }
                    var itemexists = await scopedcontext.AddStock
                       .Where(y => y.BrandName == purchaseexists.BrandName &&
                       y.ItemName == purchaseexists.ItemName).FirstOrDefaultAsync();
                    if (itemexists == null)
                    {
                        return new StockResponse(false, "Does not exist", null);
                    }



                    purchaseexists.PurchaseId = purchaseStatusvm.PurchaseId;
                    purchaseexists.PurchaseStatus = purchaseStatusvm.PurchaseStatus;
                    purchaseexists.ReasonforStatus = purchaseStatusvm.ReasonforStatus;
                    purchaseexists.DateAdded = Convert.ToDateTime(purchaseexists.DateAdded);

                    if (purchaseexists.PurchaseStatus == "Delivered")
                    {
                        itemexists.AvailableStock += purchaseexists.Quantity;
                        itemexists.Quantity += purchaseexists.Quantity;
                    }
                    if (purchaseexists.PurchaseStatus == "In Transit")
                    {
                        itemexists.StockInTransit = purchaseexists.Quantity + itemexists.StockInTransit;
                    }
                    if (itemexists.AvailableStock > itemexists.ReOrderLevel)
                    {
                        itemexists.Status = "Good";
                    }
                    else if (itemexists.AvailableStock < itemexists.ReOrderLevel && itemexists.AvailableStock > 0)
                    {
                        itemexists.Status = "Low";
                    }
                    else
                    {
                        itemexists.Status = "Out";
                    }







                    scopedcontext.Update(purchaseexists);
                    await scopedcontext.SaveChangesAsync();
                    ;

                    return new StockResponse(true, $"{purchaseexists.BrandName} {purchaseexists.ItemName} status changged success fully to '{purchaseStatusvm.PurchaseStatus}'", null);

                }

            }
            catch (Exception ex)

            {

                return new StockResponse(false, ex.Message, null);
            }
        }
        public async Task<StockResponse> GetSalesbyId(int salesId)
        {
            try
            {
                using (var scope = _serviceScopeFactory.CreateScope())
                {

                    var scopedcontext = scope.ServiceProvider.GetRequiredService<DragonFlyContext>();
                    var purchaseexists = await scopedcontext.Sales.Where(u => u.SalesId == salesId).FirstOrDefaultAsync();
                    if (purchaseexists == null)
                    {
                        return new StockResponse(false, "not found", null);
                    }
                    return new StockResponse(true, "Queried successfully", purchaseexists);


                }


            }
            catch (Exception ex)
            {
                return new StockResponse(false, ex.Message, null);
            }
        }
        public async Task<StockResponse> GenerateExcel()
        {


            using var wbook = new XLWorkbook();
            var ws = wbook.AddWorksheet("Stock");
            ws.FirstCell().Value = "Product ID";
            ws.Cell("B1").Value = "Product Name";
            ws.Cell("C1").Value = "Stock In";
            ws.Cell("D1").Value = "Stock Out";
            ws.Cell("E1").Value = "Buying Price";

            ws.Column(2).AdjustToContents();
            ws.Column(1).AdjustToContents();
            ws.Column(3).AdjustToContents();
            ws.Column(4).AdjustToContents();
            ws.Column(5).AdjustToContents();

            wbook.SaveAs("Product.xlsx");

            return new StockResponse(true, "Success", null);





        }




        //  ws.FirstCell().Value = "Product Name";
        // ws.Cell(3, 2).Value = "ASP.NET CORE MVC";
        // ws.Cell("A6").SetValue("Youtube Channel");
        // ws.Column(2).AdjustToContents();



        public async Task<StockResponse> ChangeSalesStatus(ChangeSalesStatusvm changeSalesStatusvm)
        {

            try
            {

                using (var scope = _serviceScopeFactory.CreateScope())
                {
                    var scopedcontext = scope.ServiceProvider.GetRequiredService<DragonFlyContext>();




                    var salesexists = await scopedcontext.Sales.Where(u => u.SalesId == changeSalesStatusvm.salesId).FirstOrDefaultAsync();
                    if (salesexists == null)
                    {

                        return new StockResponse(false, "purchase does not exist", null);
                    }
                    var itemexists = await scopedcontext.AddStock
                       .Where(y => y.BrandName == salesexists.BrandName &&
                       y.ItemName == salesexists.ItemName).FirstOrDefaultAsync();
                    if (itemexists == null)
                    {
                        return new StockResponse(false, "Does not exist", null);
                    }


                    salesexists.SalesId = changeSalesStatusvm.salesId;
                    salesexists.SalesStatus = changeSalesStatusvm.SalesStatus;
                    salesexists.ReasonForSalesStatus = salesexists.ReasonForSalesStatus;
                    salesexists.DateAdded = Convert.ToDateTime(salesexists.DateAdded);

                    if (salesexists.SalesStatus == "Delivered")
                    {
                        itemexists.AvailableStock -= salesexists.Quantity;
                        itemexists.Quantity -= salesexists.Quantity;
                    }
                    if (salesexists.SalesStatus == "In Transit")
                    {
                        itemexists.StockInTransit = salesexists.Quantity + itemexists.StockInTransit;
                    }
                    if (itemexists.AvailableStock > itemexists.ReOrderLevel)
                    {
                        itemexists.Status = "Good";
                    }
                    else if (itemexists.AvailableStock < itemexists.ReOrderLevel && itemexists.AvailableStock > 0)
                    {
                        itemexists.Status = "Low";
                    }
                    else
                    {
                        itemexists.Status = "Out";
                    }






                    scopedcontext.Update(salesexists);
                    await scopedcontext.SaveChangesAsync();
                    ;

                    return new StockResponse(true, $"{salesexists.BrandName} {salesexists.ItemName} status changged success fully to '{changeSalesStatusvm.SalesStatus}'", null);

                }

            }
            catch (Exception ex)

            {

                return new StockResponse(false, ex.Message, null);
            }
        }

        public async Task<StockResponse> AddReturnedStatus(AddReturnedStatusvm addReturnedStatusvm)
        {
            try
            {
                if (addReturnedStatusvm.ReturnedStatus == "")
                {

                    return new StockResponse(false, "Kindly provide  returned status to add status", null);
                }
                if (addReturnedStatusvm.ReturnedDescription == "")
                {

                    return new StockResponse(false, "Kindly provide  reason for status  to add status", null);
                }

                using (var scope = _serviceScopeFactory.CreateScope())
                {
                    var scopedcontext = scope.ServiceProvider.GetRequiredService<DragonFlyContext>();



                    var returnstatusexists = await scopedcontext.ReturnedStatusTable.Where(x => x.ReturnedID == addReturnedStatusvm.ReturnedID).FirstOrDefaultAsync();

                    if (returnstatusexists != null)
                    {
                        return new StockResponse(false, $" Returned status  '{addReturnedStatusvm.ReturnedStatus}' already exists", null);
                    }
                    var itemclass = new AddReturnedStatus
                    {
                        ReturnedStatus = addReturnedStatusvm.ReturnedStatus,
                        ReturnedDescription = addReturnedStatusvm.ReturnedDescription,

                    };

                    await scopedcontext.AddAsync(itemclass);
                    await scopedcontext.SaveChangesAsync();
                    return new StockResponse(true, $" '{addReturnedStatusvm.ReturnedStatus}'  has been added successfully", null);

                }

            }
            catch (Exception ex)
            {
                return new StockResponse(false, ex.Message, null);

            }


        }
        public async Task<StockResponse> GetAllReturnedStatus()
        {
            try
            {

                using (var scope = _serviceScopeFactory.CreateScope())
                {
                    var scopedcontext = scope.ServiceProvider.GetRequiredService<DragonFlyContext>();
                    var allreturnedstatus = await scopedcontext.ReturnedStatusTable.ToListAsync();

                    if (allreturnedstatus == null)
                    {
                        return new StockResponse(false, "Returned status doesn't exist", null);
                    }
                    return new StockResponse(true, "Successfully queried", allreturnedstatus);

                }
            }
            catch (Exception ex)
            {
                return new StockResponse(false, ex.Message, null);
            }

        }
        public async Task<StockResponse> AddReturnedStock(AddReturnedStockvm addReturnedStockvm)
        {
            try
            {
                if (addReturnedStockvm.ReturnedStatus == "")
                {

                    return new StockResponse(false, "Kindly provide  returned quantity to add returned stock", null);
                }
                if (addReturnedStockvm.ReturnedQuantity < 0)
                {

                    return new StockResponse(false, "Kindly provide  quantity  to add returned stock", null);
                }
                if (addReturnedStockvm.BrandName == "")
                {
                    return new StockResponse(false, "Kindly provide brandname to add returned stock", null);

                }
                if (addReturnedStockvm.ItemName == "")
                {
                    return new StockResponse(false, "Kindly provide itemname to add returned stock", null);

                }
                if (addReturnedStockvm.ReturnReason == "")
                {
                    return new StockResponse(false, "Kindly provide return reason to add returned stock", null);
                }


                using (var scope = _serviceScopeFactory.CreateScope())
                {
                    var scopedcontext = scope.ServiceProvider.GetRequiredService<DragonFlyContext>();
                    var itemexists = await scopedcontext.AddStock
                      .Where(y => y.BrandName == addReturnedStockvm.BrandName &&
                      y.ItemName == addReturnedStockvm.ItemName).FirstOrDefaultAsync();
                    if (itemexists == null)
                    {
                        return new StockResponse(false, "Does not exist", null);
                    }




                    var returnstatusexists = await scopedcontext.ReturnedStock.Where(x => x.ReturnedId == addReturnedStockvm.ReturnedId).FirstOrDefaultAsync();

                    if (returnstatusexists != null)
                    {
                        return new StockResponse(false, $" Returned stock  '{addReturnedStockvm.ItemName}' already exists", null);
                    }
                    var itemclass = new AddReturnedStock
                    {
                        ReturnedStatus = addReturnedStockvm.ReturnedStatus,
                        ReturnedQuantity = addReturnedStockvm.ReturnedQuantity,
                        ReturnReason = addReturnedStockvm.ReturnReason,
                        DateReturned = addReturnedStockvm.DateReturned,
                        BrandName = addReturnedStockvm.BrandName,
                        ItemName = addReturnedStockvm.ItemName,


                    };
                    itemexists.AvailableStock += addReturnedStockvm.ReturnedQuantity;
                    itemexists.Quantity += addReturnedStockvm.ReturnedQuantity;
                    itemexists.TotalReturnedStock += addReturnedStockvm.ReturnedQuantity;
                    if (itemexists.AvailableStock == itemexists.ReOrderLevel || itemexists.AvailableStock < itemexists.ReOrderLevel)
                    {
                        itemexists.ReorderRequired = "Yes";
                    }
                    else
                    {
                        itemexists.ReorderRequired = "No";
                    }
                    if (itemexists.AvailableStock > itemexists.ReOrderLevel)
                    {
                        itemexists.Status = "Good";
                    }
                    else if (itemexists.AvailableStock < itemexists.ReOrderLevel && itemexists.AvailableStock > 0 || itemexists.AvailableStock == itemexists.ReOrderLevel)
                    {
                        itemexists.Status = "Low";
                    }
                    else
                    {
                        itemexists.Status = "Out";
                    }
                    await scopedcontext.AddAsync(itemclass);
                    await scopedcontext.SaveChangesAsync();
                    return new StockResponse(true, $" '{addReturnedStockvm.ItemName}'  has been added to return stock successfully ", null);

                }

            }
            catch (Exception ex)
            {
                return new StockResponse(false, ex.Message, null);

            }


        }
        public async Task<StockResponse> GetAllReturnedStock()
        {
            try
            {

                using (var scope = _serviceScopeFactory.CreateScope())
                {
                    var scopedcontext = scope.ServiceProvider.GetRequiredService<DragonFlyContext>();
                    var allreturnedstatus = await scopedcontext.ReturnedStock.ToListAsync();

                    if (allreturnedstatus == null)
                    {
                        return new StockResponse(false, "Returned stock doesn't exist", null);
                    }
                    return new StockResponse(true, "Successfully queried", allreturnedstatus);

                }
            }
            catch (Exception ex)
            {
                return new StockResponse(false, ex.Message, null);
            }

        }
        public async Task<StockResponse> SearchForStock(string search_query)
        {

            try
            {
                using (var scope = _serviceScopeFactory.CreateScope())
                {

                    var scopedcontext = scope.ServiceProvider.GetRequiredService<DragonFlyContext>();

                    var allstock = await scopedcontext.AddStock.Where
                        (u => EF.Functions.Like(u.BrandName, $"%{search_query}%") ||
                        EF.Functions.Like(u.ItemName, $"%{search_query}%") ||
                        EF.Functions.Like(u.Status, $"%{search_query}%") ||
                        EF.Functions.Like(u.ReorderRequired, $"%{search_query}%")
                        ).ToListAsync();

                    if (allstock == null)
                        return new StockResponse(false, "", null);

                    return new StockResponse(true, "Successfully queried", allstock);


                }
            }
            catch (Exception ex)
            {

                return new StockResponse(false, ex.Message, null);
            }
        }

        public async Task<StockResponse> SearchForStockIn(string search_query)
        {

            try
            {
                using (var scope = _serviceScopeFactory.CreateScope())
                {

                    var scopedcontext = scope.ServiceProvider.GetRequiredService<DragonFlyContext>();

                    var allstock = await scopedcontext.Purchases.Where
                         (u => EF.Functions.Like(u.BrandName, $"%{search_query}%") ||
                        EF.Functions.Like(u.ItemName, $"%{search_query}%") ||
                        EF.Functions.Like(u.SupplierName, $"%{search_query}%") ||
                        EF.Functions.Like(u.Currency, $"%{search_query}%")

                        ).ToListAsync();

                    if (allstock == null)
                        return new StockResponse(false, "", null);

                    return new StockResponse(true, "Successfully queried", allstock);


                }
            }
            catch (Exception ex)
            {

                return new StockResponse(false, ex.Message, null);
            }
        }
        public async Task<StockResponse> SearchForStockOut(string search_query)
        {

            try
            {
                using (var scope = _serviceScopeFactory.CreateScope())
                {

                    var scopedcontext = scope.ServiceProvider.GetRequiredService<DragonFlyContext>();

                    var allstock = await scopedcontext.Sales.Where
                         (u => EF.Functions.Like(u.BrandName, $"%{search_query}%") ||
                        EF.Functions.Like(u.ItemName, $"%{search_query}%") ||
                        EF.Functions.Like(u.CustomerName, $"%{search_query}%") ||
                        EF.Functions.Like(u.Comments, $"%{search_query}%") ||
                         EF.Functions.Like(u.Department, $"%{search_query}%")

                        ).ToListAsync();

                    if (allstock == null)
                        return new StockResponse(false, "", null);

                    return new StockResponse(true, "Successfully queried", allstock);


                }
            }
            catch (Exception ex)
            {

                return new StockResponse(false, ex.Message, null);
            }
        }



        public async Task<StockResponse> SearchForCustomer(string search_query)
        {

            try
            {
                using (var scope = _serviceScopeFactory.CreateScope())
                {

                    var scopedcontext = scope.ServiceProvider.GetRequiredService<DragonFlyContext>();

                    var allstock = await scopedcontext.Customer.Where
                         (u => EF.Functions.Like(u.CustomerName, $"%{search_query}%") ||
                        EF.Functions.Like(u.CompanyName, $"%{search_query}%") ||
                        EF.Functions.Like(u.Email, $"%{search_query}%") ||
                        EF.Functions.Like(u.PhoneNumber, $"%{search_query}%")

                        ).ToListAsync();

                    if (allstock == null)
                        return new StockResponse(false, "", null);

                    return new StockResponse(true, "Successfully queried", allstock);


                }
            }
            catch (Exception ex)
            {

                return new StockResponse(false, ex.Message, null);
            }
        }
        public async Task<StockResponse> SearchForSupplier(string search_query)
        {

            try
            {
                using (var scope = _serviceScopeFactory.CreateScope())
                {

                    var scopedcontext = scope.ServiceProvider.GetRequiredService<DragonFlyContext>();

                    var allstock = await scopedcontext.Suppliers.Where
                         (u => EF.Functions.Like(u.SupplierName, $"%{search_query}%") ||
                        EF.Functions.Like(u.CompanyName, $"%{search_query}%") ||
                        EF.Functions.Like(u.Email, $"%{search_query}%") ||
                        EF.Functions.Like(u.PhoneNumber, $"%{search_query}%")

                        ).ToListAsync();

                    if (allstock == null)
                        return new StockResponse(false, "", null);

                    return new StockResponse(true, "Successfully queried", allstock);


                }
            }
            catch (Exception ex)
            {

                return new StockResponse(false, ex.Message, null);
            }
        }
        public async Task<StockResponse> EditSales(editSalesvm salesvm)
        {
            try
            {

                using (var scope = _serviceScopeFactory.CreateScope())
                {

                    var scopedcontext = scope.ServiceProvider.GetRequiredService<DragonFlyContext>();

                    var salesexists = await scopedcontext.Sales.Where(u => u.SalesId == salesvm.SalesId).FirstOrDefaultAsync();

                    if (salesexists == null)
                    {

                        return new StockResponse(false, "stock does not exist", null);
                    }

                    if (salesvm.BrandName == "string")
                    {
                        salesexists.BrandName = salesexists.BrandName;

                    }
                    else
                    {
                        salesexists.BrandName = salesvm.BrandName;
                    }


                    if (salesvm.ItemName == "string")
                    {
                        salesexists.ItemName = salesexists.ItemName;

                    }
                    else
                    {
                        salesexists.ItemName = salesvm.ItemName;
                    }
                    if (salesvm.CustomerName == "string")
                    {
                        salesexists.CustomerName = salesexists.CustomerName;

                    }
                    else
                    {
                        salesexists.CustomerName = salesvm.CustomerName;
                    }
                    if (salesvm.Comments == "string")
                    {
                        salesexists.Comments = salesexists.Comments;

                    }
                    else
                    {
                        salesexists.Comments = salesvm.Comments;
                    }
                    if (salesvm.Department == "string")
                    {
                        salesexists.Department = salesexists.Department;

                    }
                    else
                    {
                        salesexists.Department = salesvm.Department;
                    }




                    scopedcontext.Update(salesexists);
                    await scopedcontext.SaveChangesAsync();

                    return new StockResponse(true, "Sucessfully updated stock out details", salesexists);


                }

            }
            catch (Exception ex)
            {

                return new StockResponse(false, ex.Message, null);
            }
        }
        public async Task<StockResponse> DeleteStockOut(int salesId)
        {
            try
            {
                using (var scope = _serviceScopeFactory.CreateScope())
                {
                    var scopecontext = scope.ServiceProvider.GetRequiredService<DragonFlyContext>();

                    var departmentexists = await scopecontext.Sales.Where(x => x.SalesId == salesId).FirstOrDefaultAsync();

                    if (departmentexists == null)
                    {
                        return new StockResponse(false, "stock does not exist ", null);
                    }
                    scopecontext.Remove(departmentexists);
                    await scopecontext.SaveChangesAsync();

                    return new StockResponse(true, "stockOut deleted successfully", null);
                }
            }
            catch (Exception ex)
            {
                return new StockResponse(false, ex.Message, null);
            }


        }
        public async Task<StockResponse> EditCustomer(EditCustomervm editCustomervm)
        {
            try
            {

                using (var scope = _serviceScopeFactory.CreateScope())
                {

                    var scopedcontext = scope.ServiceProvider.GetRequiredService<DragonFlyContext>();

                    var salesexists = await scopedcontext.Customer.Where(u => u.CustomerId == editCustomervm.CustomerId).FirstOrDefaultAsync();

                    if (salesexists == null)
                    {

                        return new StockResponse(false, "stock does not exist", null);
                    }

                    if (editCustomervm.CustomerName == "string")
                    {
                        salesexists.CustomerName = salesexists.CustomerName;

                    }
                    else
                    {
                        salesexists.CustomerName = editCustomervm.CustomerName;
                    }


                    if (editCustomervm.Email == "string")
                    {
                        salesexists.Email = salesexists.Email;

                    }
                    else
                    {
                        salesexists.Email = editCustomervm.Email;
                    }
                    if (editCustomervm.CompanyName == "string")
                    {
                        salesexists.CompanyName = salesexists.CompanyName;

                    }
                    else
                    {
                        salesexists.CompanyName = editCustomervm.CompanyName;
                    }
                    if (editCustomervm.PhoneNumber == "string")
                    {
                        salesexists.PhoneNumber = salesexists.PhoneNumber;

                    }
                    else
                    {
                        salesexists.PhoneNumber = editCustomervm.PhoneNumber;
                    }





                    scopedcontext.Update(salesexists);
                    await scopedcontext.SaveChangesAsync();

                    return new StockResponse(true, "Sucessfully updated stock out details", salesexists);


                }

            }
            catch (Exception ex)
            {

                return new StockResponse(false, ex.Message, null);
            }
        }
        public async Task<StockResponse> GetCustomerById(int customerId)
        {
            try
            {
                using (var scope = _serviceScopeFactory.CreateScope())
                {

                    var scopedcontext = scope.ServiceProvider.GetRequiredService<DragonFlyContext>();
                    var customerexists = await scopedcontext.Customer.Where(u => u.CustomerId == customerId).FirstOrDefaultAsync();
                    if (customerexists == null)
                    {
                        return new StockResponse(false, "not found", null);
                    }
                    return new StockResponse(true, "Queried successfully", customerexists);
                }
            }
            catch (Exception ex)
            {
                return new StockResponse(false, ex.Message, null);
            }
        }
        public async Task<StockResponse> GetSupplierById(int supplierId)
        {
            try
            {
                using (var scope = _serviceScopeFactory.CreateScope())
                {

                    var scopedcontext = scope.ServiceProvider.GetRequiredService<DragonFlyContext>();
                    var supplierexists = await scopedcontext.Suppliers.Where(u => u.SupplierId == supplierId).FirstOrDefaultAsync();
                    if (supplierexists == null)
                    {
                        return new StockResponse(false, "not found", null);
                    }
                    return new StockResponse(true, "Queried successfully", supplierexists);
                }
            }
            catch (Exception ex)
            {
                return new StockResponse(false, ex.Message, null);
            }
        }
        public async Task<StockResponse> EditSupplier(editSuppliervm suppliervm)
        {
            try
            {

                using (var scope = _serviceScopeFactory.CreateScope())
                {

                    var scopedcontext = scope.ServiceProvider.GetRequiredService<DragonFlyContext>();

                    var salesexists = await scopedcontext.Suppliers.Where(u => u.SupplierId == suppliervm.SupplierId).FirstOrDefaultAsync();

                    if (salesexists == null)
                    {

                        return new StockResponse(false, "stock does not exist", null);
                    }

                    if (suppliervm.SupplierName == "string")
                    {
                        salesexists.SupplierName = salesexists.SupplierName;

                    }
                    else
                    {
                        salesexists.SupplierName = suppliervm.SupplierName;
                    }


                    if (suppliervm.Email == "string")
                    {
                        salesexists.Email = salesexists.Email;

                    }
                    else
                    {
                        salesexists.Email = suppliervm.Email;
                    }
                    if (suppliervm.CompanyName == "string")
                    {
                        salesexists.CompanyName = salesexists.CompanyName;

                    }
                    else
                    {
                        salesexists.CompanyName = suppliervm.CompanyName;
                    }
                    if (suppliervm.PhoneNumber == "string")
                    {
                        salesexists.PhoneNumber = salesexists.PhoneNumber;

                    }
                    else
                    {
                        salesexists.PhoneNumber = suppliervm.PhoneNumber;
                    }





                    scopedcontext.Update(salesexists);
                    await scopedcontext.SaveChangesAsync();

                    return new StockResponse(true, "Sucessfully updated stock out details", salesexists);


                }

            }
            catch (Exception ex)
            {

                return new StockResponse(false, ex.Message, null);
            }
        }
        public async Task<StockResponse> GetBrandById(int BrandId)
        {
            try
            {
                using (var scope = _serviceScopeFactory.CreateScope())
                {

                    var scopedcontext = scope.ServiceProvider.GetRequiredService<DragonFlyContext>();
                    var supplierexists = await scopedcontext.AddBrand.Where(u => u.BrandId == BrandId).FirstOrDefaultAsync();
                    if (supplierexists == null)
                    {
                        return new StockResponse(false, "not found", null);
                    }
                    return new StockResponse(true, "Queried successfully", supplierexists);
                }
            }
            catch (Exception ex)
            {
                return new StockResponse(false, ex.Message, null);
            }
        }
        public async Task<StockResponse> GetItemsById(int ItemId)
        {
            try
            {
                using (var scope = _serviceScopeFactory.CreateScope())
                {

                    var scopedcontext = scope.ServiceProvider.GetRequiredService<DragonFlyContext>();
                    var supplierexists = await scopedcontext.AddItem.Where(u => u.ItemID == ItemId).FirstOrDefaultAsync();
                    if (supplierexists == null)
                    {
                        return new StockResponse(false, "not found", null);
                    }
                    return new StockResponse(true, "Queried successfully", supplierexists);
                }
            }
            catch (Exception ex)
            {
                return new StockResponse(false, ex.Message, null);
            }
        }
        public async Task<StockResponse> EditBrand(EditBrandvm editBrandvm)
        {
            try
            {

                using (var scope = _serviceScopeFactory.CreateScope())
                {

                    var scopedcontext = scope.ServiceProvider.GetRequiredService<DragonFlyContext>();

                    var salesexists = await scopedcontext.AddBrand.Where(u => u.BrandId == editBrandvm.BrandId).FirstOrDefaultAsync();

                    if (salesexists == null)
                    {

                        return new StockResponse(false, "stock does not exist", null);
                    }

                    if (editBrandvm.BrandName == "string")
                    {
                        salesexists.BrandName = salesexists.BrandName;

                    }
                    else
                    {
                        salesexists.BrandName = editBrandvm.BrandName;
                    }




                    scopedcontext.Update(salesexists);
                    await scopedcontext.SaveChangesAsync();

                    return new StockResponse(true, "Sucessfully updated stock out details", salesexists);


                }

            }
            catch (Exception ex)
            {

                return new StockResponse(false, ex.Message, null);
            }
        }
        public async Task<StockResponse> EditItem(EditItemvm editItemvm)
        {
            try
            {

                using (var scope = _serviceScopeFactory.CreateScope())
                {

                    var scopedcontext = scope.ServiceProvider.GetRequiredService<DragonFlyContext>();

                    var salesexists = await scopedcontext.AddItem.Where(u => u.ItemID == editItemvm.ItemId).FirstOrDefaultAsync();

                    if (salesexists == null)
                    {

                        return new StockResponse(false, "item does not exist", null);
                    }

                    if (editItemvm.ItemName == "string")
                    {
                        salesexists.ItemName = salesexists.ItemName;

                    }
                    else
                    {
                        salesexists.ItemName = editItemvm.ItemName;
                    }
                    if (editItemvm.BrandName == "string")
                    {
                        salesexists.BrandName = salesexists.BrandName;

                    }
                    else
                    {
                        salesexists.BrandName = editItemvm.BrandName;
                    }
                    if (editItemvm.Category == "string")
                    {
                        salesexists.Category = salesexists.Category;

                    }
                    else
                    {
                        salesexists.Category = editItemvm.Category;
                    }
                    if (editItemvm.Currency == "string")
                    {
                        salesexists.Currency = salesexists.Currency;

                    }
                    else
                    {
                        salesexists.Currency = editItemvm.Currency;
                    }
                    if (editItemvm.ItemDescription == "string")
                    {
                        salesexists.ItemDescription = salesexists.ItemDescription;

                    }
                    else
                    {
                        salesexists.ItemDescription = editItemvm.ItemDescription;
                    }
                    if (editItemvm.IndicativePrice < 0)
                    {
                        salesexists.IndicativePrice = salesexists.IndicativePrice;

                    }
                    else
                    {
                        salesexists.IndicativePrice = editItemvm.IndicativePrice;
                    }
                    if (editItemvm.ReOrderLevel < 0)
                    {
                        salesexists.ReOrderLevel = salesexists.ReOrderLevel;

                    }
                    else
                    {
                        salesexists.ReOrderLevel = editItemvm.ReOrderLevel;
                    }





                    scopedcontext.Update(salesexists);
                    await scopedcontext.SaveChangesAsync();

                    return new StockResponse(true, "Sucessfully updated stock out details", salesexists);


                }

            }
            catch (Exception ex)
            {

                return new StockResponse(false, ex.Message, null);
            }
        }
        public async Task<StockResponse> EditStockIn(EditPurchasevm editPurchasevm)
        {
            try
            {

                using (var scope = _serviceScopeFactory.CreateScope())
                {

                    var scopedcontext = scope.ServiceProvider.GetRequiredService<DragonFlyContext>();

                    var purchaseexists = await scopedcontext.Purchases.Where(u => u.PurchaseId == editPurchasevm.PurchaseId).FirstOrDefaultAsync();

                    if (purchaseexists == null)
                    {

                        return new StockResponse(false, "stock does not exist", null);
                    }

                    if (editPurchasevm.BrandName == "string")
                    {
                        purchaseexists.BrandName = purchaseexists.BrandName;

                    }
                    else
                    {
                        purchaseexists.BrandName = editPurchasevm.BrandName;
                    }


                    if (editPurchasevm.ItemName == "string")
                    {
                        purchaseexists.ItemName = purchaseexists.ItemName;

                    }
                    else
                    {
                        purchaseexists.ItemName = editPurchasevm.ItemName;
                    }
                    if (editPurchasevm.SupplierName == "string")
                    {
                        purchaseexists.SupplierName = purchaseexists.SupplierName;

                    }
                    else
                    {
                        purchaseexists.SupplierName = editPurchasevm.SupplierName;
                    }






                    scopedcontext.Update(purchaseexists);
                    await scopedcontext.SaveChangesAsync();

                    return new StockResponse(true, "Sucessfully updated stock in details", purchaseexists);


                }

            }
            catch (Exception ex)
            {

                return new StockResponse(false, ex.Message, null);
            }
        }
        public async Task<StockResponse> AddCategory(AddCategoryvm addCategoryvm)
        {
            try
            {
                if (addCategoryvm.CategoryName == "")
                {

                    return new StockResponse(false, "Kindly provide a category name to add category", null);
                }
                using (var scope = _serviceScopeFactory.CreateScope())
                {
                    var scopedcontext = scope.ServiceProvider.GetRequiredService<DragonFlyContext>();

                    //check if role exists 

                    var categoryexists = await scopedcontext.Category.Where(x => x.CategoryName == addCategoryvm.CategoryName).FirstOrDefaultAsync();

                    if (categoryexists != null)
                    {
                        return new StockResponse(false, $" Category  '{addCategoryvm.CategoryName}' already exist, if  must add a similar category kindly change the " +
                             $"latter cases from lower to upper and vice versa depending on the existing  category . The existsing role is '{categoryexists}' with category id {categoryexists.CategoryID} ", null);
                    }
                    var categoryclass = new AddCategory
                    {
                        CategoryName = addCategoryvm.CategoryName,
                        CategoryDesc = addCategoryvm.CategoryDesc,
                    };
                    await scopedcontext.AddAsync(categoryclass);
                    await scopedcontext.SaveChangesAsync();
                    return new StockResponse(true, $"Category '{addCategoryvm.CategoryName}'  created successfully", null);

                }

            }
            catch (Exception ex)
            {
                return new StockResponse(false, ex.Message, null);

            }

        }
        public async Task<StockResponse> GetAllCategory()
        {

            try
            {

                using (var scope = _serviceScopeFactory.CreateScope())
                {
                    var scopedcontext = scope.ServiceProvider.GetRequiredService<DragonFlyContext>();
                    var allcategory = await scopedcontext.Category.ToListAsync();

                    if (allcategory == null)
                    {
                        return new StockResponse(false, "category doesn't exist", null);
                    }
                    return new StockResponse(true, "Successfully queried", allcategory);

                }
            }
            catch (Exception ex)
            {
                return new StockResponse(false, ex.Message, null);
            }
        }
        public async Task<StockResponse> AddInvoiceDetails(StockInvm stockInvm)
        {

            try
            {
                if (stockInvm.SupplierName == "")
                {

                    return new StockResponse(false, "Kindly provide an suplier name ", null);
                }


                using (var scope = _serviceScopeFactory.CreateScope())
                {
                    var scopedcontext = scope.ServiceProvider.GetRequiredService<DragonFlyContext>();



                    var itemexists = await scopedcontext.StockIn.Where(x => x.InvoiceDate == stockInvm.InvoiceDate && x.SupplierName == stockInvm.SupplierName).FirstOrDefaultAsync();

                    if (itemexists != null)
                    {
                        return new StockResponse(false, $" Invoice  already exist... ", null);
                    }

                    var lpo_no_obj = GetLpo_Number().Result;
                    var lponumber = "LPO-" + lpo_no_obj;
                    var invoice_no_obj = GetInvoiceNumber().Result;
                    var invoicenumber = "INV-" + invoice_no_obj;
                    var itemclass = new StockIn
                    {
                        SupplierName = stockInvm.SupplierName,
                        InvoiceDate = stockInvm.InvoiceDate,
                        LPODate = stockInvm.LPODate,
                        LPONumber = lponumber,
                        InvoiceNumber = invoicenumber,
                        Status = "Incomplete",



                    };
                    var invoicedateexists = await scopedcontext.StockIn.Where(x => x.InvoiceDate == itemclass.InvoiceDate && x.SupplierName == stockInvm.SupplierName).FirstOrDefaultAsync();

                    if (invoicedateexists != null)
                    {
                        return new StockResponse(false, $" Invoice with this date  already exist... ", null);
                    }
                    await scopedcontext.AddAsync(itemclass);
                    await scopedcontext.SaveChangesAsync();
                    return new StockResponse(true, $"Invoice created successfully", null);

                }

            }
            catch (Exception ex)
            {
                return new StockResponse(false, ex.Message, null);

            }



        }


        public async Task<int> GetLpo_Number()
        {

            var last_number_obj = await _dragonFlyContext.LPO_No
                .OrderByDescending(y => y.DateCreated).FirstOrDefaultAsync();


            if (last_number_obj == null)
            {
                var newvalue = new lpoNo();

                newvalue.LpoNo = 1;
                await _dragonFlyContext.AddAsync(newvalue);
                await _dragonFlyContext.SaveChangesAsync();
                return newvalue.LpoNo;
            }

            last_number_obj.LpoNo = last_number_obj.LpoNo + 1;
            _dragonFlyContext.Update(last_number_obj);
            await _dragonFlyContext.SaveChangesAsync();

            return last_number_obj.LpoNo;
        }
        public async Task<int> GetInvoiceNumber()
        {

            var last_number_obj = await _dragonFlyContext.InvoiceNo
                .OrderByDescending(y => y.DateCreated).FirstOrDefaultAsync();


            if (last_number_obj == null)
            {
                var newvalue = new InvoiceNo();

                newvalue.InvoieNo = 1;
                await _dragonFlyContext.AddAsync(newvalue);
                await _dragonFlyContext.SaveChangesAsync();
                return newvalue.InvoieNo;
            }

            last_number_obj.InvoieNo = last_number_obj.InvoieNo + 1;
            _dragonFlyContext.Update(last_number_obj);
            await _dragonFlyContext.SaveChangesAsync();

            return last_number_obj.InvoieNo;
        }
        public async Task<StockResponse> GetInvoiceDetails()
        {
            try
            {

                using (var scope = _serviceScopeFactory.CreateScope())
                {
                    var scopedcontext = scope.ServiceProvider.GetRequiredService<DragonFlyContext>();
                    var allinvoice = await scopedcontext.StockIn.OrderByDescending(y => y.StockInDate).ToListAsync();

                    if (allinvoice == null)
                    {
                        return new StockResponse(false, "Invoice doesn't exist", null);
                    }
                    return new StockResponse(true, "Successfully queried", allinvoice);

                }
            }
            catch (Exception ex)
            {
                return new StockResponse(false, ex.Message, null);
            }

        }
        public async Task<StockResponse> AddBatchDetails(AddBatchDetailsvm addBatchDetailsvm)
        {
            try
            {
                if (addBatchDetailsvm.ItemName == "")
                {

                    return new StockResponse(false, "Kindly provide an item name ", null);
                }
                if (addBatchDetailsvm.CategoryName == "")
                {
                    return new StockResponse(false, "Kindly provide category", null);

                }

                if (addBatchDetailsvm.Currency == "")
                {
                    return new StockResponse(false, "Kindly provide currency", null);
                }
                if (addBatchDetailsvm.UnitPrice < 0)
                {
                    return new StockResponse(false, "Kindly provide unit price", null);
                }
                if (addBatchDetailsvm.Quantity < 0)
                {
                    return new StockResponse(false, "Kindly provide quantity", null);
                }
                if (addBatchDetailsvm.Warranty < 0)
                {
                    return new StockResponse(false, "Kindly provide warranty", null);
                }
                using (var scope = _serviceScopeFactory.CreateScope())
                {
                    var scopedcontext = scope.ServiceProvider.GetRequiredService<DragonFlyContext>();
                    var invoiceexists = await scopedcontext.StockIn
                 .Where(y => y.InvoiceNumber == addBatchDetailsvm.InvoiceNumber).OrderByDescending(y => y.InvoiceNumber).FirstOrDefaultAsync();
                    if (invoiceexists == null)
                    {
                        return new StockResponse(false, "Invoice Number does not exist", null);
                    }

                    var loggedinuserobject = await _extraServices.LoggedInUser();

                    var userEmail = loggedinuserobject.FirstName + ' ' + loggedinuserobject.LastName;

                    if (loggedinuserobject == null)
                    {

                        return new StockResponse(false, "user not logged in. login again", null);

                    }




                    var itemNameexists = await scopedcontext.AddItem
                   .Where(y => y.ItemName == addBatchDetailsvm.ItemName).FirstOrDefaultAsync();
                    if (itemNameexists == null)
                    {
                        return new StockResponse(false, "Item name does not exist", null);
                    }

                    var itemclass = new InvoiceLinesDetails
                    {
                        ItemName = addBatchDetailsvm.ItemName,
                        CategoryName = addBatchDetailsvm.CategoryName,
                        Currency = addBatchDetailsvm.Currency,
                        UnitPrice = addBatchDetailsvm.UnitPrice,
                        Warranty = addBatchDetailsvm.Warranty,
                        Quantity = addBatchDetailsvm.Quantity,
                        WarrantyStartDate = addBatchDetailsvm.WarrantyStartDate,
                        UpdatedBy = userEmail,
                        UpdatedOn = addBatchDetailsvm.UpdatedOn,
                        InvoiceNumber = addBatchDetailsvm.InvoiceNumber,
                        BrandName = addBatchDetailsvm.BrandName,







                    };
                    //var itemassigned = await scopedcontext.AddProductDetails.Where(x => x.ItemID == itemclass.InvoiceLineId).FirstOrDefaultAsync();

                    //if (itemassigned != null)
                    //{
                    //    itemassigned.ProductStatus = itemclass.Status;
                    //    return new StockResponse(false, "ITEM ALREADY COMPLETE", null);
                    //}
                    var itemexists = await scopedcontext.InvoiceLinesDetails.Where(x => x.ItemName == addBatchDetailsvm.ItemName && x.BrandName == addBatchDetailsvm.BrandName && x.InvoiceNumber == addBatchDetailsvm.InvoiceNumber && x.CategoryName == itemNameexists.Category).FirstOrDefaultAsync();


                    itemclass.CategoryName = itemNameexists.Category;

                    if (itemexists != null)
                    {
                        return new StockResponse(false, $" Invoice {addBatchDetailsvm.InvoiceNumber} with'{addBatchDetailsvm.BrandName}-{addBatchDetailsvm.ItemName} already exists ", null);
                    }


                    itemclass.Reference_Number = await GetGeneratedref();
                    itemclass.TotalUnitPrice = itemclass.UnitPrice * itemclass.Quantity;

                    if (itemclass.CategoryName == "Accesory")
                    {
                        itemclass.Status = "Complete";
                    }
                    else
                    {
                        itemclass.Status = "Incomplete";
                    }




                    itemclass.WarrantyEndDate = itemclass.WarrantyStartDate.AddMonths(itemclass.Warranty);
                    await scopedcontext.AddAsync(itemclass);
                    await scopedcontext.SaveChangesAsync();
                    if (itemclass.CategoryName == "Product")
                    {
                        var new_numb = 0;



                        while (new_numb < itemclass.Quantity)
                        {
                            new_numb++;
                            var new_numbering = new ProductNumbering
                            {

                                NumberValue = new_numb,
                                Reference_Number = itemclass.Reference_Number,
                                Type = "Product",
                                Status = "UNASSIGNED"


                            };







                            await _dragonFlyContext.AddAsync(new_numbering);
                            await _dragonFlyContext.SaveChangesAsync();
                        }
                    }

                    return new StockResponse(true, $"Item '{addBatchDetailsvm.BrandName}-{addBatchDetailsvm.ItemName}' in invoice{addBatchDetailsvm.InvoiceNumber}  created successfully", null);

                }

            }
            catch (Exception ex)
            {
                return new StockResponse(false, ex.Message, null);

            }

        }
        public async Task<StockResponse> GetInvoiceLines()
        {
            try
            {

                using (var scope = _serviceScopeFactory.CreateScope())
                {
                    var scopedcontext = scope.ServiceProvider.GetRequiredService<DragonFlyContext>();
                    var allinvoice = await scopedcontext.InvoiceLinesDetails.OrderByDescending(y => y.UpdatedOn).ToListAsync();

                    if (allinvoice == null)
                    {
                        return new StockResponse(false, "Invoice doesn't exist", null);
                    }

                    return new StockResponse(true, "Successfully queried", allinvoice);

                }
            }
            catch (Exception ex)
            {
                return new StockResponse(false, ex.Message, null);
            }
        }
        public async Task<StockResponse> AddProductDetails(AddProductDetailvm addProductDetailvm)
        {
            try
            {
                if (addProductDetailvm.SerialNumber == "")
                {

                    return new StockResponse(false, "Kindly provide serial number ", null);
                }
                if (addProductDetailvm.IMEI1 == "")
                {

                    return new StockResponse(false, "Kindly provide  IMEI1 details ", null);
                }
                if (addProductDetailvm.IMEI2 == "")
                {

                    return new StockResponse(false, "Kindly provide IMEI2 details", null);
                }

                using (var scope = _serviceScopeFactory.CreateScope())
                {
                    var scopedcontext = scope.ServiceProvider.GetRequiredService<DragonFlyContext>();

                    //var itemexists = await scopedcontext.InvoiceLinesDetails
                    //    .Where(y => y.InvoiceLineId == addProductDetailvm.invoiceItemId ).FirstOrDefaultAsync();

                    //if (itemexists == null)
                    //{
                    //    return new StockResponse(false, "Does not exist", null);
                    //}
                    //if (itemexists.CategoryName == "Accesory")
                    //{

                    //    itemexists.Status = "Complete";
                    //}


                    var itemclass = new AddProductDetails
                    {

                        SerialNumber = addProductDetailvm.SerialNumber,
                        IMEI1 = addProductDetailvm.IMEI1,
                        IMEI2 = addProductDetailvm.IMEI2,
                        ItemID = addProductDetailvm.BatchID,
                        SerialStatus = "Not Issued",
                        BatchNumber = addProductDetailvm.BatchNumber,
                        ItemStatus = "Okay",


                    };


                    var itemexists = await scopedcontext.AddProductDetails
                    .Where(y => y.SerialNumber == itemclass.SerialNumber).FirstOrDefaultAsync();


                    if (itemexists != null)
                    {
                        return new StockResponse(false, $"Serial Number '{addProductDetailvm.SerialNumber}' already exists", null);

                    }
                    var imei1exists = await scopedcontext.AddProductDetails
                   .Where(y => y.IMEI1 == itemclass.IMEI1).FirstOrDefaultAsync();

                    if (imei1exists != null)
                    {
                        return new StockResponse(false, $"IMEI1 '{itemclass.IMEI1}' already exists", null);

                    }
                    var imei2exists = await scopedcontext.AddProductDetails
                .Where(y => y.IMEI2 == itemclass.IMEI2).FirstOrDefaultAsync();

                    if (imei2exists != null)
                    {
                        return new StockResponse(false, $"IMEI2 '{itemclass.IMEI2}' already exists", null);

                    }
                    scopedcontext.Update(itemclass);
                    //var quantitydamaged = await scopedcontext.StockAdjustment.Where(x => x.BatchNumber == itemclass.BatchNumber).FirstOrDefaultAsync();
                    //if (quantitydamaged != null)
                    //{
                    //    itemclass.ItemStatus = "Faulty";

                    //}

                    //itemclass.BrandName = itemexists.BrandName;
                    //itemclass.ItemName = itemexists.ItemName;
                    //itemclass.WarrantyStartDate =itemexists.WarrantyStartDate ;
                    //itemclass.WarrantyEndDate =itemexists.WarrantyEndDate ;

                    //var count_User_with_role = await scopedcontext.InvoiceLinesDetails
                    //      .Where(u => u.ItemName == itemclass.ItemName).CountAsync();
                    //count_User_with_role += count_User_with_role;

                    //if (itemexists.Quantity == count_User_with_role)
                    //{
                    //    itemexists.Status = "Complete";

                    //}
                    //else
                    //{
                    //    itemexists.Status = "InComplete";
                    //}

                    await scopedcontext.AddAsync(itemclass);
                    await scopedcontext.SaveChangesAsync();


                    var checkno_exists = await _dragonFlyContext.ProductNumbering
                        .Where(y => y.NumberValue == addProductDetailvm.Product_No &&
                        y.Reference_Number == addProductDetailvm.reference_number).FirstOrDefaultAsync();

                    if (checkno_exists == null)
                    {
                        return new StockResponse(false, "nothing to show", null);
                    }

                    //if (addProductDetailvm.Product_No <= 0 || addProductDetailvm.Product_No == null)
                    //{
                    //    return new StockResponse(false, "Kindly fill in the item number ", null);

                    //}

                    var all_numbers = await _dragonFlyContext.ProductNumbering
                        .Where(y => y.Reference_Number == addProductDetailvm.reference_number).Select(s => s.NumberValue).ToListAsync();
                    var max_number = all_numbers.Max();

                    if (addProductDetailvm.Product_No > max_number)
                    {
                        itemclass.ProductStatus = "Complete";
                        return new StockResponse(false, "All item are added", null);


                    }



                    //var allnumbervalue = await scopedcontext.ProductNumbering.Where(x => x.NumberValue == addProductDetailvm.Product_No).ToListAsync();

                    //if (allnumbervalue == null)
                    //{
                    //    return new StockResponse(false, "Product number doesnt exist", null);
                    //}
                    //List<AllProductNumberingm> productnumbering = new List<AllProductNumberingm>();
                    //int maxNumber = productnumbering.Max(n => n.NumberValue);
                    //var batchdetails = await scopedcontext.InvoiceLinesDetails.Where(x => x.Quantity==addProductDetailvm.Product_No).FirstOrDefaultAsync();








                    checkno_exists.Status = "ASSIGNED";


                    _dragonFlyContext.Update(checkno_exists);
                    await _dragonFlyContext.SaveChangesAsync();



                    if (addProductDetailvm.Product_No == max_number)
                    {
                        var Invoceive_item_exists = await scopedcontext.AddDeliveryNote.
                            Where(y => y.BatchNumber == itemclass.BatchNumber).FirstOrDefaultAsync();

                        if (Invoceive_item_exists == null)
                            return new StockResponse(false, "No po item found", null);
                        Invoceive_item_exists.ProductStatus = "Complete";


                        scopedcontext.Update(Invoceive_item_exists);
                        await scopedcontext.SaveChangesAsync();
                        return new StockResponse(true, " ALL ITEMS ADDED SUCCESSFULLY", null);
                    }

                    return new StockResponse(true, $"Item '{addProductDetailvm.SerialNumber}'  created successfully", null);






                }
            }
            catch (Exception ex)
            {
                return new StockResponse(false, ex.Message, null);

            }

        }
        public async Task<StockResponse> GetProductDetails()
        {
            try
            {

                using (var scope = _serviceScopeFactory.CreateScope())
                {
                    var scopedcontext = scope.ServiceProvider.GetRequiredService<DragonFlyContext>();
                    var allinvoice = await scopedcontext.AddProductDetails.ToListAsync();

                    if (allinvoice == null)
                    {
                        return new StockResponse(false, "Product doesn't exist", null);
                    }
                    return new StockResponse(true, "Successfully queried", allinvoice);

                }
            }
            catch (Exception ex)
            {
                return new StockResponse(false, ex.Message, null);
            }
        }
        public async Task<StockResponse> GetInvoiceByNumber(string InvoiceNumber)
        {
            try
            {
                using (var scope = _serviceScopeFactory.CreateScope())
                {

                    var scopedcontext = scope.ServiceProvider.GetRequiredService<DragonFlyContext>();
                    var invoicenumber = await scopedcontext.StockIn.Where(u => u.InvoiceNumber == InvoiceNumber).FirstOrDefaultAsync();
                    if (invoicenumber == null)
                    {
                        return new StockResponse(false, "not found", null);
                    }
                    return new StockResponse(true, "Queried successfully", invoicenumber);
                }
            }
            catch (Exception ex)
            {
                return new StockResponse(false, ex.Message, null);
            }
        }
        public async Task<StockResponse> AddSparePart(AddPartvm addPartvm)
        {
            try
            {
                if (addPartvm.PartName == "")
                {

                    return new StockResponse(false, "Kindly provide a part name to add item", null);
                }
                using (var scope = _serviceScopeFactory.CreateScope())
                {
                    var scopedcontext = scope.ServiceProvider.GetRequiredService<DragonFlyContext>();



                    var partexists = await scopedcontext.AddPart.Where(x => x.PartName == addPartvm.PartName).FirstOrDefaultAsync();

                    if (partexists != null)
                    {
                        return new StockResponse(false, $" Part  '{addPartvm.PartName}' already exist", null);
                    }
                    var partclass = new AddPart
                    {
                        PartName = addPartvm.PartName,
                        PartDescription = addPartvm.PartDescription,
                    };
                    await scopedcontext.AddAsync(partclass);
                    await scopedcontext.SaveChangesAsync();
                    return new StockResponse(true, $"Part '{addPartvm.PartName}'  created successfully", null);

                }

            }
            catch (Exception ex)
            {
                return new StockResponse(false, ex.Message, null);

            }

        }
        public async Task<StockResponse> GetAllParts()
        {

            try
            {

                using (var scope = _serviceScopeFactory.CreateScope())
                {
                    var scopedcontext = scope.ServiceProvider.GetRequiredService<DragonFlyContext>();
                    var allpart = await scopedcontext.AddSpareParts.ToListAsync();

                    if (allpart == null)
                    {
                        return new StockResponse(false, "Part doesn't exist", null);
                    }
                    return new StockResponse(true, "Successfully queried", allpart);

                }
            }
            catch (Exception ex)
            {
                return new StockResponse(false, ex.Message, null);
            }
        }


        public string GenerateReferenceNumber(int length)
        {
            string ValidChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789abcdefghijklmnop";

            StringBuilder sb = new StringBuilder();
            Random random = new Random();
            for (int i = 0; i < length; i++)
            {
                int randomIndex = random.Next(ValidChars.Length);
                sb.Append(ValidChars[randomIndex]);
            }
            return sb.ToString();
        }

        public async Task<string> GetGeneratedref()
        {
            try
            {
                using (var scope = _serviceScopeFactory.CreateScope())
                {
                    var scopedcontext = scope.ServiceProvider.GetRequiredService<DragonFlyContext>();
                    int length = 8;
                    var paymentref = "PH_invoice_" + GenerateReferenceNumber(length);
                    //check reference exists

                    return paymentref;
                }
            }
            catch (Exception e)
            {
                return e.Message;
            }

        }

        public async Task<StockResponse> Add_Invoice_Item_Quantity(invoice_item_quantity_vm vm)
        {

            try
            {
                using (var scope = _serviceScopeFactory.CreateScope())
                {
                    var scopedcontext = scope.ServiceProvider.GetRequiredService<DragonFlyContext>();

                    var new_invoicee_item = new Invoice_Item_Quantity
                    {
                        Invoce_No = vm.Invoce_No,
                        Quantity = vm.Quantity,
                        Type = vm.Type
                    };
                    var string_resp = GetGeneratedref().Result;

                    new_invoicee_item.Invoice_quantity_Unique_id = string_resp;





                    await scopedcontext.AddAsync(new_invoicee_item);
                    await scopedcontext.SaveChangesAsync();

                    if (new_invoicee_item.Type == "Product")
                    {
                        int number = 0;
                        while (new_invoicee_item.Quantity > 0)
                        {
                            number++;
                            var new_numbering = new Item_Numbering_Stock
                            {

                                Product_No = number,
                                Invoice_quantity_Id = new_invoicee_item.Invoice_quantity_Unique_id,
                                Status = "UNASSIGNED"


                            };

                        };
                    }
                    return new StockResponse(true, "Successfully updated items on invoice ", null);

                }

            }
            catch (Exception ex)
            {

                return new StockResponse(false, ex.Message, null);
            }
        }

        public async Task<StockResponse> AddPartsSpare(AddSparePartvm addSparePartvm)
        {
            try
            {
                if (addSparePartvm.PartName == "")
                {

                    return new StockResponse(false, "Kindly provide a part name ", null);
                }
                using (var scope = _serviceScopeFactory.CreateScope())
                {
                    var scopedcontext = scope.ServiceProvider.GetRequiredService<DragonFlyContext>();

                    //check if role exists 

                    var brandexists = await scopedcontext.AddSpareParts.Where(x => x.PartName == addSparePartvm.PartName).FirstOrDefaultAsync();

                    if (brandexists != null)
                    {
                        return new StockResponse(false, $" Device  '{addSparePartvm.PartName}' already exist", null);
                    }
                    var brandclass = new AddSpareParts
                    {
                        PartName = addSparePartvm.PartName,
                        PartDescription = addSparePartvm.PartDescription,
                    };
                    await scopedcontext.AddAsync(brandclass);
                    await scopedcontext.SaveChangesAsync();
                    return new StockResponse(true, $"Spare part '{addSparePartvm.PartName}'  created successfully", null);


                }


            }
            catch (Exception ex)
            {
                return new StockResponse(false, ex.Message, null);

            }

        }
        public async Task<StockResponse> GetAllSpareParts()
        {

            try
            {

                using (var scope = _serviceScopeFactory.CreateScope())
                {
                    var scopedcontext = scope.ServiceProvider.GetRequiredService<DragonFlyContext>();
                    var allparts = await scopedcontext.AddSpareParts.ToListAsync();

                    if (allparts == null)
                    {
                        return new StockResponse(false, "spare parts doesn't exist", null);
                    }
                    return new StockResponse(true, "Successfully queried", allparts);

                }
            }
            catch (Exception ex)
            {
                return new StockResponse(false, ex.Message, null);
            }

        }
        public async Task<StockResponse> GetInvoiceLinByNumber(string invoiceNumber)
        {
            try
            {
                using (var scope = _serviceScopeFactory.CreateScope())
                {

                    var scopedcontext = scope.ServiceProvider.GetRequiredService<DragonFlyContext>();
                    var supplierexists = await scopedcontext.InvoiceLinesDetails.Where(u => u.InvoiceNumber == invoiceNumber).OrderByDescending(u => u.UpdatedOn).ToListAsync();
                    if (supplierexists == null)
                    {
                        return new StockResponse(false, "not found", null);
                    }
                    return new StockResponse(true, "Queried successfully", supplierexists);
                }
            }
            catch (Exception ex)
            {
                return new StockResponse(false, ex.Message, null);
            }
        }
        public async Task<StockResponse> GetInvoiceItemByID(int invoicelineId)
        {
            try
            {
                using (var scope = _serviceScopeFactory.CreateScope())
                {

                    var scopedcontext = scope.ServiceProvider.GetRequiredService<DragonFlyContext>();
                    var supplierexists = await scopedcontext.InvoiceLinesDetails.Where(u => u.InvoiceLineId == invoicelineId).FirstOrDefaultAsync();
                    if (supplierexists == null)
                    {
                        return new StockResponse(false, "not found", null);
                    }
                    return new StockResponse(true, "Queried successfully", supplierexists);
                }
            }
            catch (Exception ex)
            {
                return new StockResponse(false, ex.Message, null);
            }
        }
        public async Task<StockResponse> GetProductDetailsbyid(string BatchNumber)
        {
            try
            {
                using (var scope = _serviceScopeFactory.CreateScope())
                {

                    var scopedcontext = scope.ServiceProvider.GetRequiredService<DragonFlyContext>();
                    var supplierexists = await scopedcontext.AddProductDetails.Where(u => u.BatchNumber == BatchNumber).ToListAsync();
                    if (supplierexists == null)
                    {
                        return new StockResponse(false, "not found", null);
                    }
                    return new StockResponse(true, "Queried successfully", supplierexists);
                }
            }
            catch (Exception ex)
            {
                return new StockResponse(false, ex.Message, null);
            }
        }

        public async Task<StockResponse> GetProduct_Numbers_ByReference(string reference)
        {



            try
            {

                var allnumberings = await _dragonFlyContext.ProductNumbering
                    .Where(y => y.Status == "UNASSIGNED" && y.Reference_Number == reference).ToListAsync();
                return new StockResponse(true, "successfully queried", allnumberings);
            }
            catch (Exception ex)
            {

                return new StockResponse(false, ex.Message, null);
            }
        }

        public async Task<StockResponse> GetProduvctLineyId(int product_line_id)
        {
            try
            {
                using (var scope = _serviceScopeFactory.CreateScope())
                {

                    var scopedcontext = scope.ServiceProvider.GetRequiredService<DragonFlyContext>();
                    var product_exist = await scopedcontext.UploadPOFile.Where(u => u.ID == product_line_id).FirstOrDefaultAsync();
                    if (product_exist == null)
                    {
                        return new StockResponse(false, "not found", null);
                    }
                    return new StockResponse(true, "Queried successfully", product_exist);
                }
            }
            catch (Exception ex)
            {
                return new StockResponse(false, ex.Message, null);
            }
        }
        //public async Task<StockResponse> GetAllProductNumbering(int productNumberID)
        //{

        //    try
        //    {

        //        using (var scope = _serviceScopeFactory.CreateScope())
        //        {
        //            var scopedcontext = scope.ServiceProvider.GetRequiredService<DragonFlyContext>();
        //            var allnumbervalue = await scopedcontext.ProductNumbering.Where(x => x.ProductNumberID==productNumberID).FirstOrDefaultAsync();

        //            if (allnumbervalue == null)
        //            {
        //                return new StockResponse(false, "Product number doesnt exist", null);
        //            }
        //            List<AllProductNumberingm> productnumbering = new List<AllProductNumberingm>();
        //            int maxNumber = productnumbering.Max(n => n.NumberValue);
        //            var batchdetails=await scopedcontext.InvoiceLinesDetails.Where(x=>x.Quantity==maxNumber).FirstOrDefaultAsync();
        //            if (maxNumber == batchdetails.Quantity)
        //            {
        //                batchdetails.Status = "COMPLETE";
        //            }


        //                                return new StockResponse(true, "Successfully queried", productnumbering);

        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        return new StockResponse(false, ex.Message, null);
        //    }
        //}
        public async Task<StockResponse> SearchForInvoice(string search_query)
        {

            try
            {
                using (var scope = _serviceScopeFactory.CreateScope())
                {

                    var scopedcontext = scope.ServiceProvider.GetRequiredService<DragonFlyContext>();

                    var allstock = await scopedcontext.StockIn.Where
                        (u => EF.Functions.Like(u.LPONumber, $"%{search_query}%") ||
                        EF.Functions.Like(u.InvoiceNumber, $"%{search_query}%")
                        ).ToListAsync();

                    if (allstock == null)
                        return new StockResponse(false, "", null);

                    return new StockResponse(true, "Successfully queried", allstock);


                }
            }
            catch (Exception ex)
            {

                return new StockResponse(false, ex.Message, null);
            }
        }
        public async Task<StockResponse> SearchForItem(string search_query)
        {

            try
            {
                using (var scope = _serviceScopeFactory.CreateScope())
                {

                    var scopedcontext = scope.ServiceProvider.GetRequiredService<DragonFlyContext>();

                    var allstock = await scopedcontext.AddItem.Where
                        (u => EF.Functions.Like(u.BrandName, $"%{search_query}%") ||
                        EF.Functions.Like(u.ItemName, $"%{search_query}%") ||
                        EF.Functions.Like(u.ItemDescription, $"%{search_query}%") ||
                        EF.Functions.Like(u.Category, $"%{search_query}%") ||
                        EF.Functions.Like(u.Currency, $"%{search_query}%")
                        ).ToListAsync();

                    if (allstock == null)
                        return new StockResponse(false, "", null);

                    return new StockResponse(true, "Successfully queried", allstock);


                }
            }
            catch (Exception ex)
            {

                return new StockResponse(false, ex.Message, null);
            }
        }
        public async Task<StockResponse> GetItemsbyBrandName(string BrandName)
        {
            try
            {
                using (var scope = _serviceScopeFactory.CreateScope())
                {

                    var scopedcontext = scope.ServiceProvider.GetRequiredService<DragonFlyContext>();
                    var supplierexists = await scopedcontext.AddItem.Where(u => u.BrandName == BrandName).ToListAsync();
                    if (supplierexists == null)
                    {
                        return new StockResponse(false, "not found", null);
                    }
                    return new StockResponse(true, "Queried successfully", supplierexists);
                }
            }
            catch (Exception ex)
            {
                return new StockResponse(false, ex.Message, null);
            }
        }
        public async Task<StockResponse> SearchForInvoiceLines(string search_query)
        {

            try
            {
                using (var scope = _serviceScopeFactory.CreateScope())
                {

                    var scopedcontext = scope.ServiceProvider.GetRequiredService<DragonFlyContext>();

                    var allstock = await scopedcontext.InvoiceLinesDetails.Where
                        (u => EF.Functions.Like(u.ItemName, $"%{search_query}%") ||
                        EF.Functions.Like(u.CategoryName, $"%{search_query}%") ||
                          EF.Functions.Like(u.Currency, $"%{search_query}%") ||
                            EF.Functions.Like(u.UpdatedBy, $"%{search_query}%") ||
                               EF.Functions.Like(u.BrandName, $"%{search_query}%") ||

                              EF.Functions.Like(u.Status, $"%{search_query}%")

                        ).ToListAsync();

                    if (allstock == null)
                        return new StockResponse(false, "", null);

                    return new StockResponse(true, "Successfully queried", allstock);


                }
            }
            catch (Exception ex)
            {

                return new StockResponse(false, ex.Message, null);
            }
        }
        public async Task<StockResponse> UploadData([FromBody] List<uploadDatavm> data)
        {
            try
            {
                using (var scope = _serviceScopeFactory.CreateScope())
                {

                    var scopedcontext = scope.ServiceProvider.GetRequiredService<DragonFlyContext>();

                    foreach (var item in data)
                    {
                        var new_product_item = new AddProductDetails
                        {
                            IMEI1 = item.IMEI1,
                            IMEI2 = item.IMEI2,
                            SerialNumber = item.SerialNumber,
                        };

          
                        await scopedcontext.AddAsync(new_product_item);
                        await scopedcontext.SaveChangesAsync();

                    }


                    return new StockResponse(true, "Added successful", null);
                }
            }
            catch (Exception ex)
            {
                return new StockResponse(false, ex.Message, null);
            }


        }
        public async Task<StockResponse> UploadingData(IFormFile file, string BatchNumber)
        {
            try
            {
                using (var scope = _serviceScopeFactory.CreateScope())
                {
                    var scopedcontext = scope.ServiceProvider.GetRequiredService<DragonFlyContext>();

                    using (var stream = new MemoryStream())
                    {
                        file.CopyTo(stream);
                        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

                        using (var package = new ExcelPackage(stream))
                        {
                            var worksheet = package.Workbook.Worksheets.FirstOrDefault();
                            if (worksheet != null)
                            {
                                var serialNumbers = new List<string>();
                                int rowCount = worksheet.Dimension.Rows;
                                for (int row = 2; row <= rowCount; row++) // Assuming the first row contains headers
                                {
                                    var serialNumber = worksheet.Cells[row, 1].Value?.ToString();
                                    var imei1 = worksheet.Cells[row, 2].Value?.ToString();
                                    var imei2 = worksheet.Cells[row, 3].Value?.ToString();

                                    if (!string.IsNullOrEmpty(serialNumber))
                                    {
                                        serialNumbers.Add(serialNumber);

                                        // Your existing code to add products with serial numbers and IMEI values goes here
                                        try
                                        {
                                            var product = new AddProductDetails
                                            {
                                                SerialNumber = serialNumber,
                                                IMEI1 = imei1,
                                                IMEI2 = imei2,
                                                BatchNumber = BatchNumber,
                                            };

                                            // Set other properties of product as needed

                                            product.ItemID = product.BatchID;
                                            await scopedcontext.AddAsync(product);
                                            // await scopedcontext.SaveChangesAsync();
                                            Console.WriteLine("Uploaded serial number: " + serialNumber);
                                        }
                                        catch (Exception ex)
                                        {
                                            Console.WriteLine("Error uploading serial number " + serialNumber + ": " + ex.Message);
                                        }
                                    }
                                }

                                if (serialNumbers.Count == 0)
                                {
                                    return new StockResponse(false, "No serial numbers uploaded.", null);
                                }

                                var batchQuantityInDatabase = await scopedcontext.AddDeliveryNote
                                    .Where(x => x.BatchNumber == BatchNumber)
                                    .Select(x => x.BatchQuantity)
                                    .FirstOrDefaultAsync();

                                var existingSerialNumbersCount = await scopedcontext.AddProductDetails
                                    .Where(x => x.BatchNumber == BatchNumber && serialNumbers.Contains(x.SerialNumber))
                                    .CountAsync();

                                int uploadedSerialNumbersCount = serialNumbers.Count;

                                if (uploadedSerialNumbersCount + existingSerialNumbersCount > batchQuantityInDatabase)
                                {
                                    int remainingSerialNumbersCount = batchQuantityInDatabase - existingSerialNumbersCount;
                                    return new StockResponse(false, "Total serial numbers count cannot exceed the specified quantity for BatchNumber: " + BatchNumber + ". Remaining count: " + remainingSerialNumbersCount, null);
                                }

                                var existingSerialNumbers = await scopedcontext.AddProductDetails
                                    .Where(x => x.BatchNumber == BatchNumber && serialNumbers.Contains(x.SerialNumber))
                                    .Select(x => x.SerialNumber)
                                    .ToListAsync();

                                // Rest of your code for checking and validating serial numbers
                                // ...
                                var deliveryNote = await scopedcontext.AddDeliveryNote.FirstOrDefaultAsync(x => x.BatchNumber == BatchNumber);
                                if (deliveryNote != null)
                                {
                                    if (uploadedSerialNumbersCount+existingSerialNumbersCount == batchQuantityInDatabase)
                                    {
                                        deliveryNote.ProductStatus = "Complete";
                                    }
                                    else
                                    {
                                        deliveryNote.ProductStatus = "Incomplete";
                                    }
                                    await scopedcontext.SaveChangesAsync();

                                    return new StockResponse(true, "Uploaded successfully", serialNumbers.Except(existingSerialNumbers).ToList());
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return new StockResponse(false, ex.Message, null);
            }

            return new StockResponse(false, "Invalid file or file format.", null);
        }



        public async Task<StockResponse> EditSerialNumber(EditSerialNumbervm editSerialNumbervm)
        {
            try
            {

                using (var scope = _serviceScopeFactory.CreateScope())
                {

                    var scopedcontext = scope.ServiceProvider.GetRequiredService<DragonFlyContext>();

                    var salesexists = await scopedcontext.AddProductDetails.Where(u => u.BatchID == editSerialNumbervm.ItemID).FirstOrDefaultAsync();
                    var itemexists = await scopedcontext.AddProductDetails
                    .Where(y => y.SerialNumber == editSerialNumbervm.SerialNumber || y.IMEI1 == editSerialNumbervm.IMEI1 || y.IMEI2 == editSerialNumbervm.IMEI2).FirstOrDefaultAsync();
                    if (itemexists != null)
                    {
                        return new StockResponse(false, $"Serial Number '{editSerialNumbervm.SerialNumber}'  already exists", null);

                    }


                    if (salesexists == null)
                    {

                        return new StockResponse(false, "product does not exist", null);
                    }

                    if (editSerialNumbervm.SerialNumber == "string")
                    {
                        salesexists.SerialNumber = salesexists.SerialNumber;

                    }
                    else
                    {
                        salesexists.SerialNumber = editSerialNumbervm.SerialNumber;
                    }


                    if (editSerialNumbervm.IMEI1 == "")
                    {
                        salesexists.IMEI1 = salesexists.IMEI1;

                    }
                    else
                    {
                        salesexists.IMEI1 = editSerialNumbervm.IMEI1;
                    }
                    if (editSerialNumbervm.IMEI2 == "")
                    {
                        salesexists.IMEI2 = salesexists.IMEI2;

                    }
                    else
                    {
                        salesexists.IMEI2 = editSerialNumbervm.IMEI2;
                    }

                    //    if (itemexists != null)
                    //    {
                    //        return new StockResponse(false, $"Serial Number '{editSerialNumbervm.SerialNumber}' already exists", null);

                    //    }
                    //    var imei1exists = await scopedcontext.AddProductDetails
                    //   .Where(y => y.IMEI1 == editSerialNumbervm.IMEI1).FirstOrDefaultAsync();

                    //    if (imei1exists != null)
                    //    {
                    //        return new StockResponse(false, $"IMEI1 '{editSerialNumbervm.IMEI1}' already exists", null);

                    //    }
                    //    var imei2exists = await scopedcontext.AddProductDetails
                    //.Where(y => y.IMEI2 == editSerialNumbervm.IMEI2).FirstOrDefaultAsync();

                    //    if (imei2exists != null)
                    //    {
                    //        return new StockResponse(false, $"IMEI2 '{editSerialNumbervm.IMEI2}' already exists", null);

                    //    }

                    scopedcontext.Update(salesexists);
                    await scopedcontext.SaveChangesAsync();

                    return new StockResponse(true, "Sucessfully updated product details", salesexists);


                }

            }
            catch (Exception ex)
            {

                return new StockResponse(false, ex.Message, null);
            }
        }
        public async Task<StockResponse> GetSerialNumberbyid(int itemID)
        {
            try
            {
                using (var scope = _serviceScopeFactory.CreateScope())
                {

                    var scopedcontext = scope.ServiceProvider.GetRequiredService<DragonFlyContext>();
                    var product_exist = await scopedcontext.AddProductDetails.Where(u => u.BatchID == itemID).FirstOrDefaultAsync();
                    if (product_exist == null)
                    {
                        return new StockResponse(false, "not found", null);
                    }
                    return new StockResponse(true, "Queried successfully", product_exist);
                }
            }
            catch (Exception ex)
            {
                return new StockResponse(false, ex.Message, null);
            }
        }


        public async Task<StockResponse> PostScannedData([FromBody] ScannedDataModel data)
        {
            try
            {
                using (var scope = _serviceScopeFactory.CreateScope())
                {

                    var scopedcontext = scope.ServiceProvider.GetRequiredService<DragonFlyContext>();

                    // Process the scanned data
                    var serialNumber = data.SerialNumber;
                    var imei1 = data.IMEI1;
                    var imei2 = data.IMEI2;
                }


                // Perform further actions with the scanned data


                return new StockResponse(true, "Successfully scanned the details", null);
            }
            catch (Exception ex)
            {
                return new StockResponse(false, ex.Message, null);
            }
        }
        public async Task<StockResponse> Upload([FromBody] PODetailsvm pODetailsvm)
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var scopedcontext = scope.ServiceProvider.GetRequiredService<DragonFlyContext>();


                var new_podetails = new PODetails
                {
                    PONumber = pODetailsvm.PONumber,
                    PODate = Convert.ToDateTime(pODetailsvm.PODate),
                    Vendor = pODetailsvm.Vendor,
                };
                await scopedcontext.AddAsync(new_podetails);
                await scopedcontext.SaveChangesAsync();
                return new StockResponse(true, "Uploaded file successfully", new_podetails);

            }


            // Save the student data to the database or perform any other actions
            // For simplicity, let's just return the received data as a response

            //return new StockResponse(true, "Uploaded data successfully", null);

        }
        public async Task<StockResponse> UploadingPODetails(PODetailsvm pODetailsvm)
        {
            try
            {
                if (pODetailsvm.PONumber == "")
                {

                    return new StockResponse(false, "Kindly upload a file", null);
                }
                if (pODetailsvm.PODate == "")
                {

                    return new StockResponse(false, "Kindly upload a file", null);
                }
                if (pODetailsvm.Vendor == "")
                {

                    return new StockResponse(false, "Kindly provide a vendor name to add po", null);
                }
                using (var scope = _serviceScopeFactory.CreateScope())
                {
                    var scopedcontext = scope.ServiceProvider.GetRequiredService<DragonFlyContext>();

                    //check if role exists 

                    var poexists = await scopedcontext.PODetails.Where(x => x.PONumber == pODetailsvm.PONumber).FirstOrDefaultAsync();

                    if (poexists != null)
                    {
                        return new StockResponse(false, $" PO  '{pODetailsvm.PONumber}' already exists", null);
                    }
                    var poclass = new PODetails
                    {
                        PONumber = pODetailsvm.PONumber,
                        PODate = Convert.ToDateTime(pODetailsvm.PODate),
                        Vendor = pODetailsvm.Vendor,
                        DateCreated = DateTime.Now,
                        CaptureStatus = "Incomplete",
                        DeliveryStatus="Incomplete",
                    
                    };
                    await scopedcontext.AddAsync(poclass);
                    await scopedcontext.SaveChangesAsync();
                    return new StockResponse(true, $"PO-Number '{pODetailsvm.PONumber}'  created successfully", poclass);

                }

            }

            catch (Exception ex)
            {
                return new StockResponse(false, ex.Message, null);
            }

            return new StockResponse(false, "Invalid file or file format.", null);
        }
        public async Task<StockResponse> GetAllPOs()
        {

            try
            {

                using (var scope = _serviceScopeFactory.CreateScope())
                {
                    var scopedcontext = scope.ServiceProvider.GetRequiredService<DragonFlyContext>();
                    var allpos = await scopedcontext.PODetails.ToListAsync();

                    if (allpos == null)
                    {
                        return new StockResponse(false, "PO doesn't exist", null);
                    }
                    return new StockResponse(true, "Successfully queried", allpos);

                }
            }
            catch (Exception ex)
            {
                return new StockResponse(false, ex.Message, null);
            }
        }
        public async Task<StockResponse> UploadingPOItems([FromBody] DataWrapper dataWrapper)
        {
            try
            {
                using (var scope = _serviceScopeFactory.CreateScope())
                {
                    var scopedcontext = scope.ServiceProvider.GetRequiredService<DragonFlyContext>();
                    //Console.WriteLine("Uploaded items", uploadPOItemvm);
                    foreach (var item in dataWrapper.ArrayList)
                    {
                        Console.WriteLine(item);
                    }


                    scopedcontext.SaveChanges();
                }



                return new StockResponse(true, "Data received and stored successfully.", null);
            }


            // var purchaseOrder = JsonConvert.DeserializeObject<PurchaseOrderModel>(data);
            //    if (uploadPOItemvm.ItemName == "")
            //    {

            //        return new StockResponse(false, "Kindly upload a file", null);
            //    }
            //    if (uploadPOItemvm.Amount == "")
            //    {

            //        return new StockResponse(false, "Kindly upload a file", null);
            //    }
            //    if (uploadPOItemvm.Rate == "")
            //    {

            //        return new StockResponse(false, "Kindly upload a file", null);
            //    }
            //    if (uploadPOItemvm.Quantity == 0)
            //    {

            //        return new StockResponse(false, "Kindly upload a file", null);
            //    }
            //    using (var scope = _serviceScopeFactory.CreateScope())
            //    {
            //        var scopedcontext = scope.ServiceProvider.GetRequiredService<DragonFlyContext>();

            //        //check if role exists 

            //        var itemexists = await scopedcontext.UploadPOItem.Where(x => x.ItemName == uploadPOItemvm.ItemName).FirstOrDefaultAsync();

            //        if (itemexists != null)
            //        {
            //            return new StockResponse(false, $" Item  '{uploadPOItemvm.ItemName}' already exists! please update quantity", null);
            //        }
            //        var items = new List<UploadPOItem>();
            //        var itempoclass = new UploadPOItem
            //        {
            //            ItemName = uploadPOItemvm.ItemName,
            //            Amount = uploadPOItemvm.Amount,
            //            Rate = uploadPOItemvm.Rate,
            //            Quantity = uploadPOItemvm.Quantity,
            //        };
            //        items.Add(itempoclass);
            //        foreach (var each_item in items)
            //        {
            //            // Save the list of students to the database
            //            await scopedcontext.AddAsync(each_item);
            //            await scopedcontext.SaveChangesAsync();
            //        }

            //        return new StockResponse(true, $"Item '{uploadPOItemvm.ItemName}'  created successfully", itempoclass);




            catch (Exception ex)
            {
                return new StockResponse(false, ex.Message, null);
            }

            return new StockResponse(false, "Invalid file or file format.", null);
        }
        public async Task UploadingItemsPO(IFormFile file, string PONumber)
        {
            try
            {
                using (var scope = _serviceScopeFactory.CreateScope())
                {


                    var scopedcontext = scope.ServiceProvider.GetRequiredService<DragonFlyContext>();
                    using (var stream = new MemoryStream())
                    {
                        file.CopyTo(stream);
                        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

                        using (var package = new ExcelPackage(stream))
                        {

                            var worksheet = package.Workbook.Worksheets.FirstOrDefault();
                            if (worksheet != null)
                            {

                                var products = new List<UploadPOItem>();
                                int rowCount = worksheet.Dimension.Rows;
                                for (int row = 5; row <= rowCount; row++) // Assuming the first row contains headers
                                {


                                    var product = new UploadPOItem

                                    {
                                        //Id = int.Parse(worksheet.Cells[row, 1].Value?.ToString()),
                                        ItemName = worksheet.Cells[row, 2].Value?.ToString(),
                                        Rate = worksheet.Cells[row, 6].Value?.ToString(),
                                        Quantity = worksheet.Cells[row, 4].Value?.ToString(),
                                        Amount = worksheet.Cells[row, 7].Value?.ToString(),
                                        PONumber = PONumber,


                                    };
                                    PONumber = product.PONumber;

                                    // product.Id = product.Id;
                                    await scopedcontext.AddAsync(product);
                                    await scopedcontext.SaveChangesAsync();
                                    _logger.LogInformation("saved successfully");
                                    //products.Add(product);
                                    //await scopedcontext.SaveChangesAsync();

                                }
                                //foreach (var each_product in products)
                                //{
                                //    // Save the list of students to the database
                                //    await scopedcontext.AddAsync(each_product);
                                //    await scopedcontext.SaveChangesAsync();
                                //}



                                //_logger.LogInformation("Uploaded successfully");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.Message);
            }

            _logger.LogInformation("Invalid file or file format.");

        }
        public async Task<StockResponse> GetItemsByPO(string PONumber)
        {
            try
            {
                using (var scope = _serviceScopeFactory.CreateScope())
                {
                    var scopedcontext = scope.ServiceProvider.GetRequiredService<DragonFlyContext>();

                    var itemexists = await scopedcontext.UploadPOItem.Where(y => y.PONumber == PONumber).ToListAsync();
                    if (itemexists == null)
                    {
                        return new StockResponse(false, "nothing to show ", null);
                    }
                    return new StockResponse(true, "Queried successfully", itemexists);


                }

            }
            catch (Exception ex)
            {
                return new StockResponse(false, ex.Message, null);
            }
        }
        public async Task<StockResponse> UploadingPO(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                // Handle the case where the file is not provided or empty
                return new StockResponse(false, "Please provide a valid file.", null);
            }

            try
            {
                using (var scope = _serviceScopeFactory.CreateScope())
                {
                    var scopedcontext = scope.ServiceProvider.GetRequiredService<DragonFlyContext>();
                    using (var stream = new MemoryStream())
                    {
                        file.CopyTo(stream);
                        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

                        using (var package = new ExcelPackage(stream))
                        {
                            var worksheet = package.Workbook.Worksheets.FirstOrDefault();

                            if (worksheet != null)
                            {
                                var products = new List<UploadPOFile>();
                                int rowCount = worksheet.Dimension.Rows;

                                int totalStock = 0;
                                for (int row = 2; row <= rowCount; row++) // Assuming the first row contains headers
                                {
                                    string item_name = worksheet.Cells[row, 2].Value?.ToString();
                                    int quantity = int.Parse(worksheet.Cells[row, 4].Value?.ToString());
                                    string BrandName = worksheet.Cells[row, 3].Value?.ToString();

                                    var itemexists = await scopedcontext.UploadPOFile
                                        .Where(x => x.ItemName == item_name && x.BrandName == BrandName)
                                        .OrderByDescending(y => y.DateAdded)
                                        .FirstOrDefaultAsync();


                                    var product = new UploadPOFile
                                    {
                                        PONumber = worksheet.Cells[row, 1].Value?.ToString(),
                                        ItemName = item_name,
                                        BrandName = BrandName,
                                        Quantity = quantity,
                                        ReOrderLevel = int.Parse(worksheet.Cells[row, 5].Value?.ToString()),
                                        Rate = worksheet.Cells[row, 6].Value?.ToString(),
                                        Amount = worksheet.Cells[row, 7].Value?.ToString(),
                                        Warranty = int.Parse(worksheet.Cells[row, 8].Value?.ToString()),
                                        WarrantyStartDate = Convert.ToDateTime(worksheet.Cells[row, 9].Value?.ToString()),
                                        CategoryName = worksheet.Cells[row, 10].Value?.ToString(),
                                        DateAdded = DateTime.Now,
                                        Reference_Number = await GetGeneratedref(),
                                        ProductStatus = "Incomplete",
                                        
                                    };
                                    var itemidexissst = await scopedcontext.AddDeliveryNote.Where(u => u.ItemID == product.ID).FirstOrDefaultAsync();
                                    
                                    product.OpeningStock = quantity;
                                    product.ActualQuantity = quantity;

                                    if (itemidexissst != null)
                                    {
                                        product.TotalClosed = itemidexissst.TotalClosed;
                                    }
                                    if (itemexists != null)
                                    {
                                        product.AvailableStock = itemexists.AvailableStock + product.TotalClosed;
                                        product.TotalClosed += itemexists.TotalClosed;
                                        product.StockOut = itemexists.StockOut;
                                        scopedcontext.Update(product);
                                    }
                                    else
                                    {
                                        // product.AvailableStock = product.Quantity;
                                        product.AvailableStock = product.TotalClosed;
                                        await scopedcontext.UploadPOFile.AddAsync(product);
                                    }
                                  

                                    if (product.AvailableStock > product.ReOrderLevel)
                                    {
                                        product.Status = "Good";
                                    }
                                    else if (product.AvailableStock < product.ReOrderLevel || product.AvailableStock > 0)
                                    {
                                        product.Status = "Low";
                                    }
                                    else
                                    {
                                        product.Status = "Out";
                                    }



                                    //await scopedcontext.SaveChangesAsync();



                                    //if (product.CategoryName == "Product")
                                    //{
                                    //    var new_numb = 0;

                                    //    while (new_numb < product.ActualQuantity)
                                    //    {
                                    //        new_numb++;
                                    //        var new_numbering = new ProductNumbering
                                    //        {
                                    //            NumberValue = new_numb,
                                    //            Reference_Number = product.Reference_Number,
                                    //            Type = "Product",
                                    //            Status = "UNASSIGNED",
                                    //        };


                                    //        await scopedcontext.ProductNumbering.AddAsync(new_numbering);
                                    //        products.Add(product);
                                    //    }

                                    //}

                                    //product.AjustedQuantity = product.Quantity;
                                    totalStock += product.AvailableStock;
                                    product.WarrantyEndDate = product.WarrantyStartDate.AddMonths(product.Warranty);
                                    product.TotalStockIn = totalStock;

                                    //if (product.CategoryName == "Accesory")
                                    //{
                                    //    product.ProductStatus = "Complete";
                                    //}
                                    //else
                                    //{
                                    //    product.ProductStatus = "Incomplete";
                                    //}
                                }

                                await scopedcontext.SaveChangesAsync(); // Save changes after all items are added/updated

                                return new StockResponse(true, "Uploaded successfully", products);
                            }
                        }
                    }
                }


            }
            catch (Exception ex)
            {
                return new StockResponse(false, ex.Message, null);
            }

            return new StockResponse(false, "Item Already Saved.", null);
        }


        public async Task<StockResponse> GetAllPOSDetails()
        {

            try
            {

                using (var scope = _serviceScopeFactory.CreateScope())
                {
                    var scopedcontext = scope.ServiceProvider.GetRequiredService<DragonFlyContext>();
                    var allpos = await scopedcontext.UploadPOFile.ToListAsync();


                    if (allpos == null)
                    {
                        return new StockResponse(false, "PO doesn't exist", null);
                    }
                    return new StockResponse(true, "Successfully queried", allpos);

                }
            }
            catch (Exception ex)
            {
                return new StockResponse(false, ex.Message, null);
            }
        }
        public async Task<StockResponse> GetItemsByPOS(string PONumber)
        {
            try
            {
                using (var scope = _serviceScopeFactory.CreateScope())
                {
                    var scopedcontext = scope.ServiceProvider.GetRequiredService<DragonFlyContext>();

                    var itemexists = await scopedcontext.UploadPOFile.Where(y => y.PONumber == PONumber).ToListAsync();
                    if (itemexists == null)
                    {
                        return new StockResponse(false, "No items found", null);
                    }
                    return new StockResponse(true, "Queried successfully", itemexists);


                }

            }
            catch (Exception ex)
            {
                return new StockResponse(false, ex.Message, null);
            }
        }
        public async Task<StockResponse> AddPurchaseOrdersDetails(PurchaseOrderssvm purchaseOrderssvm)
        {
            try
            {
                if (purchaseOrderssvm.PONumber == "")
                {

                    return new StockResponse(false, "Kindly provide PO number to add Purchase order", null);
                }
                if (purchaseOrderssvm.PODate == null)
                {
                    return new StockResponse(false, "Kindly provide PO Date", null);

                }
                if (purchaseOrderssvm.Vendor == "")
                {
                    return new StockResponse(false, "Kindly provide vendor", null);

                }

                using (var scope = _serviceScopeFactory.CreateScope())
                {
                    var scopedcontext = scope.ServiceProvider.GetRequiredService<DragonFlyContext>();



                    var poexistss = await scopedcontext.PurchaseOrderss.Where(x => x.PONumber == purchaseOrderssvm.PONumber).FirstOrDefaultAsync();

                    if (poexistss != null)
                    {
                        return new StockResponse(false, $" Purchase Order  '{purchaseOrderssvm.PONumber}' already exists", null);
                    }

                    var purchaseorderss = new PurchaseOrderss
                    {
                        PONumber = purchaseOrderssvm.PONumber,
                        PODate = purchaseOrderssvm.PODate,
                        Vendor = purchaseOrderssvm.Vendor,
                        DateCreated = DateTime.Now,


                    };

                    await scopedcontext.AddAsync(purchaseorderss);
                    await scopedcontext.SaveChangesAsync();
                    return new StockResponse(true, $"Purchase Order '{purchaseOrderssvm.PONumber}'  created successfully", null);

                }

            }
            catch (Exception ex)
            {
                return new StockResponse(false, ex.Message, null);

            }

        }
        public async Task<StockResponse> GetAllPurchaseOrderDetails()
        {

            try
            {

                using (var scope = _serviceScopeFactory.CreateScope())
                {
                    var scopedcontext = scope.ServiceProvider.GetRequiredService<DragonFlyContext>();
                    var allpos = await scopedcontext.PurchaseOrderss.ToListAsync();


                    if (allpos == null)
                    {
                        return new StockResponse(false, "PO doesn't exist", null);
                    }
                    return new StockResponse(true, "Successfully queried", allpos);

                }
            }
            catch (Exception ex)
            {
                return new StockResponse(false, ex.Message, null);
            }
        }
        public async Task<StockResponse> AdjustStock(AdjustStockvm adjustStockvm)
        {

            try
            {
                using (var scope = _serviceScopeFactory.CreateScope())
                {
                    var scopedcontext = scope.ServiceProvider.GetRequiredService<DragonFlyContext>();
                    var lastupdate = await scopedcontext.UploadPOFile.Where(y => y.ID == adjustStockvm.ItemID)
                      .FirstOrDefaultAsync();

                    if (lastupdate == null)
                    {
                        return new StockResponse(false, "item not found", null);
                    }

                    var Items = new AdjustStock
                    {
                        QuantityDamaged = adjustStockvm.QuantityDamaged,
                        Description = adjustStockvm.Description,

                    };
                    Items.ItemID = lastupdate.ID;

                    lastupdate.AjustedQuantity -= Items.QuantityDamaged;
                    scopedcontext.Update(lastupdate);
                    await scopedcontext.AddAsync(Items);
                    await scopedcontext.SaveChangesAsync();
                    return new StockResponse(true, " updated successfully", Items);

                }


                //lastupdate.AvailableStock = quantityadded + lastupdate.AvailableStock;
                //if (lastupdate.AvailableStock > lastupdate.ReOrderLevel)
                //{
                //    lastupdate.Status = "Good";
                //}
                //else if (lastupdate.AvailableStock < lastupdate.ReOrderLevel && lastupdate.AvailableStock > 0)
                //{
                //    lastupdate.Status = "Low";
                //}
                //else
                //{
                //    lastupdate.Status = "Out";
                //}


            }
            catch (Exception ex)
            {
                return new StockResponse(false, ex.Message, null);
            }
        }
        public async Task<StockResponse> GetPOItemsByID(int ItemId)
        {
            try
            {
                using (var scope = _serviceScopeFactory.CreateScope())
                {

                    var scopedcontext = scope.ServiceProvider.GetRequiredService<DragonFlyContext>();
                    var itemexists = await scopedcontext.UploadPOFile.Where(u => u.ID == ItemId).FirstOrDefaultAsync();
                    if (itemexists == null)
                    {
                        return new StockResponse(false, "not found", null);
                    }
                    return new StockResponse(true, "Queried successfully", itemexists);
                }
            }
            catch (Exception ex)
            {
                return new StockResponse(false, ex.Message, null);
            }
        }
        public async Task<StockResponse> GetAdjustedStockById(string batchNumber)
        {
            try
            {
                using (var scope = _serviceScopeFactory.CreateScope())
                {

                    var scopedcontext = scope.ServiceProvider.GetRequiredService<DragonFlyContext>();
                    var itemexist = await scopedcontext.StockAdjustment.Where(u => u.BatchNumber == batchNumber).ToListAsync();
                    if (itemexist == null)
                    {
                        return new StockResponse(false, "not found", null);
                    }
                    return new StockResponse(true, "Queried successfully", itemexist);
                }
            }
            catch (Exception ex)
            {
                return new StockResponse(false, ex.Message, null);
            }
        }
        public async Task<StockResponse> GetAllItemStock()
        {
            try
            {
                using (var scope = _serviceScopeFactory.CreateScope())
                {
                    var scopedcontext = scope.ServiceProvider.GetRequiredService<DragonFlyContext>();

                    var allstock = await scopedcontext.UploadPOFile.OrderByDescending(x => x.DateAdded).ToListAsync();
                    var loggedinuserobject = await _extraServices.LoggedInUser();

                    if (loggedinuserobject == null)
                    {

                        return new StockResponse(false, "user not logged in. login again", null);

                    }
                    var userName = loggedinuserobject.FirstName + ' ' + loggedinuserobject.LastName;



                    if (allstock == null || allstock.Count == 0)
                    {
                        return new StockResponse(false, "Stock doesn't exist", null);
                    }

                    Dictionary<string, AllStockListItems> stockDictionary = new Dictionary<string, AllStockListItems>();

                    foreach (var stock in allstock)
                    {
                        if (!stockDictionary.TryGetValue(stock.ItemName, out var existingStock))
                        {
                            var newStockItem = new AllStockListItems
                            {
                                ItemName = stock.ItemName,
                                OpeningStock = stock.OpeningStock,
                                // AvailableStock = stock.AvailableStock,
                                DateAdded = DateTime.Now,
                                Quantity = stock.TotalClosed,
                                AvailableStock = stock.AvailableStock,
                                StockOut = stock.StockOut,
                                Status = stock.Status,
                                BrandName = stock.BrandName,
                                StockIn = stock.TotalStockIn,
                                UpdatedBy = userName,
                               



                            };

                            stockDictionary.Add(stock.ItemName, newStockItem);
                        }
                        //else
                        //{
                        //    // Update existing stock quantities
                        //    existingStock.Quantity += stock.Quantity;
                        //    existingStock.AvailableStock += stock.AjustedQuantity;

                        //}

                    }

                    // Process issued quantities and update available stock

                    //foreach (var stock in stockDictionary.Values)
                    //{
                    //    var itemexists = await scopedcontext.IssueProcess
                    //        .Where(u => u.ItemName == stock.ItemName)
                    //        .FirstOrDefaultAsync();

                    //    if (itemexists != null)
                    //    {
                    //        stock.StockOut += itemexists.Quantity;
                    //        stock.AvailableStock -= itemexists.Quantity; // Deduct stock out quantity
                    //    }
                    //}
                    // Convert the dictionary values to a list
                    List<AllStockListItems> stockList = stockDictionary.Values.ToList();

                    return new StockResponse(true, "Successfully queried", stockList);
                }
            }
            catch (Exception ex)
            {
                return new StockResponse(false, ex.Message, null);
            }
        }

        public async Task<StockResponse> ApplyRequisition(AddRequisition addRequisition)
        {
            try
            {
                if (addRequisition.BrandName == "")
                {

                    return new StockResponse(false, "Kindly provide  brand name", null);
                }
                if (addRequisition.itemName == "")
                {

                    return new StockResponse(false, "Kindly provide  item name ", null);
                }
                if (addRequisition.Quantity == 0)
                {

                    return new StockResponse(false, "Kindly provide quantity", null);
                }
                if (addRequisition.clientName == "")
                {

                    return new StockResponse(false, "Kindly provide customer name ", null);
                }
                if (addRequisition.DeviceBeingRepaired == "")
                {
                    return new StockResponse(false, "Kindly provide device being repaired ", null);
                }
                if (addRequisition.Department == "")
                {
                    return new StockResponse(false, "Kindly provide department details", null);
                }
                if (addRequisition.clientName == "")
                {
                    return new StockResponse(false, "Kindly provide client details", null);
                }
                if (addRequisition.Requisitioner == "")
                {
                    return new StockResponse(false, "Kindly provide person requisitioning ", null);
                }

                using (var scope = _serviceScopeFactory.CreateScope())
                {
                    var scopedcontext = scope.ServiceProvider.GetRequiredService<DragonFlyContext>();
                    var itemexists = await scopedcontext.RequisitionForm
                    .Where(y => y.BrandName == addRequisition.BrandName &&
                    y.itemName == addRequisition.itemName).FirstOrDefaultAsync();
                    if (itemexists == null)
                    {
                        return new StockResponse(false, "Does not exist", null);
                    }



                    var itemclass = new RequisitionForm
                    {
                        BrandName = addRequisition.BrandName,
                        itemName = addRequisition.itemName,
                        Quantity = addRequisition.Quantity,
                        clientName = addRequisition.clientName,
                        DeviceBeingRepaired = addRequisition.DeviceBeingRepaired,
                        IssuedByDate = DateTime.Now,
                        Department = addRequisition.Department,
                        Description = addRequisition.Description,
                        Requisitioner = addRequisition.Requisitioner,
                        ApprovedStatus = "Waiting For Approval",
                        stockNeed = addRequisition.stockNeed,
                        Purpose = addRequisition.Purpose,
                        ApprovedDate = DateTime.Now,






                    };

                    //itemexists.Quantity -= addSalesOrdersVm.Quantity;
                    //itemexists.StockOut += addSalesOrdersVm.Quantity;
                    //if (itemexists.AvailableStock == 0)
                    //{
                    //    return new StockResponse(false, "No available stock please restock first....", null);
                    //}
                    //if (itemexists.AvailableStock < itemclass.Quantity)
                    //{
                    //    return new StockResponse(false, $"Note:You can only stockOut from '{itemexists.AvailableStock}'!!! ", null);
                    //}
                    //itemexists.AvailableStock -= addSalesOrdersVm.Quantity;

                    //if (itemexists.AvailableStock > itemexists.ReOrderLevel)
                    //{
                    //    itemexists.Status = "Good";
                    //}
                    //else if (itemexists.AvailableStock < itemexists.ReOrderLevel && itemexists.AvailableStock > 0 || itemexists.AvailableStock == itemexists.ReOrderLevel)
                    //{
                    //    itemexists.Status = "Low";
                    //}
                    //else
                    //{
                    //    itemexists.Status = "Out";
                    //}
                    //if (itemexists.AvailableStock < itemexists.ReOrderLevel || itemexists.AvailableStock == itemexists.ReOrderLevel)
                    //{
                    //    itemexists.ReorderRequired = "Yes";
                    //}
                    //else
                    //{
                    //    itemexists.ReorderRequired = "No";
                    //}



                    await scopedcontext.AddAsync(itemclass);
                    await scopedcontext.SaveChangesAsync();
                    return new StockResponse(true, "Requisition has been sent successfully", null);

                }


            }
            catch (Exception ex)
            {
                return new StockResponse(false, ex.Message, null);

            }

        }
        public async Task<StockResponse> ApplyRequisitionForm(ApplyRequistionvm addRequisition)
        {
            try
            {
                if (addRequisition.BrandName == "")
                {

                    return new StockResponse(false, "Kindly provide  brand name", null);
                }
                if (addRequisition.itemName == "")
                {

                    return new StockResponse(false, "Kindly provide  item name ", null);
                }
                if (addRequisition.Quantity == 0)
                {

                    return new StockResponse(false, "Kindly provide quantity", null);
                }
                if (addRequisition.clientName == "")
                {

                    return new StockResponse(false, "Kindly provide customer name ", null);
                }

                if (addRequisition.Department == "")
                {
                    return new StockResponse(false, "Kindly provide department details", null);
                }
                if (addRequisition.clientName == "")
                {
                    return new StockResponse(false, "Kindly provide client details", null);
                }
                //if (addRequisition.Requisitioner == "")
                //{
                //    return new StockResponse(false, "Kindly provide person requisitioning ", null);
                //}

                using (var scope = _serviceScopeFactory.CreateScope())
                {
                    var scopedcontext = scope.ServiceProvider.GetRequiredService<DragonFlyContext>();
                    //var itemexists = await scopedcontext.UploadPOFile
                    //.Where(y => y.ItemName == addRequisition.itemName).FirstOrDefaultAsync();
                    //if (itemexists == null)
                    //{
                    //    return new StockResponse(false, "Does not exist", null);
                    //}
                    //Console.WriteLine("Initial AvailableStock: " + itemexists.AvailableStock);


                    //if (itemexists.AvailableStock < addRequisition.Quantity)
                    //{
                    //    addRequisition.ApprovedStatus = "Failed";
                    //    addRequisition.RejectReason = "Insufficient stock";
                    //    return new StockResponse(false, $"Insufficient stock for the requisition,Please apply from '{itemexists.AvailableStock}' and below ", itemexists);



                    //}
                    var itemsWithSameName = await scopedcontext.UploadPOFile
                   .Where(u => u.ItemName == addRequisition.itemName && u.BrandName == addRequisition.BrandName)
                   .OrderByDescending(y => y.DateAdded)
                   .FirstOrDefaultAsync();
                    if (itemsWithSameName == null)
                    {
                        return new StockResponse(false, "Item with such brand does not exist", null);
                    }
                    var loggedinuserobject = await _extraServices.LoggedInUser();

                    if (loggedinuserobject == null)
                    {

                        return new StockResponse(false, "user not logged in. login again", null);

                    }
                    var useremails = loggedinuserobject.Email;
                    var userName = loggedinuserobject.FirstName + ' ' + loggedinuserobject.LastName;

                    var availableStockResponse = await GetAllItemStock();

                    if (!availableStockResponse.isTrue || !(availableStockResponse.Body is List<AllStockListItems> stockList))
                    {
                        return new StockResponse(false, "Error retrieving available stock information", null);
                    }



                    AllStockListItems itemExistsInStock = null;

                    foreach (var stock in stockList)
                    {
                        if (stock.ItemName == addRequisition.itemName)
                        {
                            itemExistsInStock = stock;
                            break;
                        }
                    }

                    if (itemExistsInStock == null)
                    {
                        return new StockResponse(false, "Item does not exist in stock", null);
                    }



                    var itemclass = new ApplyRequistionForm
                    {
                        BrandName = addRequisition.BrandName,
                        itemName = addRequisition.itemName,
                        Quantity = addRequisition.Quantity,
                        clientName = addRequisition.clientName,
                        DeviceBeingRepaired = addRequisition.DeviceBeingRepaired,
                        IssuedByDate = DateTime.Now,
                        Department = addRequisition.Department,
                        Description = addRequisition.Description,
                        Requisitioner = userName,
                        ApprovedStatus = "Pending",
                        stockNeed = addRequisition.stockNeed,
                        Purpose = addRequisition.Purpose,
                        ApprovedDate = DateTime.Now,
                        useremail = useremails

                    };
                    itemclass.CategoryName = itemsWithSameName.CategoryName;


                    if (itemExistsInStock.AvailableStock < itemclass.Quantity)
                    {
                        itemclass.ApprovedStatus = "Failed";
                        itemclass.RejectReason = $"Insufficient stock." +
                            $"Available stock: {itemExistsInStock.AvailableStock}";
                        return new StockResponse(false, $"Insufficient stock for the requisition. Available stock: {itemExistsInStock.AvailableStock}", itemExistsInStock);

                    }
                    scopedcontext.Update(itemclass);

                    await scopedcontext.AddAsync(itemclass);
                    await scopedcontext.SaveChangesAsync();

                    await _iemail_service.MakerEmail();
                    return new StockResponse(true, "Requisition has been sent successfully", itemclass);






                }



            }
            catch (Exception ex)
            {
                return new StockResponse(false, ex.Message, null);

            }



        }
        public async Task<StockResponse> ApplicationStatus(ApprovalProcessvm approvalProcessvm)
        {
            try
            {
                using (var scope = _serviceScopeFactory.CreateScope())
                {

                    var scopedcontext = scope.ServiceProvider.GetRequiredService<DragonFlyContext>();
                    var loggedinuserobject = await _extraServices.LoggedInUser();

                    var userEmail = loggedinuserobject.FirstName + ' ' + loggedinuserobject.LastName;

                    if (loggedinuserobject == null)
                    {

                        return new StockResponse(false, "user not logged in. login again", null);

                    }

                    var applieditem = await scopedcontext.ApplyRequistionForm.Where(u => u.ID == approvalProcessvm.id).FirstOrDefaultAsync();
                    if (applieditem == null)
                    {
                        return new StockResponse(false, "not found", null);
                    }
                    //var quantityexists = await scopedcontext.UploadPOFile.Where(y => y.ItemName == applieditem.itemName).FirstOrDefaultAsync();
                    //if (quantityexists == null)
                    //{
                    //    return new StockResponse(false, "item does not exist", null);

                    //}
                    //if (quantityexists.AvailableStock < applieditem.Quantity)
                    //{
                    //    applieditem.ApprovedStatus = "Failed";
                    //    applieditem.RejectReason = "Not Available";


                    //       return new StockResponse(false, $"Note:You can only apply  from '{quantityexists.AvailableStock} and below'!!! ", applieditem);


                    //{
                    //    applieditem.ApprovedStatus = "Failed";
                    //    applieditem.RejectReason = "Not Available";

                    //    return new StockResponse(false, $"Note:You can only apply  from '{quantityexists.AvailableStock}'!!! ", null);

                    //}

                    if (approvalProcessvm.selectedOption == "Approve")
                    {
                        approvalProcessvm.RejectedReason = "";
                    }

                    applieditem.selectedOption = approvalProcessvm.selectedOption;
                    applieditem.ApprovedDate = DateTime.Now;
                    applieditem.RejectReason = approvalProcessvm.RejectedReason;
                    applieditem.ApprovedBy = userEmail;
                    if (approvalProcessvm.selectedOption == "Reject")
                    {
                        applieditem.ApprovedStatus = "Rejected";

                    }
                    else
                    {
                        applieditem.ApprovedStatus = "Approved";
                    }

                    scopedcontext.Update(applieditem);
                    await scopedcontext.SaveChangesAsync();

                    if (approvalProcessvm.selectedOption == "Reject")
                    {
                        var new_mail = new emailbody
                        {
                            ToEmail = applieditem.useremail,
                            PayLoad = "Your request has been Rejected,Please check the reason why",
                            UserName = "N/A",
                        };
                        await _iemail_service.send_status_to_Requester(new_mail);
                    }
                    else if (approvalProcessvm.selectedOption == "Approve")
                    {
                        var new_mail = new emailbody
                        {
                            ToEmail = applieditem.useremail,
                            PayLoad = "Your request has been approved,Wait a minute for it to undergo issue process",
                            UserName = "N/A"
                        };
                        await _iemail_service.send_status_to_Requester(new_mail);
                        await _iemail_service.IssuerEmail();
                    }
                    else
                    {
                        var new_mail = new emailbody
                        {
                            ToEmail = applieditem.useremail,
                            PayLoad = "Your request has been Rejected",
                            UserName = "N/A",
                        };
                        await _iemail_service.send_status_to_Requester(new_mail);

                    }
                    return new StockResponse(true, "Successfully updated", applieditem);
                }
            }
            catch (Exception ex)
            {
                return new StockResponse(false, ex.Message, null);
            }
        }
        public async Task<StockResponse> GetAllRequisitionApplication()
        {

            try
            {

                using (var scope = _serviceScopeFactory.CreateScope())
                {
                    var scopedcontext = scope.ServiceProvider.GetRequiredService<DragonFlyContext>();
                    var allbrands = await scopedcontext.ApplyRequistionForm.ToListAsync();

                    if (allbrands == null)
                    {
                        return new StockResponse(false, "Requisition doesn't exist", null);
                    }
                    return new StockResponse(true, "Successfully queried", allbrands);

                }
            }
            catch (Exception ex)
            {
                return new StockResponse(false, ex.Message, null);
            }
        }
        public async Task<StockResponse> GetRequisitionbyId(int Id)
        {
            try
            {
                using (var scope = _serviceScopeFactory.CreateScope())
                {

                    var scopedcontext = scope.ServiceProvider.GetRequiredService<DragonFlyContext>();
                    var supplierexists = await scopedcontext.ApplyRequistionForm.Where(u => u.ID == Id).FirstOrDefaultAsync();
                    if (supplierexists == null)
                    {
                        return new StockResponse(false, "not found", null);
                    }
                    return new StockResponse(true, "Queried successfully", supplierexists);
                }
            }
            catch (Exception ex)
            {
                return new StockResponse(false, ex.Message, null);
            }
        }
        public async Task<StockResponse> IssueProcess(int id)
        {
            try
            {
                using (var scope = _serviceScopeFactory.CreateScope())
                {
                    var scopedcontext = scope.ServiceProvider.GetRequiredService<DragonFlyContext>();
                    var applieditem = await scopedcontext.ApplyRequistionForm.Where(u => u.ID == id).FirstOrDefaultAsync();
                    var loggedinuserobject = await _extraServices.LoggedInUser();
                    var userEmail = loggedinuserobject.FirstName + ' ' + loggedinuserobject.LastName;

                    if (loggedinuserobject == null)
                    {
                        return new StockResponse(false, "User not logged in. Please log in again.", null);
                    }

                    if (applieditem == null)
                    {
                        return new StockResponse(false, "Item not found.", null);
                    }

                    if (applieditem.ApprovedStatus == "Rejected")
                    {
                        return new StockResponse(false, "Check reason for rejection and revise again.", null);
                    }


                    // Retrieve the items with the same itemName from UploadPOFile
                    var itemsWithSameName = await scopedcontext.UploadPOFile
                        .Where(u => u.ItemName == applieditem.itemName && u.BrandName == applieditem.BrandName)
                        .OrderByDescending(y => y.DateAdded)
                        .FirstOrDefaultAsync();
                    if (itemsWithSameName == null)
                    {
                        return new StockResponse(false, "Item with such brandname does not exist", null);
                    }

                    //var issueResponses = new List<StockResponse>();

                    //foreach (var item in itemsWithSameName)
                    //{
                    var itemclass = new IssueProcess
                    {
                        IssueStatus = "Issued",
                        IssuedBy = userEmail,
                        DateIssued = DateTime.Now,


                    };
                    itemclass.CategoryName = itemsWithSameName.CategoryName;

                    itemclass.Quantity = applieditem.Quantity;
                    itemclass.ItemName = applieditem.itemName;
                    itemclass.FormID = applieditem.ID;
                    applieditem.ApprovedStatus = itemclass.IssueStatus;
                    itemsWithSameName.StockOut += itemclass.Quantity;

                    if (itemsWithSameName.AvailableStock < itemclass.Quantity)
                    {
                        applieditem.ApprovedStatus = "Failed";
                        applieditem.RejectReason = "Not available...Will be restocked soon";
                        scopedcontext.Update(applieditem);
                        return new StockResponse(false, $"Not enough available stock to issue {itemclass.Quantity} units for item {itemclass.ItemName}.", null);




                    }
                    else
                    {
                        // Update stock quantities
                        itemsWithSameName.AvailableStock = itemsWithSameName.AvailableStock - applieditem.Quantity;
                        scopedcontext.Update(itemsWithSameName);
                        await scopedcontext.SaveChangesAsync();

                        if (itemsWithSameName.AvailableStock > itemsWithSameName.ReOrderLevel)
                        {
                            itemsWithSameName.Status = "Good";
                        }
                        else if (itemsWithSameName.AvailableStock < itemsWithSameName.ReOrderLevel || itemsWithSameName.AvailableStock > 0)
                        {
                            itemsWithSameName.Status = "Low";
                        }
                        else if (itemsWithSameName.AvailableStock == 0)
                        {
                            itemsWithSameName.Status = "Out";
                        }
                        else
                        {
                            itemsWithSameName.Status = "Out";
                        }



                        // Save changes to the database
                        await scopedcontext.AddAsync(itemclass);
                        await scopedcontext.SaveChangesAsync();

                        new StockResponse(true, $"Successfully issued and updated for item {itemclass.ItemName}.", itemclass);
                    }
                }

                return new StockResponse(true, "Issue process completed.", null);

            }
            catch (Exception ex)
            {
                return new StockResponse(false, ex.Message, null);
            }
        }

        public async Task<StockResponse> GetRequisitionByEmail()
        {
            try
            {

                using (var scope = _serviceScopeFactory.CreateScope())
                {

                    var scopedcontext = scope.ServiceProvider.GetRequiredService<DragonFlyContext>();
                    var loggedinuserobject = await _extraServices.LoggedInUser();
                    var useremailexists = await scopedcontext.ApplyRequistionForm.Where(u => u.useremail == loggedinuserobject.Email).ToListAsync();
                    if (useremailexists == null)
                    {
                        return new StockResponse(false, "not found", null);
                    }
                    return new StockResponse(true, "Queried successfully", useremailexists);
                }
            }
            catch (Exception ex)
            {
                return new StockResponse(false, ex.Message, null);
            }
        }
        public async Task<StockResponse> GetFormByStatusPending()
        {
            try
            {
                using (var scope = _serviceScopeFactory.CreateScope())
                {

                    var scopedcontext = scope.ServiceProvider.GetRequiredService<DragonFlyContext>();
                    var supplierexists = await scopedcontext.ApplyRequistionForm.Where(u => u.ApprovedStatus == "Pending").OrderByDescending(x => x.ApprovedDate).ToListAsync();
                    if (supplierexists == null)
                    {
                        return new StockResponse(false, "not found", null);
                    }
                    return new StockResponse(true, "Queried successfully", supplierexists);
                }
            }
            catch (Exception ex)
            {
                return new StockResponse(false, ex.Message, null);
            }
        }
        public async Task<StockResponse> GetFormStatusApproved()
        {
            try
            {
                using (var scope = _serviceScopeFactory.CreateScope())
                {

                    var scopedcontext = scope.ServiceProvider.GetRequiredService<DragonFlyContext>();
                    var supplierexists = await scopedcontext.ApplyRequistionForm.Where(u => u.ApprovedStatus == "Approved").OrderByDescending(x => x.ApprovedDate).ToListAsync();
                    if (supplierexists == null)
                    {
                        return new StockResponse(false, "not found", null);
                    }
                    return new StockResponse(true, "Queried successfully", supplierexists);
                }
            }
            catch (Exception ex)
            {
                return new StockResponse(false, ex.Message, null);
            }
        }
        public async Task<StockResponse> SelectSerialNumber(SelectSerialvm selectSerialvm)
        {
            try
            {
                using (var scope = _serviceScopeFactory.CreateScope())
                {
                    var scopedcontext = scope.ServiceProvider.GetRequiredService<DragonFlyContext>();
                    var applieditem = await scopedcontext.ApplyRequistionForm.Where(u => u.ID == selectSerialvm.IssueID).FirstOrDefaultAsync();


                    if (applieditem == null)
                    {
                        return new StockResponse(false, "Issue item not found.", null);
                    }
                    if (applieditem.CategoryName == "Accesory")
                    {
                        return new StockResponse(false, "Oops wrong page", null);
                    }




                    // Retrieve the items with the same itemName from UploadPOFile


                    //var issueResponses = new List<StockResponse>();

                    //foreach (var item in itemsWithSameName)
                    //{
                    var itemclass = new SelectSerial
                    {
                        SerialNumber = selectSerialvm.SerialNumber,
                        IssueID = applieditem.ID,
                        SerialStatus = "Not issued",


                    };
                    itemclass.SerialStatus = "Issued";
                    var ProductDetails = await scopedcontext.AddProductDetails
                       .Where(u => u.SerialNumber == itemclass.SerialNumber)
                       .FirstOrDefaultAsync();
                    if (ProductDetails != null)
                    {
                        ProductDetails.SerialStatus = itemclass.SerialStatus;
                        scopedcontext.Update(ProductDetails);
                    }


                    // Save changes to the database
                    await scopedcontext.AddAsync(itemclass);
                    await scopedcontext.SaveChangesAsync();

                    return new StockResponse(true, "Successfully issued and updated", itemclass);
                }
            }


            catch (Exception ex)
            {
                return new StockResponse(false, ex.Message, null);
            }
        }
        public async Task<StockResponse> GetSelectedSerials(int issueID)
        {
            try
            {
                using (var scope = _serviceScopeFactory.CreateScope())
                {

                    var scopedcontext = scope.ServiceProvider.GetRequiredService<DragonFlyContext>();
                    var supplierexists = await scopedcontext.SelectSerial.Where(u => u.IssueID == issueID).ToListAsync();
                    if (supplierexists == null)
                    {
                        return new StockResponse(false, "not found", null);
                    }
                    return new StockResponse(true, "Queried successfully", supplierexists);
                }
            }
            catch (Exception ex)
            {
                return new StockResponse(false, ex.Message, null);
            }
        }
        public async Task<StockResponse> GetSerialByIssued()
        {
            try
            {
                using (var scope = _serviceScopeFactory.CreateScope())
                {

                    var scopedcontext = scope.ServiceProvider.GetRequiredService<DragonFlyContext>();
                    var supplierexists = await scopedcontext.AddProductDetails.Where(u => u.SerialStatus == "Not Issued").ToListAsync();
                    if (supplierexists == null)
                    {
                        return new StockResponse(false, "not found", null);
                    }
                    return new StockResponse(true, "Queried successfully", supplierexists);
                }
            }
            catch (Exception ex)
            {
                return new StockResponse(false, ex.Message, null);
            }
        }
        public async Task<StockResponse> GetFormByStatusIssued()
        {
            try
            {
                using (var scope = _serviceScopeFactory.CreateScope())
                {

                    var scopedcontext = scope.ServiceProvider.GetRequiredService<DragonFlyContext>();
                    var supplierexists = await scopedcontext.ApplyRequistionForm.Where(u => u.ApprovedStatus == "Issued").OrderByDescending(x => x.ApprovedDate).ToListAsync();
                    if (supplierexists == null)
                    {
                        return new StockResponse(false, "not found", null);
                    }
                    return new StockResponse(true, "Queried successfully", supplierexists);
                }
            }
            catch (Exception ex)
            {
                return new StockResponse(false, ex.Message, null);
            }
        }
        public async Task<StockResponse> AddDeliveryNote(AddDeliveryNotevm addDeliveryNotevm)
        {
            try
            {
                if (addDeliveryNotevm.DeliveryNumber == "")
                {

                    return new StockResponse(false, "Kindly provide delivery number", null);
                }
                if (addDeliveryNotevm.MeansOfDelivery == "")
                {

                    return new StockResponse(false, "Kindly provide means of delivery", null);
                }
                if (addDeliveryNotevm.BatchQuantity == 0)
                {

                    return new StockResponse(false, "Kindly provide batch quantity", null);
                }
                using (var scope = _serviceScopeFactory.CreateScope())
                {
                    var scopedcontext = scope.ServiceProvider.GetRequiredService<DragonFlyContext>();

                    //check if role exists 

                    //var deliverynumberexists = await scopedcontext.AddDeliveryNote.Where(x => x.DeliveryNumber == addDeliveryNotevm.DeliveryNumber).FirstOrDefaultAsync();

                    //if (deliverynumberexists != null)
                    //{
                    //    return new StockResponse(false, $" Batch with delivery note  '{addDeliveryNotevm.DeliveryNumber}' already exists ", null);
                    //}
                    var itemexists = await scopedcontext.UploadPOFile.Where(x => x.ID == addDeliveryNotevm.ItemID).FirstOrDefaultAsync();

                    if (itemexists == null)
                    {
                        return new StockResponse(false, $" Item  '{addDeliveryNotevm.ItemID}' doesn't exist", null);
                    }
                    var invoice_no_obj = GetBatchNumber().Result;
                    var batchnumber = "BATCH-" + invoice_no_obj;
                    var itemId = addDeliveryNotevm.ItemID;
                    var itemidexists = await scopedcontext.AddDeliveryNote
                                      .Where(x => x.ItemID == itemId)
                                      .OrderByDescending(y => y.DateCreated)
                                      .FirstOrDefaultAsync();
                    var CalculateTotalClosed = await scopedcontext.ApprovalBatch
          .Where(u => u.itemID == addDeliveryNotevm.ItemID && u.selectedOption == "Approve")
          .SumAsync(y => y.ClosedQuantity);
                    var TotalDelivered = await scopedcontext.UploadPOFile.Where(x => x.ID == addDeliveryNotevm.ItemID).FirstOrDefaultAsync();
                    var deliveryclass = new AddDeliveryNote
                    {
                        DeliveryNumber = addDeliveryNotevm.DeliveryNumber,
                        BatchQuantity = addDeliveryNotevm.BatchQuantity,
                        BatchNumber = batchnumber,
                        DeliveryDate = addDeliveryNotevm.DeliveryDate,
                        MeansOfDelivery = addDeliveryNotevm.MeansOfDelivery,
                        DateCreated = DateTime.Now,
                        AirWayBillNumber = addDeliveryNotevm.AirWayBillNumber,
                        ItemID = addDeliveryNotevm.ItemID,
                        ProductStatus = "Incomplete",
                        Reference_Number = await GetGeneratedref(),
                        PONumber = TotalDelivered.PONumber,
                          TotalClosed = CalculateTotalClosed,
                };


                    var quantitydamaged = await scopedcontext.StockAdjustment.Where(x => x.BatchNumber == deliveryclass.BatchNumber).FirstOrDefaultAsync();
                    if (quantitydamaged != null)
                    {
                        deliveryclass.quantityDamaged += quantitydamaged.QuantityDamaged;
                    }

                    deliveryclass.CategoryName = itemexists.CategoryName;


                    if (deliveryclass.CategoryName == "Accesory")
                    {
                        deliveryclass.ProductStatus = "Complete";
                    }
                    else
                    {
                        deliveryclass.ProductStatus = "Incomplete";
                    }

                  

                    if (itemidexists != null)
                    {
                        deliveryclass.TotalQuantity = itemidexists.TotalQuantity + deliveryclass.BatchQuantity;
                        scopedcontext.Update(deliveryclass);
                    }
                   
                    else
                    {
                        deliveryclass.TotalQuantity = deliveryclass.BatchQuantity;
                    }
                  

                        if (deliveryclass.CategoryName == "Product")
                    {
                        var new_numb = 0;

                        while (new_numb < deliveryclass.BatchQuantity)
                        {
                            new_numb++;
                            var new_numbering = new ProductNumbering
                            {
                                NumberValue = new_numb,
                                Reference_Number = deliveryclass.Reference_Number,
                                Type = "Product",
                                Status = "UNASSIGNED",
                            };


                            await scopedcontext.ProductNumbering.AddAsync(new_numbering);

                        }

                    }
                    if (TotalDelivered != null)
                    {
                        if (TotalDelivered.TotalDelivered == TotalDelivered.Quantity)
                        {
                            return new StockResponse(false, $"Cannot add more quantity. Item is already complete.", null);
                        }

                        TotalDelivered.TotalDelivered = deliveryclass.TotalQuantity;
                        TotalDelivered.TotalClosed = deliveryclass.TotalClosed;
                        TotalDelivered.OutstandingQuantity = TotalDelivered.Quantity - TotalDelivered.TotalDelivered;

                        if (TotalDelivered.TotalDelivered == TotalDelivered.Quantity)
                        {
                            TotalDelivered.ProductStatus = "Complete";
                        }
                        else
                        {
                            TotalDelivered.ProductStatus = "Incomplete";
                        }

                        scopedcontext.Update(TotalDelivered);
                    }


                    await scopedcontext.AddAsync(deliveryclass);
                    await scopedcontext.SaveChangesAsync();
                    return new StockResponse(true, $"Batch with delivery note number: '{addDeliveryNotevm.DeliveryNumber}'  created successfully", null);

                }

            }
            catch (Exception ex)
            {
                return new StockResponse(false, ex.Message, null);

            }

        }
        public async Task<int> GetBatchNumber()
        {

            var last_number_obj = await _dragonFlyContext.BatchNumber
                .OrderByDescending(y => y.DateCreated).FirstOrDefaultAsync();


            if (last_number_obj == null)
            {
                var newvalue = new BatchNo();

                newvalue.BatchNumber = 1;
                await _dragonFlyContext.AddAsync(newvalue);
                await _dragonFlyContext.SaveChangesAsync();
                return newvalue.BatchNumber;
            }

            last_number_obj.BatchNumber = last_number_obj.BatchNumber + 1;
            _dragonFlyContext.Update(last_number_obj);
            await _dragonFlyContext.SaveChangesAsync();

            return last_number_obj.BatchNumber;
        }
        public async Task<StockResponse> GetBatchByItems(int itemId)
        {
            try
            {
                using (var scope = _serviceScopeFactory.CreateScope())
                {

                    var scopedcontext = scope.ServiceProvider.GetRequiredService<DragonFlyContext>();
                    var product_exist = await scopedcontext.AddDeliveryNote.Where(u => u.ItemID == itemId).ToListAsync();
                    if (product_exist == null)
                    {
                        return new StockResponse(false, "not found", null);
                    }
                    return new StockResponse(true, "Queried successfully", product_exist);
                }
            }
            catch (Exception ex)
            {
                return new StockResponse(false, ex.Message, null);
            }
        }
        public async Task<StockResponse> GetProductbyBatchid(string BatchNumber)
        {
            try
            {
                using (var scope = _serviceScopeFactory.CreateScope())
                {

                    var scopedcontext = scope.ServiceProvider.GetRequiredService<DragonFlyContext>();
                    var product_exist = await scopedcontext.AddDeliveryNote.Where(u => u.BatchNumber == BatchNumber).FirstOrDefaultAsync();
                    if (product_exist == null)
                    {
                        return new StockResponse(false, "not found", null);
                    }
                    return new StockResponse(true, "Queried successfully", product_exist);
                }
            }
            catch (Exception ex)
            {
                return new StockResponse(false, ex.Message, null);
            }
        }
        public async Task<StockResponse> StockAdjustment([FromBody]  StockAdjustvm adjustStockvm)
        {

            try
            {
                using (var scope = _serviceScopeFactory.CreateScope())
                {
                    var scopedcontext = scope.ServiceProvider.GetRequiredService<DragonFlyContext>();
                    var lastupdate = await scopedcontext.AddDeliveryNote.Where(y => y.BatchNumber == adjustStockvm.BatchNumber)
                      .FirstOrDefaultAsync();

                    if (lastupdate == null)
                    {
                        return new StockResponse(false, "item not found", null);
                    }

                    var Items = new StockAdjustment
                    {
                        QuantityDamaged = adjustStockvm.QuantityDamaged,
                        Description = adjustStockvm.Description,
                        ItemID = adjustStockvm.ItemID,
                        BatchNumber = adjustStockvm.BatchNumber,
                        SerialNumber = adjustStockvm.SerialNumber,
                        ConditionStatus = "Okay",

                    };
                    Items.ItemID = lastupdate.ItemID;
                    Items.CategoryName = lastupdate.CategoryName;
                    Items.ConditionStatus = "Faulty";
                    var quantitydamagedItems = await scopedcontext.AddDeliveryNote
                    .Where(x => x.BatchNumber == Items.BatchNumber)
                    .FirstOrDefaultAsync();
                    if (quantitydamagedItems != null)
                    {
                        quantitydamagedItems.quantityDamaged = Items.QuantityDamaged + quantitydamagedItems.quantityDamaged;
                        scopedcontext.Update(quantitydamagedItems);
                    }
                    var okquantity = await scopedcontext.UploadPOFile
                  .Where(x => x.ID == Items.ItemID)
                  .FirstOrDefaultAsync();
                    var itemidexists = await scopedcontext.AddDeliveryNote
                                 .Where(x => x.ItemID == Items.ItemID)
                                 .OrderByDescending(y => y.DateCreated)
                                 .LastOrDefaultAsync();


                    //foreach (var quantitydamagedItem in quantitydamagedItems)
                    //{
                    //    quantitydamagedItem.quantityDamaged = Items.QuantityDamaged+quantitydamagedItem.quantityDamaged;
                    //    scopedcontext.Update(quantitydamagedItem);
                    //}
                    var serialNumbersToUpdate = Items.SerialNumber;


                    var serailStatusItems = await scopedcontext.AddProductDetails
    .Where(x => x.BatchNumber == Items.BatchNumber && serialNumbersToUpdate.Contains(x.SerialNumber))
    .ToListAsync();

                    if (serailStatusItems.Count == 0)
                    {
                        Console.WriteLine($"No matching items found for BatchNumber: {Items.BatchNumber} and SerialNumber: {Items.SerialNumber}");
                    }

                    foreach (var serailStatusItem in serailStatusItems)
                    {
                        Console.WriteLine($"Updating ItemStatus for BatchNumber: {Items.BatchNumber} and SerialNumber: {Items.SerialNumber}");

                        serailStatusItem.ItemStatus = Items.ConditionStatus;
                        scopedcontext.Update(serailStatusItem);
                    }


                        if (itemidexists!=null)
                        {
                            // Update the specific item
                            itemidexists.TotalDamages += adjustStockvm.QuantityDamaged;
                            itemidexists.OKQuantity = okquantity.TotalDelivered - itemidexists.TotalDamages;
                            scopedcontext.Update(itemidexists);

                            // Update the UploadPOFile OKQuantity
                          

                            if (okquantity != null)
                            {
                                okquantity.OKQuantity = itemidexists.OKQuantity;
                                scopedcontext.Update(okquantity);
                            }

                            // Update the item in the context
                            scopedcontext.Update(itemidexists);
                        
                    }

                    await scopedcontext.SaveChangesAsync(); // Save changes to the database







                    await scopedcontext.SaveChangesAsync(); // Save changes to the database


                    // ... rest of the code ...


                    //lastupdate.AjustedQuantity -= Items.QuantityDamaged;
                    //scopedcontext.Update(lastupdate);
                 

                    await scopedcontext.AddAsync(Items);
                    await scopedcontext.SaveChangesAsync();
                }

               
                return new StockResponse(true, "Updated successfully", adjustStockvm);
            }
   
    catch (Exception ex)
    {
        return new StockResponse(false, ex.Message, null);
    }
}
        public async Task<StockResponse> ApprovalsReview(ApproveBatchvm approvalProcessvm)
        {
            try
            {
                using (var scope = _serviceScopeFactory.CreateScope())
                {

                    var scopedcontext = scope.ServiceProvider.GetRequiredService<DragonFlyContext>();
                    var loggedinuserobject = await _extraServices.LoggedInUser();

                    var userEmail = loggedinuserobject.FirstName + ' ' + loggedinuserobject.LastName;

                    if (loggedinuserobject == null)
                    {

                        return new StockResponse(false, "user not logged in. login again", null);

                    }

                    var applieditem = await scopedcontext.AddDeliveryNote.Where(u => u.BatchNumber == approvalProcessvm.BatchNumber).FirstOrDefaultAsync();
                   
                  
                    if (applieditem == null)
                    {
                        return new StockResponse(false, "not found", null);
                    }
                    var Items = new ApprovalBatch
                    {
                        selectedOption = approvalProcessvm.selectedOption,
                        RejectedReason = approvalProcessvm.RejectedReason,


                    };
                    Items.itemID = applieditem.ItemID;
                    Items.BatchNumber = applieditem.BatchNumber;
                    Items.ClosedQuantity = applieditem.BatchQuantity-applieditem.quantityDamaged;
                    await scopedcontext.AddAsync(Items);
                    await scopedcontext.SaveChangesAsync();
                    var CalculateTotalClosed = await scopedcontext.ApprovalBatch
    .Where(u => u.itemID == Items.itemID && u.selectedOption == "Approve")
    .SumAsync(y => y.ClosedQuantity);

                    // Save the sum as TotalClosed property
                                                              //   var totalCosed = await scopedcontext.UploadPOFile.Where(u => u.ID == Items.itemID).FirstOrDefaultAsync();
                    if (CalculateTotalClosed == null)
                    {
                        return new StockResponse(false, "Item does not exist", null);

                    }
                    Items.TotalClosed = CalculateTotalClosed;


                    if (approvalProcessvm.selectedOption == "Approve")
                    {
                        approvalProcessvm.RejectedReason = "";
                    }

                    applieditem.selectedOption = approvalProcessvm.selectedOption;
                    applieditem.AprrovedDate = DateTime.Now;
                    applieditem.RejectedReason = approvalProcessvm.RejectedReason;
                    applieditem.ApprovedBy = userEmail;
                    if (approvalProcessvm.selectedOption == "Reject")
                    {
                        applieditem.ProductStatus = "Incomplete";

                    }
                    else
                    {
                        applieditem.ProductStatus = "Closed";

                    }
                    //if (CalculateTotalClosed!=null)
                    //{
                    //    CalculateTotalClosed.TotalClosed = CalculateTotalClosed.TotalClosed + Items.ClosedQuantity;
                    //    scopedcontext.Update(CalculateTotalClosed);
                    //}
                    //if (totalCosed != null)
                    //{
                    //    totalCosed.TotalClosed = CalculateTotalClosed.TotalClosed;
                    //}

                        scopedcontext.Update(applieditem);
                  
                    await scopedcontext.SaveChangesAsync();
               
                return new StockResponse(true, "Successfully updated", applieditem);
                }

            }
            catch (Exception ex)
            {
                return new StockResponse(false, ex.Message, null);
    }
}
        public async Task<StockResponse> GetBatchCompleteStatus()
        {
            try
            {
                using (var scope = _serviceScopeFactory.CreateScope())
                {

                    var scopedcontext = scope.ServiceProvider.GetRequiredService<DragonFlyContext>();
                    var supplierexists = await scopedcontext.AddDeliveryNote.Where(u => u.ProductStatus == "Complete").OrderByDescending(x => x.AprrovedDate).ToListAsync();
                    if (supplierexists == null)
                    {
                        return new StockResponse(false, "not found", null);
                    }
                    return new StockResponse(true, "Queried successfully", supplierexists);
                }
            }
            catch (Exception ex)
            {
                return new StockResponse(false, ex.Message, null);
            }
        }

        public async Task<StockResponse> GetBatchByBatchNumber(string BatchNumber)
        {
            try
            {
                using (var scope = _serviceScopeFactory.CreateScope())
                {

                    var scopedcontext = scope.ServiceProvider.GetRequiredService<DragonFlyContext>();
                    var product_exist = await scopedcontext.AddDeliveryNote.Where(u => u.BatchNumber == BatchNumber).FirstOrDefaultAsync();
                    if (product_exist == null)
                    {
                        return new StockResponse(false, "not found", null);
                    }
                    return new StockResponse(true, "Queried successfully", product_exist);
                }
            }
            catch (Exception ex)
            {
                return new StockResponse(false, ex.Message, null);
            }
        }
        public async Task<StockResponse> AddPOItemLines(AddPOItemLinesvm addBatchDetailsvm)
        {
            try
            {
                if (addBatchDetailsvm.ItemName == "")
                {

                    return new StockResponse(false, "Kindly provide an item name ", null);
                }
                if (addBatchDetailsvm.CategoryName == "")
                {
                    return new StockResponse(false, "Kindly provide category", null);

                }

                if (addBatchDetailsvm.Currency == "")
                {
                    return new StockResponse(false, "Kindly provide currency", null);
                }
                if (addBatchDetailsvm.UnitPrice < 0)
                {
                    return new StockResponse(false, "Kindly provide unit price", null);
                }
                if (addBatchDetailsvm.Quantity < 0)
                {
                    return new StockResponse(false, "Kindly provide quantity", null);
                }
                if (addBatchDetailsvm.Warranty < 0)
                {
                    return new StockResponse(false, "Kindly provide warranty", null);
                }
                using (var scope = _serviceScopeFactory.CreateScope())
                {
                    var scopedcontext = scope.ServiceProvider.GetRequiredService<DragonFlyContext>();
                    var invoiceexists = await scopedcontext.PurchaseOrderss
                 .Where(y => y.PONumber == addBatchDetailsvm.PONumber).OrderByDescending(y => y.PONumber).FirstOrDefaultAsync();
                    if (invoiceexists == null)
                    {
                        return new StockResponse(false, "PO Number does not exist", null);
                    }

                    var loggedinuserobject = await _extraServices.LoggedInUser();

                    var userEmail = loggedinuserobject.FirstName + ' ' + loggedinuserobject.LastName;

                    if (loggedinuserobject == null)
                    {

                        return new StockResponse(false, "user not logged in. login again", null);

                    }




                    var itemNameexists = await scopedcontext.AddItem
                   .Where(y => y.ItemName == addBatchDetailsvm.ItemName).FirstOrDefaultAsync();
                    if (itemNameexists == null)
                    {
                        return new StockResponse(false, "Item name does not exist", null);
                    }

                    var itemclass = new UploadPOFile
                    {
                        ItemName = addBatchDetailsvm.ItemName,
                        CategoryName = addBatchDetailsvm.CategoryName,
                        Currency = addBatchDetailsvm.Currency,
                        UnitPrice = addBatchDetailsvm.UnitPrice,
                        Warranty = addBatchDetailsvm.Warranty,
                        Quantity = addBatchDetailsvm.Quantity,
                        WarrantyStartDate = addBatchDetailsvm.WarrantyStartDate,
                        UpdatedBy = userEmail,
                        UpdatedOn = addBatchDetailsvm.UpdatedOn,
                        PONumber = addBatchDetailsvm.PONumber,
                        BrandName = addBatchDetailsvm.BrandName,
                        Amount = "Unknown",
                        ProductStatus="Incomplete",







                    };
                    //var itemassigned = await scopedcontext.AddProductDetails.Where(x => x.ItemID == itemclass.InvoiceLineId).FirstOrDefaultAsync();

                    //if (itemassigned != null)
                    //{
                    //    itemassigned.ProductStatus = itemclass.Status;
                    //    return new StockResponse(false, "ITEM ALREADY COMPLETE", null);
                    //}
                    var itemexists = await scopedcontext.UploadPOFile.Where(x => x.ItemName == addBatchDetailsvm.ItemName && x.BrandName == addBatchDetailsvm.BrandName && x.PONumber == addBatchDetailsvm.PONumber && x.CategoryName == itemNameexists.Category).FirstOrDefaultAsync();


                    itemclass.CategoryName = itemNameexists.Category;

                    if (itemexists != null)
                    {
                        return new StockResponse(false, $" Invoice {addBatchDetailsvm.PONumber} with'{addBatchDetailsvm.BrandName}-{addBatchDetailsvm.ItemName} already exists ", null);
                    }


                    itemclass.Reference_Number = await GetGeneratedref();
                    itemclass.TotalUnitPrice = itemclass.UnitPrice * itemclass.Quantity;

                    if (itemclass.CategoryName == "Accesory")
                    {
                        itemclass.Status = "Incomplete";
                    }
                    else
                    {
                        itemclass.Status = "Incomplete";
                    }

                    if (itemclass.Quantity == itemclass.Quantity) { 
                    }


                    itemclass.WarrantyEndDate = itemclass.WarrantyStartDate.AddMonths(itemclass.Warranty);
                    await scopedcontext.AddAsync(itemclass);
                    await scopedcontext.SaveChangesAsync();
                    if (itemclass.CategoryName == "Product")
                    {
                        var new_numb = 0;



                        while (new_numb < itemclass.Quantity)
                        {
                            new_numb++;
                            var new_numbering = new ProductNumbering
                            {

                                NumberValue = new_numb,
                                Reference_Number = itemclass.Reference_Number,
                                Type = "Product",
                                Status = "UNASSIGNED"


                            };







                            await _dragonFlyContext.AddAsync(new_numbering);
                            await _dragonFlyContext.SaveChangesAsync();


                        }
                    }

                    return new StockResponse(true, $"Item '{addBatchDetailsvm.BrandName}-{addBatchDetailsvm.ItemName}' in invoice{addBatchDetailsvm.PONumber}  created successfully", null);

                }

            }
            catch (Exception ex)
            {
                return new StockResponse(false, ex.Message, null);

            }

        }
        public async Task<StockResponse> MarkPOComplete( string PONumber)
        {
            try
            {
                using (var scope = _serviceScopeFactory.CreateScope())
                {

                    var scopedcontext = scope.ServiceProvider.GetRequiredService<DragonFlyContext>();
               

                    var applieditem = await scopedcontext.PurchaseOrderss.Where(u => u.PONumber == PONumber).FirstOrDefaultAsync();
                    var uploadedpo = await scopedcontext.UploadPOFile.Where(u => u.PONumber == PONumber).FirstOrDefaultAsync();
                    if (applieditem == null)
                    {
                        return new StockResponse(false, "not found", null);
                    }
                    if (uploadedpo != null)
                    {
                        uploadedpo.CaptureStatus = "Pending";
                        scopedcontext.Update(uploadedpo);
                    }
                    applieditem.DateCreated = DateTime.Now;
                    applieditem.CaptureStatus = "Pending";

                   

                    scopedcontext.Update(applieditem);
                    await scopedcontext.SaveChangesAsync();

                    return new StockResponse(true, "Successfully updated ", applieditem);
                }
            }
            catch (Exception ex)
            {
                return new StockResponse(false, ex.Message, null);
            }
        }
        public async Task<StockResponse> POApprovalReview(POApprovalvm pOApprovalvm)
        {
            try
            {
                using (var scope = _serviceScopeFactory.CreateScope())
                {

                    var scopedcontext = scope.ServiceProvider.GetRequiredService<DragonFlyContext>();
                    var loggedinuserobject = await _extraServices.LoggedInUser();

                    var userEmail = loggedinuserobject.FirstName + ' ' + loggedinuserobject.LastName;

                    if (loggedinuserobject == null)
                    {

                        return new StockResponse(false, "user not logged in. login again", null);

                    }

                    var applieditem = await scopedcontext.PurchaseOrderss.Where(u => u.PONumber == pOApprovalvm.PONumber).FirstOrDefaultAsync();
                    var uploadedpoitem = await scopedcontext.UploadPOFile.Where(u => u.PONumber == pOApprovalvm.PONumber).FirstOrDefaultAsync();
                    if (applieditem == null)
                    {
                        return new StockResponse(false, "not found", null);
                    }
                    if (uploadedpoitem == null)
                    {
                        return new StockResponse(false, "not found", null);
                    }
                    //var quantityexists = await scopedcontext.UploadPOFile.Where(y => y.ItemName == applieditem.itemName).FirstOrDefaultAsync();
                    //if (quantityexists == null)
                    //{
                    //    return new StockResponse(false, "item does not exist", null);

                    //}
                    //if (quantityexists.AvailableStock < applieditem.Quantity)
                    //{
                    //    applieditem.ApprovedStatus = "Failed";
                    //    applieditem.RejectReason = "Not Available";


                    //       return new StockResponse(false, $"Note:You can only apply  from '{quantityexists.AvailableStock} and below'!!! ", applieditem);


                    //{
                    //    applieditem.ApprovedStatus = "Failed";
                    //    applieditem.RejectReason = "Not Available";

                    //    return new StockResponse(false, $"Note:You can only apply  from '{quantityexists.AvailableStock}'!!! ", null);

                    //}

                    if (pOApprovalvm.selectedOption == "Approve")
                    {
                        pOApprovalvm.RejectedReason = "";
                    }
                    var poApproval =new POApproval
                    {
                        selectedOption=pOApprovalvm.selectedOption,
                        RejectedReason=pOApprovalvm.RejectedReason,
                        AprrovedDate = DateTime.Now,
                        PONumber=pOApprovalvm.PONumber,


                    };
                    applieditem.DateCreated = pOApprovalvm.AprrovedDate;

                    if (pOApprovalvm.selectedOption == "Reject")
                    {
                        applieditem.CaptureStatus = "Incomplete";
                        uploadedpoitem.CaptureStatus = "Incomplete";
                        poApproval.ApprovalStatus = "Incomplete";
                        scopedcontext.Update(applieditem);
                        scopedcontext.Update(uploadedpoitem);

                    }
                    else
                    {
                        applieditem.CaptureStatus = "Complete";
                        uploadedpoitem.CaptureStatus = "Complete";
                        poApproval.ApprovalStatus = "Incomplete";
                        scopedcontext.Update(applieditem);
                        scopedcontext.Update(uploadedpoitem);
                    }
                    await scopedcontext.AddAsync(poApproval);
                
                    await scopedcontext.SaveChangesAsync();

                    
                    return new StockResponse(true, "Successfully updated", poApproval);
                }
            }
            catch (Exception ex)
            {
                return new StockResponse(false, ex.Message, null);
            }
        }
        public async Task<StockResponse> GetPOByStatusPending()
        {
            try
            {
                using (var scope = _serviceScopeFactory.CreateScope())
                {

                    var scopedcontext = scope.ServiceProvider.GetRequiredService<DragonFlyContext>();
                    var supplierexists = await scopedcontext.PurchaseOrderss.Where(u => u.CaptureStatus == "Pending").OrderByDescending(x => x.DateCreated).ToListAsync();
                    if (supplierexists == null)
                    {
                        return new StockResponse(false, "not found", null);
                    }
                    return new StockResponse(true, "Queried successfully", supplierexists);
                }
            }
            catch (Exception ex)
            {
                return new StockResponse(false, ex.Message, null);
            }
        }
        public async Task<StockResponse> GetAllPOSWithStatusComplete()
        {
            try
            {
                using (var scope = _serviceScopeFactory.CreateScope())
                {
                    var scopedcontext = scope.ServiceProvider.GetRequiredService<DragonFlyContext>();

                    var allpos = await scopedcontext.PurchaseOrderss
                        .Where(x => x.CaptureStatus == "Complete" && x.DeliveryStatus == "Incomplete")
                        .ToListAsync();

                    if (allpos.Count == 0)  // Check for empty list using Count instead of null check
                    {
                        return new StockResponse(false, "No POs with the specified status combination", null);
                    }

                    return new StockResponse(true, "Successfully queried", allpos);
                }
            }
            catch (Exception ex)
            {
                return new StockResponse(false, ex.Message, null);
            }
        }
        public async Task<StockResponse> GetPONumberbyNumber(string POnumber)
        {
            try
            {
                using (var scope = _serviceScopeFactory.CreateScope())
                {

                    var scopedcontext = scope.ServiceProvider.GetRequiredService<DragonFlyContext>();
                    var supplierexists = await scopedcontext.PurchaseOrderss.Where(u => u.PONumber == POnumber).OrderByDescending(x => x.DateCreated).ToListAsync();
                    if (supplierexists == null)
                    {
                        return new StockResponse(false, "not found", null);
                    }
                    return new StockResponse(true, "Queried successfully", supplierexists);
                }
            }
            catch (Exception ex)
            {
                return new StockResponse(false, ex.Message, null);
            }
        }



    }
}
    


    



    


