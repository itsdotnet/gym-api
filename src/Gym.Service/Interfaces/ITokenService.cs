using Gym.Domain.Entities;

namespace Gym.Service.Interfaces;


public interface ITokenService
{
    public string GenerateToken(User user);
}