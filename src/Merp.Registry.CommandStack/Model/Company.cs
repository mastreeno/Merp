using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Memento;
using Memento.Domain;
using Merp.Registry.CommandStack.Events;

namespace Merp.Registry.CommandStack.Model
{
    /// <summary>
    /// Respresents a Company
    /// </summary>
    public class Company : Party,
        IApplyEvent<CompanyRegisteredEvent>,
        IApplyEvent<CompanyNameChangedEvent>
    {
        /// <summary>
        /// Gets or sets the company name
        /// </summary>
        public string CompanyName { get; private set; }

        protected Company()
        {

        }

        public void ApplyEvent(CompanyRegisteredEvent evt)
        {
            this.Id = evt.CompanyId;
            this.CompanyName = evt.CompanyName;
            this.VatIndex = evt.VatIndex;
            this.NationalIdentificationNumber = evt.NationalIdentificationNumber;
        }

        public void ApplyEvent(CompanyNameChangedEvent evt)
        {
            this.CompanyName = evt.CompanyName;
        }

        public void ChangeName(string newName, DateTime effectiveDate)
        {
            if (string.IsNullOrEmpty(newName))
                throw new ArgumentException("The new Company name must be specified", nameof(newName));
            if (effectiveDate > DateTime.Now)
                throw new ArgumentException("The name change cannot be scheduled in the future", nameof(effectiveDate));

            var e = new CompanyNameChangedEvent(this.Id, newName, effectiveDate);
            RaiseEvent(e);
        }

        public static class Factory
        {
            public static Company CreateNewEntry(string companyName, string vatNumber, string nationalIdentificationNumber)
            {
                var companyId = Guid.NewGuid();
                var company = CreateNewEntryByImport(companyId, companyName, vatNumber, nationalIdentificationNumber);
                return company;
            }

            public static Company CreateNewEntryByImport(Guid companyId, string companyName, string vatNumber, string nationalIdentificationNumber)
            {
                if (string.IsNullOrWhiteSpace(companyName))
                    throw new ArgumentException("The company name must be specified", nameof(companyName));
                if (string.IsNullOrWhiteSpace(vatNumber))
                    throw new ArgumentException("The VAT number must be specified", nameof(vatNumber));

                var p = new Company();
                var e = new CompanyRegisteredEvent(companyId, companyName, vatNumber, nationalIdentificationNumber);
                p.RaiseEvent(e);
                return p;
            }
        }
    }
}
