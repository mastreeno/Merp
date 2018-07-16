using Acl.Vies.Mappers;
using System;
using System.Collections.Generic;

namespace Acl.Vies
{
    public class PersonInformationMapperFactory
    {
        private IDictionary<string, Func<PersonInformationMapper>> _mapperFactoryMethods = new Dictionary<string, Func<PersonInformationMapper>>();

        private Func<PersonInformationMapper> _defaultMapperFactoryMethod = () => new DefaultPersonInformationMapper();

        internal PersonInformationMapperFactory()
        {
            _mapperFactoryMethods.Add("AT", () => new ATPersonInformationMapper());
            _mapperFactoryMethods.Add("BE", () => new BEPersonInformationMapper());
            _mapperFactoryMethods.Add("BG", () => new BGPersonInformationMapper());
            _mapperFactoryMethods.Add("GB", () => new GBPersonInformationMapper());
            _mapperFactoryMethods.Add("IT", () => new ITPersonInformationMapper());
            _mapperFactoryMethods.Add("RO", () => new ROPersonInformationMapper());
        }

        internal PersonInformationMapper CreateMapper(string countryCode)
        {
            if (string.IsNullOrWhiteSpace(countryCode))
            {
                throw new ArgumentException("countryCode must be provided", nameof(countryCode));
            }

            if (_mapperFactoryMethods.ContainsKey(countryCode))
            {
                return _mapperFactoryMethods[countryCode]();
            }

            return _defaultMapperFactoryMethod();
        }
    }
}
