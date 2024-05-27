using Microsoft.AspNetCore.Http;

namespace Gym.Service.DTOs.Videos;

public class VideoUpdateDto
{
    public long Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string Teacher { get; set; }
    
    public long CourseId { get; set; }
    public IFormFile? ImagePath { get; set; }
}