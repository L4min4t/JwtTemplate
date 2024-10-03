namespace App.Models.User.Response;

public class UserContent
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string? RefreshToken { get; set; }
    public DateTime? RefreshTokenExpiryTime { get; set; }
    public string[] Roles { get; set; }
}
