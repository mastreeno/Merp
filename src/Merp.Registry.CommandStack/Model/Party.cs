using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Memento;
using Memento.Domain;

namespace Merp.Registry.CommandStack.Model
{
    public class Party : Aggregate
    {
        public string VatIndex { get; protected set; }
        public string NationalIdentificationNumber { get; protected set; }
    }
}
