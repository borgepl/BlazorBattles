using BlazorBattles.Models.Dto;
using System.Collections.Generic;

namespace BlazorBattles.Client.Services.Contracts
{
    public interface IBattleService
    {
        BattleResultDTO LastBattle { get; set; }
        IList<BattleHistoryDTO> BattleHistory { get; set; }
        Task<BattleResultDTO> StartBattle(string opponentId);
        Task<IList<BattleHistoryDTO>> GetBattleHistory();
    }
}
