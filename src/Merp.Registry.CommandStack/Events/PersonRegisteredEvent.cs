using MementoFX;
using MementoFX.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Merp.Registry.CommandStack.Events
{
    public class PersonRegisteredEvent : DomainEvent
    {
        public Guid PersonId { get; set; }
        [Timestamp]
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

        public PersonRegisteredEvent(Guid personId, DateTime registrationDate, string firstName, string lastName, string nationalIdentificationNumber, string vatNumber, string address, string city, string postalCode, string province, string country, string phoneNumber, string mobileNumber, string faxNumber, string websiteAddress, string emailAddress, string instantMessaging, string skype)
        {
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
