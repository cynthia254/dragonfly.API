using ClosedXML.Excel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using OfficeOpenXml;
using PayhouseDragonFly.CORE.ConnectorClasses.Response.StockResponse;
using PayhouseDragonFly.CORE.DTOs.Stock;
using PayhouseDragonFly.CORE.DTOs.Stock.Invoicing_vm;
using PayhouseDragonFly.CORE.Models.Stock;
using PayhouseDragonFly.CORE.Models.Stock.Invoicing;
using PayhouseDragonFly.INFRASTRUCTURE.DataContext;
using PayhouseDragonFly.INFRASTRUCTURE.Services.ExtraServices;
using PayhouseDragonFly.INFRASTRUCTURE.Services.IServiceCoreInterfaces.IStockServices;
using System.Text;

namespace PayhouseDragonFly.INFRASTRUCTURE.Services.StockServices
{
    public class StockServices : IStockServices
    {
        private readonly DragonFlyContext _dragonFlyContext;
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly IEExtraServices _extraServices;
        public StockServices(DragonFlyContext dragonFlyContext, IServiceScopeFactory serviceScopeFactory, IEExtraServices extraServices)
        {
            _dragonFlyContext = dragonFlyContext;
            _serviceScopeFactory = serviceScopeFactory;
            _extraServices = extraServices;



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
                if(addItemvm.BrandName=="")
                {
                    return new StockResponse(false, "Kindly provide brand name", null);

                }
                if (addItemvm.Currency=="")
                {
                    return new StockResponse(false, "Kindly provide currency", null);
                }
                if (addItemvm.IndicativePrice < 0)
                {
                    return new StockResponse(false, "Kindly provide indicative price",null);
                }
                if(addItemvm.ReOrderLevel < 0)
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



                    var itemexists = await scopedcontext.AddItem.Where(x => x.ItemName == addItemvm.ItemName && x.BrandName==addItemvm.BrandName).FirstOrDefaultAsync();

                    if (itemexists != null)
                    {
                        return new StockResponse(false, $" Item  '{addItemvm.BrandName}-{addItemvm.ItemName}' already exist", null);
                    }
                   
                    var itemclass = new AddItem
                    {
                        ItemName = addItemvm.ItemName,
                        Category=addItemvm.Category,
                        Currency=addItemvm.Currency,
                        IndicativePrice=addItemvm.IndicativePrice,
                        ReOrderLevel=addItemvm.ReOrderLevel,
                        BrandName=addItemvm.BrandName,
                        ItemDescription=addItemvm.ItemDescription,
                        
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
                        ReorderRequired=addStockvm.ReorderRequired,


                    };


                    itemclass.OpeningStock = addStockvm.Quantity;
                    itemclass.AvailableStock = addStockvm.Quantity;

                    if (itemclass.AvailableStock > addStockvm.ReOrderLevel)
                    {
                        itemclass.Status = "Good";
                    }
                    else if (itemclass.AvailableStock < addStockvm.ReOrderLevel && itemclass.AvailableStock > 0 ||itemclass.AvailableStock==addStockvm.ReOrderLevel)
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
                    else if (lastupdate.AvailableStock < lastupdate.ReOrderLevel && lastupdate.AvailableStock>0)
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
            catch (Exception ex) {
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
                    var allstock = await scopedcontext.AddStock.OrderByDescending(x=>x.DateAdded).ToListAsync();

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
                            ReorderRequired=stock.ReorderRequired,
                            StockOut=stock.StockOut,
                            TotalReturnedStock = stock.TotalReturnedStock,
                            StockIn=stock.StockIn,
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
                    if (  itemexists.AvailableStock == 0)
                    {
                        return new StockResponse(false, "No available stock please restock first....", null);
                    }
                    if(itemexists.AvailableStock < itemclass.Quantity)
                    {
                        return new StockResponse(false, $"Note:You can only stockOut from '{itemexists.AvailableStock}'!!! ",null);
                    }
                    itemexists.AvailableStock -= addSalesOrdersVm.Quantity;

                    if (itemexists.AvailableStock > itemexists.ReOrderLevel)
                    {
                        itemexists.Status = "Good";
                    }
                    else if (itemexists.AvailableStock < itemexists.ReOrderLevel && itemexists.AvailableStock > 0 ||itemexists.AvailableStock==itemexists.ReOrderLevel)
                    {
                        itemexists.Status = "Low";
                    }
                    else
                    {
                        itemexists.Status = "Out";
                    }
                    if(itemexists.AvailableStock < itemexists.ReOrderLevel || itemexists.AvailableStock == itemexists.ReOrderLevel)
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

            return new StockResponse(true,"Success",null);
           




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
                        EF.Functions.Like(u.ReorderRequired,$"%{search_query}%")
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
                        EF.Functions.Like(u.Comments, $"%{search_query}%")||
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
                    if (editItemvm.IndicativePrice <0)
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
                        CategoryDesc=addCategoryvm.CategoryDesc,
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



                    var itemexists = await scopedcontext.StockIn.Where(x => x.InvoiceDate == stockInvm.InvoiceDate && x.SupplierName==stockInvm.SupplierName).FirstOrDefaultAsync();

                    if (itemexists != null)
                    {
                        return new StockResponse(false, $" Invoice  already exist... ", null);
                    }

                    var lpo_no_obj =  GetLpo_Number().Result;
                    var lponumber = "LPO-" + lpo_no_obj;
                    var invoice_no_obj = GetInvoiceNumber().Result;
                    var invoicenumber = "INV-" + invoice_no_obj;
                    var itemclass = new StockIn
                    {
                        SupplierName = stockInvm.SupplierName,
                        InvoiceDate = stockInvm.InvoiceDate,
                        LPODate = stockInvm.LPODate,
                        LPONumber = lponumber,
                        InvoiceNumber=invoicenumber,
                        Status="Incomplete",



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

                newvalue.LpoNo=1;
                await _dragonFlyContext.AddAsync(newvalue);
                await _dragonFlyContext.SaveChangesAsync();
                return newvalue.LpoNo ;
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
                    var allinvoice = await scopedcontext.StockIn.OrderByDescending(y=>y.StockInDate).ToListAsync();

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
                if (addBatchDetailsvm.Quantity <0)
                {
                    return new StockResponse(false, "Kindly provide quantity", null);
                }
                if (addBatchDetailsvm.Warranty <0)
                {
                    return new StockResponse(false, "Kindly provide warranty", null);
                }
                using (var scope = _serviceScopeFactory.CreateScope())
                {
                    var scopedcontext = scope.ServiceProvider.GetRequiredService<DragonFlyContext>();
                    var invoiceexists = await scopedcontext.StockIn
                 .Where(y => y.InvoiceNumber == addBatchDetailsvm.InvoiceNumber ).OrderByDescending(y=>y.InvoiceNumber).FirstOrDefaultAsync();
                    if (invoiceexists == null)
                    {
                        return new StockResponse(false, "Invoice Number does not exist", null);
                    }

                    var loggedinuserobject = await _extraServices.LoggedInUser();

                    var userEmail = loggedinuserobject.FirstName + ' '   +loggedinuserobject.LastName;

                    if (loggedinuserobject == null)
                    {

                        return new StockResponse(false, "user not logged in. login again", null);

                    }



                  
                    var itemNameexists = await scopedcontext.AddItem
                   .Where(y => y.ItemName == addBatchDetailsvm.ItemName ).FirstOrDefaultAsync();
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
                        Quantity=addBatchDetailsvm.Quantity,
                        WarrantyStartDate=addBatchDetailsvm.WarrantyStartDate,
                        UpdatedBy= userEmail,
                        UpdatedOn=addBatchDetailsvm.UpdatedOn,
                        InvoiceNumber=addBatchDetailsvm.InvoiceNumber ,
                        BrandName=addBatchDetailsvm.BrandName,
                       





                    };
                    var itemassigned = await scopedcontext.AddProductDetails.Where(x => x.ItemID == itemclass.InvoiceLineId).FirstOrDefaultAsync();
                    
                    if (itemassigned != null)
                    {
                        itemassigned.ProductStatus = itemclass.Status;
                        return new StockResponse(false, "ITEM ALREADY COMPLETE", null);
                    }
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
                    if (itemclass.CategoryName == "Product") { 
                    var new_numb = 0;

                      

                        while (new_numb < itemclass.Quantity)
                        {
                            new_numb++;
                            var new_numbering = new ProductNumbering
                            {

                                NumberValue = new_numb,
                                Reference_Number = itemclass.Reference_Number,
                                Type = "Product",
                                Status="UNASSIGNED"


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
                    var allinvoice = await scopedcontext.InvoiceLinesDetails.OrderByDescending(y=>y.UpdatedOn).ToListAsync();

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
                if (addProductDetailvm.IMEI1 < 0)
                {

                    return new StockResponse(false, "Kindly provide  IMEI1 details ", null);
                }
                if (addProductDetailvm.IMEI2 < 0)
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

                    if (addProductDetailvm.Product_No <= 0 || addProductDetailvm.Product_No == null)
                    {
                        return new StockResponse(false, "Kindly fill in the item number ", null);

                    }

                    var all_numbers = await _dragonFlyContext.ProductNumbering
                        .Where(y => y.Reference_Number == addProductDetailvm.reference_number).Select(s =>  s.NumberValue).ToListAsync();
                    var max_number = all_numbers.Max();    

                    if(addProductDetailvm.Product_No> max_number)
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


                    
                    if (addProductDetailvm.Product_No== max_number)
                    {
                        var Invoceive_item_exists = await scopedcontext.InvoiceLinesDetails.
                            Where(y => y.InvoiceLineId == itemclass.ItemID).FirstOrDefaultAsync();

                        if (Invoceive_item_exists == null)
                            return new StockResponse(false, "No invice item found", null);
                        Invoceive_item_exists.Status = "Complete";

                        
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
                    var paymentref = "PH_invoice_" +  GenerateReferenceNumber(length);
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
                    var string_resp=  GetGeneratedref().Result;

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
                    var supplierexists = await scopedcontext.InvoiceLinesDetails.Where(u => u.InvoiceNumber == invoiceNumber).OrderByDescending(u=>u.UpdatedOn).ToListAsync();
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
        public async Task<StockResponse> GetProductDetailsbyid(int itemID)
        {
            try
            {
                using (var scope = _serviceScopeFactory.CreateScope())
                {

                    var scopedcontext = scope.ServiceProvider.GetRequiredService<DragonFlyContext>();
                    var supplierexists = await scopedcontext.AddProductDetails.Where(u => u.ItemID == itemID).ToListAsync();
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

        public async Task<StockResponse> GetProduct_Numbers_ByReference( string reference)
        {
            


            try
            {

                var allnumberings = await _dragonFlyContext.ProductNumbering
                    .Where(y => y.Status == "UNASSIGNED" && y.Reference_Number == reference).ToListAsync();
                return new StockResponse(true, "successfully queried", allnumberings);
            }
            catch(Exception ex){

             return new StockResponse(false,ex.Message, null);
            }
        }

        public async Task<StockResponse> GetProduvctLineyId(int product_line_id)
        {
            try
            {
                using (var scope = _serviceScopeFactory.CreateScope())
                {

                    var scopedcontext = scope.ServiceProvider.GetRequiredService<DragonFlyContext>();
                    var product_exist = await scopedcontext.InvoiceLinesDetails.Where(u => u.InvoiceLineId == product_line_id).FirstOrDefaultAsync();
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
                        EF.Functions.Like(u.ItemName, $"%{search_query}%")||
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
                        EF.Functions.Like(u.CategoryName, $"%{search_query}%")||
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
        public async Task <StockResponse> UploadingData(IFormFile file)
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

                                var products = new List<AddProductDetails>();
                                int rowCount = worksheet.Dimension.Rows;
                                for (int row = 2; row <= rowCount; row++) // Assuming the first row contains headers
                                {

                                    var product = new AddProductDetails

                                    {
                                        SerialNumber = worksheet.Cells[row, 1].Value?.ToString(),
                                        IMEI1 = int.Parse(worksheet.Cells[row, 2].Value?.ToString()),
                                        IMEI2 = int.Parse(worksheet.Cells[row, 3].Value?.ToString()),
                                       
                                    };

                                    products.Add(product);
                                   
                                }
                                foreach(var each_product in products)
                                {
                                    // Save the list of students to the database
                                    await scopedcontext.AddAsync(each_product);
                                    await scopedcontext.SaveChangesAsync();
                                }

                                

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

            return new StockResponse(false,"Invalid file or file format.",null);
        }
    }
}

