using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace OnTime.TaskManagement.QueryStack.Model
{
    public class Task : IEntityTypeConfiguration<Task>
    {
        [Required]
        public Guid Id { get; set; }

        [Required]
        public Guid UserId { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public bool IsCompleted { get; set; }

        [Required]
        public DateTime DateOfCreation { get; set; }

        public DateTime? DateOfCancellation { get; set; }

        public DateTime? DateOfCompletion { get; set; }

        public DateTime? DueDate { get; set; }

        public TaskPriority Priority { get; set; }

        public Guid? JobOrderId { get; set; }

        void IEntityTypeConfiguration<Task>.Configure(EntityTypeBuilder<Task> builder)
        {
            builder.HasIndex(o => o.UserId);
            builder.HasIndex(o => o.Name);
            builder.HasIndex(o => o.IsCompleted);
        }
    }
}
