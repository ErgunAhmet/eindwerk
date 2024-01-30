using PhoneNumbers;

namespace Domain.Validation
{
    public static class PhoneNumberValidator
    {
        private const string DefaultCountryCode = "BE";

        public static bool IsValidPhoneNumber(string phoneNumber)
        {
            if (string.IsNullOrEmpty(phoneNumber))
                return false;

            var instance = PhoneNumberUtil.GetInstance();

            PhoneNumber parsedPhoneNumber = null;

            try
            {
                parsedPhoneNumber = instance.Parse(phoneNumber, DefaultCountryCode);
            }
            catch (NumberParseException) { }

            if (parsedPhoneNumber == null)
                return false;

            return instance.IsValidNumber(parsedPhoneNumber);
        }

        public static string FormatPhoneNumber(string phoneNumber)
        {
            var instance = PhoneNumberUtil.GetInstance();

            var parsedPhoneNumber = instance.Parse(phoneNumber, DefaultCountryCode);

            return instance.FormatOutOfCountryCallingNumber(parsedPhoneNumber, string.Empty);
        }
    }
}
