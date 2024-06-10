using Gym.Domain.Entities;

namespace Gym.Service.DTOs.Videos;


public class VideoResultDto
{
    public long Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string Teacher { get; set; }
    
    public long CourseId { get; set; }
    
    public long AttachmentId { get; set; }
    public Attachment Attachment { get; set; }
}