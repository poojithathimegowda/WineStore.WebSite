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
using iTextSharp.text.pdf;
using iTextSharp.text;

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
                        Text = "Add Employees",
                        Area = "",
                        Controller = "Admin",
                        Action = "AddEmployee",
                         Icon="fa fa-user-plus",
                    },

                     new NavigationLinkViewModel
                    {
                        Text = "View Reports",
                        Area = "",
                        Controller = "Admin",
                        Action = "GoToReport",
                         Icon="fa fa-user-plus",
                    }
                }
            };

            return View("Index", model);
        }
       

        public async Task<IActionResult> AddEmployee()
        {

            return RedirectToAction("Index", "Employee");
        }

        public async Task<IActionResult> GoToShops()
        {

            return RedirectToAction("Index", "Shop");
        }

      

        public async Task<IActionResult> GoToReport()
        {

            return RedirectToAction("Index", "Report");
        }


    }
}
