using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;

namespace BlazorBattles.Client.Services
{
    public class CustomAuthStateProvider : AuthenticationStateProvider
    {
        private readonly ILocalStorageService _localStorage;

        public CustomAuthStateProvider(ILocalStorageService localStorage)
        {
            _localStorage = localStorage;
        }
        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var state = new AuthenticationState(new ClaimsPrincipal());

            if (await _localStorage.GetItemAsync<bool>("IsAuthenticated"))
            {
                var identity = new ClaimsIdentity(new[]
                {
                new Claim(ClaimTypes.Name, "Me")
                }, "test authentication type");

                var user = new ClaimsPrincipal(identity);
                state = new AuthenticationState(user);

            }

            NotifyAuthenticationStateChanged(Task.FromResult(state));

            return state;

        }
    }
}
