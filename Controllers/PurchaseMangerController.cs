using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using WineStore.WebSite.Models;
using System.Threading.Tasks;
using System.Net.Http;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using WineStore.WebSite.Models.Admin;

namespace WineStore.WebSite.Controllers
{

    public class PurchaseManagerController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public PurchaseManagerController(IHttpClientFactory httpClientFactory, IHttpContextAccessor httpContextAccessor)
        {
            _httpClient = httpClientFactory.CreateClient("MyHttpClient");
            _httpContextAccessor = httpContextAccessor;
        }





        public async Task<IActionResult> Index()
        {
            try
            {
                // Retrieve auth token from session
                string authToken = _httpContextAccessor.HttpContext.Session.GetString("AuthToken");

                // Check if auth token exists
                if (string.IsNullOrEmpty(authToken))
                {
                    // Redirect to login page or handle unauthorized access
                    return RedirectToAction("Index", "Login");
                }

                // Set auth token in request header
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authToken);

                // Send GET request to API
                HttpResponseMessage response = await _httpClient.GetAsync("/api/Admin/shops");

                // Handle response
                if (response.IsSuccessStatusCode)
                {
                    // Read response content
                    string responseData = await response.Content.ReadAsStringAsync();

                    // Deserialize JSON response into list of Shop objects
                    List<ShopViewModel> shops = JsonConvert.DeserializeObject<List<ShopViewModel>>(responseData);

                    // Pass shops data to view
                    return View(shops);
                }
                else
                {
                    // Handle unsuccessful response
                    return View("Error");
                }
            }
            catch
            {
                // Handle exceptions
                return View("Error");
            }
        }
    }
}
