using WineStore.WebSite.Models.Admin;

namespace WineStore.WebSite.Models.StoreManager
{
    public class StoreManagerHomeViewModel
    {
        public List<NavigationLinkViewModel> NavigationLinkViewModel { get; set; }

        public string UserName { get; set; }
    }
}
