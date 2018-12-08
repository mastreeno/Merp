db.getCollection('OutgoingInvoiceIssuedEvent').updateMany({}, {
    $set: { PricesAreVatIncluded: false }
});

db.getCollection('IncomingInvoiceRegisteredEvent').updateMany({}, {
    $set: { PricesAreVatIncluded: false }
});