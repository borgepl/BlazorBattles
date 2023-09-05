using BlazorBattles.Client.Services.Contracts;
using BlazorBattles.Models.Dto;
using System.Net.Http.Json;

namespace BlazorBattles.Client.Services
{
    public class LeaderboardService : ILeaderboardService
    {
        private readonly HttpClient _http;

        public LeaderboardService(HttpClient http)
        {
            _http = http;
        }
        public IList<UserStatisticDTO> Leaderboard { get; set; } = new List<UserStatisticDTO>();

        public async Task GetLeaderboard()
        {
            Leaderboard = await _http.GetFromJsonAsync<IList<UserStatisticDTO>>("api/user/leaderboard");
        }
    }
}
