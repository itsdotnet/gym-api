using Gym.Service.DTOs.Videos;
using Gym.Service.Services;
using Gym.WebApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Gym.WebApi.Controllers;

public class VideosController : BaseController
{
    private readonly VideoService _videoService;

    public VideosController(VideoService videoService)
    {
        _videoService = videoService;
    }

    [Authorize(Roles = "Admin")]
    [HttpDelete("delete/{id:long}")]
    public async Task<IActionResult> DeleteAsync(long id)
        => Ok(new Response
        {
            StatusCode = 200,
            Message = "Success",
            Data = await _videoService.DeleteAsync(id)
        });

    [HttpGet("get/{id:long}")]
    public async Task<IActionResult> GetByIdAsync(long id)
        => Ok(new Response
        {
            StatusCode = 200,
            Message = "Success",
            Data = await _videoService.GetByIdAsync(id)
        });

    [HttpGet("get-all")]
    public async Task<IActionResult> GetAllAsync()
        => Ok(new Response
        {
            StatusCode = 200,
            Message = "Success",
            Data = await _videoService.GetAllAsync()
        });

    [HttpGet("get-by-course/{id:long}")]
    public async Task<IActionResult> GetWithCourseIdAsync(long courseId)
        => Ok(new Response
        {
            StatusCode = 200,
            Message = "Success",
            Data = await _videoService.GetByCourseAsync(courseId)
        });

    [Authorize(Roles = "Admin")]
    [HttpPost("create")]
    public async Task<IActionResult> CreateAsync(VideoCreationDto dto)
        => Ok(new Response
        {
            StatusCode = 200,
            Message = "Success",
            Data = await _videoService.CreateAsync(dto)
        });

    [Authorize(Roles = "Admin")]
    [HttpPut("update")]
    public async Task<IActionResult> UpdateAsync(VideoUpdateDto dto)
        => Ok(new Response
        {
            StatusCode = 200,
            Message = "Success",
            Data = await _videoService.UpdateAsync(dto)
        });
}