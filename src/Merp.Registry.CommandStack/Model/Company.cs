using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Memento;
using Memento.Domain;
using Merp.Registry.CommandStack.Events;
using Merp.Registry.CommandStack.Helpers;

namespace Merp.Registry.CommandStack.Model
{
    /// <summary>
    /// Respresents a Company
    /// </summary>
    public class Company : Party,
        IApplyEvent<CompanyRegisteredEvent>,
        IApplyEvent<CompanyNameChangedEvent>,
        IApplyEvent<CompanyAdministrativeContactAssociatedEvent>,
        IApplyEvent<CompanyMainContactAssociatedEvent>
    {
        /// <summary>
        /// Gets or sets the company name
        /// </summary>
        public string CompanyName { get; private set; }
        public Guid? AdministrativeContactId { get; private set; }
        public Guid? MainContactId { get; private set; }

        protected Company()
        {

        }

        public void ApplyEvent(CompanyRegisteredEvent evt)
        { 
            this.Id = evt.CompanyId;
            this.CompanyName = evt.CompanyName;
            this.VatNumber = evt.VatIndex;
            this.NationalIdentificationNumber = evt.NationalIdentificationNumber;
            this.RegistrationDate = evt.TimeStamp;
            if (!string.IsNullOrWhiteSpace(evt.LegalAddressAddress) && !string.IsNullOrWhiteSpace(evt.LegalAddressCity) && !string.IsNullOrWhiteSpace(evt.LegalAddressCountry))
            {
                var legalAddress = new PostalAddress(evt.LegalAddressAddress, evt.LegalAddressCity, evt.LegalAddressCountry)
                {
                    PostalCode = evt.LegalAddressPostalCode,
                    Province = evt.LegalAddressProvince
                };
                this.LegalAddress = legalAddress;
            }
        }

        public void ApplyEvent(CompanyNameChangedEvent evt)
        {
            this.CompanyName = evt.CompanyName;
        }

        public void ApplyEvent(CompanyAdministrativeContactAssociatedEvent evt)
        {
            this.AdministrativeContactId = evt.AdministrativeContactId;
        }

        public void ApplyEvent(CompanyMainContactAssociatedEvent evt)
        {
            this.MainContactId = evt.MainContactId;
        }

        public void ChangeName(string newName, DateTime effectiveDate)
        {
            if (string.IsNullOrEmpty(newName))
            {
                throw new ArgumentException("The new Company name must be specified", nameof(newName));
            }
            if (effectiveDate > DateTime.Now)
            {
                throw new ArgumentException("The name change cannot be scheduled in the future", nameof(effectiveDate));
            }
            if (effectiveDate < RegistrationDate.ToLocalTime())
            {
                throw new ArgumentException("Cannot change the company name to an effective date before the registration date", nameof(effectiveDate));
            }

            var e = new CompanyNameChangedEvent(this.Id, newName, effectiveDate);
            RaiseEvent(e);
        }

        public void AssociateAdministrativeContact(Guid administrativeContactId)
        {
            var e = new CompanyAdministrativeContactAssociatedEvent(this.Id, administrativeContactId);
            RaiseEvent(e);
        }

        public void AssociateMainContact(Guid mainContactId)
        {
            var e = new CompanyMainContactAssociatedEvent(this.Id, mainContactId);
            RaiseEvent(e);
        }

        public static class Factory
        {
            public static Company CreateNewEntry(string companyName, string vatNumber, string nationalIdentificationNumber, string legalAddressAddress, string legalAddressCity, string legalAddressPostalCode, string legalAddressProvince, string legalAddressCountry)
            {
                var companyId = Guid.NewGuid();
                var company = CreateNewEntryByImport(companyId, companyName, vatNumber, nationalIdentificationNumber, legalAddressAddress, legalAddressCity, legalAddressPostalCode, legalAddressProvince,  legalAddressCountry);
                return company;
            }

            public static Company CreateNewEntryByImport(Guid companyId, string companyName, string vatNumber, string nationalIdentificationNumber, string legalAddressAddress, string legalAddressCity, string legalAddressPostalCode, string legalAddressProvince, string legalAddressCountry)
            {
                if (string.IsNullOrWhiteSpace(companyName))
                {
                    throw new ArgumentException("The company name must be specified", nameof(companyName));
                }
                if (string.IsNullOrWhiteSpace(vatNumber))
                {
                    throw new ArgumentException("The VAT number must be specified", nameof(vatNumber));
                }
                if (string.IsNullOrEmpty(legalAddressAddress) && (!string.IsNullOrEmpty(legalAddressCity) || !string.IsNullOrEmpty(legalAddressCountry)))
                {
                    throw new ArgumentException("address must be specified when city and country are also specified", nameof(legalAddressAddress));
                }
                if (string.IsNullOrEmpty(legalAddressCity) && (!string.IsNullOrEmpty(legalAddressAddress) || !string.IsNullOrEmpty(legalAddressCountry)))
                {
                    throw new ArgumentException("city must be specified when address and country are also specified", nameof(legalAddressCity));
                }
                if (string.IsNullOrEmpty(legalAddressCountry) && (!string.IsNullOrEmpty(legalAddressAddress) || !string.IsNullOrEmpty(legalAddressCity)))
                {
                    throw new ArgumentException("country must be specified when address and country are also specified", nameof(legalAddressCountry));
                }
                if (!PostalAddressHelper.IsValidAddress(legalAddressAddress, legalAddressCity, legalAddressPostalCode, legalAddressProvince, legalAddressCountry))
                {
                    throw new ArgumentException("legal address must either be empty or comprehensive of both address and city");
                }
                var p = new Company();
                var e = new CompanyRegisteredEvent(companyId, companyName, vatNumber, nationalIdentificationNumber, legalAddressAddress, legalAddressCity, legalAddressPostalCode, legalAddressProvince, legalAddressCountry);
                p.RaiseEvent(e);
                return p;
            }
        }
    }
}
