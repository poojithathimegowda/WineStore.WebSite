namespace WineStore.WebSite.Models.StoreManager
{
    public class OrderViewModel
    {
        public int Order_ID { get; set; }
        public int Shop_ID { get; set; } // Foreign Key to Shop Table
        public int Product_ID { get; set; } // Foreign Key to Product Table
        public int Quantity { get; set; }
        public decimal Total_Amount { get; set; }
        public DateTime Order_Date { get; set; }

    }
}
