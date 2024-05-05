using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WineStore.WebSite.Models.SalesRep
{
    public class SalesRegisterViewModel
    {
        public int Id { get; set; }


        public int CustomerId { get; set; }


        public int ItemId { get; set; }

        public int QuantitySold { get; set; }
        public decimal AmountSold { get; set; }
    }
}
