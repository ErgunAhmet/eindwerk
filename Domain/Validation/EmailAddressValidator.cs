using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Domain.Validation
{
    public static class EmailAddressValidator
    {
        private const string EmailRegexPattern = @"^(([^<>()[\]\\.,;:\s@""]+(\.[^<>()[\]\\.,;:\s@""]+)*)|("".+""))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$";

        public static bool IsValidEmail(string email)
        {
            return !string.IsNullOrEmpty(email) && new Regex(EmailRegexPattern).IsMatch(email);
        }
    }
}
