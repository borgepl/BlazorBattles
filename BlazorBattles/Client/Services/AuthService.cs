using BlazorBattles.Client.Services.Contracts;
using BlazorBattles.Models.Dto.Auth;
using System.Net.Http.Json;

namespace BlazorBattles.Client.Services
{
    public class AuthService : IAuthService
    {
        private readonly HttpClient _http;

        public AuthService(HttpClient http)
        {
            _http = http;
        }

        public async Task<AuthenticationResponseDTO> Login(UserLoginDTO request)
        {
            var result = await _http.PostAsJsonAsync("api/auth/login", request);
            var loginResult = await result.Content.ReadFromJsonAsync<AuthenticationResponseDTO>();
            if (loginResult != null) 
            {
                return loginResult;
            }
            else
            {
                return new AuthenticationResponseDTO { IsAuthSuccessful = false };
            }
        }

        public Task Logout()
        {
            throw new NotImplementedException();
        }

        public async Task<RegistrationResponseDTO> Register(UserRegisterDTO request)
        {
            var result = await _http.PostAsJsonAsync("api/auth/register", request);
            var registrationResult = await result.Content.ReadFromJsonAsync<RegistrationResponseDTO>();
            if (registrationResult != null)
            {
                return registrationResult;
            }
            else
            {
                return new RegistrationResponseDTO { IsRegisterationSuccessful = false};
            }
        }
    }
}
