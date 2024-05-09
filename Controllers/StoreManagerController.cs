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
using WineStore.WebSite.Models.StoreManager;

namespace WineStore.WebSite.Controllers
{

    public class StoreManagerController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public StoreManagerController(IHttpClientFactory httpClientFactory, IHttpContextAccessor httpContextAccessor)
        {
            _httpClient = httpClientFactory.CreateClient("MyHttpClient");
            _httpContextAccessor = httpContextAccessor;

            string authToken = _httpContextAccessor.HttpContext.Session.GetString("AuthToken");
            // Set auth token in request header
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authToken);
        }
        public async Task<IActionResult> Index()
        {
            var model = new StoreManagerHomeViewModel
            {
                UserName = _httpContextAccessor.HttpContext.Session.GetString("UserName"),
                NavigationLinkViewModel = new List<NavigationLinkViewModel>
                {
                    new NavigationLinkViewModel
                    {
                        Text = "Orders",
                        Area = "",
                        Controller = "StoreManager",
                        Action = "GoToOrders",
                         Icon="fa-solid fa-shop",
                    },
                    new NavigationLinkViewModel
                    {
                        Text = "Expenses",
                        Area = "",
                        Controller = "StoreManager",
                        Action = "GoToExpenses",
                        Icon="fa-solid fa-money-bill",

                    }
                    
                }
            };

            return View("Index", model);
        }
       

        public async Task<IActionResult> GoToOrders()
        {

            return RedirectToAction("Index", "Order");
        }

        public async Task<IActionResult> GoToExpenses()
        {

            return RedirectToAction("Index", "Expense");
        }
        


    }
}
