using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Merp.Accountancy.Drafts.Model
{
    [ComplexType]
    public class PartyInfo
    {
        public Guid OriginalId { get; set; }
        //[Index]
        [MaxLength(100)]
        public string Name { get; set; }
        public string StreetName { get; set; }
        public string PostalCode { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string VatIndex { get; set; }
        public string NationalIdentificationNumber { get; set; }
    }
}
