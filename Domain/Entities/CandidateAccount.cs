using Domain.Entities.Base;
using Domain.Resources;
using Domain.Validation;
using Shared.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class CandidateAccount : RootEntity
    {
        public string FirstName { get; private set; }
        public string LastName { get; private set;}
        public string Email { get; private set;}
        public string PhoneNumber { get; private set;}

        private CandidateAccount() { }
        
        public static CandidateAccount Create(Guid id, string firstName, string lastName, string email, string mobilePhone)
        {
            var account = new CandidateAccount
            {
                Id = id,
                CreatedOn = DateTime.UtcNow,
            };
            account.UpdateInternal(firstName, lastName, email, mobilePhone);

            return account;
        }

        private void UpdateInternal(string firstName, string lastName, string email, string mobilePhone)
        {
            if (!string.IsNullOrEmpty(firstName))
                firstName = firstName.Trim();

            if (!string.IsNullOrEmpty(lastName))
                lastName = lastName.Trim();

            if (!string.IsNullOrEmpty(mobilePhone))
            {
                Guard.ThatIsTrue(PhoneNumberValidator.IsValidPhoneNumber(mobilePhone), nameof(ErrorCodeResource.UserContact_InvalidPhoneNumber));
                mobilePhone = PhoneNumberValidator.FormatPhoneNumber(mobilePhone);
            }

            if (!string.IsNullOrEmpty(email))
                Guard.ThatIsTrue(EmailAddressValidator.IsValidEmail(email), nameof(ErrorCodeResource.UserContact_InvalidEmailAddress));

            UpdatedOn = DateTime.UtcNow;
            FirstName = firstName;
            LastName = lastName;
            PhoneNumber = mobilePhone;
            Email = email;
        }
    }
}
