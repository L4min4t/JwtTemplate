using System.ComponentModel.DataAnnotations;

namespace App.Models.Role.Request;

public class CreateRoleModel
{
    [Required(ErrorMessage = "Role name is required.")]
    [StringLength(50, MinimumLength = 3, ErrorMessage = "Role name must be between 3 and 50 characters.")]
    public string Name { get; set; }
}
