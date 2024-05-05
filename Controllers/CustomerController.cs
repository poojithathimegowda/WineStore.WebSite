using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography.X509Certificates;
using WineStore.WebSite.Managers;
using WineStore.WebSite.Models.Admin;

namespace WineStore.WebSite.Controllers
{
    public class CustomerController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CustomerController(IHttpClientFactory httpClientFactory, IHttpContextAccessor httpContextAccessor)
        {
            _httpClient = httpClientFactory.CreateClient("MyHttpClient");
            _httpContextAccessor = httpContextAccessor;

            string authToken = _httpContextAccessor.HttpContext.Session.GetString("AuthToken");
            // Set auth token in request header
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authToken);
        }

        // GET: CustomerController
        public async Task<ActionResult> Index()
        {
            try
            {
                if (TempData.ContainsKey("Message"))
                {
                    if (!string.IsNullOrEmpty(TempData["Message"].ToString()))
                    {
                        ViewBag.Message = TempData["Message"].ToString();
                    }
                }

                ApiManager apiManager = new ApiManager(_httpClient);
                var input1 = new { };
                var output1 = await apiManager.CallApiAsync<dynamic, List<CustomersViewModel>>("/api/Admin/Customer", input1, System.Web.Mvc.HttpVerbs.Get);
                return View("ListOfCustomers", output1);
            }
            catch
            {
                return View();
            }
        }

        // GET: CustomerController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: CustomerController/Create
        public ActionResult Create()
        {
            return View("AddCustomer");
        }

        // POST: CustomerController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(IFormCollection collection)
        {
            try
            {
                var name = collection["Name"];
                var address = collection["Address"];
                var phoneNo = collection["PhoneNo"];
                var email = collection["Email"];

                // Create a CustomersViewModel object
                var customersViewModel = new CustomersViewModel
                {

                    Name = name,
                    Address = address,
                    PhoneNo = phoneNo,
                    Email = email
                };

                // Call the API with the CustomersViewModel object
                ApiManager apiManager = new ApiManager(_httpClient);
                var output1 = await apiManager.CallApiAsync<CustomersViewModel, CustomersViewModel>($"/api/Admin/Customer", customersViewModel, System.Web.Mvc.HttpVerbs.Post);

                // Set success message
                //ViewBag.Message = "Customer details updated successfully.";
                TempData["Message"] = "Customer details added successfully.";

                return RedirectToAction("Index");

            }
            catch
            {
                TempData["Error"] = "An error occurred while adding customer details. Please try again.";
                return View();
            }
        }

        // GET: CustomerController/Edit/5
        public async Task<ActionResult> Edit(int id)
        {
            try
            {
                ApiManager apiManager = new ApiManager(_httpClient);

                var input1 = new { Id = id };
                var output1 = await apiManager.CallApiAsync<dynamic, CustomersViewModel>($"/api/Admin/Customer/{id}", input1, System.Web.Mvc.HttpVerbs.Get);
                return View("EditCustomer", output1);
            }
            catch
            {
                return View();
            }
        }


        //POST: CustomerController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SaveEdit(int id, IFormCollection collection)
        {
            try
            {

                // Extract values from the IFormCollection
                var name = collection["Name"];
                var address = collection["Address"];
                var phoneNo = collection["PhoneNo"];
                var email = collection["Email"];

                // Create a CustomersViewModel object
                var customersViewModel = new CustomersViewModel
                {
                    Id = id,
                    Name = name,
                    Address = address,
                    PhoneNo = phoneNo,
                    Email = email
                };

                // Call the API with the CustomersViewModel object
                ApiManager apiManager = new ApiManager(_httpClient);
                var output1 = await apiManager.CallApiAsync<CustomersViewModel, CustomersViewModel>($"/api/Admin/Customer/{id}", customersViewModel, System.Web.Mvc.HttpVerbs.Put);

                // Set success message

                TempData["Message"] = "Customer details updated successfully.";
                return RedirectToAction("Index");
            }
            catch
            {
                // Set success message
                TempData["Message"] = "Something went wrong when updating the data.";

                return View();
            }
        }


        // GET: CustomerController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: CustomerController/Delete/5
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
