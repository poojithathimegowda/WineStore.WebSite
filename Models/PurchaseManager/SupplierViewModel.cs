using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WineStore.WebSite.Models.PurchaseManager
{
    public class SupplierViewModel
    {
        public int Supplier_ID { get; set; }
        public string Supplier_Name { get; set; }
        public string Contact_Details { get; set; }
    }
}
