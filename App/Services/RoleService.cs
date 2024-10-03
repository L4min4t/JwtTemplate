using App.Models.Common;
using App.Models.Role.Request;
using App.Models.Role.Response;
using App.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace App.Services;

public class RoleService : IRoleService
{
    protected readonly RoleManager<IdentityRole> _roleManager;
    
    public RoleService(RoleManager<IdentityRole> roleManager)
    {
        _roleManager = roleManager;
    }
    
    public async Task<RolesResponseModel> GetAllAsync()
    {
        var roles = await _roleManager.Roles.ToListAsync();
        
        return new RolesResponseModel
        {
            Content = roles.Select
                (
                    r => new RoleContent
                    {
                        Id = r.Id,
                        Name = r.Name
                    }
                )
                .ToList()
        };
    }
    
    public async Task<RoleResponseModel> CreateAsync(CreateRoleModel model)
    {
        var name = model.Name.Trim();
        name = char.ToUpper(name[0]) +
               name.Substring(1)
                   .ToLower();
        
        var roleExists = await _roleManager.RoleExistsAsync(name);
        if (!roleExists)
        {
            var roleResult = await _roleManager.CreateAsync(new IdentityRole(name));
            if (roleResult.Succeeded)
            {
                var role = await _roleManager.FindByNameAsync(name);
                return new RoleResponseModel
                {
                    Content = new RoleContent
                    {
                        Id = role.Id,
                        Name = role.Name
                    }
                };
            }
            
            return new RoleResponseModel
            {
                Errors = roleResult.Errors.Select(e => e.Description)
                    .ToArray()
            };
        }
        
        return new RoleResponseModel
        {
            Errors = new[]
            {
                $"Role '{name}' already exists."
            }
        };
    }
    
    public async Task<RoleResponseModel> GetByIdAsync(string id)
    {
        var role = await _roleManager.FindByIdAsync(id);
        if (role != null)
            return new RoleResponseModel
            {
                Content = new RoleContent
                {
                    Id = role.Id,
                    Name = role.Name
                }
            };
        
        return new RoleResponseModel
        {
            Errors = new[]
            {
                "Role doesn't exist!"
            }
        };
    }
    
    public async Task<RoleResponseModel> EditAsync(EditRoleModel model)
    {
        var name = model.Name.Trim();
        name = char.ToUpper(name[0]) +
               name.Substring(1)
                   .ToLower();
        
        var role = await _roleManager.FindByIdAsync(model.Id);
        if (role != null)
        {
            role.Name = name;
            var result = await _roleManager.UpdateAsync(role);
            
            return result.Succeeded
                ? new RoleResponseModel
                {
                    Content = new RoleContent
                    {
                        Id = role.Id,
                        Name = role.Name
                    }
                }
                : new RoleResponseModel
                {
                    Errors = result.Errors.Select(e => e.Description)
                        .ToArray()
                };
        }
        
        return new RoleResponseModel
        {
            Errors = new[]
            {
                $"Role {model.Name} doesn't exist!"
            }
        };
    }
    
    public async Task<DeleteResponseModel> DeleteByIdAsync(string id)
    {
        var role = await _roleManager.FindByIdAsync(id);
        if (role != null)
        {
            var result = await _roleManager.DeleteAsync(role);
            
            return result.Succeeded
                ? new DeleteResponseModel
                {
                    Content = true
                }
                : new DeleteResponseModel
                {
                    Errors = result.Errors.Select(e => e.Description)
                        .ToArray()
                };
        }
        
        return new DeleteResponseModel
        {
            Errors = new[]
            {
                "Role doesn't exist!"
            }
        };
    }
}
