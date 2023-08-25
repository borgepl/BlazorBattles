using DataAccess.Data.Identity;

namespace BlazorBattles.Server.Services.Contracts
{
    public interface ITokenService
    {
        string CreateToken(User user);

    }
}
