using Gym.Domain.Enums;

namespace Gym.Service.DTOs.Users;

public class UserResultDto
{
    public long Id { get; set; }
    public string Firstname { get; set; }
    public string Lastname { get; set; }
    public string Phone { get; set; }
    public bool IsPayed { get; set; }
    public UserRole Role { get; set; }
}