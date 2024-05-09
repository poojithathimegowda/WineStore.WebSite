using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using WineStore.WebSite.Managers;
using WineStore.WebSite.Models.Admin;
using WineStore.WebSite.Models.PurchaseManager;
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
                var output1 = await apiManager.CallApiAsync<dynamic, List<OrderViewModel>>("/api/Orders", input1, System.Web.Mvc.HttpVerbs.Get);
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
        public ActionResult Create()
        {
            return View("AddNewOrder");
        }

        // POST: ShopController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AddNewOrder(IFormCollection collection)
        {
            try
            {
                var id = collection["Order_ID"];
                var product = collection["Product_ID"];
                var shop = collection["Shop_ID"];
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

            var input1 = new { Id = id };
            var output1 = await apiManager.CallApiAsync<dynamic, OrderViewModel>($"/api/Orders/{id}", input1, System.Web.Mvc.HttpVerbs.Get);
            return View("EditOrders", output1);

        }

        // POST: ShopController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditOrders(int id, IFormCollection collection)
        {
            try
            {
                var Id = collection["Order_ID"];
                var product = collection["Product_ID"];
                var shop = collection["Shop_ID"];
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
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: ShopController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
