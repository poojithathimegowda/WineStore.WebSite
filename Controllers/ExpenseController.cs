using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using WineStore.WebSite.Managers;
using WineStore.WebSite.Models.Admin;
using WineStore.WebSite.Models.PurchaseManager;
using WineStore.WebSite.Models.StoreManager;

namespace WineStore.WebSite.Controllers
{
    public class ExpenseController : Controller
    {
        private readonly HttpClient _httpClient;

        public ExpenseController(IHttpClientFactory httpClientFactory, IHttpContextAccessor httpContextAccessor)
        {
            _httpClient = httpClientFactory.CreateClient("MyHttpClient");
            var authToken = httpContextAccessor.HttpContext.Session.GetString("AuthToken");
            if (!string.IsNullOrEmpty(authToken))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authToken);
            }
        }

        // GET: ExpenseController
        public async Task<IActionResult> Index()
        {
            try
            {
                var apiManager = new ApiManager(_httpClient);
                var expenses = await apiManager.CallApiAsync<dynamic, List<ExpenseViewModel>>("/api/Expenses", null, System.Web.Mvc.HttpVerbs.Get);
                return View("ListOfExpenses", expenses);
            }
            catch
            {
                return View("Error");
            }
        }

        // GET: ExpenseController/Details/5
        public async Task<IActionResult> Details(int id)
        {
            try
            {
                var apiManager = new ApiManager(_httpClient);
                var expense = await apiManager.CallApiAsync<dynamic, ExpenseViewModel>($"/api/Expenses/{id}", null, System.Web.Mvc.HttpVerbs.Get);
                return View("ExpenseDetails", expense);
            }
            catch
            {
                return View("Error");
            }
        }

        // GET: ExpenseController/Create
        public IActionResult Create()
        {
            return View("AddNewExpense");
        }

        // POST: ExpenseController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddNewExpense(ExpenseViewModel expenseViewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var apiManager = new ApiManager(_httpClient);
                    await apiManager.CallApiAsync<ExpenseViewModel, ExpenseViewModel>("/api/Expenses", expenseViewModel, System.Web.Mvc.HttpVerbs.Post);
                    return RedirectToAction(nameof(Index));
                }
                return View("AddNewExpense", expenseViewModel);
            }
            catch
            {
                return View("Error");
            }
        }

        // GET: ExpenseController/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var apiManager = new ApiManager(_httpClient);
                var expense = await apiManager.CallApiAsync<dynamic, ExpenseViewModel>($"/api/Expenses/{id}", null, System.Web.Mvc.HttpVerbs.Get);
                return View("EditExpenses", expense);
            }
            catch
            {
                return View("Error");
            }
        }

        // POST: ExpenseController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditExpenses(ExpenseViewModel expenseViewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var apiManager = new ApiManager(_httpClient);
                    await apiManager.CallApiAsync<ExpenseViewModel, ExpenseViewModel>($"/api/Expenses/{expenseViewModel.Expense_ID}", expenseViewModel, System.Web.Mvc.HttpVerbs.Put);
                    return RedirectToAction(nameof(Index));
                }
                return View("EditExpenses", expenseViewModel);
            }
            catch
            {
                return View("Error");
            }
        }

        // GET: ExpenseController/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var apiManager = new ApiManager(_httpClient);
                var expense = await apiManager.CallApiAsync<dynamic, ExpenseViewModel>($"/api/Expenses/{id}", null, System.Web.Mvc.HttpVerbs.Get);
                return View("DeleteExpense", expense);
            }
            catch
            {
                return View("Error");
            }
        }

        // POST: ExpenseController/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var apiManager = new ApiManager(_httpClient);
                await apiManager.CallApiAsync<dynamic, dynamic>($"/api/Expenses/{id}", null, System.Web.Mvc.HttpVerbs.Delete);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View("Error");
            }
        }
    }
}
