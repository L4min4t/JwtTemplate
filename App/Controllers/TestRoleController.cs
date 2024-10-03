using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace App.Controllers;

[ApiController]
[Route("[controller]/[action]")]
public class TestRoleController : ControllerBase
{
    [HttpGet]
    public IActionResult NonAuth()
    {
        return Ok(GetUserInfo());
    }
    
    [HttpGet]
    [Authorize]
    public IActionResult Auth()
    {
        return Ok(GetUserInfo());
    }
    
    [HttpGet]
    [Authorize(Roles = "Admin")]
    public IActionResult Admin()
    {
        return Ok(GetUserInfo());
    }
    
    private object GetUserInfo()
    {
        var user = HttpContext.User;
        
        if (user.Identity.IsAuthenticated)
        {
            var userClaims = user.Claims.Select
            (
                c => new
                {
                    c.Type,
                    c.Value
                }
            );
            
            return new
            {
                user.Identity.IsAuthenticated,
                UserId = user.FindFirst(ClaimTypes.NameIdentifier)
                    ?.Value,
                UserName = user.FindFirst(ClaimTypes.Name)
                    ?.Value,
                Email = user.FindFirst(ClaimTypes.Email)
                    ?.Value,
                Roles = user.FindFirst(ClaimTypes.Role)
                    ?.Value,
                Claims = userClaims
            };
        }
        
        return new
        {
            IsAuthenticated = false
        };
    }
}
