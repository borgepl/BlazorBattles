using BlazorBattles.Client.Helper;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Security.Claims;
using BlazorBattles.Client.Services.Contracts;
using Microsoft.VisualBasic;

namespace BlazorBattles.Client.Services
{
    public class CustomAuthStateProvider : AuthenticationStateProvider
    {
        private readonly ILocalStorageService _localStorage;
        private readonly HttpClient _http;
        private readonly IBananaService _bananaService;

        public CustomAuthStateProvider(ILocalStorageService localStorage, HttpClient http, IBananaService bananaService)
        {
            _localStorage = localStorage;
            _http = http;
            _bananaService = bananaService;
        }
        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var token = await _localStorage.GetItemAsStringAsync("AuthToken");
            // var token = await _localStorage.GetItemAsync<string>(SD.Local_Token);
            if (string.IsNullOrEmpty(token))
            {
                NotifyUserLoggedOut();
                return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
            }

            var identity = new ClaimsIdentity();
        
            try
            {
                identity = new ClaimsIdentity(JwtParser.ParseClaimsFromJwt(token), "jwtAuthType");
                _http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.Replace("\"",""));
                await _bananaService.GetBananas();
            }
            catch (Exception)
            {

                await _localStorage.RemoveItemAsync("AuthToken");
                identity = new ClaimsIdentity();
            }
            

            var user = new ClaimsPrincipal(identity);
            var state = new AuthenticationState(user);

            // return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity(JwtParser.ParseClaimsFromJwt(token), "jwtAuthType")));

            NotifyAuthenticationStateChanged(Task.FromResult(state));

            return state;

        }

        public void NotifyUserLoggedIn(string token)
        {
            var authenticatedUser = new ClaimsPrincipal(new ClaimsIdentity(JwtParser.ParseClaimsFromJwt(token), "jwtAuthType"));
            var authState = Task.FromResult(new AuthenticationState(authenticatedUser));
            NotifyAuthenticationStateChanged(authState);
        }

        public void NotifyUserLoggedOut()
        {
            var authState = Task.FromResult(new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity())));
            NotifyAuthenticationStateChanged(authState);
        }
    }
}
