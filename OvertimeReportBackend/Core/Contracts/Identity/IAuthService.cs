using Core.Feature.Login.LoginUser;
using Core.Models.Identity;
using Core.Models.Response;

namespace Core.Contracts.Identity
{
    public interface IAuthService
    {
        Task<Response<LoginResponseDto>> Login(LoginRequestDto request, ApplicationUser user);
        Task<ApplicationUser?> GetUserByEmail(string email);
        Task DeleteUser(ApplicationUser user);
        Task<Response<LoginResponseDto>> Register(RegisterDtLoginRequest request);
        Task<Response<bool>> ValidateToken(string token);
    }
}