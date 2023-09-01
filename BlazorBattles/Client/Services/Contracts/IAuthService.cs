using BlazorBattles.Models.Dto.Auth;

namespace BlazorBattles.Client.Services.Contracts
{
    public interface IAuthService
    {
        Task<RegistrationResponseDTO> Register(UserRegisterDTO request);
        Task<AuthenticationResponseDTO> Login(UserLoginDTO request);

        Task Logout();
    }
}
