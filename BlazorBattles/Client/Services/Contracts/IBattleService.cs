using BlazorBattles.Models.Dto;

namespace BlazorBattles.Client.Services.Contracts
{
    public interface IBattleService
    {
        BattleResultDTO LastBattle { get; set; }
        Task<BattleResultDTO> StartBattle(string opponentId);
    }
}
