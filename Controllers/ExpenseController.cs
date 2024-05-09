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
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ExpenseController(IHttpClientFactory httpClientFactory, IHttpContextAccessor httpContextAccessor)
        {
            _httpClient = httpClientFactory.CreateClient("MyHttpClient");
            _httpContextAccessor = httpContextAccessor;

            string authToken = _httpContextAccessor.HttpContext.Session.GetString("AuthToken");
            // Set auth token in request header
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authToken);
        }
        // GET: ShopController
        public async Task<ActionResult> Index()
        {
            try
            {
                ApiManager apiManager = new ApiManager(_httpClient);
                var input1 = new { };
                var output1 = await apiManager.CallApiAsync<dynamic, List<ExpenseViewModel>>($"/api/Expenses", input1, System.Web.Mvc.HttpVerbs.Get);
                return View("ListOfExpenses", output1);
            }
            catch
            {
                return View();
            }

        }

        // GET: ShopController/Details/5
        public async Task<ActionResult> Details(int id)
        {
            return View();
        }

        // GET: ShopController/Create
        public ActionResult Create()
        {
            return View("AddNewExpense");
        }

        // POST: ShopController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AddNewExpense(IFormCollection collection)
        {
            try
            {
                var id = collection["Expense_ID"];
                var shop = collection["Shop_ID"];
                var type = collection["Expense_Type"];
                var amt = collection["Amount"];
                var date = collection["Date"];
                var expenseViewModel = new ExpenseViewModel
                {
                    Expense_ID = Convert.ToInt32(id),
                    Shop_ID = Convert.ToInt32(shop),
                    Expense_Type = type,
                    Amount = Convert.ToDecimal(amt),
                    Date = Convert.ToDateTime(date)
                };
                // Call the API with the CustomersViewModel object
                ApiManager apiManager = new ApiManager(_httpClient);
                var output1 = await apiManager.CallApiAsync<ExpenseViewModel, ExpenseViewModel>($"/api/Expenses", expenseViewModel, System.Web.Mvc.HttpVerbs.Post);

                return RedirectToAction("Index");

            }
            catch
            {
                return View();
            }
        }

        // GET: ShopController/Edit/5
        public async Task<ActionResult> Edit(int id)
        {
            ApiManager apiManager = new ApiManager(_httpClient);

            var input1 = new { Id = id };
            var output1 = await apiManager.CallApiAsync<dynamic, OrderViewModel>($"/api/Expenses/{id}", input1, System.Web.Mvc.HttpVerbs.Get);
            return View("EditExpenses", output1);

        }

        // POST: ShopController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditOrders(int id, IFormCollection collection)
        {
            try
            {
                var Id = collection["Expense_ID"];
                var shop = collection["Shop_ID"];
                var type = collection["Expense_Type"];
                var amt = collection["Amount"];
                var date = collection["Date"];
                var expenseViewModel = new ExpenseViewModel
                {
                    Expense_ID = Convert.ToInt32(Id),
                    Shop_ID = Convert.ToInt32(shop),
                    Expense_Type = type,
                    Amount = Convert.ToDecimal(amt),
                    Date = Convert.ToDateTime(date)
                };

                // Call the API with the CustomersViewModel object
                ApiManager apiManager = new ApiManager(_httpClient);

                var output1 = await apiManager.CallApiAsync<dynamic, ExpenseViewModel>($"/api/Expenses/{Id}", expenseViewModel, System.Web.Mvc.HttpVerbs.Put);

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: ShopController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: ShopController/Delete/5
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
