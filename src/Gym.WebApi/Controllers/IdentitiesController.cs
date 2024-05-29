using Gym.Service.Interfaces;
using Gym.WebApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Gym.WebApi.Controllers;

public class IdentitiesController : BaseController
{
    private readonly IIdentityService _identityService;

    public IdentitiesController(IIdentityService identityService)
    {
        _identityService = identityService;
    }

    [HttpGet("role")]
    public async Task<IActionResult> CurrentRole()
        => Ok(new Response
        {
            StatusCode = 200,
            Message = "User role selected",
            Data = await _identityService.CurrentRole()
        });
    
    
    [HttpGet("user")]
    public async Task<IActionResult> CurrentUser()
        => Ok(new Response
        {
            StatusCode = 200,
            Message = "User selected",
            Data = await _identityService.CurrentUser()
        });
    
    
    [Authorize(Roles = "Admin")]
    [HttpPut("upgrade/{userId:long}")]
    public async Task<IActionResult> UpgradeRole(long userId)
        => Ok(new Response
        {
            StatusCode = 200,
            Message = "User role upgraded",
            Data = await _identityService.UpgradeRole(userId)
        });
    
    
    [Authorize(Roles = "Admin")]
    [HttpPut("downgrade/{userId:long}")]
    public async Task<IActionResult> DowngradeRole(long userId)
        => Ok(new Response
        {
            StatusCode = 200,
            Message = "User role downgraded",
            Data = await _identityService.DowngradeRole(userId)
        });
}