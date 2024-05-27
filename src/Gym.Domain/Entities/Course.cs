using Gym.Domain.Commons;

namespace Gym.Domain.Entities;

public class Course : Auditable
{
    public string Title { get; set; }
    public string Description { get; set; }
    public IList<Video> Videos { get; set; }
}