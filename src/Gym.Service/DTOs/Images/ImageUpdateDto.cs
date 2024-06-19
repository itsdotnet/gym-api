using Microsoft.AspNetCore.Http;

namespace Gym.Service.DTOs.Images;

public class ImageUpdateDto
{
    public long Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    
    public IFormFile? Image { get; set; }
}