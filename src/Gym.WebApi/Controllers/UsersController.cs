using Gym.Service.DTOs.Users;
using Gym.Service.Helpers;
using Gym.Service.Interfaces;
using Gym.WebApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Gym.WebApi.Controllers;

public class UsersController : BaseController
{
    private readonly IUserService userService;

    public UsersController(IUserService userService)
    {
        this.userService = userService;
    }
    
    [HttpPost("create")]
    public async Task<IActionResult> CreateAsync(UserCreationDto dto)
    {
        
        var nameValid = Validator.IsValidName(dto.Firstname);
        var surnameValid = Validator.IsValidName(dto.Lastname);

        if (nameValid && surnameValid)
            return Ok(new Response
            {
                StatusCode = 200,
                Message = "Success",
                Data = await this.userService.CreateAsync(dto)
            });

        return BadRequest(new Response
        {
            StatusCode = 400,
            Message = "Invalid Information",
        });
    }
    
    [HttpPut("update")]
    public async Task<IActionResult> UpdateAsync(UserUpdateDto dto)
    {
        var nameValid = Validator.IsValidName(dto.Firstname);
        var surnameValid = Validator.IsValidName(dto.Lastname);

        if (nameValid && surnameValid)
            return Ok(new Response
            {
                StatusCode = 200,
                Message = "Success",
                Data = await this.userService.UpdateAsync(dto)
            });

        return BadRequest(new Response
        {
            StatusCode = 400,
            Message = "Invalid Information",
        });
    }

    [HttpDelete("delete/{id:long}")]
    public async Task<IActionResult> DeleteAsync(long id)
        => Ok(new Response
        {
            StatusCode = 200,
            Message = "Success",
            Data = await this.userService.DeleteAsync(id)
        });

    [HttpGet("get/{id:long}")]
    public async Task<IActionResult> GetByIdAsync(long id)
        => Ok(new Response
        {
            StatusCode = 200,
            Message = "Success",
            Data = await this.userService.GetByIdAsync(id)
        });

    [Authorize(Roles = "Admin")] 
    [HttpGet("get-all")]
    public async Task<IActionResult> GetAllAsync()
        => Ok(new Response
        {
            StatusCode = 200,
            Message = "Success",
            Data = await this.userService.GetAllAsync()
        });

    [Authorize(Roles = "Admin")] 
    [HttpGet("get-name")]
    public async Task<IActionResult> GetByNameAsync(string name)
        => Ok(new Response
        {
            StatusCode = 200,
            Message = "Success",
            Data = await this.userService.GetByName(name)
        });

    [HttpPut("update-password")]
    public async Task<IActionResult> UpdatePasswordAsync(long id, string oldPass, string newPass)
        => Ok(new Response
            {
                StatusCode = 200,
                Message = "Success",
                Data = await this.userService.UpdatePasswordAsync(id, oldPass, newPass)
            });

    [Authorize(Roles = "Admin")] 
    [HttpPut("add-payment")]
    public async Task<ActionResult> AddPayment(long id)
    {
        return Ok(new Response()
        {
            StatusCode = 200,
            Message = "User payment added",
            Data = await this.userService.PayAsync(id)
        });
    }
    
    [Authorize(Roles = "Admin")] 
    [HttpPut("remove-payment")]
    public async Task<ActionResult> RemovePayment(long id)
    {
        return Ok(new Response()
        {
            StatusCode = 200,
            Message = "User payment removed",
            Data = await this.userService.RemovePayAsync(id)
        });
    }
}