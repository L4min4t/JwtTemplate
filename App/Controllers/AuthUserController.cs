using App.Extensions;
using App.Models.User.Request;
using App.Models.User.Response;
using App.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace App.Controllers;

[ApiController]
[Authorize]
[Route("[controller]/[action]")]
public class AuthUserController : ControllerBase
{
    protected readonly IAuthUserService _authUserService;
    
    public AuthUserController(IAuthUserService authUserService)
    {
        _authUserService = authUserService;
    }
    
    [HttpGet]
    [Route("{id:guid}")]
    public async Task<IActionResult> GetById([FromRoute] string id)
    {
        var result = await _authUserService.GetById(id);
        
        return result.IsSuccessed
            ? Ok(result.Content)
            : BadRequest(result.Errors);
    }
    
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _authUserService.GetAll();
        
        return result.IsSuccessed
            ? Ok(result.Content)
            : BadRequest(result.Errors);
    }
    
    [HttpDelete]
    [Authorize(Roles = "Admin")]
    [Route("{id:guid}")]
    public async Task<IActionResult> Delete([FromRoute] string id)
    {
        var result = await _authUserService.DeleteById(id);
        
        return result.IsSuccessed
            ? Ok(result.Content)
            : BadRequest(result.Errors);
    }
    
    [HttpPut]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> UpdateRoles([FromBody] UpdateUserRolesModel model)
    {
        if (!ModelState.IsValid)
            return BadRequest
            (
                new UserResponseModel
                {
                    Errors = ModelState.GetErrorMessages()
                }
            );
        
        var result = await _authUserService.UpdateRolesAsync(model);
        
        return result.IsSuccessed
            ? Ok(result.Content)
            : BadRequest(result.Errors);
    }
}
