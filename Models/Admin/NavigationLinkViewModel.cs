namespace WineStore.WebSite.Models.Admin
{

    public class NavigationLinkViewModel
    {
        public string Text { get; set; }
        public string Area { get; set; }
        public string Controller { get; set; }
        public string Action { get; set; }
        public string Icon { get; internal set; }
    }
}
