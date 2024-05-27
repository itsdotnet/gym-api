using Gym.Service.DTOs.Users;

namespace Gym.Service.Interfaces;

public interface IAuthService
{
    Task<string> LoginAsync(string phone, string password);
    Task<(bool Result, int CashedMinutes)> RegisterAsync(UserCreationDto registerDto);
    Task<(bool Result, int CashedVerificationMinutes)> SendCodeForRegisterAsync(string phone);
    Task<(bool Result, string Token)> VerifyRegisterAsync(string phone, int code);
}   