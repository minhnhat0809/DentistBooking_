using System;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

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
    public class PhoneNumberAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null)
            {
                return new ValidationResult("Phone number is required.");
            }

            var phoneNumber = value.ToString();

            // Regular expression to match phone numbers starting with 0 or +84 followed by exactly 9 digits
            var regex = new Regex(@"^(0\d{9})$");

            if (!regex.IsMatch(phoneNumber))
            {
                return new ValidationResult("Invalid phone number format. It should start with 0 by exactly 9 digits.");
            }

            return ValidationResult.Success;
        }
    }
}
