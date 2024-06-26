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
        var user = await _unitOfWork.UserRepository.SelectAsync(q => q.Id == id);

        if (user is null)
            throw new NotFoundException("User not found");
        
        return _mapper.Map<UserResultDto>(user);
    }

    public async Task<IEnumerable<UserResultDto>> GetAllAsync()
    {
        var users = (IEnumerable<User>)_unitOfWork.UserRepository.SelectAll();

        return _mapper.Map<IEnumerable<UserResultDto>>(users);
    }

    public async Task<UserResultDto> UpdateAsync(UserUpdateDto dto)
    {   
        if (!Validator.IsValidPhoneNumber(dto.Phone) || !Validator.IsValidName(dto.Firstname) ||
            !Validator.IsValidName(dto.Lastname))
            throw new CustomException(401, "Invalid informations");
        
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
        if (!Validator.IsValidPhoneNumber(dto.Phone) || !Validator.IsValidName(dto.Firstname) ||
            !Validator.IsValidName(dto.Lastname))
            throw new CustomException(401, "Invalid informations");
        
        var exist = await _unitOfWork.UserRepository.SelectAsync(q => q.Phone == dto.Phone);

        if (exist is not null)
            throw new AlreadyExistException("User already exist with this Email");
        dto.Password = PasswordHasher.Hash(dto.Password);        
        var newUser = _mapper.Map<User>(dto);
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
        if(oldPass == newPass)
            throw new CustomException(400, "Password cant be equal to old password");
            
        var exist = await _unitOfWork.UserRepository.SelectAsync(q => q.Id == id);

        if (exist is null)
            throw new NotFoundException("User not found");

        var isCorrect = PasswordHasher.Verify(oldPass, exist.Password);
        if(!isCorrect)    
            throw new CustomException(401, $"Password {oldPass} is invalid");
        
        if (!Validator.IsValidPassword(newPass))
            throw new CustomException(400, "New password is too weak");
        
        exist.Password = newPass.Hash();
        await _unitOfWork.SaveAsync();

        return _mapper.Map<UserResultDto>(exist);
    }
}