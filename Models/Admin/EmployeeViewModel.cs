

using Microsoft.AspNetCore.Mvc.Rendering;

namespace WineStore.WebSite.Models.Admin
{
    public class EmployeeViewModel
    {

        public string Username { get; set; }


        public string Email { get; set; }


        public string Password { get; set; }

        public string Role { get; set; }
        public int Shop_ID { get; set; }

        // Additional properties
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }

        public List<SelectListItem> Rolelist = new List<SelectListItem>()
        {   new SelectListItem() { Text = "Admin", Value = "Admin" , Selected= true},
            new SelectListItem() { Text = "PurchaseManager", Value = "PurchaseManager" },
            new SelectListItem() { Text = "StoreManager", Value = "StoreManager" },
            new SelectListItem() { Text = "Employee", Value = "Employee" }
        };
    }
   
}

