using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Merp.Web.Site.Areas.Registry.Models.Company
{
    public class ChangeNameViewModel : IValidatableObject
    {
        [Required]
        public Guid CompanyId { get; set; }

        [BindNever]
        [DisplayName("Current Company Name")]
        public string CurrentCompanyName { get; set; }

        [Required]
        [DisplayName("New Company Name")]
        public string NewCompanyName { get; set; }

        [Required]
        [DisplayName("Effective Date")]
        public DateTime EffectiveDate { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var validationResults = new List<ValidationResult>();
            
            if (EffectiveDate > DateTime.Now)
            {
                validationResults.Add(new ValidationResult($"The Company Name change cannot be scheduled in the future", new[] { $"{nameof(EffectiveDate)}" }));
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
                modelStateDictionary.AddModelError(nameof(EffectiveDate), $"The Company Name change cannot happen before the registration date");
            }

            return modelStateDictionary;
        }
        public class CompanyDto
        {
            public DateTime RegistrationDate { get; internal set; }
        }
    }
}
