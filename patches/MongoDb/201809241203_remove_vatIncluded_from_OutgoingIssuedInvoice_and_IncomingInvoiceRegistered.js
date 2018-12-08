db.getCollection('OutgoingInvoiceIssuedEvent').updateMany({}, {
    $unset: { "LineItems.$[].VatIncluded": false }
});

db.getCollection('IncomingInvoiceRegisteredEvent').updateMany({}, {
    $unset: { "LineItems.$[].VatIncluded": false }
});