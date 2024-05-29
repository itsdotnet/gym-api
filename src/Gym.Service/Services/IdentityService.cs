using AutoMapper;
using Gym.Domain.Enums;
using Gym.Repository.UnitOfWorks;
using Gym.Service.DTOs.Users;
using Gym.Service.Exceptions;
using Gym.Service.Helpers;
using Gym.Service.Interfaces;
using Microsoft.AspNetCore.Http;

namespace Gym.Service.Services;

public class IdentityService : IIdentityService
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IHttpContextAccessor _httpContextAccessor;
    
    public IdentityService(IMapper mapper, IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor)
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<string> CurrentRole()
    {
        return _httpContextAccessor.HttpContext?.User.FindFirst("Role")?.Value;
    }

    public async Task<UserResultDto> CurrentUser()
    {
        long userId = long.Parse(_httpContextAccessor.HttpContext.User.FindFirst("Id")?.Value);
        var user = await _unitOfWork.UserRepository
            .SelectAsync(x => x.Id == userId);
        if (user is null)
            throw new NotFoundException("User not found");
        
        return _mapper.Map<UserResultDto>(user);
    }

    public async Task<bool> UpgradeRole(long userId)
    {
        var user = await _unitOfWork.UserRepository.SelectAsync(x => x.Id == userId);
        if (user is null)
            throw new NotFoundException("User not found");
        user.Role = UserRole.Admin;
        await _unitOfWork.UserRepository.UpdateAsync(user);
        await _unitOfWork.SaveAsync();
        return true;
    }
    
    public async Task<bool> DowngradeRole(long userId)
    {
        var user = await _unitOfWork.UserRepository.SelectAsync(x => x.Id == userId);
        if (user is null)
            throw new NotFoundException("User not found");
        user.Role = UserRole.User;
        await _unitOfWork.UserRepository.UpdateAsync(user);
        await _unitOfWork.SaveAsync();
        return true;
    }
}