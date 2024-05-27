using Gym.Service.DTOs.Videos;

namespace Gym.Service.Interfaces;

public interface IVideoService
{
    Task<bool> DeleteAsync(long id);
    Task<VideoResultDto> GetByIdAsync(long id);
    Task<IEnumerable<VideoResultDto>> GetAllAsync();
    Task<IEnumerable<VideoResultDto>> GetByCourseAsync(long courseId);
    Task<VideoResultDto> CreateAsync(VideoCreationDto dto);
    Task<VideoResultDto> UpdateAsync(VideoUpdateDto dto);
}