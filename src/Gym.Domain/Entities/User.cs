using Gym.Domain.Commons;
using Gym.Domain.Enums;

namespace Gym.Domain.Entities;

public class User : Auditable
{
    public string Firstname { get; set; }
    public string Lastname { get; set; }
    public string Phone { get; set; }
    public string Password { get; set; }
    public bool IsPayed { get; set; }
    public UserRole Role { get; set; }
}