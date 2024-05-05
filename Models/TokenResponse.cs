namespace WineStore.WebSite.Models
{
    public class TokenResponse
    {
        public string Token { get; set; }
        public DateTime Expiration { get; set; }
        public string[] Roles { get; set; }
    }

}