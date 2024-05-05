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

namespace WineStore.WebSite.Controllers
{

    public class AdminController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AdminController(IHttpClientFactory httpClientFactory, IHttpContextAccessor httpContextAccessor)
        {
            _httpClient = httpClientFactory.CreateClient("MyHttpClient");
            _httpContextAccessor = httpContextAccessor;

            string authToken = _httpContextAccessor.HttpContext.Session.GetString("AuthToken");
            // Set auth token in request header
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authToken);
        }

        //public async Task<IActionResult> Index()
        //{
        //    ApiManager apiManager = new ApiManager(_httpClient);

        //    // Example 1: Call API with input and output types
        //    var input1 = new { };
        //    var output1 = await apiManager.CallApiAsync<dynamic, List<ShopViewModel>>("/api/Admin/shops", input1, System.Web.Mvc.HttpVerbs.Get);


        //    return View("Shop", output1);

        //}
        public async Task<IActionResult> Index()
        {
            var model = new AdminHomeViewModel
            {
                UserName = _httpContextAccessor.HttpContext.Session.GetString("UserName"),
                NavigationLinkViewModel = new List<NavigationLinkViewModel>
                {
                    new NavigationLinkViewModel
                    {
                        Text = "Shop",
                        Area = "",
                        Controller = "Admin",
                        Action = "GoToShops",
                         Icon="fa fa-shopping-cart",
                    },
                    new NavigationLinkViewModel
                    {
                        Text = "Customers",
                        Area = "",
                        Controller = "Admin",
                        Action = "GoToCustomersPage",
                        Icon="fa fa-users",

                    },
                    new NavigationLinkViewModel
                    {
                        Text = "Suppliers",
                        Area = "",
                        Controller = "Admin",
                        Action = "Logout",
                        Icon="fa fa-industry",
                    },
                    new NavigationLinkViewModel
                    {
                        Text = "Chart of Accounts",
                        Area = "",
                        Controller = "Admin",
                        Action = "Logout",
                        Icon="fa fa-book",
                    },
                    new NavigationLinkViewModel
                    {
                        Text = "Add Employees",
                        Area = "",
                        Controller = "Admin",
                        Action = "Logout",
                         Icon="fa fa-user-plus",
                    }
                }
            };

            return View("Index", model);
        }

        public async Task<IActionResult> GoToCustomersPage()
        {

            return RedirectToAction("Index", "Customer");
        }



      


    }
}
