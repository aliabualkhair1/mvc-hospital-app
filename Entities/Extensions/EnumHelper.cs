using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

public static class EnumHelper
{
    public static IEnumerable<SelectListItem> ToSelectList<T>() where T : Enum
    {
        return Enum.GetValues(typeof(T))
                   .Cast<T>()
                   .Select(e => new SelectListItem
                   {
                       Value = e.ToString(),
                       Text = GetDisplayName(e)
                   });
    }

    private static string GetDisplayName<T>(T enumValue)
    {
        var displayAttribute = enumValue.GetType()
            .GetField(enumValue.ToString())
            ?.GetCustomAttributes(typeof(DisplayAttribute), false)
            .FirstOrDefault() as DisplayAttribute;

        return displayAttribute?.Name ?? enumValue.ToString();
    }
}
