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
using WineStore.WebSite.Managers;
using WineStore.WebSite.Models.PurchaseManager;

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

            string authToken = _httpContextAccessor.HttpContext.Session.GetString("AuthToken");
            // Set auth token in request header
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authToken);
        }
        public async Task<IActionResult> Index()
        {
            var model = new PurchaseManagerHomeViewModel
			{
                UserName = _httpContextAccessor.HttpContext.Session.GetString("UserName"),
                NavigationLinkViewModel = new List<NavigationLinkViewModel>
                {
                    new NavigationLinkViewModel
                    {
                        Text = "Suppliers",
                        Area = "",
                        Controller = "PurchaseManager",
                        Action = "GoToSuppliers",
                         Icon="fa fa-users",
                    },
                    new NavigationLinkViewModel
                    {
                        Text = "Products",
                        Area = "",
                        Controller = "PurchaseManager",
                        Action = "GoToProducts",
                        Icon="fa fa-industry",

                    },
                    new NavigationLinkViewModel
                    {
                        Text = "Inventory",
                        Area = "",
                        Controller = "PurchaseManager",
                        Action = "GoToInventory",
                        Icon="fa-solid fa-warehouse",
                    }
                   
                }
            };

            return View("Index", model);
        }
       

        public async Task<IActionResult> GoToSuppliers()
        {

            return RedirectToAction("Index", "Supplier");
        }

        public async Task<IActionResult> GoToProducts()
        {

            return RedirectToAction("Index", "Product");
        }
        public async Task<IActionResult> GoToInventory()
        {

            return RedirectToAction("Index", "Inventory");
        }


    }
}
