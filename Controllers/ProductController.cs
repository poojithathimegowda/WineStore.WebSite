using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using WineStore.WebSite.Managers;
using WineStore.WebSite.Models.Admin;
using WineStore.WebSite.Models.PurchaseManager;

namespace WineStore.WebSite.Controllers
{
    public class ProductController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ProductController(IHttpClientFactory httpClientFactory, IHttpContextAccessor httpContextAccessor)
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
                var output1 = await apiManager.CallApiAsync<dynamic, List<ProductViewModel>>("/api/Products", input1, System.Web.Mvc.HttpVerbs.Get);
                return View("ListOfProducts", output1);
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
            return View("AddProduct");
        }

        // POST: ShopController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AddProduct(IFormCollection collection)
        {
            try
            {
                var id = collection["Product_ID"];
                var name = collection["Product_Name"];
                var description = collection["Description"];
                var price = collection["Price"];
                var supplier = collection["Supplier_ID"];
                // Create a CustomersViewModel object
                var productViewModel = new ProductViewModel
                {
                    Product_ID = Convert.ToInt32(id),
                    Product_Name = name,
                    Description = description,
                    Price= Convert.ToDecimal(price),
                    Supplier_ID = Convert.ToInt32(supplier)
                };

                // Call the API with the CustomersViewModel object
                ApiManager apiManager = new ApiManager(_httpClient);
                var output1 = await apiManager.CallApiAsync<ProductViewModel, ProductViewModel>($"/api/Products", productViewModel, System.Web.Mvc.HttpVerbs.Post);

                // Set success message
                //ViewBag.Message = "Customer details updated successfully.";
                //TempData["Message"] = "Customer details added successfully.";

                return RedirectToAction("Index");

            }
            catch
            {
                //TempData["Error"] = "An error occurred while adding customer details. Please try again.";
                return View();
            }
        }

        // GET: ShopController/Edit/5
        public async Task<ActionResult> Edit(int id)
        {
            ApiManager apiManager = new ApiManager(_httpClient);

            var input1 = new { Id = id };
            var output1 = await apiManager.CallApiAsync<dynamic, ProductViewModel>($"/api/Products/{id}", input1, System.Web.Mvc.HttpVerbs.Get);
            return View("EditProduct", output1);

        }

        // POST: ShopController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditProduct(int id, IFormCollection collection)
        {
            try
            {
                var Id = collection["Product_ID"];
                var name = collection["Product_Name"];
                var description = collection["Description"];
                var price = collection["Price"];
                var supplier = collection["Supplier_ID"];
                // Create a CustomersViewModel object
                var productViewModel = new ProductViewModel
                {
                    Product_ID = Convert.ToInt32(Id),
                    Product_Name = name,
                    Description = description,
                    Price = Convert.ToDecimal(price),
                    Supplier_ID = Convert.ToInt32(supplier)
                };

                // Call the API with the CustomersViewModel object
                ApiManager apiManager = new ApiManager(_httpClient);

                var output1 = await apiManager.CallApiAsync<dynamic, ProductViewModel>($"/api/Products/{Id}", productViewModel, System.Web.Mvc.HttpVerbs.Put);
                
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
