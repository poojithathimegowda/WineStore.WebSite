namespace WineStore.WebSite.Models.PurchaseManager
{
    public class ProductViewModel
    {
        public int Product_ID { get; set; }
        public string Product_Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }

        public int Supplier_ID { get; set; }

    }

}
