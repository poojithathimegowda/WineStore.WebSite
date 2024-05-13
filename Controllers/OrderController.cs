using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using WineStore.WebSite.Managers;
using WineStore.WebSite.Models.Admin;
using WineStore.WebSite.Models.PurchaseManager;
using Microsoft.AspNetCore.Mvc.Rendering;
using WineStore.WebSite.Models.StoreManager;

namespace WineStore.WebSite.Controllers
{
    public class OrderController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public OrderController(IHttpClientFactory httpClientFactory, IHttpContextAccessor httpContextAccessor)
        {
            _httpClient = httpClientFactory.CreateClient("MyHttpClient");
            _httpContextAccessor = httpContextAccessor;

            string authToken = _httpContextAccessor.HttpContext.Session.GetString("AuthToken");
            // Set auth token in request header
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authToken);
        }
        // GET: ShopController
        public async Task<ActionResult> Index()
        {
            try
            {
                ApiManager apiManager = new ApiManager(_httpClient);
                var input1 = new { };
                var output1 = await apiManager.CallApiAsync<dynamic, List<Orders>>("/api/Orders", input1, System.Web.Mvc.HttpVerbs.Get);
                return View("ListOfOrders", output1);
            }
            catch
            {
                return View();
            }

        }

        // GET: ShopController/Details/5
        public async Task<ActionResult> Details(int id)
        {
            return View();
        }

        // GET: ShopController/Create
        public async Task<ActionResult> Create()
        {
            ApiManager apiManager = new ApiManager(_httpClient);

            var input1 = new { };
            var input2 = new { };
            var outputProductViewModel = await apiManager.CallApiAsync<dynamic, List<ProductViewModel>>("/api/Products", input1, System.Web.Mvc.HttpVerbs.Get);
            var outputShopViewModel = await apiManager.CallApiAsync<dynamic, List<ShopViewModel>>("/api/Shop", input2, System.Web.Mvc.HttpVerbs.Get);

            OrderViewModel o1 = new OrderViewModel();
            o1.ExistingProducts = new List<SelectListItem>();
            OrderViewModel o2 = new OrderViewModel();
            o2.ExistingShop = new List<SelectListItem>();
            foreach (var item in outputProductViewModel)
            {
                o1.ExistingProducts.Add(new SelectListItem() { Text = item.Product_Name, Value = item.Product_ID.ToString() });
            }
            foreach (var shopitem in outputShopViewModel)
            {
                o2.ExistingShop.Add(new SelectListItem() { Text = shopitem.Shop_Name, Value = shopitem.Shop_ID.ToString() });
            }
            OrderViewModel output1 = new OrderViewModel();
            output1.ExistingProducts = o1.ExistingProducts;
            output1.ExistingShop = o2.ExistingShop;
            return View("AddOrder", output1);
        }

        // POST: ShopController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AddOrder(IFormCollection collection)
        {
            try
            {
                var id = collection["Order_ID"];
                var product = collection["SelectedProducts"];
                var shop = collection["SelectedShop"];
                var quantity = collection["Quantity"];
                var amt = collection["Total_Amount"];
                var date = collection["Order_Date"];
                var orderViewModel = new OrderViewModel
                {
                    Order_ID = Convert.ToInt32(id),
                    Product_ID = Convert.ToInt32(product),
                    Shop_ID = Convert.ToInt32(shop),
                    Quantity = Convert.ToInt32(quantity),

                    Total_Amount = Convert.ToDecimal(amt),
                    Order_Date = Convert.ToDateTime(date)
                };

                // Call the API with the CustomersViewModel object
                ApiManager apiManager = new ApiManager(_httpClient);
                var output1 = await apiManager.CallApiAsync<OrderViewModel, OrderViewModel>($"/api/Orders", orderViewModel, System.Web.Mvc.HttpVerbs.Post);

                return RedirectToAction("Index");

            }
            catch
            {
                return View();
            }
        }

        // GET: ShopController/Edit/5
        public async Task<ActionResult> Edit(int id)
        {
            ApiManager apiManager = new ApiManager(_httpClient);

            // Fetch specific order information
            var input1 = new { Id = id };
            var output1 = await apiManager.CallApiAsync<dynamic, OrderViewModel>($"/api/Orders/{id}", input1, System.Web.Mvc.HttpVerbs.Get);

            // Fetch product and shop information
            var input2 = new { };
            var outputProductViewModel = await apiManager.CallApiAsync<dynamic, List<ProductViewModel>>("/api/Products", input2, System.Web.Mvc.HttpVerbs.Get);
            var outputShopViewModel = await apiManager.CallApiAsync<dynamic, List<ShopViewModel>>("/api/Shop", input2, System.Web.Mvc.HttpVerbs.Get);

            // Populate the view model
            OrderViewModel viewModel = new OrderViewModel();
            viewModel.ExistingProducts = new List<SelectListItem>();
            viewModel.ExistingShop = new List<SelectListItem>();

            // Add product and shop information to the view model
            foreach (var item in outputProductViewModel)
            {
                viewModel.ExistingProducts.Add(new SelectListItem() { Text = item.Product_Name, Value = item.Product_ID.ToString() });
            }
            foreach (var item in outputShopViewModel)
            {
                viewModel.ExistingShop.Add(new SelectListItem() { Text = item.Shop_Name, Value = item.Shop_ID.ToString() });
            }

            // Set additional data obtained from specific order
            viewModel.Order_ID = output1.Order_ID; // Assuming OrderViewModel has properties to hold this data
            viewModel.Quantity = output1.Quantity; // Assuming OrderViewModel has properties to hold this data
            viewModel.Total_Amount = output1.Total_Amount; // Assuming OrderViewModel has properties to hold this data
            viewModel.Order_Date = output1.Order_Date; // Assuming OrderViewModel has properties to hold this data

            return View("EditOrders", viewModel);
        }


        // POST: ShopController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditOrders(int id, IFormCollection collection)
        {
            try
            {
                var Id = collection["Order_ID"];
                var product = collection["SelectedProducts"];
                var shop = collection["SelectedShop"];
                var quantity = collection["Quantity"];
                var amt = collection["Total_Amount"];
                var date = collection["Order_Date"];
                var orderViewModel = new OrderViewModel
                {
                    Order_ID = Convert.ToInt32(Id),
                    Product_ID = Convert.ToInt32(product),
                    Shop_ID = Convert.ToInt32(shop),
                    Quantity = Convert.ToInt32(quantity),

                    Total_Amount = Convert.ToDecimal(amt),
                    Order_Date = Convert.ToDateTime(date)
                };


                // Call the API with the CustomersViewModel object
                ApiManager apiManager = new ApiManager(_httpClient);

                var output1 = await apiManager.CallApiAsync<dynamic, OrderViewModel>($"/api/Orders/{Id}", orderViewModel, System.Web.Mvc.HttpVerbs.Put);

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: ShopController/Delete/5
        public async Task<ActionResult> Delete(int id)
        {
            ApiManager apiManager = new ApiManager(_httpClient);

            var input1 = new { Id = id };
            var output1 = await apiManager.CallApiAsync<dynamic, OrderViewModel>($"/api/Orders/{id}", input1, System.Web.Mvc.HttpVerbs.Get);
            var outputProductViewModel = await apiManager.CallApiAsync<dynamic, List<ProductViewModel>>("/api/Products", input1, System.Web.Mvc.HttpVerbs.Get);
            var outputShopViewModel = await apiManager.CallApiAsync<dynamic, List<ShopViewModel>>("/api/Shop", input1, System.Web.Mvc.HttpVerbs.Get);

            output1.ExistingProducts = new List<SelectListItem>();

            foreach (var item in outputProductViewModel)
            {
                output1.ExistingProducts.Add(new SelectListItem() { Text = item.Product_Name, Value = item.Product_ID.ToString() });
            }
            output1.ExistingShop = new List<SelectListItem>();

            foreach (var item in outputShopViewModel)
            {
                output1.ExistingShop.Add(new SelectListItem() { Text = item.Shop_Name, Value = item.Shop_ID.ToString() });
            }
            return View("DeleteOrder",output1);
        }

        // POST: ShopController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(int id, IFormCollection collection)
        {
            try
            {
                ApiManager apiManager = new ApiManager(_httpClient);

                var input1 = new { Id = id };
                var output1 = await apiManager.CallApiAsync<dynamic, OrderViewModel>($"/api/Orders/{id}", input1, System.Web.Mvc.HttpVerbs.Delete);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
