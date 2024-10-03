using App.Models.Common;
using App.Models.User.Request;
using App.Models.User.Response;

namespace App.Services.Interfaces;

public interface IAuthUserService
{
    Task<UserResponseModel> GetById(string id);
    Task<UsersResponseModel> GetAll();
    Task<DeleteResponseModel> DeleteById(string id);
    Task<UserResponseModel> UpdateRolesAsync(UpdateUserRolesModel model);
}
