using System.ComponentModel.DataAnnotations;

namespace App.Models.Auth;

public class ChangePasswordModel
{
    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Invalid email address format")]
    public string Email { get; set; }
    
    [Required(ErrorMessage = "Old password is required")]
    [StringLength(100, ErrorMessage = "Password must be between 6 and 100 characters", MinimumLength = 6)]
    [DataType(DataType.Password)]
    public string OldPassword { get; set; }
    
    [Required(ErrorMessage = "New password is required")]
    [DataType(DataType.Password)]
    public string NewPassword { get; set; }
}
