using Gym.Domain.Commons;
using Gym.Domain.Enums;

namespace Gym.Domain.Entities;

public class User : Auditable
{
    public string Name { get; set; }
    public string Phone { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public Gender Gender { get; set; }
    public UserRole Role { get; set; }
}