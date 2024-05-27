namespace Gym.Service.DTOs.Users;

public class UserUpdateDto
{
    public long Id { get; set; }
    public string Firstname { get; set; }
    public string Lastname { get; set; }
    public string Phone { get; set; }
    public bool IsPayed { get; set; }
}