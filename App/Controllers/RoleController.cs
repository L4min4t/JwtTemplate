using App.Extensions;
using App.Models.Role.Request;
using App.Models.Role.Response;
using App.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace App.Controllers;

[ApiController]
[Authorize(Roles = "Admin")]
[Route("[controller]/[action]")]
public class RoleController : ControllerBase
{
    protected readonly IRoleService _roleService;
    
    public RoleController(IRoleService roleService)
    {
        _roleService = roleService;
    }
    
    [HttpPost]
    public async Task<IActionResult> CreateRole([FromBody] CreateRoleModel model)
    {
        if (!ModelState.IsValid)
            return BadRequest
            (
                new RoleResponseModel
                {
                    Errors = ModelState.GetErrorMessages()
                }
            );
        
        var result = await _roleService.CreateAsync(model);
        
        return result.IsSuccessed
            ? Ok(result)
            : BadRequest(result);
    }
    
    [HttpDelete]
    [Route("{id:guid}")]
    public async Task<IActionResult> Delete([FromRoute] string id)
    {
        var result = await _roleService.DeleteByIdAsync(id);
        
        return result.IsSuccessed
            ? Ok(result.Content)
            : BadRequest(result.Errors);
    }
    
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _roleService.GetAllAsync();
        
        return result.IsSuccessed
            ? Ok(result.Content)
            : BadRequest(result.Errors);
    }
    
    [HttpGet]
    [Route("{id:guid}")]
    public async Task<IActionResult> GetById([FromRoute] string id)
    {
        var result = await _roleService.GetByIdAsync(id);
        
        return result.IsSuccessed
            ? Ok(result.Content)
            : BadRequest(result.Errors);
    }
    
    [HttpPut]
    public async Task<IActionResult> Edit([FromBody] EditRoleModel model)
    {
        if (!ModelState.IsValid)
            return BadRequest
            (
                new RoleResponseModel
                {
                    Errors = ModelState.GetErrorMessages()
                }
            );
        
        var result = await _roleService.EditAsync(model);
        
        return result.IsSuccessed
            ? Ok(result.Content)
            : BadRequest(result.Errors);
    }
}
