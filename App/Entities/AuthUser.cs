using Microsoft.AspNetCore.Identity;

namespace App.Entities;

public class AuthUser : IdentityUser
{
    public string? RefreshToken { get; set; }
    public DateTime? RefreshTokenExpiryTime { get; set; }
}
