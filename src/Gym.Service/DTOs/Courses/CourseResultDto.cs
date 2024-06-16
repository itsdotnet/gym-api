using Gym.Domain.Entities;
using Gym.Service.DTOs.Attachments;
using Gym.Service.DTOs.Videos;

namespace Gym.Service.DTOs.Courses;

public class CourseResultDto
{
    public long Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }

    public AttachmentResultDto Attachment { get; set; }
    
    public IList<VideoResultDto> Videos { get; set; }
}