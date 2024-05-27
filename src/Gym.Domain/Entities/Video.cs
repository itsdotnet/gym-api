using Gym.Domain.Commons;

namespace Gym.Domain.Entities;

public class Video : Auditable
{
    public string Name { get; set; }
    public string Description { get; set; }
    public string Teacher { get; set; }
    public decimal Price { get; set; }
    
    public long CourseId { get; set; }
    public Course Course { get; set; }
    
    public long AttachmentId { get; set; }
    public Attachment Attachment { get; set; }
}