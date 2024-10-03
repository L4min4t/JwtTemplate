namespace App.Models.User.Request;

public class UpdateUserRolesModel
{
    public string UserId { get; set; }
    
    public string[] RoleNames { get; set; }
}
