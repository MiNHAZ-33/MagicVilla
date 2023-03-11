using MagicVilla_Utility;
using MagicVilla_Web.Models;
using MagicVilla_Web.Services.IServices;
using Newtonsoft.Json;
using System.Text;

namespace MagicVilla_Web.Services
{
    public class BaseService : IBaseServices
    {
        public APIResponse responseModel { get; set; }
        
        public IHttpClientFactory httpClient { get; set; }

        public BaseService(IHttpClientFactory httpClient)
        {
            this.responseModel = new();
            this.httpClient = httpClient;
        }

        public async Task<T> SendAsync<T>(APIRequest apiRequest)
        {
            try
            {
                var client = httpClient.CreateClient("MagicAPI");
                HttpRequestMessage messsage = new HttpRequestMessage();
                messsage.Headers.Add("Content-Type", "application/json");
                messsage.RequestUri = new Uri(apiRequest.Url);

                if (apiRequest.Data != null)
                {
                    messsage.Content = new StringContent(JsonConvert.SerializeObject(apiRequest.Data),
                        Encoding.UTF8, "application/json");
                }
                switch (apiRequest.ApiType)
                {
                    case SD.ApiType.POST:
                        messsage.Method = HttpMethod.Post;
                        break;
                    case SD.ApiType.PUT:
                        messsage.Method = HttpMethod.Put;
                        break;
                    case SD.ApiType.DELETE:
                        messsage.Method = HttpMethod.Delete;
                        break;
                    default:
                        messsage.Method = HttpMethod.Get;
                        break;
                }

                HttpResponseMessage apiResponse = null;
                apiResponse = await client.SendAsync(messsage);

                var apiContent = await apiResponse.Content.ReadAsStringAsync();
                var APIResponse = JsonConvert.DeserializeObject<T>(apiContent);

                return APIResponse;
            } catch (Exception ex)
            {
                var DTO = new APIResponse
                {
                    ErrorMessage = new List<string> { Convert.ToString(ex.Message) },
                    IsSuccess = false,
                };
                var res = JsonConvert.SerializeObject(DTO);
                var APIResponse = JsonConvert.DeserializeObject<T>(res);
                return APIResponse;
            }
            
        }
    }
}
