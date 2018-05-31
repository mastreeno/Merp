using MementoFX;
using Merp.Registry.CommandStack.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Merp.Registry.CommandStack.Commands
{
    /// <summary>
    /// The command which allows to import a person from an external source
    /// </summary>
    public class ImportPersonCommand : Command
    {
        public Guid PersonId { get; set; }
        public DateTime RegistrationDate { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string NationalIdentificationNumber { get; set; }
        public string VatNumber { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string PostalCode { get; set; }
        public string Province { get; set; }
        public string Country { get; set; }
        public string PhoneNumber { get; set; }
        public string MobileNumber { get; set; }
        public string FaxNumber { get; set; }
        public string WebsiteAddress { get; set; }
        public string EmailAddress { get; set; }
        public string InstantMessaging { get; set; }

        public string Skype { get; set; }

        public ImportPersonCommand(Guid personId, DateTime registrationDate, string firstName, string lastName, string nationalIdentificationNumber, string vatNumber, string address, string city, string postalCode, string province, string country, string phoneNumber, string mobileNumber, string faxNumber, string websiteAddress, string emailAddress, string instantMessaging, string skype)
        {
            if (personId == Guid.Empty)
                throw new ArgumentException("A non-empty personId should be provided", nameof(personId));

            if (string.IsNullOrWhiteSpace(firstName))
                throw new ArgumentException("First name must be provided", nameof(firstName));

            if (string.IsNullOrWhiteSpace(lastName))
                throw new ArgumentException("Last name must be provided", nameof(lastName));

            if (!PostalAddressHelper.IsValidAddress(address, city, postalCode, province, country))
                throw new ArgumentException("postal address must either be empty or comprehensive of both address and city");

            PersonId = personId;
            RegistrationDate = registrationDate;
            FirstName = firstName;
            LastName = lastName;
            NationalIdentificationNumber = nationalIdentificationNumber;
            VatNumber = vatNumber;
            Address = address;
            City = city;
            PostalCode = postalCode;
            Province = province;
            Country = country;
            PhoneNumber = phoneNumber;
            MobileNumber = mobileNumber;
            FaxNumber = faxNumber;
            WebsiteAddress = websiteAddress;
            EmailAddress = emailAddress;
            InstantMessaging = instantMessaging;

            Skype = skype;
        }
    }
}
