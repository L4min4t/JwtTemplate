using App.Models.Auth;

namespace App.Services.Interfaces;

public interface IAuthService
{
    Task<RegisterResponseModel> RegisterAsync(RegisterModel model);
    Task<LoginResponseModel> LoginAsync(LoginModel model);
    
    Task<LoginResponseModel> ChangePasswordAsync(ChangePasswordModel model);
}
