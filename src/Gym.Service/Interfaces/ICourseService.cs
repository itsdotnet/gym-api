using Gym.Service.DTOs.Courses;

namespace Gym.Service.Interfaces;

public interface ICourseService
{
    Task<bool> DeleteAsync(long id);
    Task<CourseResultDto> GetByIdAsync(long id);
    Task<IEnumerable<CourseResultDto>> GetAllAsync();
    Task<CourseResultDto> GetByIdWithVideosAsync(long id);
    Task<CourseResultDto> CreateAsync(CourseCreationDto dto);
    Task<CourseResultDto> UpdateAsync(CourseUpdateDto dto);
}