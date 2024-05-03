namespace WineStore.WebSite.Models
{
    public class LoginViewModel
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }

    public class Shop
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string PhoneNo { get; set; }
        public string Email { get; set; }
    }


    public class TokenResponse
    {
        public string Token { get; set; }
        public DateTime Expiration { get; set; }
    }

}