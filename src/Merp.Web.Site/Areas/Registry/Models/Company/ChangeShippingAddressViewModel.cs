using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Merp.Web.Site.Areas.Registry.Models.Company
{
    public class ChangeShippingAddressViewModel : IValidatableObject
    {
        [Required]
        public Guid CompanyId { get; set; }

        public string CompanyName { get; set; }
   
        [DisplayName("Shipping Address")]
        public PostalAddress ShippingAddress { get; set; }

        public DateTime EffectiveDate { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var validationResults = new List<ValidationResult>();

            if (string.IsNullOrWhiteSpace(ShippingAddress.Address))
            {
                validationResults.Add(new ValidationResult($"{nameof(ShippingAddress.Address)} is required", new[] { $"{nameof(ShippingAddress)}.{nameof(ShippingAddress.Address)}" }));
            }

            if (string.IsNullOrWhiteSpace(ShippingAddress.City))
            {
                validationResults.Add(new ValidationResult($"{nameof(ShippingAddress.City)} is required", new[] { $"{nameof(ShippingAddress)}.{nameof(ShippingAddress.City)}" }));
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
                modelStateDictionary.AddModelError(nameof(EffectiveDate), $"The {nameof(ShippingAddress)} change cannot happen before the registration date");
            }

            return modelStateDictionary;
        }

        public class CompanyDto
        {
            public DateTime RegistrationDate { get; internal set; }
        }
    }
}