using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Merp.Accountancy.Web.Models.JobOrder
{
    public class CreateModel : IValidatableObject
    {
        public PartyInfo Customer { get; set; }

        public PersonInfo ContactPerson { get; set; }

        public PersonInfo Manager { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public DateTime DateOfStart { get; set; }

        [Required]
        public DateTime DueDate { get; set; }

        public bool IsTimeAndMaterial { get; set; }

        [Required]
        public PositiveMoney Price { get; set; }

        public string Description { get; set; }

        public string PurchaseOrderNumber { get; set; }

        IEnumerable<ValidationResult> IValidatableObject.Validate(ValidationContext validationContext)
        {
            var errors = new List<ValidationResult>();
            if (DueDate < DateOfStart)
            {
                errors.Add(new ValidationResult("The due date cannot precede the date of start", new string[] { "DateOfStart", "DueDate" }));
            }
            return errors;
        }
    }
}
