using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace DataAccessLayer.Validators
{

    public class LandlineOrMobileAttribute : ValidationAttribute
    {
        public string GetErrorMessage(string displayName, string inputValue) => $"The {displayName} {inputValue} is in an invalid format.";

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            string inputValue = value as string;

            if (string.IsNullOrWhiteSpace(inputValue)) return ValidationResult.Success;

            if (Regex.IsMatch(inputValue, StandardRegex.PHONE_LANDLINE) || Regex.IsMatch(inputValue, StandardRegex.PHONE_MOBILE))
            {
                return ValidationResult.Success;
            }
            return new ValidationResult(GetErrorMessage(validationContext.DisplayName, inputValue));
        }
    }
    public static class StandardRegex
    {
        public const string PHONE_LANDLINE = @"^(?:(?:\(?(?:0(?:0|11)\)?[\s-]?\(?|\+)44\)?[\s-]?(?:\(?0\)?[\s-]?)?)|(?:\(?0))(?:(?:\d{5}\)?[\s-]?\d{4,5})|(?:\d{4}\)?[\s-]?(?:\d{5}|\d{3}[\s-]?\d{3}))|(?:\d{3}\)?[\s-]?\d{3}[\s-]?\d{3,4})|(?:\d{2}\)?[\s-]?\d{4}[\s-]?\d{4}))(?:[\s-]?(?:x|ext\.?|\#)\d{3,4})?$";
        public const string PHONE_MOBILE = @"^([(]?\+44[)]?\s?[(]?0[)]?|[(]?\+44[)]?\s?[7]|0)7\s?([3|4|5|7|8|9]\d{2}|624)(\s?\d{6})$";
    }
}
