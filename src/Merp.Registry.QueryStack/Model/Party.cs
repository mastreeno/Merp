using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Merp.Registry.QueryStack.Model
{
    public class Party
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int Id { get; set; }
        public Guid OriginalId { get; set; }
        [Index]
        [MaxLength(200)]
        public string DisplayName { get; set; }
        public string VatIndex { get; set; }
        public string NationalIdentificationNumber { get; set; }

    }
}
