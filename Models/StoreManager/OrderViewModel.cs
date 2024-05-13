using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace WineStore.WebSite.Models.StoreManager
{
    public class OrderViewModel
    {
        public int Order_ID { get; set; }

        public string Shop_Name { get; set; }
        public int Shop_ID { get; set; }
        public int Product_ID { get; set; } 

        public string Product_Name { get; set; }
        public int Quantity { get; set; }
        public decimal Total_Amount { get; set; }
        public DateTime Order_Date { get; set; }
        public List<SelectListItem> ExistingProducts { get; set; }

        public SelectListItem SelectedProducts { get; set; }
        public List<SelectListItem> ExistingShop { get; set; }

        public SelectListItem SelectedShop { get; set; }

    }
    public class Orders
    {
        public int Order_ID { get; set; }
        public int Shop_ID { get; set; }
        public string Shop_Name { get; set; }
        public int Product_ID { get; set; }
        public string Product_Name { get; set; }
        public int Quantity { get; set; }
        public decimal Total_Amount { get; set; }
        public DateTime Order_Date { get; set; }

    }
}
