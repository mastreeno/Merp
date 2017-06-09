using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Merp.Web.Site.Areas.Registry.Models.Person
{
    public class ChangeAddressViewModel : IValidatableObject
    {
        [Required]
        public Guid PersonId { get; set; }

        [BindNever]
        public string PersonFirstName { get; set; }

        [BindNever]
        public string PersonLastName { get; set; }
        
        public string PersonDisplayName { get { return $"{PersonFirstName} {PersonLastName}".Trim(); } }

        [DisplayName("Address")]
        public PostalAddress Address { get; set; }

        public DateTime EffectiveDate { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var validationResults = new List<ValidationResult>();

            if (string.IsNullOrWhiteSpace(Address.Address))
            {
                validationResults.Add(new ValidationResult($"{nameof(Address.Address)} is required", new[] { $"{nameof(Address)}.{nameof(Address.Address)}" }));
            }

            if (string.IsNullOrWhiteSpace(Address.City))
            {
                validationResults.Add(new ValidationResult($"{nameof(Address.City)} is required", new[] { $"{nameof(Address)}.{nameof(Address.City)}" }));
            }

            if (EffectiveDate > DateTime.Now)
            {
                validationResults.Add(new ValidationResult($"The {nameof(Address)} change cannot be scheduled in the future", new[] { $"{nameof(EffectiveDate)}" }));
            }

            return validationResults;
        }

        public ModelStateDictionary Validate(PersonDto personDto)
        {
            if (personDto == null)
            {
                throw new ArgumentNullException(nameof(personDto));
            }

            var modelStateDictionary = new ModelStateDictionary();

            if (EffectiveDate < personDto.RegistrationDate.ToLocalTime())
            {
                modelStateDictionary.AddModelError(nameof(EffectiveDate), $"The {nameof(Address)} change cannot happen before the registration date");
            }

            return modelStateDictionary;
        }

        public class PersonDto
        {
            public DateTime RegistrationDate { get; internal set; }
        }
    }
}