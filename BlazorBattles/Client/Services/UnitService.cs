using BlazorBattles.Client.Services.Contracts;
using Blazored.Toast.Services;
using DataAccess.Data.Domain;
using System.Net.Http.Json;

namespace BlazorBattles.Client.Services
{
    public class UnitService : IUnitService
    {
        private readonly IToastService _toastService;
        private readonly HttpClient _http;
        private readonly IBananaService _bananaService;

        public UnitService(IToastService toastService, HttpClient http, IBananaService bananaService)
        {
            _toastService = toastService;
            _http = http;
            _bananaService = bananaService;
        }
        public IList<Unit> Units { get; set; }  = new List<Unit>();

        public IList<UserUnit> MyUnits { get; set; } = new List<UserUnit>();

        public async Task AddUnit(int unitId)
        {
            var unit = Units.First(unit => unit.Id == unitId);

            var result = await _http.PostAsJsonAsync<int>("api/userunit", unitId);
            if (result != null)
            {
                if (result.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    _toastService.ShowError(await result.Content.ReadAsStringAsync());
                }
                else
                {
                    await _bananaService.GetBananas();
                    _toastService.ShowSuccess($"Your {unit.Title} has been built!");
                }
            } 
            else
            {
                _toastService.ShowError("Server not available !");
            }


            //Console.WriteLine($"{unit.Title} was built!");
            //Console.WriteLine($"Your army size: {MyUnits.Count}");
        }

        public async Task LoadUnitsAsync()
        {
            if (Units.Count == 0)
            {
                Units = await _http.GetFromJsonAsync<IList<Unit>>("api/Unit");
            }
        }

        public async Task LoadUserUnitsAsync()
        {
            MyUnits = await _http.GetFromJsonAsync<IList<UserUnit>>("api/userunit");
        }

        public async Task ReviveArmy()
        {
            var result = await _http.PostAsJsonAsync<string>("api/userunit/revive", null);
            if (result.StatusCode == System.Net.HttpStatusCode.OK)
            {
                _toastService.ShowSuccess(await result.Content.ReadAsStringAsync());
            }
            else _toastService.ShowError(await result.Content.ReadAsStringAsync());

            await LoadUserUnitsAsync();
            await _bananaService.GetBananas();
        }
    }
}
