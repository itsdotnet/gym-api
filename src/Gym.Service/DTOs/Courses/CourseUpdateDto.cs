using Microsoft.AspNetCore.Http;

namespace Gym.Service.DTOs.Courses;

public class CourseUpdateDto
{
    public long Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public IFormFile? Image { get; set; }
}