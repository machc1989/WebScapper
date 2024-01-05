using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using WebScrapper.Models;

namespace WebScrapper
{
    public class APIConsumer : IAPIConsumer
    {
        private readonly HttpClient _httpClient;
        private const string LOGIN = "/login";
        private const string CREATEUSER = "/register";
        private const string GETPAGES = "/GetScrappedPages";
        private const string POSTPAGE = "/PostScrappePage";
        public APIConsumer(HttpClient client) 
        {
            _httpClient = client;
        }

        public async Task<bool> CreateUser(UserModel user)
        {
            //var data = new StringContent(JsonSerializer.Serialize(user), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsJsonAsync(CREATEUSER, user);
            
            if(response.IsSuccessStatusCode)
            {
                return true;
            }
            else
            {
                return false;
            }

        }

        public async Task<List<ScrappedPageModel>> GetPages(string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = await _httpClient.GetAsync(GETPAGES);

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<List<ScrappedPageModel>>();
            }
            else
            {
                return new List<ScrappedPageModel>();
            }
        }

        public async Task<string> Login(string userName, string password)
        {
            var response = await _httpClient.GetAsync(LOGIN + "?userName=" + userName + "&passwordHash=" + password);

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsStringAsync();
            }
            else
            {
                return string.Empty;
            }
        }

        public async Task PostPage(ScrappedPageDTO pageDTO, string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = await _httpClient.PostAsJsonAsync(POSTPAGE, pageDTO);
            response.EnsureSuccessStatusCode();
        }
    }
}
