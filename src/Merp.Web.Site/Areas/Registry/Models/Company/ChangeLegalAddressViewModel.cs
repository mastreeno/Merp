using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Merp.Web.Site.Areas.Registry.Models.Company
{    
    public class ChangeLegalAddressViewModel : IValidatableObject
    {
        [Required]
        public Guid CompanyId { get; set; }
        
        [BindNever]
        public string CompanyName { get; set; }

        [DisplayName("Legal Address")]
        public PostalAddress LegalAddress { get; set; }

        [Required]
        [DisplayName("Effective Date")]
        public DateTime EffectiveDate { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var validationResults = new List<ValidationResult>();

            if (string.IsNullOrWhiteSpace(LegalAddress.Address))
            {
                validationResults.Add(new ValidationResult($"{nameof(LegalAddress.Address)} is required", new[] { $"{nameof(LegalAddress)}.{nameof(LegalAddress.Address)}" }));
            }
            if (string.IsNullOrWhiteSpace(LegalAddress.City))
            {
                validationResults.Add(new ValidationResult($"{nameof(LegalAddress.City)} is required", new[] { $"{nameof(LegalAddress)}.{nameof(LegalAddress.City)}" }));
            }

            if(EffectiveDate > DateTime.Now)
            {
                validationResults.Add(new ValidationResult($"The {nameof(LegalAddress)} change cannot be scheduled in the future", new[] { $"{nameof(EffectiveDate)}" }));
            }

            return validationResults;
        }

        public ModelStateDictionary Validate(CompanyDto companyDto)
        {
            if(companyDto == null)
            {
                throw new ArgumentNullException(nameof(companyDto));
            }

            var modelStateDictionary = new ModelStateDictionary();

            if (EffectiveDate < companyDto.RegistrationDate.ToLocalTime())
            {
                modelStateDictionary.AddModelError(nameof(EffectiveDate), $"The {nameof(LegalAddress)} change cannot happen before the registration date");
            }

            return modelStateDictionary;
        }

        public class CompanyDto
        {            
            public DateTime RegistrationDate { get; internal set; }
        }
    }
}