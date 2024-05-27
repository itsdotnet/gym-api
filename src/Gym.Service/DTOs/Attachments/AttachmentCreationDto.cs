using Microsoft.AspNetCore.Http;

namespace Gym.Service.DTOs.Attachments;

public class AttachmentCreationDto
{
    public IFormFile File { get; set; }
}