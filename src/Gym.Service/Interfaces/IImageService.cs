using Gym.Service.DTOs.Images;

namespace Gym.Service.Interfaces;

public interface IImageService
{
    Task<bool> DeleteAsync(long id);
    Task<ImageResultDto> GetByIdAsync(long id);
    Task<IEnumerable<ImageResultDto>> GetAllAsync();
    Task<ImageResultDto> CreateAsync(ImageCreationDto dto);
    Task<ImageResultDto> UpdateAsync(ImageUpdateDto dto);
}