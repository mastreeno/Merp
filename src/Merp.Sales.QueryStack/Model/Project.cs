using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Merp.Sales.QueryStack.Model
{
    public class Project : IEntityTypeConfiguration<Project>
    {
        [Key]
        [Required]
        public Guid Id { get; set; }

        [Required]
        public Guid CustomerId { get; set; }

        public Guid? ContactPersonId { get; set; }

        [Required]
        public Guid ManagerId { get; set; }

        public DateTime DateOfRegistration { get; set; }

        public DateTime? DateOfStart { get; set; }

        public DateTime? DueDate { get; set; }

        public DateTime? DateOfCompletion { get; set; }

        public bool IsCompleted { get; set; }

        public bool IsTimeAndMaterial { get; set; }

        [Required]
        public string Number { get; set; }

        public string CustomerPurchaseOrderNumber { get; set; }

        public decimal? Price { get; set; }

        [Required]
        [StringLength(3)]
        public string Currency { get; set; }

        [Required]
        public string Name { get; set; }

        public string Description { get; set; }

        public Project()
        {
            IsTimeAndMaterial = false;
        }

        public void Configure(EntityTypeBuilder<Project> builder)
        {
            builder.HasIndex(o => o.CustomerId);
            builder.HasIndex(o => o.ManagerId);
            builder.HasIndex(o => o.Name);
            builder.HasIndex(o => o.IsCompleted);
        }
    }
}
