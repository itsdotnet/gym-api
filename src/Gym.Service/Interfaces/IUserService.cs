using Gym.Service.DTOs.Users;

namespace Gym.Service.Interfaces;

public interface IUserService
{
    Task<bool> DeleteAsync(long id);
    Task<UserResultDto> GetByIdAsync(long id);
    Task<IEnumerable<UserResultDto>> GetAllAsync();
    Task<UserResultDto> UpdateAsync(UserUpdateDto dto);
    Task<UserResultDto> CreateAsync(UserCreationDto dto);
    Task<IEnumerable<UserResultDto>> GetByName(string name);
    Task<UserResultDto> UpdatePasswordAsync(long id, string oldPass, string newPass);
}