using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Net.Http.Headers;
using WineStore.WebSite.Managers;
using WineStore.WebSite.Models.Admin;
using WineStore.WebSite.Models.PurchaseManager;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.IO;
namespace WineStore.WebSite.Controllers
{
    public class ReportController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IWebHostEnvironment _env;

        public ReportController(IWebHostEnvironment env,IHttpClientFactory httpClientFactory, IHttpContextAccessor httpContextAccessor)
        {
            _httpClient = httpClientFactory.CreateClient("MyHttpClient");
            _httpContextAccessor = httpContextAccessor;
            _env = env;

            string authToken = _httpContextAccessor.HttpContext.Session.GetString("AuthToken");
            // Set auth token in request header
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authToken);
        }
        // GET: ReportController
        public async Task<ActionResult> Index()
        {
            try
            {
                ApiManager apiManager = new ApiManager(_httpClient);
                var input1 = new { };
                var shopoutput1 = await apiManager.CallApiAsync<dynamic, List<ShopViewModel>>("/api/Shop", input1, System.Web.Mvc.HttpVerbs.Get);

                ReportViewModel r1 = new ReportViewModel();
                r1.ExistingShops = new List<SelectListItem>();
                r1.StartDate = DateTime.UtcNow.AddDays(-30);
                r1.EndDate = DateTime.UtcNow;
                foreach (var item in shopoutput1)
                {
                    r1.ExistingShops.Add(new SelectListItem() { Text = item.Shop_Name, Value = item.Shop_ID.ToString() });
                }

                return View("Index", r1);
            }
            catch
            {
                return View();
            }

        }







        // POST: ReportController/Index
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Index(IFormCollection collection)
        {
            try
            {
                var id = collection["Shop_ID"];
                var startDate = collection["StartDate"];
                var endDate = collection["EndDate"];



                // Call the API with the CustomersViewModel object
                ApiManager apiManager = new ApiManager(_httpClient);
                var output1 = await apiManager.CallApiAsync<dynamic, ReportViewModel>(
                    $"/api/Shop/GetProfitAndLoss?shopId={id}&start={startDate}&end={endDate}", "", System.Web.Mvc.HttpVerbs.Get);


                output1.StartDate = Convert.ToDateTime(startDate);
                output1.EndDate = Convert.ToDateTime(endDate);

                return PartialView("_ReportPartial", output1);
            }
            catch
            {
                // TempData["Error"] = "An error occurred while adding customer details. Please try again.";
                return View();
            }
        }


        // get: ReportController/Download  Download(int Shop_ID, DateTime start, DateTime end )
        public async Task<IActionResult> Download(int Shop_ID, DateTime start, DateTime end)
        {


            try
            {
                var id = Shop_ID;
                var startDate = start;
                var endDate = end;



                // Call the API with the CustomersViewModel object
                ApiManager apiManager = new ApiManager(_httpClient);
                var data = await apiManager.CallApiAsync<dynamic, ReportViewModel>(
                    $"/api/Shop/GetProfitAndLoss?shopId={id}&start={startDate.ToString("yyyy-MM-dd")}&end={endDate.ToString("yyyy-MM-dd")}", "", System.Web.Mvc.HttpVerbs.Get);



                MemoryStream memoryStream = new MemoryStream();
                Document document = new Document();
                PdfWriter writer = PdfWriter.GetInstance(document, memoryStream);

                string logoPath = Path.Combine(_env.WebRootPath, "Images", "wineLogo.jpg");

                Image logo = Image.GetInstance(logoPath);



                logo.ScaleToFit(1600f, 120f); // adjust the size as needed
                logo.Alignment = Element.ALIGN_CENTER; // align the logo as needed

                // Add the logo to the document

                // Add header and footer
                writer.PageEvent = new CustomPdfPageEventHelper();

                document.Open();

             
                document.Add(logo);
                document.Add(new Paragraph("Profit and Loss Statement"));
                // Add title
                document.Add(new Paragraph($"Shop: {data.Shop.Shop_Name}"));
                document.Add(new Paragraph($"Location: {data.Shop.Location}"));
                document.Add(new Paragraph($"Period: {start.ToString("yyyy-MM-dd")} to {end.ToString("yyyy-MM-dd")}"));
                document.Add(new Paragraph("\n"));

                // Create a table for the P&L statement
                PdfPTable table = new PdfPTable(2);
                table.WidthPercentage = 100;

                // Add headers
                table.AddCell("Description");
                table.AddCell("Amount");

                // Add income row
                table.AddCell("Income");
                table.AddCell(data.Income.ToString());

                // Add expenses row
                table.AddCell("Expenses");
                table.AddCell(data.Expenses.ToString());

                // Add profit/loss row
                table.AddCell("Profit/Loss");





                // Create a new cell
                PdfPCell cell = new PdfPCell();

                // Set the cell data
                cell.Phrase = new Phrase(data.ProfitOrLoss.ToString());

                // Check the condition and set the cell background color
                if (data.ProfitOrLoss > 0)
                {
                    // If ProfitOrLoss is positive, set the cell color to green
                    cell.BackgroundColor = new BaseColor(0, 255, 0); // RGB for green
                }
                else
                {
                    // If ProfitOrLoss is negative or zero, set the cell color to red
                    cell.BackgroundColor = new BaseColor(255, 0, 0); // RGB for red
                }

                // Add the cell to the table
                table.AddCell(cell);



                // Add table to document
                document.Add(table);
                document.Add(new Paragraph("\n"));

                // Create tables for orders and expenses
                PdfPTable ordersTable = new PdfPTable(2);
                ordersTable.WidthPercentage = 100;
                ordersTable.AddCell("Order Date");
                ordersTable.AddCell("Total Amount");

                foreach (var order in data.Orders)
                {
                    ordersTable.AddCell(order.Date.ToString("yyyy-MM-dd"));
                    ordersTable.AddCell(order.TotalAmount.ToString());
                }

                document.Add(new Paragraph("Orders:"));
                document.Add(ordersTable);
                document.Add(new Paragraph("\n"));

                PdfPTable expensesTable = new PdfPTable(2);
                expensesTable.WidthPercentage = 100;
                expensesTable.AddCell("Expense Date");
                expensesTable.AddCell("Amount");

                foreach (var expense in data.ExpensesList)
                {
                    expensesTable.AddCell(expense.Date.ToString("yyyy-MM-dd"));
                    expensesTable.AddCell(expense.Amount.ToString());
                }

                document.Add(new Paragraph("Expenses:"));
                document.Add(expensesTable);
              
                document.Close();

                byte[] bytes = memoryStream.ToArray();
                memoryStream.Close();

                // Clear the response for proper PDF content
                Response.Clear();

                // Set the content type and headers
                Response.ContentType = "application/pdf";
                Response.Headers.Add("Content-Disposition", "attachment;filename=ProfitAndLossStatement.pdf");

                // Write the PDF content to the response
                await Response.Body.WriteAsync(bytes, 0, bytes.Length);


                return new EmptyResult();
            }
            catch (Exception exp)
            {
                // TempData["Error"] = "An error occurred while adding customer details. Please try again.";

                return new EmptyResult();
            }

        }


        // GET: ShopController/Edit/5
        public async Task<ActionResult> Edit(int id)
        {
            ApiManager apiManager = new ApiManager(_httpClient);

            var input1 = new { Id = id };
            var output1 = await apiManager.CallApiAsync<dynamic, ShopViewModel>($"/api/Shop/{id}", input1, System.Web.Mvc.HttpVerbs.Get);
            return View("EditShop", output1);

        }

        // POST: ShopController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditShop(int id, IFormCollection collection)
        {
            try
            {

                var Id = id;
                var name = collection["Shop_Name"];
                var address = collection["Location"];

                // Create a CustomersViewModel object
                var shopViewModel = new ShopViewModel
                {
                    Shop_ID = Convert.ToInt32(Id),
                    Shop_Name = name,
                    Location = address

                };

                // Call the API with the CustomersViewModel object
                ApiManager apiManager = new ApiManager(_httpClient);

                var output1 = await apiManager.CallApiAsync<dynamic, ShopViewModel>($"/api/Shop/{id}", shopViewModel, System.Web.Mvc.HttpVerbs.Put);
                // Set success message

                //TempData["Message"] = "Customer details updated successfully.";
                return RedirectToAction("Index");
            }
            catch
            {
                // Set success message
                //TempData["Message"] = "Something went wrong when updating the data.";

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
