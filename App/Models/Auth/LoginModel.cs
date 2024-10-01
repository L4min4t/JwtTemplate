using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace App.Models.Auth;

public class LoginModel
{
    [Required(ErrorMessage = "Email is required")]
    [DefaultValue("test@mail.com")]
    public string Email { get; set; }
    
    [Required(ErrorMessage = "Password is required")]
    [DataType(DataType.Password)]
    [DefaultValue("Test1!")]
    public string Password { get; set; }
}
