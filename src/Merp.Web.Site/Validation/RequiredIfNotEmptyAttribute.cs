using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace System.ComponentModel.DataAnnotations
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class RequiredIfNotEmptyAttribute : ValidationAttribute
    {
        private string[] _dependentProperties;

        public new string ErrorMessage { get; protected set; }

        public RequiredIfNotEmptyAttribute(params string[] dependentProperties)
            : base(GetErrorMessage(dependentProperties))
        {
            if(dependentProperties.Length == 0)
            {
                throw new ArgumentException("must specify at least one dependent property");
            }
            _dependentProperties = dependentProperties;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {            
            foreach (var dependentProperty in _dependentProperties)
            {
                var property = validationContext.ObjectType.GetProperty(dependentProperty);
                var propertyValue = property.GetValue(validationContext.ObjectInstance);
                if(!IsEmpty(propertyValue) && IsEmpty(value))
                {
                    string[] memberNames = validationContext.MemberName != null ? new string[] { validationContext.MemberName } : null;
                    return new ValidationResult(this.FormatErrorMessage(validationContext.DisplayName), memberNames);
                }                    
            }
            return ValidationResult.Success;
        }

        private static string GetErrorMessage(string[] dependentProperties)
        {
            return "{0} field is required when setting " + string.Join(", ", dependentProperties);
        }

        private bool IsEmpty(object value)
        {
            return value == null || string.IsNullOrWhiteSpace(value as string);
        }
    }
}
