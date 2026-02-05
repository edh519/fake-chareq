using System;
using System.ComponentModel.DataAnnotations;

namespace DataAccessLayer.Validators;

public class DateLessThanOrEqualToTodayAttribute : ValidationAttribute
{
    public override string FormatErrorMessage(string name)
    {
        return $"{name} should not be in the future.";
    }

    protected override ValidationResult IsValid(object objValue,
        ValidationContext validationContext)
    {
        if (objValue == null)
        {
            return ValidationResult.Success;
        }

        var dateValue = objValue as DateTime? ?? new DateTime();

        if (dateValue.Date > DateTime.Now.Date)
        {
            return new ValidationResult(FormatErrorMessage(validationContext.DisplayName));
        }
        return ValidationResult.Success;
    }
}