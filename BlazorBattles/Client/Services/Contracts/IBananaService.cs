namespace BlazorBattles.Client.Services.Contracts
{
    public interface IBananaService
    {
        event Action OnChange;
        int Bananas { get; set; }
        void EatBananas(int amount);
        void AddBananas(int amount);
    }
}
