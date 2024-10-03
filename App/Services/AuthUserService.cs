using App.Entities;
using App.Models.Common;
using App.Models.User.Request;
using App.Models.User.Response;
using App.Services.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace App.Services;

public class AuthUserService : IAuthUserService
{
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly UserManager<AuthUser> _userManager;
    
    public AuthUserService(UserManager<AuthUser> userManager, RoleManager<IdentityRole> roleManager)
    {
        _userManager = userManager;
        _roleManager = roleManager;
    }
    
    public async Task<UserResponseModel> GetById(string id)
    {
        var user = await _userManager.FindByIdAsync(id);
        if (user == null)
            return new UserResponseModel
            {
                Errors = new[]
                {
                    "User not found!"
                }
            };
        
        var roles = await _userManager.GetRolesAsync(user);
        
        return new UserResponseModel
        {
            Content = new UserContent
            {
                Id = user.Id,
                Email = user.Email,
                Name = user.UserName,
                RefreshToken = user.RefreshToken,
                RefreshTokenExpiryTime = user.RefreshTokenExpiryTime,
                Roles = roles.ToArray()
            }
        };
    }
    
    public async Task<UsersResponseModel> GetAll()
    {
        var users = _userManager.Users.ToList();
        var userResponses = new List<UserContent>();
        
        foreach (var user in users)
        {
            var roles = await _userManager.GetRolesAsync(user);
            userResponses.Add
            (
                new UserContent
                {
                    Id = user.Id,
                    Email = user.Email,
                    Name = user.UserName,
                    RefreshToken = user.RefreshToken,
                    RefreshTokenExpiryTime = user.RefreshTokenExpiryTime,
                    Roles = roles.ToArray()
                }
            );
        }
        
        return new UsersResponseModel
        {
            Content = userResponses
        };
    }
    
    public async Task<DeleteResponseModel> DeleteById(string id)
    {
        var user = await _userManager.FindByIdAsync(id);
        if (user == null)
            return new DeleteResponseModel
            {
                Errors = new[]
                {
                    "User not found!"
                }
            };
        
        var result = await _userManager.DeleteAsync(user);
        if (!result.Succeeded)
            return new DeleteResponseModel
            {
                Errors = result.Errors.Select(e => e.Description)
                    .ToArray()
            };
        
        return new DeleteResponseModel
        {
            Content = true
        };
    }
    
    public async Task<UserResponseModel> UpdateRolesAsync(UpdateUserRolesModel model)
    {
        var user = await _userManager.FindByIdAsync(model.UserId);
        if (user == null)
            return new UserResponseModel
            {
                Errors = new[]
                {
                    "User not found!"
                }
            };
        
        foreach (var role in model.RoleNames)
            if (await _roleManager.FindByIdAsync(role) != null)
                return new UserResponseModel
                {
                    Errors = new[]
                    {
                        $"Role '{role}' does not exist"
                    }
                };
        
        var currentRoles = await _userManager.GetRolesAsync(user);
        
        var removeRolesResult = await _userManager.RemoveFromRolesAsync(user, currentRoles);
        if (!removeRolesResult.Succeeded)
            return new UserResponseModel
            {
                Errors = removeRolesResult.Errors.Select(e => e.Description)
                    .ToArray()
            };
        
        var addRolesResult = await _userManager.AddToRolesAsync(user, model.RoleNames);
        if (!addRolesResult.Succeeded)
            return new UserResponseModel
            {
                Errors = addRolesResult.Errors.Select(e => e.Description)
                    .ToArray()
            };
        
        var updatedRoles = await _userManager.GetRolesAsync(user);
        
        return new UserResponseModel
        {
            Content = new UserContent
            {
                Id = user.Id,
                Email = user.Email,
                Name = user.UserName,
                RefreshToken = user.RefreshToken,
                RefreshTokenExpiryTime = user.RefreshTokenExpiryTime,
                Roles = updatedRoles.ToArray()
            }
        };
    }
}
