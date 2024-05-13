using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
namespace WineStore.WebSite.Models.PurchaseManager
{
    public class Inventory
    {

        public int Inventory_ID { get; set; }
        public int Product_ID { get; set; }

        public string Product_Name { get; set; }
        public int Shop_ID { get; set; }
        public string Shop_Name { get; set; }
        public int Quantity { get; set; }

    }



    public class InventoryViewModel
    {
        public int Inventory_ID { get; set; }
        public int Product_ID { get; set; }

        //public string Product_Name { get; set; }
        public int Shop_ID { get; set; }
        //public string Shop_Name { get; set; }
        public int Quantity { get; set; }


        public int Supplier_ID { get; set; }
        public List<SelectListItem> ExistingProducts { get; set; }

        public SelectListItem SelectedProducts { get; set; }
        public List<SelectListItem> ExistingShop { get; set; }

        public SelectListItem SelectedShop { get; set; }
    }
    public class InventoryCompositeViewModel
    {
        public InventoryViewModel ExistingProducts { get; set; }
        public InventoryViewModel ExistingShop { get; set; }
    }
}
