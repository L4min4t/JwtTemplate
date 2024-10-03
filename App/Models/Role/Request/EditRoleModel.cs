using System.ComponentModel.DataAnnotations;

namespace App.Models.Role.Request;

public class EditRoleModel
{
    [Required(ErrorMessage = "The Role ID is required.")]
    [DataType(DataType.Text)]
    public string Id { get; set; }
    
    [Required(ErrorMessage = "The Role Name is required.")]
    [StringLength(50, MinimumLength = 3, ErrorMessage = "The Role Name must be between 3 and 50 characters.")]
    [RegularExpression
        (@"^[a-zA-Z0-9 ]*$", ErrorMessage = "The Role Name can only contain letters, numbers, and spaces.")]
    public string Name { get; set; }
}
