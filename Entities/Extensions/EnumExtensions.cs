using System.ComponentModel.DataAnnotations;

namespace Hospital_Project.Entities.Extensions
{
    public static class EnumExtensions
    {
        public static string GetDisplayName(this Enum enumValue)
        {
            var field = enumValue.GetType().GetField(enumValue.ToString());
            var attr = field?.GetCustomAttributes(typeof(DisplayAttribute), false)
                             .FirstOrDefault() as DisplayAttribute;

            return attr?.Name ?? enumValue.ToString();
        }
    }
}
