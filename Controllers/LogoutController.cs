//using Microsoft.AspNetCore.Mvc;
//using System.Diagnostics;
//using WineStore.WebSite.Models;

//namespace WineStore.WebSite.Controllers
//{
//    public class LogoutController : Controller
//    {
//        private readonly ILogger<HomeController> _logger;

//        public LogoutController(ILogger<HomeController> logger)
//        {
//            _logger = logger;
//        }

//        public IActionResult Index()
//        {
//            return View("Index");
//        }

       
//        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
//        public IActionResult Error()
//        {
//            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
//        }
//    }
//}