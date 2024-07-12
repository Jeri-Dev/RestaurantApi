namespace RestaurantApi.Core.Application.Dtos.Account;

public class AuthRequest
{
    public string Email { get; set; }
    public string Password { get; set; }
}