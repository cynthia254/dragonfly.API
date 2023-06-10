using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PayhouseDragonFly.CORE.ConnectorClasses.Response;
using PayhouseDragonFly.CORE.ConnectorClasses.Response.BseResponse;
using PayhouseDragonFly.CORE.ConnectorClasses.Response.StockResponse;
using PayhouseDragonFly.CORE.DTOs.RegisterVms;
using PayhouseDragonFly.CORE.DTOs.Stock;
using PayhouseDragonFly.CORE.DTOs.Ticketsvms;
using PayhouseDragonFly.CORE.DTOs.userStatusvm;
using PayhouseDragonFly.CORE.Models.Roles;
using PayhouseDragonFly.CORE.Models.statusTable;
using PayhouseDragonFly.CORE.Models.Stock;
using PayhouseDragonFly.INFRASTRUCTURE.DataContext;
using PayhouseDragonFly.INFRASTRUCTURE.Migrations;
using PayhouseDragonFly.INFRASTRUCTURE.Services.IServiceCoreInterfaces.IStockServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Org.BouncyCastle.Asn1.Cmp.Challenge;

namespace PayhouseDragonFly.INFRASTRUCTURE.Services.StockServices
{
    public class StockServices : IStockServices
    {
        private readonly DragonFlyContext _dragonFlyContext;
        private readonly IServiceScopeFactory _serviceScopeFactory;
        public StockServices(DragonFlyContext dragonFlyContext, IServiceScopeFactory serviceScopeFactory)
        {
            _dragonFlyContext = dragonFlyContext;
            _serviceScopeFactory = serviceScopeFactory;


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
                using (var scope = _serviceScopeFactory.CreateScope())
                {
                    var scopedcontext = scope.ServiceProvider.GetRequiredService<DragonFlyContext>();



                    var itemexists = await scopedcontext.AddItem.Where(x => x.ItemName == addItemvm.ItemName).FirstOrDefaultAsync();

                    if (itemexists != null)
                    {
                        return new StockResponse(false, $" Item  '{addItemvm.ItemName}' already exist, if  must add a similar item kindly change the " +
                             $"latter cases from lower to upper and vice versa depending on the existing  item . The existsing item is '{itemexists}' with item id {itemexists.ItemID} ", null);
                    }
                    var itemclass = new AddItem
                    {
                        ItemName = addItemvm.ItemName,
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


                    };


                    itemclass.OpeningStock = addStockvm.Quantity;
                    itemclass.AvailableStock = addStockvm.Quantity;

                    if (itemclass.AvailableStock > addStockvm.ReOrderLevel)
                    {
                        itemclass.Status = "Good";
                    }
                    else if (itemclass.AvailableStock < addStockvm.ReOrderLevel && itemclass.AvailableStock > 0)
                    {
                        itemclass.Status = "Low";
                    }
                    else
                    {
                        itemclass.Status = "Out";
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
                    else if (lastupdate.AvailableStock < lastupdate.ReOrderLevel)
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
                    var allstock = await scopedcontext.AddStock.ToListAsync();

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
                        };
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
                    itemexists.AvailableStock -= addSalesOrdersVm.Quantity;
                    itemexists.Quantity -= addSalesOrdersVm.Quantity;
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
                    itemclass.Currency = itemexists.Currency;
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
                        EF.Functions.Like(u.Status, $"%{search_query}%") || EF.Functions.Like(Convert.ToString(u.DateAdded), $"%{search_query}%")

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
    









    }
}
