using Project_v1.Models.Users;

namespace Project_v1.Services.TokenService
{
    public interface ITokenService
    {
        String CreateToken(SystemUser user);
    }
}