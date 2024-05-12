namespace WineStore.WebSite.Models.PurchaseManager
{
    public class InventoryViewModel
    {

        public int Inventory_ID { get; set; }
        public int Product_ID { get; set; }

        public string Product_Name { get; set; }
        public int Shop_ID { get; set; }
        public string Shop_Name { get; set; }
        public int Quantity { get; set; }

    }
}
