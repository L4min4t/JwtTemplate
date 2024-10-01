namespace App.Models.Auth;

public class RegisterResponseModel
{
    public bool IsSuccessed { get; set; }
    public string? Id { get; set; }
    public string? Email { get; set; }
    public string[]? Errors { get; set; }
}
