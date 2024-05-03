using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using WineStore.WebSite.Models;

namespace WineStore.WebSite.Controllers
{
    public class LoginController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public LoginController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            try
            {
                // Create HttpClient instance
                var httpClient = _httpClientFactory.CreateClient("MyHttpClient");

                // Prepare the form data
                var formData = new MultipartFormDataContent();
                formData.Add(new StringContent(model.Username), "Username");
                formData.Add(new StringContent(model.Password), "Password");

                // Send the POST request with form data
                var response = await httpClient.PostAsync("/api/Authenticate/login", formData);

                // Check if the request was successful
                if (response.IsSuccessStatusCode)
                {
                    // Read response content which should contain the access token
                    string token = await response.Content.ReadAsStringAsync();


                    // Provided token string
                    string tokenString = "{\"token\":\"eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1lIjoicG9vaml0aGEiLCJqdGkiOiJmOTAwOTc0Yi0xZjcwLTQxOTMtODNmNi1kM2FkYTliZTQ3YWMiLCJleHAiOjE3MTQ0MzkyNjUsImlzcyI6Imh0dHA6Ly9sb2NhbGhvc3Q6NjE5NTUiLCJhdWQiOiJodHRwOi8vbG9jYWxob3N0OjQyMDAifQ.-XyR7OecuWnrSsOAn88-T5UQEvxT1UXWQTGa6yEIBK8\",\"expiration\":\"2024-04-30T01:07:45Z\"}";

                    // Deserialize the token response
                    TokenResponse tokenResponse = JsonConvert.DeserializeObject<TokenResponse>(tokenString);

                    // Extract the token and prepend it with "Bearer "
                    string bearerToken = tokenResponse.Token;


                    // Store the token securely (e.g., in session or cookie)

                    // Inside the Login action of LoginController after successful login
                    HttpContext.Session.SetString("AuthToken", bearerToken);

                    


                    // Redirect to home page or another secure area after successful login
                    return RedirectToAction("Index", "Shop");
                }
                else
                {
                    // Authentication failed, return back to login page with error message
                    ModelState.AddModelError(string.Empty, "Invalid username or password.");
                    return View("Index", model);
                }
            }
            catch (Exception ex)
            {
                // Handle any exceptions
                ModelState.AddModelError(string.Empty, "An error occurred while processing your request.");
                return View("Index", model);
            }
        }
    }
}
