using BlazorBattles.Models.Dto;

namespace BlazorBattles.Client.Services.Contracts
{
    public interface ILeaderboardService
    {
        IList<UserStatisticDTO> Leaderboard { get; set; }
        Task GetLeaderboard();
    }
}
