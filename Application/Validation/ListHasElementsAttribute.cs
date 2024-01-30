using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Validation
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class ListHasElementsAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value is IEnumerable collection && !collection.Cast<object>().Any())
            {
                return new ValidationResult(ErrorMessage ?? "At least one value is required");
            }

            return ValidationResult.Success;
        }
    }
}
