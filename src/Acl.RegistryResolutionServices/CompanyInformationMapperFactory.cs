using Acl.RegistryResolutionServices.Mappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Acl.RegistryResolutionServices
{
    internal class CompanyInformationMapperFactory
    {
        private IDictionary<string, Func<CompanyInformationMapper>> _mapperFactoryMethods = new Dictionary<string, Func<CompanyInformationMapper>>();
        private Func<CompanyInformationMapper> _defaultMapperFactoryMethod = () => new DefaultCompanyInformationMapper();

        internal CompanyInformationMapperFactory()
        {
            _mapperFactoryMethods.Add("AT", () => new ATCompanyInformationMapper());
            _mapperFactoryMethods.Add("BE", () => new BECompanyInformationMapper());
            _mapperFactoryMethods.Add("BG", () => new BGCompanyInformationMapper());
            _mapperFactoryMethods.Add("GB", () => new GBCompanyInformationMapper());
            _mapperFactoryMethods.Add("IT", () => new ITCompanyInformationMapper());
            _mapperFactoryMethods.Add("RO", () => new ROCompanyInformationMapper());
        }

        internal CompanyInformationMapper CreateMapper(string countryCode)
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
