using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using API.Models;
using Newtonsoft.Json;
using Pages.Models;

namespace Pages.ApiDal
{
    public class ApiDal : IApiDal
    {
        private string requestUri = "https://localhost:5001";
        private HttpClient httpClient;

        public ApiDal()
        {
            httpClient = new HttpClient { BaseAddress = new Uri(requestUri) };
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public bool Create<T>(T t, string uri)
        {
            var result = httpClient.PostAsJsonAsync(uri, t).Result;
//            var result = httpClient.PostAsJsonAsync("api/player/login", new LoginViewModelDto { Avatar = "", EmailAddress = "", PassPhrase = "" }).Result;
//            var result = httpClient.PostAsJsonAsync("api/player/register", new LoginViewModelDto { Avatar = "", EmailAddress = "", PassPhrase = "" }).Result;
//            var result = httpClient.PostAsJsonAsync("api/player/register", new Player { Avatar = "Hoi", CurrentlyPlayingGame = new Game(), EmailAddress = "", Id = 0, PassPhrase = "Test1234567890abcdefghij!" }).Result;
//            var result = httpClient.PostAsJsonAsync("api/player/login", new Player { Avatar = "Hoi", CurrentlyPlayingGame = new Game(), EmailAddress = "", Id = 0, PassPhrase = "Test1234567890abcdefghij!" }).Result;
            return result.IsSuccessStatusCode;
        }

        public async Task<T> Get<T>(string uri)
        {
            var response = httpClient.GetAsync(uri).Result;
            response.EnsureSuccessStatusCode();

            var data = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(data);
        }

        public async Task<PlayerDto> GetPlayer(string avatar, string emailAddress, string passPhrase)
        {
            var response = await httpClient.PostAsJsonAsync("api/player/login",
                new LoginViewModelDto {Avatar = avatar, EmailAddress = emailAddress, PassPhrase = passPhrase});
            response.EnsureSuccessStatusCode();
            var data = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<PlayerDto>(data);
        }
    }
}