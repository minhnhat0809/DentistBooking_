using System.Text.RegularExpressions;

namespace Service.Lib
{
    public class Validation
    {
       
        private const string UserNamePattern = @"^\w{3,20}$";
        private const string NamePattern = @"^[A-Za-z ]{2,50}$";
        private const string PasswordPattern = @"^.{6,20}$";
        private const string GenderPattern = @"^(Male|Female|Other)$";
        private const string PhoneNumberPattern = @"^\d{10,15}$";
        private const string EmailPattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";

        
        public static bool ValidateUserName(string userName)
        {
            return Regex.IsMatch(userName, UserNamePattern);
        }

        public static bool ValidateName(string name)
        {
            return Regex.IsMatch(name, NamePattern);
        }

        public static bool ValidatePassword(string password)
        {
            return Regex.IsMatch(password, PasswordPattern);
        }

        public static bool ValidateGender(string? gender)
        {
            if (gender == null)
                return true; 

            return Regex.IsMatch(gender, GenderPattern, RegexOptions.IgnoreCase);
        }

        public static bool ValidatePhoneNumber(string? phoneNumber)
        {
            if (phoneNumber == null)
                return true;

            return Regex.IsMatch(phoneNumber, PhoneNumberPattern);
        }

        public static bool ValidateEmail(string? email)
        {
            if (email == null)
                return true;

            return Regex.IsMatch(email, EmailPattern);
        }
    }
}
