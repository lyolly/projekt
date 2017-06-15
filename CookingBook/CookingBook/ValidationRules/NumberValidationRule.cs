using System.Text.RegularExpressions;
using System.Windows.Controls;

namespace CookingBook.ValidationRules
{
    public class NumberValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, System.Globalization.CultureInfo cultureInfo)
        {
            var regex = new Regex(@"^[0-9]+(\,[0-9]+)?$");
            bool canConvert = regex.IsMatch(value.ToString());
            return new ValidationResult(canConvert, "Zły format liczby");
        }
    }
}