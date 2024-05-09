using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using WineStore.WebSite.Managers;
using WineStore.WebSite.Models.Admin;
using WineStore.WebSite.Models.PurchaseManager;

namespace WineStore.WebSite.Controllers
{
    public class InventoryController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public InventoryController(IHttpClientFactory httpClientFactory, IHttpContextAccessor httpContextAccessor)
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
                var output1 = await apiManager.CallApiAsync<dynamic, List<InventoryViewModel>>("/api/Inventory", input1, System.Web.Mvc.HttpVerbs.Get);
                return View("ListOfInventory", output1);
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
            return View("AddInventory");
        }

        // POST: ShopController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AddInventory(IFormCollection collection)
        {
            try
            {
                var id = collection["Inventory_ID"];
                var product = collection["Product_ID"];
                var shop = collection["Shop_ID"];
                var quantity = collection["Quantity"];

                var inventoryViewModel = new InventoryViewModel
                {
                    Inventory_ID = Convert.ToInt32(id),
                    Product_ID = Convert.ToInt32(product),
                    Shop_ID = Convert.ToInt32(shop),
                    Quantity = Convert.ToInt32(quantity)

                };

                // Call the API with the CustomersViewModel object
                ApiManager apiManager = new ApiManager(_httpClient);
                var output1 = await apiManager.CallApiAsync<InventoryViewModel, InventoryViewModel>($"/api/Inventory", inventoryViewModel, System.Web.Mvc.HttpVerbs.Post);

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
            var output1 = await apiManager.CallApiAsync<dynamic, InventoryViewModel>($"/api/Inventory/{id}", input1, System.Web.Mvc.HttpVerbs.Get);
            return View("EditInventory", output1);

        }

        // POST: ShopController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditInventory(int id, IFormCollection collection)
        {
            try
            {
                var Id = collection["Inventory_ID"];
                var product = collection["Product_ID"];
                var shop = collection["Shop_ID"];
                var quantity = collection["Quantity"];

                var inventoryViewModel = new InventoryViewModel
                {
                    Inventory_ID = Convert.ToInt32(Id),
                    Product_ID = Convert.ToInt32(product),
                    Shop_ID = Convert.ToInt32(shop),
                    Quantity = Convert.ToInt32(quantity)

                };
                // Call the API with the CustomersViewModel object
                ApiManager apiManager = new ApiManager(_httpClient);

                var output1 = await apiManager.CallApiAsync<dynamic, InventoryViewModel>($"/api/Inventory/{Id}", inventoryViewModel, System.Web.Mvc.HttpVerbs.Put);
                
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
