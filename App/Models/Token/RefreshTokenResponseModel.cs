namespace App.Models.Token;

public class RefreshTokenResponseModel
{
    public bool IsSuccessed { get; set; }
    
    public string? AccessToken { get; set; }
    public string? RefreshToken { get; set; }
    
    public string[]? Errors { get; set; }
}
