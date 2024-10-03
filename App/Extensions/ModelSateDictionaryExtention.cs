using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace App.Extensions;

public static class ModelSateDictionaryExtension
{
    public static string[] GetErrorMessages(this ModelStateDictionary modelState)
    {
        return modelState.Where(ms => ms.Value.Errors.Count > 0)
            .SelectMany(ms => ms.Value.Errors.Select(e => e.ErrorMessage))
            .ToArray();
    }
}
