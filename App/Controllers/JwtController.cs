using App.Models.Token;
using App.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace App.Controllers;

[ApiController]
[Route("[controller]/[action]")]
public class JwtController : ControllerBase
{
    protected readonly IJwtService _jwtService;
    
    public JwtController(IJwtService jwtService)
    {
        _jwtService = jwtService;
    }
    
    [HttpPost]
    public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenModel model)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        
        var result = await _jwtService.RefreshTokenAsync(model);
        
        return result.IsSuccessed
            ? Ok(result)
            : BadRequest(result);
    }
}
