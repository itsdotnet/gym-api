using Gym.Domain.Commons;

namespace Gym.Domain.Entities;

public class Attachment : Auditable
{
    public string FileName { get; set; }
    public string FilePath { get; set; }
}   