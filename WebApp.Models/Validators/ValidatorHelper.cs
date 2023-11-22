using System.Text.RegularExpressions;

namespace WebApp.Models.Validators;

public static class ValidatorHelper
{
    public static bool ValidateEmail(string email)
    {
        if (email is null) return false;

        Regex validEmailRegex = new Regex("^[\\w-\\.]+@([\\w-]+\\.)+[\\w-]{2,4}$");

        bool isValidEmail = validEmailRegex.IsMatch(email);
        bool containsUpperCase = email.Any(char.IsUpper);

        if (!isValidEmail || containsUpperCase)
            return false;

        return true;
    }
}
