using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using WineStore.WebSite.Managers;
using WineStore.WebSite.Models.Admin;
using WineStore.WebSite.Models.PurchaseManager;
using Microsoft.AspNetCore.Mvc.Rendering;
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
                var output1 = await apiManager.CallApiAsync<dynamic, List<Inventory>>("/api/Inventory", input1, System.Web.Mvc.HttpVerbs.Get);
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
        public async Task<ActionResult> Create()
        {
            ApiManager apiManager = new ApiManager(_httpClient);

            var input1 = new { };
            var input2 = new { };
            var outputProductViewModel = await apiManager.CallApiAsync<dynamic, List<ProductViewModel>>("/api/Products", input1, System.Web.Mvc.HttpVerbs.Get);
            var outputShopViewModel = await apiManager.CallApiAsync<dynamic, List<ShopViewModel>>("/api/Shop", input2, System.Web.Mvc.HttpVerbs.Get);

            InventoryViewModel i1 = new InventoryViewModel();
            i1.ExistingProducts = new List<SelectListItem>();
            InventoryViewModel i2 = new InventoryViewModel();
            i2.ExistingShop = new List<SelectListItem>();
            foreach (var item in outputProductViewModel)
            {
                i1.ExistingProducts.Add(new SelectListItem() { Text = item.Product_Name, Value = item.Product_ID.ToString() });
            }
            foreach (var shopitem in outputShopViewModel)
            {
                i2.ExistingShop.Add(new SelectListItem() { Text = shopitem.Shop_Name, Value = shopitem.Shop_ID.ToString() });
            }
            InventoryViewModel output1 = new InventoryViewModel();
            output1.ExistingProducts = i1.ExistingProducts;
            output1.ExistingShop = i2.ExistingShop;
            return View("AddInventory", output1);
        }

        // POST: ShopController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AddInventory(IFormCollection collection)
        {
            try
            {
                var id = collection["Inventory_ID"];
                var product = collection["SelectedProducts"];
                var shop = collection["SelectedShop"];
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

            // Fetch specific inventory information
            var input1 = new { Id = id };
            var output1 = await apiManager.CallApiAsync<dynamic, InventoryViewModel>($"/api/Inventory/{id}", input1, System.Web.Mvc.HttpVerbs.Get);

            // Fetch product and shop information
            var input2 = new { };
            var outputProductViewModel = await apiManager.CallApiAsync<dynamic, List<ProductViewModel>>("/api/Products", input2, System.Web.Mvc.HttpVerbs.Get);
            var outputShopViewModel = await apiManager.CallApiAsync<dynamic, List<ShopViewModel>>("/api/Shop", input2, System.Web.Mvc.HttpVerbs.Get);

            // Populate the view model
            InventoryViewModel viewModel = new InventoryViewModel();
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

            // Set additional data obtained from specific inventory
            viewModel.Inventory_ID = output1.Inventory_ID; // Assuming InventoryViewModel has properties to hold this data
            viewModel.Quantity = output1.Quantity; // Assuming InventoryViewModel has properties to hold this data

            return View("EditInventory", viewModel);
        }

        // POST: ShopController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditInventory(int id, IFormCollection collection)
        {
            try
            {
                var Id = collection["Inventory_ID"];
                var product = collection["SelectedProducts"];
                var shop = collection["SelectedShop"];
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
        public async Task<ActionResult> Delete(int id)
        {
            ApiManager apiManager = new ApiManager(_httpClient);

            var input1 = new { Id = id };
            var output1 = await apiManager.CallApiAsync<dynamic, InventoryViewModel>($"/api/Inventory/{id}", input1, System.Web.Mvc.HttpVerbs.Get);
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
            return View("DeleteInventory",output1);
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
                var output1 = await apiManager.CallApiAsync<dynamic, InventoryViewModel>($"/api/Inventory/{id}", input1, System.Web.Mvc.HttpVerbs.Delete);

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
