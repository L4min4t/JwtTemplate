namespace App.Models.Auth;

public class LoginResponseModel
{
    public bool IsSuccessed { get; set; }
    
    public string? Id { get; set; }
    public string? Email { get; set; }
    public string UserName { get; set; }
    public string? AccessToken { get; set; }
    public string? RefreshToken { get; set; }
    public string[]? Roles { get; set; }
    
    public string[]? Errors { get; set; }
}
