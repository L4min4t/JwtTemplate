using App.Models.Auth.Request;
using App.Models.Auth.Response;

namespace App.Services.Interfaces;

public interface IAuthService
{
    Task<RegisterResponseModel> RegisterAsync(RegisterModel model);
    Task<UserInfoResponseModel> LoginAsync(LoginModel model);
    
    Task<UserInfoResponseModel> ChangePasswordAsync(ChangePasswordModel model);
}
