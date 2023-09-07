using BlazorBattles.Models.Dto;

namespace BlazorBattles.Client.Services.Contracts
{
    public interface IBattleService
    {
        Task<BattleResultDTO> StartBattle(string opponentId);
    }
}
