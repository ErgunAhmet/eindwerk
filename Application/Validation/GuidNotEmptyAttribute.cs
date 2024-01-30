using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Validation
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class GuidNotEmptyAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if ((Guid)value == Guid.Empty)
                return new ValidationResult($"{validationContext.DisplayName} cannot be empty.", new[] { validationContext.MemberName });

            return ValidationResult.Success;
        }

    }
}
