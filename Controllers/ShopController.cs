using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using WineStore.WebSite.Managers;
using WineStore.WebSite.Models.Admin;

namespace WineStore.WebSite.Controllers
{
    public class ShopController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ShopController(IHttpClientFactory httpClientFactory, IHttpContextAccessor httpContextAccessor)
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
                var output1 = await apiManager.CallApiAsync<dynamic, List<ShopViewModel>>("/api/Shop", input1, System.Web.Mvc.HttpVerbs.Get);
                return View("ListOfShops", output1);
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
            return View("AddShop");
        }

        // POST: ShopController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(IFormCollection collection)
        {
            try
            {
                //var id = collection["Shop_ID"];
                var name = collection["Shop_Name"];
                var address = collection["Location"];

                // Create a CustomersViewModel object
                var shopViewModel = new ShopViewModel
                {
                    //Shop_ID = Convert.ToInt32(id) , 
                    Shop_Name = name,
                    Location = address
                    
                };

                // Call the API with the CustomersViewModel object
                ApiManager apiManager = new ApiManager(_httpClient);
                var output1 = await apiManager.CallApiAsync<ShopViewModel, ShopViewModel>($"/api/Shop", shopViewModel, System.Web.Mvc.HttpVerbs.Post);

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
            var output1 = await apiManager.CallApiAsync<dynamic, ShopViewModel>($"/api/Shop/{id}", input1, System.Web.Mvc.HttpVerbs.Get);
            return View("EditShop", output1);
          
        }

        // POST: ShopController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditShop(int id, IFormCollection collection)
        {
            try
            {

                var Id = id;
                var name = collection["Shop_Name"];
                var address = collection["Location"];

                // Create a CustomersViewModel object
                var shopViewModel = new ShopViewModel
                {
                    Shop_ID = Convert.ToInt32(Id),
                    Shop_Name = name,
                    Location = address

                };

                // Call the API with the CustomersViewModel object
                ApiManager apiManager = new ApiManager(_httpClient);
              
                var output1 = await apiManager.CallApiAsync<dynamic, ShopViewModel>($"/api/Shop/{id}", shopViewModel, System.Web.Mvc.HttpVerbs.Put);
                // Set success message

                //TempData["Message"] = "Customer details updated successfully.";
                return RedirectToAction("Index");
            }
            catch
            {
                // Set success message
                //TempData["Message"] = "Something went wrong when updating the data.";

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
