db.getCollection('OutgoingInvoiceIssuedEvent').updateMany({}, {
    $set: { WithholdingTaxDescription: null, WithholdingTaxRate: null, WithholdingTaxTaxableAmountRate: null, WithholdingTaxAmount: null }
});

db.getCollection('OutgoingCreditNoteIssuedEvent').updateMany({}, {
    $set: { WithholdingTaxDescription: null, WithholdingTaxRate: null, WithholdingTaxTaxableAmountRate: null, WithholdingTaxAmount: null }
});