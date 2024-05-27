using Gym.Service.DTOs.Attachments;

namespace Gym.Service.DTOs.Images;

public class ImageResultDto
{
    public long Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    
    public AttachmentResultDto Attachment { get; set; }
}