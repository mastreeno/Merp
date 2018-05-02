using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MementoFX;
using MementoFX.Domain;
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
            if (!string.IsNullOrWhiteSpace(evt.BillingAddressAddress) && !string.IsNullOrWhiteSpace(evt.BillingAddressCity) && !string.IsNullOrWhiteSpace(evt.BillingAddressCountry))
            {
                var billingAddress = new PostalAddress(evt.BillingAddressAddress, evt.BillingAddressCity, evt.BillingAddressCountry)
                {
                    PostalCode = evt.BillingAddressPostalCode,
                    Province = evt.BillingAddressProvince
                };
                this.BillingAddress = billingAddress;
            }
            if (!string.IsNullOrWhiteSpace(evt.ShippingAddressAddress) && !string.IsNullOrWhiteSpace(evt.ShippingAddressCity) && !string.IsNullOrWhiteSpace(evt.ShippingAddressCountry))
            {
                var shippingAddress = new PostalAddress(evt.ShippingAddressAddress, evt.ShippingAddressCity, evt.ShippingAddressCountry)
                {
                    PostalCode = evt.ShippingAddressPostalCode,
                    Province = evt.ShippingAddressProvince
                };
                this.ShippingAddress = shippingAddress;
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
            public static Company CreateNewEntry(string companyName, string vatNumber, string nationalIdentificationNumber, 
                string legalAddressAddress, string legalAddressCity, string legalAddressPostalCode, string legalAddressProvince, string legalAddressCountry,
                string billingAddressAddress, string billingAddressCity, string billingAddressPostalCode, string billingAddressProvince, string billingAddressCountry,
                string shippingAddressAddress, string shippingAddressCity, string shippingAddressPostalCode, string shippingAddressProvince, string shippingAddressCountry)
            {
                if (string.IsNullOrWhiteSpace(companyName))
                    throw new ArgumentException("The company name must be specified", nameof(companyName));

                if (string.IsNullOrWhiteSpace(nationalIdentificationNumber))
                    throw new ArgumentException("The NIN must be specified", nameof(nationalIdentificationNumber));

                if (string.IsNullOrWhiteSpace(vatNumber))
                    throw new ArgumentException("The VAT number must be specified", nameof(vatNumber));

                var companyId = Guid.NewGuid();
                var p = new Company();
                var e = new CompanyRegisteredEvent(companyId, DateTime.Now, companyName, vatNumber, nationalIdentificationNumber,
                    legalAddressAddress, legalAddressCity, legalAddressPostalCode, legalAddressProvince, legalAddressCountry,
                    billingAddressAddress, billingAddressCity, billingAddressPostalCode, billingAddressProvince, billingAddressCountry,
                    shippingAddressAddress, shippingAddressCity, shippingAddressPostalCode, shippingAddressProvince, shippingAddressCountry);
                p.RaiseEvent(e);
                return p;
            }

            public static Company CreateNewEntryByImport(Guid companyId, DateTime registrationDate, string companyName, string vatNumber, string nationalIdentificationNumber, 
                string legalAddressAddress, string legalAddressCity, string legalAddressPostalCode, string legalAddressProvince, string legalAddressCountry,
                string billingAddressAddress, string billingAddressCity, string billingAddressPostalCode, string billingAddressProvince, string billingAddressCountry,
                string shippingAddressAddress, string shippingAddressCity, string shippingAddressPostalCode, string shippingAddressProvince, string shippingAddressCountry)
            {
                if (string.IsNullOrWhiteSpace(companyName))
                    throw new ArgumentException("The company name must be specified", nameof(companyName));

                if (string.IsNullOrWhiteSpace(nationalIdentificationNumber) && string.IsNullOrWhiteSpace(vatNumber))
                    throw new ArgumentException("Either the VAT number or the NIN must be specified", nameof(vatNumber));

                var p = new Company();
                var e = new CompanyRegisteredEvent(companyId, registrationDate, companyName, vatNumber, nationalIdentificationNumber, 
                    legalAddressAddress, legalAddressCity, legalAddressPostalCode, legalAddressProvince, legalAddressCountry,
                    billingAddressAddress, billingAddressCity, billingAddressPostalCode, billingAddressProvince, billingAddressCountry,
                    shippingAddressAddress, shippingAddressCity, shippingAddressPostalCode, shippingAddressProvince, shippingAddressCountry);
                p.RaiseEvent(e);
                return p;
            }
        }
    }
}
