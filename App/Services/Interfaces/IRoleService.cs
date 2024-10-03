using App.Models.Common;
using App.Models.Role.Request;
using App.Models.Role.Response;

namespace App.Services.Interfaces;

public interface IRoleService
{
    Task<RolesResponseModel> GetAllAsync();
    Task<RoleResponseModel> CreateAsync(CreateRoleModel name);
    Task<RoleResponseModel> GetByIdAsync(string id);
    Task<RoleResponseModel> EditAsync(EditRoleModel model);
    Task<DeleteResponseModel> DeleteByIdAsync(string id);
}
