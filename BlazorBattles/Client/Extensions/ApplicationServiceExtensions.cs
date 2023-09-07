using BlazorBattles.Client.Services;
using BlazorBattles.Client.Services.Contracts;
using Blazored.LocalStorage;
using Blazored.Toast;
using Microsoft.AspNetCore.Components.Authorization;

namespace BlazorBattles.Client.Extensions
{
    public static class ApplicationServiceExtensions
    {
        public static IServiceCollection AddMyAppServices(this IServiceCollection services, IConfiguration config)
        {

            services.AddBlazoredToast();
            services.AddBlazoredLocalStorage();

            services.AddScoped<IBananaService, BananaService>();
            services.AddScoped<IUnitService, UnitService>();

            services.AddScoped<AuthenticationStateProvider, CustomAuthStateProvider>();
            services.AddScoped<IAuthService, AuthService>();

            services.AddOptions();
            services.AddAuthorizationCore();

            services.AddScoped<ILeaderboardService, LeaderboardService>();
            services.AddScoped<IBattleService, BattleService>();

            return services;
        }
    }
}

