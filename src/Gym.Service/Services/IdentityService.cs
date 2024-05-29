using AutoMapper;
using Gym.Domain.Enums;
using Gym.Repository.UnitOfWorks;
using Gym.Service.DTOs.Users;
using Gym.Service.Exceptions;
using Gym.Service.Helpers;
using Gym.Service.Interfaces;

namespace Gym.Service.Services;

public class IdentityService : IIdentityService
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;
    
    public IdentityService(IMapper mapper, IUnitOfWork unitOfWork)
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
    }

    public async Task<string> CurrentRole()
    {
        return HttpContextHelper.UserRole;
    }

    public async Task<UserResultDto> CurrentUser()
    {
        var user = await _unitOfWork.UserRepository.SelectAsync(x => x.Id == HttpContextHelper.GetUserId);
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