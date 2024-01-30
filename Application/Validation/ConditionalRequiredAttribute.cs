using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Validation
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class ConditionalRequiredAttribute : ValidationAttribute
    {
        private readonly string _dependentPropertyName;

        public ConditionalRequiredAttribute(string dependentPropertyName)
        {
            _dependentPropertyName = dependentPropertyName;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var dependentProperty = validationContext.ObjectInstance.GetType().GetProperty(_dependentPropertyName);

            if (dependentProperty == null)
            {
                return new ValidationResult($"Property {_dependentPropertyName} not found", new[] { validationContext.MemberName });
            }

            var dependentValue = (bool)dependentProperty.GetValue(validationContext.ObjectInstance);

            if (dependentValue && String.IsNullOrEmpty(value.ToString()))
            {
                return new ValidationResult($"{validationContext.DisplayName} is required when {_dependentPropertyName} is true.", new[] { validationContext.MemberName });
            }

            return ValidationResult.Success;
        }
    }
}
