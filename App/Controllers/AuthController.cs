using App.Extensions;
using App.Models.Auth.Request;
using App.Models.Auth.Response;
using App.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace App.Controllers;

[ApiController]
[Route("[controller]/[action]")]
public class AuthController : ControllerBase
{
    protected readonly IAuthService _authService;
    
    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }
    
    [HttpPost]
    public async Task<IActionResult> Register([FromBody] RegisterModel model)
    {
        if (!ModelState.IsValid)
            return BadRequest
            (
                new RegisterResponseModel
                {
                    Errors = ModelState.GetErrorMessages()
                }
            );
        
        var result = await _authService.RegisterAsync(model);
        
        return result.IsSuccessed
            ? Ok(result)
            : BadRequest(result);
    }
    
    [HttpPost]
    public async Task<IActionResult> Login([FromBody] LoginModel model)
    {
        if (!ModelState.IsValid)
            return BadRequest
            (
                new UserInfoResponseModel
                {
                    Errors = ModelState.GetErrorMessages()
                }
            );
        
        var result = await _authService.LoginAsync(model);
        
        return result.IsSuccessed
            ? Ok(result)
            : BadRequest(result);
    }
    
    [HttpPost]
    [Authorize]
    public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordModel model)
    {
        if (!ModelState.IsValid)
            return BadRequest
            (
                new UserInfoResponseModel
                {
                    Errors = ModelState.GetErrorMessages()
                }
            );
        
        var result = await _authService.ChangePasswordAsync(model);
        
        return result.IsSuccessed
            ? Ok(result)
            : BadRequest(result);
    }
}
