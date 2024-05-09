using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using WineStore.WebSite.Models;
using Microsoft.AspNetCore.Identity;

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
                    string responseObject = await response.Content.ReadAsStringAsync();


                    // Provided token string

                    // Deserialize the token response
                    TokenResponse tokenResponse = JsonConvert.DeserializeObject<TokenResponse>(responseObject);

                    //TODO: Poojitha 
                    string role;
                    if (tokenResponse.Roles != null && tokenResponse.Roles.Any())
                    {
                        role = tokenResponse.Roles.FirstOrDefault();
                    }
                    else
                    {
                        role = "admin"; // Set default role to "admin"
                    }


                    // Extract the token and prepend it with "Bearer "
                    string bearerToken = tokenResponse.Token;
                    
                    HttpContext.Session.SetString("AuthToken", bearerToken);
                    HttpContext.Session.SetString("AuthRole", role);

                    HttpContext.Session.SetString("UserName", model.Username);


                    switch (role.ToUpper())
                    {

                        case "ADMIN":
                            return RedirectToAction("Index", "Admin");
                            break;

                        case "STOREMANAGER":

                            return RedirectToAction("Index", "StoreManager");
                            break;
                        case "PURCHASEMANAGER":

                            return RedirectToAction("Index", "PurchaseManager");
                            break;

                        default:
                            ModelState.AddModelError(string.Empty, "Invalid username or password.");
                            return View("Index", model);
                            break;
                    }

                    // Redirect to home page or another secure area after successful login

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


        // GET: /Login/Logout
        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            // Clear the authentication token
            HttpContext.Session.Remove("AuthToken");
            HttpContext.Session.Remove("AuthRole");

            // Redirect to the login page or any other page
            return RedirectToAction("Index", "Home");
        }
    }
}
