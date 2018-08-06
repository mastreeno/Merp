db.getCollection('PersonRegisteredEvent').updateMany({}, {
    $rename: {
        'Address': 'LegalAddressAddress',
        'City': 'LegalAddressCity',
        'PostalCode': 'LegalAddressPostalCode',
        'Province': 'LegalAddressProvince',
        'Country': 'LegalAddressCountry'
    },
    $set: {
        'ShippingAddressAddress': '',
        'ShippingAddressPostalCode': '',
        'ShippingAddressCity': '',
        'ShippingAddressCountry': '',
        'ShippingAddressProvince': '',
        'BillingAddressAddress': '',
        'BillingAddressPostalCode': '',
        'BillingAddressCity': '',
        'BillingAddressCountry': '',
        'BillingAddressProvince': ''
    }
});