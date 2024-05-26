namespace Gym.Domain.Constants;

public static class TimeConstants
{
    public static int UTC = 5;
    
    public static DateTime Now() 
        => DateTime.UtcNow.AddHours(UTC);
}