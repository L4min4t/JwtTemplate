namespace App.Models.Auth.Response;

public class UserInfoContent
{
    public string? Id { get; set; }
    public string? Email { get; set; }
    public string UserName { get; set; }
    public string? AccessToken { get; set; }
    public string? RefreshToken { get; set; }
    public string[]? Roles { get; set; }
}
