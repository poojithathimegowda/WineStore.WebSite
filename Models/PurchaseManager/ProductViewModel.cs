using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace WineStore.WebSite.Models.PurchaseManager
{
    public class Product
    {
       
        public int Product_ID { get; set; }
        public string Product_Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }

        public int Supplier_ID { get; set; }
       
    }


    public class ProductViewModel
    {
        public int Product_ID { get; set; }
        public string Product_Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }

        public int Supplier_ID { get; set; }
        public List<SelectListItem> ExistingSuppliers { get; set; }

        public SelectListItem SelectedSupplier { get; set; }
    }


}
