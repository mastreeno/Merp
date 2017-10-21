using MementoFX;
using Merp.Registry.CommandStack.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Merp.Registry.CommandStack.Commands
{
    public class RegisterPersonCommand : Command
    {
        public Guid PersonId { get; set; }
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

        public RegisterPersonCommand(string firstName, string lastName, string nationalIdentificationNumber, string vatNumber, string address, string city, string postalCode, string province, string country, string phoneNumber, string mobileNumber, string faxNumber, string websiteAddress, string emailAddress, string instantMessaging)
        {
            if (string.IsNullOrWhiteSpace(firstName))
            {
                throw new ArgumentException("First name must be provided", nameof(firstName));
            }
            if (string.IsNullOrWhiteSpace(lastName))
            {
                throw new ArgumentException("Last name must be provided", nameof(lastName));
            }
            if (!PostalAddressHelper.IsValidAddress(address, city, postalCode, province, country))
            {
                throw new ArgumentException("postal address must either be empty or comprehensive of both address and city");
            }
            FirstName = firstName;
            LastName = lastName;
            NationalIdentificationNumber = nationalIdentificationNumber;
            VatNumber = vatNumber;
            Address = address;
            City = city ;
            PostalCode = postalCode;
            Province = province;
            Country = country;
            PhoneNumber = phoneNumber;
            MobileNumber = mobileNumber;
            FaxNumber = faxNumber;
            WebsiteAddress = websiteAddress;
            EmailAddress = emailAddress;
            InstantMessaging = instantMessaging;
        }
    }
}
