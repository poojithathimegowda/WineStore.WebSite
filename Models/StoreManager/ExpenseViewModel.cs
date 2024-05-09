namespace WineStore.WebSite.Models.StoreManager
{
    public class ExpenseViewModel
    {

        public int Expense_ID { get; set; }

        public int Shop_ID { get; set; }

        public string Expense_Type { get; set; }
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }


    }
}
