using DataAccess.Data.Domain;

namespace BlazorBattles.Client.Services.Contracts
{
    public interface IUnitService
    {
        IList<Unit> Units { get; set; }
        IList<UserUnit> MyUnits { get; set; }
        void AddUnit(int unitId);
        Task LoadUnitsAsync();
    }
}
