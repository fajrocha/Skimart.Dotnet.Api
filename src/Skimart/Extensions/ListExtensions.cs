using System.Text.Json;
using ErrorOr;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Skimart.Extensions;

public static class ListExtensions
{
    public static ModelStateDictionary ToModelStateDictionary(this List<Error> collection)
    {
        var modelStateDictionary = new ModelStateDictionary();

        foreach (var error in collection)
        {
            modelStateDictionary.AddModelError(
                JsonNamingPolicy.CamelCase.ConvertName(error.Code),
                error.Description);
        }

        return modelStateDictionary;
    }
}