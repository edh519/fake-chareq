using Enums.Enums;
using Enums.Enums.Attributes;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Enums
{
    public static class EnumHelpers
    {
        private static TAttribute GetAttribute<TAttribute>(this Enum value)
        where TAttribute : Attribute
        {
            Type enumType = value.GetType();
            string name = Enum.GetName(enumType, value);
            return enumType.GetField(name).GetCustomAttributes(false).OfType<TAttribute>().SingleOrDefault();
        }


        /// <summary>
        /// Gets the Display Name attribute from an enum.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string GetDisplayName(Enum value)
        {
            return GetAttribute<DisplayAttribute>(value).Name;
        }

        /// <summary>
        /// Gets the IsActive attribute from an enum.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool GetIsActive(Enum value)
        {
            return GetAttribute<IsActiveAttribute>(value).Value;
        }

        /// <summary>
        /// Gets the CausesArchive attribute from an enum.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool GetCausesArchive(Enum value)
        {
            return GetAttribute<CausesArchiveAttribute>(value).Value;
        }

        public static string? GetEmail(TrialAttribution? value)
        {
            System.Reflection.FieldInfo field = value.GetType().GetField(value.ToString());
            DisplayAttribute display = (DisplayAttribute?)Attribute.GetCustomAttribute(field!, typeof(DisplayAttribute));
            return display?.Description;
        }

    }
}
