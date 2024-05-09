using Newtonsoft.Json;
using System.Text;
using System.Web.Mvc;

namespace WineStore.WebSite.Managers
{
    public class ApiManager
    {
        private readonly HttpClient _httpClient;
        private  Encoding encodingSchema;
        private  string contentType;

        public ApiManager(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<TOutput> CallApiAsync<TInput, TOutput>(string endpoint, TInput input, HttpVerbs httpMethod)
        {

          
            encodingSchema = Encoding.UTF8;
            contentType = "application/json";
            // Serialize input object to JSON
            string jsonInput = JsonConvert.SerializeObject(input);
            HttpResponseMessage response;

            switch (httpMethod)
            {
                case (HttpVerbs.Get):
                    response = await _httpClient.GetAsync(endpoint);
                    break;
                case (HttpVerbs.Post):
                    response = await _httpClient.PostAsync(endpoint, new StringContent(jsonInput, encodingSchema, contentType));
                    break;
                case (HttpVerbs.Put):
                    response = await _httpClient.PutAsync(endpoint, new StringContent(jsonInput, encodingSchema, contentType));
                    break;
                default:
                    response = await _httpClient.GetAsync(endpoint);
                    break;
            }

            //if (httpMethod == HttpVerbs.Post)
            //{

            //    // Send HTTP request
            //    response = await _httpClient.PostAsync(endpoint, new StringContent(jsonInput));
            //}
            //response = await _httpClient.GetAsync(endpoint);

            // Check if request was successful
            response.EnsureSuccessStatusCode();

            // Read response content as string
            string jsonResponse = await response.Content.ReadAsStringAsync();

            // Deserialize JSON response to output type
            TOutput output = JsonConvert.DeserializeObject<TOutput>(jsonResponse);

            return output;
        }
    }
}





