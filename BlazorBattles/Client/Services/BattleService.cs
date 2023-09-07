using BlazorBattles.Client.Services.Contracts;
using BlazorBattles.Models.Dto;
using System.Net.Http.Json;

namespace BlazorBattles.Client.Services
{
    public class BattleService : IBattleService
    {
        private readonly HttpClient _http;

        public BattleService(HttpClient http)
        {
            _http = http;
        }

        public BattleResultDTO LastBattle { get; set; } = new BattleResultDTO();
        public IList<BattleHistoryDTO> BattleHistory { get; set; } = new List<BattleHistoryDTO>();

        public async Task<IList<BattleHistoryDTO>> GetBattleHistory()
        {
            BattleHistory = await _http.GetFromJsonAsync<BattleHistoryDTO[]>("api/battle/history");
      
            return BattleHistory;
        }

        public async Task<BattleResultDTO> StartBattle(string opponentId)
        {
            var result = await _http.PostAsJsonAsync("api/battle", opponentId);
            LastBattle = await result.Content.ReadFromJsonAsync<BattleResultDTO>();
            return LastBattle;

        }
    }
}
