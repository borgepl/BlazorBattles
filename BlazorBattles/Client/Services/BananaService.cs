
using BlazorBattles.Client.Services.Contracts;
using System.Net.Http.Json;

namespace BlazorBattles.Client.Services
{
    public class BananaService : IBananaService
    {
        private readonly HttpClient _http;

        public int Bananas { get; set; } = 0;

        public event Action OnChange;

        public BananaService( HttpClient http )
        {
            _http = http;
        }

        public async Task AddBananas(int amount)
        {
            var result = await _http.PutAsJsonAsync<int>("api/user/addbananas", amount);
            Bananas = await result.Content.ReadFromJsonAsync<int>();
            BananasChanged();
        }

        public void EatBananas(int amount)
        {
            Bananas -= amount;
            BananasChanged();
        }

        public async Task GetBananas()
        {
            Bananas = await _http.GetFromJsonAsync<int>("api/user/getbananas");
            BananasChanged();
        }

        private void BananasChanged()
        {
             OnChange.Invoke();
        }
    }
}
