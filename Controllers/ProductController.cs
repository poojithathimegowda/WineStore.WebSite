using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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
                var output1 = await apiManager.CallApiAsync<dynamic, List<Product>>("/api/Products", input1, System.Web.Mvc.HttpVerbs.Get);
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
        public async Task<ActionResult>  Create()
        {
            ApiManager apiManager = new ApiManager(_httpClient);
            var input1 = new { };
            var outputSupplierViewModel = await apiManager.CallApiAsync<dynamic, List<SupplierViewModel>>("/api/Suppliers", input1, System.Web.Mvc.HttpVerbs.Get);



            ProductViewModel p1 = new ProductViewModel();


            p1.ExistingSuppliers = new List<SelectListItem>();
   
            foreach (var supplieritem in outputSupplierViewModel)
            {
                p1.ExistingSuppliers.Add(new SelectListItem() { Text = supplieritem.Supplier_Name, Value = supplieritem.Supplier_ID.ToString() });
            }

            return View("AddProduct", p1);
        }

        // POST: ShopController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AddProduct(IFormCollection collection)
        {
            try
            {
              
                var name = collection["Product_Name"];
                var description = collection["Description"];
                var price = collection["Price"];
                var supplier = collection["SelectedSupplier"];
                // Create a CustomersViewModel object
                var product = new Product()
                {
                   
                    Product_Name = name,
                    Description = description,
                    Price = Convert.ToDecimal(price),
                    Supplier_ID = Convert.ToInt32(supplier)
                };

               

                // Call the API with the CustomersViewModel object
                ApiManager apiManager = new ApiManager(_httpClient);
                var output1 = await apiManager.CallApiAsync<Product, dynamic>($"/api/Products", product, System.Web.Mvc.HttpVerbs.Post);

                return RedirectToAction("Index");

            }
            catch(Exception exp)
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


            var outputSupplierViewModel = await apiManager.CallApiAsync<dynamic, List<SupplierViewModel>>("/api/Suppliers", input1, System.Web.Mvc.HttpVerbs.Get);


            output1.ExistingSuppliers = new List<SelectListItem>();

            foreach (var supplieritem in outputSupplierViewModel)
            {
                output1.ExistingSuppliers.Add(new SelectListItem() { Text = supplieritem.Supplier_Name, Value = supplieritem.Supplier_ID.ToString() });
            }


         

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
                var supplier = collection["SelectedSupplier"];
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
        public async Task<ActionResult> Delete(int id)
        {
            ApiManager apiManager = new ApiManager(_httpClient);

            var input1 = new { Id = id };
            var output1 = await apiManager.CallApiAsync<dynamic, ProductViewModel>($"/api/Products/{id}", input1, System.Web.Mvc.HttpVerbs.Get);


            var outputSupplierViewModel = await apiManager.CallApiAsync<dynamic, List<SupplierViewModel>>("/api/Suppliers", input1, System.Web.Mvc.HttpVerbs.Get);





            output1.ExistingSuppliers = new List<SelectListItem>();

            foreach (var supplieritem in outputSupplierViewModel)
            {
                output1.ExistingSuppliers.Add(new SelectListItem() { Text = supplieritem.Supplier_Name, Value = supplieritem.Supplier_ID.ToString() });
            }




           
            return View("DeleteProduct", output1);
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
                var output1 = await apiManager.CallApiAsync<dynamic, ProductViewModel>($"/api/Products/{id}", input1, System.Web.Mvc.HttpVerbs.Delete);


                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
