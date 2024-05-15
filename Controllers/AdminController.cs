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
                        Action = "DownloadPDF",
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

        public async Task<IActionResult> DownloadPDF()
        {
            MemoryStream memoryStream = new MemoryStream();
            Document document = new Document();
            PdfWriter.GetInstance(document, memoryStream);

            document.Open();
            document.Add(new Paragraph("Hello, World!"));
            document.Close();

            byte[] bytes = memoryStream.ToArray();
            memoryStream.Close();

            // Clear the response for proper PDF content
            Response.Clear();

            // Set the content type and headers
            Response.ContentType = "application/pdf";
            Response.Headers.Add("Content-Disposition", "attachment;filename=example.pdf");

            // Write the PDF content to the response
            await Response.Body.WriteAsync(bytes, 0, bytes.Length);

            return new EmptyResult();
        }



    }
}
