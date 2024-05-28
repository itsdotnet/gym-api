using AutoMapper;
using Gym.Domain.Entities;
using Gym.Repository.UnitOfWorks;
using Gym.Service.DTOs.Users;
using Gym.Service.Exceptions;
using Gym.Service.Helpers;
using Gym.Service.Interfaces;

namespace Gym.Service.Services;

public class UserService : IUserService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public UserService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<bool> DeleteAsync(long id)
    {
        var user = await _unitOfWork.UserRepository.SelectAsync(q => q.Id == id);

        if (user is null)
            throw new NotFoundException("User not found");

        await _unitOfWork.UserRepository.DeleteAsync(x => x == user);
        return await _unitOfWork.SaveAsync();
    }

    public async Task<UserResultDto> PayAsync(long id)
    {
        var exist = await _unitOfWork.UserRepository.SelectAsync(d => d.Id == id);
    
        if (exist is null)
            throw new NotFoundException("User not found");
        if (exist.IsPayed)
            throw new CustomException(400, "This user is already payed");
        exist.IsPayed = true;
        
        await _unitOfWork.UserRepository.UpdateAsync(exist);
        await _unitOfWork.SaveAsync();

        return _mapper.Map<UserResultDto>(exist);
    }

    public async Task<UserResultDto> RemovePayAsync(long id)
    {
        var exist = await _unitOfWork.UserRepository.SelectAsync(d => d.Id == id);
    
        if (exist is null)
            throw new NotFoundException("User not found");
        if (exist.IsPayed is false)
            throw new CustomException(400, "This user not payed yet");
        exist.IsPayed = false;
        
        await _unitOfWork.UserRepository.UpdateAsync(exist);
        await _unitOfWork.SaveAsync();

        return _mapper.Map<UserResultDto>(exist);
    }

    public async Task<UserResultDto> GetByIdAsync(long id)
    {
        var user = await _unitOfWork.UserRepository.SelectAsync(q => q.Id == id, new []{"Attachment"});

        if (user is null)
            throw new NotFoundException("User not found");
        
        return _mapper.Map<UserResultDto>(user);
    }

    public async Task<IEnumerable<UserResultDto>> GetAllAsync()
    {
        var users = (IEnumerable<User>)_unitOfWork.UserRepository.SelectAll(includes: new []{"Attachment"});

        return _mapper.Map<IEnumerable<UserResultDto>>(users);
    }

    public async Task<UserResultDto> UpdateAsync(UserUpdateDto dto)
    {   
        var exist = await _unitOfWork.UserRepository.SelectAsync(d => d.Id == dto.Id);
    
        if (exist is null)
            throw new NotFoundException("User not found");
        if (dto.Firstname == exist.Firstname && dto.Lastname == exist.Lastname && dto.Phone == exist.Phone)
            throw new CustomException(400, "You changed nothing");
        
        _mapper.Map(dto, exist);

        await _unitOfWork.UserRepository.UpdateAsync(exist);
        await _unitOfWork.SaveAsync();

        return _mapper.Map<UserResultDto>(exist);
    }

    public async Task<UserResultDto> CreateAsync(UserCreationDto dto)
    {
        var exist = await _unitOfWork.UserRepository.SelectAsync(q => q.Phone == dto.Phone);

        if (exist is not null)
            throw new AlreadyExistException("User already exist with this Email");
        
        var newUser = _mapper.Map<User>(dto);
        newUser.Password = PasswordHasher.Hash(newUser.Password);
        await _unitOfWork.UserRepository.AddAsync(newUser);
        await _unitOfWork.SaveAsync();

        return _mapper.Map<UserResultDto>(newUser);
    }

    public async Task<IEnumerable<UserResultDto>> GetByName(string name)
    {
        
        var users = _unitOfWork.UserRepository.SelectAll(u =>
            u.Firstname.Contains(name) || u.Lastname.Contains(name));

        return _mapper.Map<IEnumerable<UserResultDto>>(users);
    }

    public async Task<UserResultDto> UpdatePasswordAsync(long id, string oldPass, string newPass)
    {
        var exist = await _unitOfWork.UserRepository.SelectAsync(q => q.Id == id);

        if (exist is null)
            throw new NotFoundException("User not found");

        if (oldPass.Verify(exist.Password))
            throw new CustomException(403, "Passwor is invalid");

        exist.Password = newPass.Hash();
        await _unitOfWork.SaveAsync();

        return _mapper.Map<UserResultDto>(exist);
    }
}