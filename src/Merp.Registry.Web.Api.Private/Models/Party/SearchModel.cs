using System;
using System.Collections.Generic;

namespace Merp.Registry.Web.Models.Party
{
    public class SearchModel
    {
        public IEnumerable<PartyDescriptor> Parties { get; set; }

        public int TotalNumberOfParties { get; set; }

        public class PartyDescriptor
        {
            public int Id { get; set; }

            public Guid Uid { get; set; }

            public string Name { get; set; }

            public string PhoneNumber { get; set; }

            public string NationalIdentificationNumber { get; set; }

            public string VatIndex { get; set; }

            public string PartyType { get; set; }
        }
    }
}
