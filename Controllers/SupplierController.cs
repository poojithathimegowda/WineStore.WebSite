using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using WineStore.WebSite.Managers;
using WineStore.WebSite.Models.Admin;
using WineStore.WebSite.Models.PurchaseManager;

namespace WineStore.WebSite.Controllers
{
    public class SupplierController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public SupplierController(IHttpClientFactory httpClientFactory, IHttpContextAccessor httpContextAccessor)
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
                var output1 = await apiManager.CallApiAsync<dynamic, List<SupplierViewModel>>("/api/Suppliers", input1, System.Web.Mvc.HttpVerbs.Get);
                return View("ListOfSuppliers", output1);
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
            return View("AddSupplier");
        }

        // POST: ShopController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AddSupplier(IFormCollection collection)
        {
            try
            {

                var name = collection["Supplier_Name"];
                var contact = collection["Contact_Details"];

                // Create a CustomersViewModel object
                var supplierViewModel = new SupplierViewModel
                {

                    Supplier_Name = name,
                    Contact_Details = contact

                };

                // Call the API with the CustomersViewModel object
                ApiManager apiManager = new ApiManager(_httpClient);
                var output1 = await apiManager.CallApiAsync<SupplierViewModel, SupplierViewModel>($"/api/Suppliers", supplierViewModel, System.Web.Mvc.HttpVerbs.Post);

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
            var output1 = await apiManager.CallApiAsync<dynamic, SupplierViewModel>($"/api/Suppliers/{id}", input1, System.Web.Mvc.HttpVerbs.Get);
            return View("EditSupplier", output1);

        }

        // POST: ShopController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditSupplier(int id, IFormCollection collection)
        {
            try
            {

                var Id = collection["Supplier_ID"];
                var name = collection["Supplier_Name"];
                var contact = collection["Contact_Details"];

                // Create a CustomersViewModel object
                var supplierViewModel = new SupplierViewModel
                {
                    Supplier_ID = Convert.ToInt32(Id),
                    Supplier_Name = name,
                    Contact_Details = contact

                };

                // Call the API with the CustomersViewModel object
                ApiManager apiManager = new ApiManager(_httpClient);

                var output1 = await apiManager.CallApiAsync<dynamic, ShopViewModel>($"/api/Suppliers/{id}", supplierViewModel, System.Web.Mvc.HttpVerbs.Put);
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
