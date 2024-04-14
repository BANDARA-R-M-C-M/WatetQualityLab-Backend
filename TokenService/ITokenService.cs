using Project_v1.Models.Users;

namespace Project_v1.TokenService {
    public interface ITokenService {
        String CreateToken(SystemUser user);
    }
}