using Merp.Web.Site.Areas.Registry.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Merp.Web.Site.Areas.Accountancy.Models.JobOrder
{
    public class CreateTimeAndMaterialViewModel : IValidatableObject
    {
        public PartyInfo Customer { get; set; }
        public PersonInfo Manager { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public DateTime DateOfStart { get; set; }
        public DateTime? DateOfExpiration { get; set; }
        public PositiveMoney Value { get; set; }
        public string Description { get; set; }
        public string PurchaseOrderNumber { get; set; }

        IEnumerable<ValidationResult> IValidatableObject.Validate(ValidationContext validationContext)
        {
            var errors = new List<ValidationResult>();
            if(DateOfExpiration.HasValue && (DateOfExpiration.Value < DateOfStart))
            {
                errors.Add(new ValidationResult("The expiration date cannot precede the date of start", new string[] { "DateOfStart", "DateOfExpiration" }));
            }
            //if (!DateOfExpiration.HasValue && !Value.HasValue)
            //{
            //    errors.Add(new ValidationResult("Either the expiration date or the value have to be specified", new string[] { "DateOfExpiration", "Value" }));
            //}
            return errors;
        }
    }
}