using System;
using System.ComponentModel.DataAnnotations;

namespace BusinessObject.ValidationDTO
{
    public class DateRangeAttribute : ValidationAttribute
    {
        private const string DynamicDateToken = "now";

        public DateOnly MinimumDate { get; }
        public DateOnly MaximumDate { get; }

        public DateRangeAttribute(string minimumDate, string maximumDate)
        {
            if (minimumDate.ToLower() == DynamicDateToken)
            {
                MinimumDate = DateOnly.FromDateTime(DateTime.Now);
            }
            else
            {
                MinimumDate = DateOnly.Parse(minimumDate);
            }

            if (maximumDate.ToLower() == DynamicDateToken)
            {
                MaximumDate = DateOnly.FromDateTime(DateTime.Now);
            }
            else
            {
                MaximumDate = DateOnly.Parse(maximumDate);
            }
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value is DateOnly date)
            {
                if (date < MinimumDate || date > MaximumDate)
                {
                    return new ValidationResult($"The date must be between {MinimumDate} and {MaximumDate}.");
                }
            }

            return ValidationResult.Success;
        }
    }
    
}
