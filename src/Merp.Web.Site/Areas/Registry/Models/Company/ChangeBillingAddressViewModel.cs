using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Merp.Web.Site.Areas.Registry.Models.Company
{
    public class ChangeBillingAddressViewModel : IValidatableObject
    {
        [Required]
        public Guid CompanyId { get; set; }
        public string CompanyName { get; set; }
   
        [DisplayName("Billing Address")]
        public PostalAddress BillingAddress { get; set; }

        public DateTime EffectiveDate { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var validationResults = new List<ValidationResult>();

            if (string.IsNullOrWhiteSpace(BillingAddress.Address))
            {
                validationResults.Add(new ValidationResult($"{nameof(BillingAddress.Address)} is required", new[] { $"{nameof(BillingAddress)}.{nameof(BillingAddress.Address)}" }));
            }
            if (string.IsNullOrWhiteSpace(BillingAddress.City))
            {
                validationResults.Add(new ValidationResult($"{nameof(BillingAddress.City)} is required", new[] { $"{nameof(BillingAddress)}.{nameof(BillingAddress.City)}" }));
            }

            return validationResults;
        }

        public ModelStateDictionary Validate(CompanyDto companyDto)
        {
            if (companyDto == null)
            {
                throw new ArgumentNullException(nameof(companyDto));
            }

            var modelStateDictionary = new ModelStateDictionary();

            if (EffectiveDate < companyDto.RegistrationDate.ToLocalTime())
            {
                modelStateDictionary.AddModelError(nameof(EffectiveDate), $"The {nameof(BillingAddress)} change cannot happen before the registration date");
            }

            return modelStateDictionary;
        }

        public class CompanyDto
        {
            public DateTime RegistrationDate { get; internal set; }
        }
    }
}