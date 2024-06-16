using Gym.Domain.Commons;

namespace Gym.Domain.Entities;

public class Course : Auditable
{
    public string Title { get; set; }
    public string Description { get; set; }
    
    public long? AttachmentId { get; set; }
    public Attachment Attachment { get; set; }
}