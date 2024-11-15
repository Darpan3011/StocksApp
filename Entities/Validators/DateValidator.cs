using System.ComponentModel.DataAnnotations;

namespace UnitTesting_UDEMY.Validators
{
    public class DateValidator : ValidationAttribute
    {
        public int Year { get; set; } = 2000;
        public string DefaultYearError { get; set; } = "This is Default error year message";
        public DateValidator() { }
        public DateValidator(int year)
        {
            Year = year;
        }
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value != null)
            {
                DateTime d = (DateTime)value;

                if (d.Year < Year)
                {
                    return new ValidationResult(String.Format(ErrorMessage ?? DefaultYearError, Year));
                }
                else
                {
                    return ValidationResult.Success;
                }
            }
            else
            {
                return null;
            }
        }
    }
}
