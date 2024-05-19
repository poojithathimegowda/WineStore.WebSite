using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace WineStore.WebSite.Models.Admin
{
    public class ShopViewModel
    {
        public int Shop_ID { get; set; }
        public string Shop_Name { get; set; }
        public string Location { get; set; }
    }
    public class OrderDto
    {
        public DateTime Date { get; set; }
        public decimal TotalAmount { get; set; }
    }

    public class ExpenseDto
    {
        public DateTime Date { get; set; }
        public decimal Amount { get; set; }
    }

    public class ProfitLossDto
    {
        public ShopViewModel Shop { get; set; }
        public decimal Income { get; set; }
        public decimal Expenses { get; set; }
        public decimal ProfitOrLoss { get; set; }
        public List<OrderDto> Orders { get; set; }
        public List<ExpenseDto> ExpensesList { get; set; }
    }

    public class ReportViewModel
    {
        public int Shop_ID { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public ShopViewModel Shop { get; set; }
        public decimal Income { get; set; }
        public decimal Expenses { get; set; }
        public decimal ProfitOrLoss { get; set; }
        public List<SelectListItem> ExistingShops { get; set; }

        public SelectListItem SelectedShops { get; set; }

        public List<OrderDto> Orders { get; set; }
        public List<ExpenseDto> ExpensesList { get; set; }
    }


}
