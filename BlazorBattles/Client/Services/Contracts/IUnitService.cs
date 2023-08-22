using DataAccess.Data.Domain;

namespace BlazorBattles.Client.Services.Contracts
{
    public interface IUnitService
    {
        IList<Unit> Units { get; }
        IList<UserUnit> MyUnits { get; set; }
        void AddUnit(int unitId);
    }
}
