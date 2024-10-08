using System.Security.Claims;
using App.Entities;
using App.Models.Auth.Request;
using App.Models.Auth.Response;
using App.Services.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace App.Services;

public class AuthService : IAuthService
{
    protected readonly IHttpContextAccessor _httpContextAccessor;
    protected readonly IJwtService _jwtService;
    protected readonly SignInManager<AuthUser> _signInManager;
    protected readonly UserManager<AuthUser> _userManager;
    
    public AuthService
    (
        UserManager<AuthUser> userManager,
        SignInManager<AuthUser> signInManager,
        IJwtService jwtService,
        IHttpContextAccessor httpContextAccessor
    )
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _jwtService = jwtService;
        _httpContextAccessor = httpContextAccessor;
    }
    
    public async Task<RegisterResponseModel> RegisterAsync(RegisterModel model)
    {
        var user = new AuthUser
        {
            UserName = model.UserName,
            Email = model.Email
        };
        var result = await _userManager.CreateAsync(user, model.Password);
        
        if (result.Succeeded)
            return new RegisterResponseModel
            {
                Content = new RegisterContent
                {
                    Id = user.Id,
                    Email = user.Email
                }
            };
        return new RegisterResponseModel
        {
            Errors = result.Errors.Select(e => e.Description)
                .ToArray()
        };
    }
    
    public async Task<UserInfoResponseModel> LoginAsync(LoginModel model)
    {
        var user = await _userManager.FindByEmailAsync(model.Email);
        
        if (user == null)
            return new UserInfoResponseModel
            {
                Errors = new[]
                {
                    "Invalid email"
                }
            };
        
        var result = await _signInManager.PasswordSignInAsync(user, model.Password, false, false);
        
        if (result.Succeeded)
        {
            var tokenModel = await _jwtService.GenerateTokenPairAsync(user);
            
            return new UserInfoResponseModel
            {
                Content = new UserInfoContent
                {
                    Id = user.Id,
                    UserName = user.UserName,
                    AccessToken = tokenModel.AccessToken,
                    RefreshToken = tokenModel.RefreshToken,
                    Email = user.Email,
                    Roles = (await _userManager.GetRolesAsync(user)).ToArray()
                }
            };
        }
        
        return new UserInfoResponseModel
        {
            Errors = new[]
            {
                "Invalid login attempt"
            }
        };
    }
    
    public async Task<UserInfoResponseModel> ChangePasswordAsync(ChangePasswordModel model)
    {
        var jwtEmail = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Email)
            ?.Value;
        
        if (string.IsNullOrEmpty(jwtEmail) || !jwtEmail.Equals(model.Email, StringComparison.OrdinalIgnoreCase))
            return new UserInfoResponseModel
            {
                Errors = new[]
                {
                    "You can only change your own password!"
                }
            };
        
        var user = await _userManager.FindByEmailAsync(model.Email);
        
        if (user is null)
            return new UserInfoResponseModel
            {
                Errors = new[]
                {
                    "User not found!"
                }
            };
        
        var passwordCheck = _userManager.PasswordHasher.VerifyHashedPassword
            (user!, user.PasswordHash!, model.OldPassword);
        
        if (passwordCheck is PasswordVerificationResult.Failed)
            return new UserInfoResponseModel
            {
                Errors = new[]
                {
                    "The old password is not correct!"
                }
            };
        
        user.PasswordHash = _userManager.PasswordHasher.HashPassword(user, model.NewPassword);
        
        var updateResult = await _userManager.UpdateAsync(user);
        
        if (updateResult.Succeeded)
        {
            var tokenModel = await _jwtService.GenerateTokenPairAsync(user);
            
            return new UserInfoResponseModel
            {
                Content = new UserInfoContent
                {
                    Id = user.Id,
                    UserName = user.UserName,
                    AccessToken = tokenModel.AccessToken,
                    RefreshToken = tokenModel.RefreshToken,
                    Email = user.Email,
                    Roles = (await _userManager.GetRolesAsync(user)).ToArray()
                }
            };
        }
        
        return new UserInfoResponseModel
        {
            Errors = new[]
            {
                "Invalid change password attempt!"
            }
        };
    }
}
