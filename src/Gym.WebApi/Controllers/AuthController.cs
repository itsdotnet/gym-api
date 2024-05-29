using Gym.Service.DTOs.Users;
using Gym.Service.Interfaces;
using Gym.WebApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Gym.WebApi.Controllers;

public class AuthController : BaseController
{
    private readonly IAuthService authService;

    public AuthController(IAuthService authService)
    {
        this.authService = authService;
    }
    
    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<IActionResult> LoginAsync(LoginModel model)
    {
        var serviceResult = await authService.LoginAsync(model.Phone, model.Password);
        if(serviceResult is not null)
            return Ok(new Response()
            {
                StatusCode = 200,
                Message = "Login successful",
                Data = serviceResult
            });
        else
            return BadRequest(new Response()
            {
                StatusCode = 401,
                Message = "Invalid password"
            });
        
    }
    [HttpPost("register")]
    [AllowAnonymous]
    public async Task<IActionResult> RegisterAsync(UserCreationDto registerDto)
    {
        await authService.RegisterAsync(registerDto);
        var serviceResult = await authService.SendCodeForRegisterAsync($"{registerDto.Phone}");
        return Ok(new Response()
        {
            StatusCode = 200,
            Message = "User registered successfully",
            Data = serviceResult
        });
    }
    
    [HttpPost("register/verify")]
    [AllowAnonymous]
    public async Task<IActionResult> VerifyRegisterAsync(VerifyModel model)
    {
        var serviceResult = await authService.VerifyRegisterAsync(model.Phone, int.Parse(model.Code));
        if (serviceResult.Result == false)
        {
            return BadRequest(new Response()
            {
                StatusCode = 401,
                Message = "Invalid code"
            });
        }
        return Ok(new Response()
        {
            StatusCode = 200,
            Message = "User verfied successfully",
            Data = serviceResult.Token
        });
    }
}