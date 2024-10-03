namespace App.Models;

public class ResponseModel<T>
{
    public bool IsSuccessed => !(Errors?.Length > 0) && Content != null;
    
    public T? Content { get; set; }
    
    public string[]? Errors { get; set; }
}
