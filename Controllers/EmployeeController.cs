using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using WineStore.WebSite.Managers;
using WineStore.WebSite.Models.Admin;

namespace WineStore.WebSite.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public EmployeeController(IHttpClientFactory httpClientFactory, IHttpContextAccessor httpContextAccessor)
        {
            _httpClient = httpClientFactory.CreateClient("MyHttpClient");
            _httpContextAccessor = httpContextAccessor;

            string authToken = _httpContextAccessor.HttpContext.Session.GetString("AuthToken");
            if (!string.IsNullOrEmpty(authToken))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authToken);
            }
        }

        // GET: EmployeeController
        public ActionResult Index()
        {
            var employeeViewModel = new EmployeeViewModel();
            return View("AddEmployee", employeeViewModel);
        }

        // GET: EmployeeController/Details/5
        public ActionResult Details(int id)
        {
            // Implement logic to retrieve employee details by id
            return View();
        }

        // GET: EmployeeController/Create
        public ActionResult Create()
        {
            var employeeViewModel = new EmployeeViewModel();
            return View("AddEmployee", employeeViewModel);
        }

        // POST: EmployeeController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(IFormCollection collection)
        {
            try
            {
                var employee = Helper.ConvertToObjectType<EmployeeViewModel>(collection);

                var apiManager = new ApiManager(_httpClient);
                var response = await apiManager.CallApiAsync<EmployeeViewModel, dynamic>("/api/Admin/register", employee, System.Web.Mvc.HttpVerbs.Post);

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View("AddEmployee");
            }
        }

        // GET: EmployeeController/Edit/5
        public ActionResult Edit(int id)
        {
            // Implement logic to retrieve employee details by id for editing
            return View();
        }

        // POST: EmployeeController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int id, IFormCollection collection)
        {
            try
            {
                var employee = Helper.ConvertToObjectType<EmployeeViewModel>(collection);

                var apiManager = new ApiManager(_httpClient);
                var response = await apiManager.CallApiAsync<EmployeeViewModel, dynamic>($"/api/Admin/edit/{id}", employee, System.Web.Mvc.HttpVerbs.Put);

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: EmployeeController/Delete/5
        public ActionResult Delete(int id)
        {
            // Implement logic to retrieve employee details by id for deletion confirmation
            return View();
        }

        // POST: EmployeeController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(int id, IFormCollection collection)
        {
            try
            {
                var apiManager = new ApiManager(_httpClient);
                var response = await apiManager.CallApiAsync<dynamic, dynamic>($"/api/Admin/delete/{id}", null, System.Web.Mvc.HttpVerbs.Delete);

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
