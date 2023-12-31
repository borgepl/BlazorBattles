﻿using DataAccess.Data.Domain;

namespace BlazorBattles.Client.Services.Contracts
{
    public interface IUnitService
    {
        IList<Unit> Units { get; set; }
        IList<UserUnit> MyUnits { get; set; }
        Task AddUnit(int unitId);
        Task LoadUnitsAsync();
        Task LoadUserUnitsAsync();

        Task ReviveArmy();
        int CountTotalHitPoints();
    }
}
