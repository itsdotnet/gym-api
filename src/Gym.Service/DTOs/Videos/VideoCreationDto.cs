using Gym.Service.DTOs.Courses;
using Microsoft.AspNetCore.Http;

namespace Gym.Service.DTOs.Videos;

public class VideoCreationDto
{
    public string Name { get; set; }
    public string Description { get; set; }
    public string Teacher { get; set; }
    
    public long CourseId { get; set; }
    public IFormFile? Video { get; set; }
}