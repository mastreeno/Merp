db.IncomingInvoiceRegisteredEvent.find({}).forEach(function(i){
    i.WithholdingTaxDescription = null;
    i.WithholdingTaxRate = null; 
    i.WithholdingTaxTaxableAmountRate = null; 
    i.WithholdingTaxAmount = null;
    i.TotalToPay = i.TotalPrice;
    db.IncomingInvoiceRegisteredEvent.update({ _id: i._id }, i);
});

db.IncomingCreditNoteRegisteredEvent.find({}).forEach(function(i){
    i.WithholdingTaxDescription = null;
    i.WithholdingTaxRate = null; 
    i.WithholdingTaxTaxableAmountRate = null; 
    i.WithholdingTaxAmount = null;
    i.TotalToPay = i.TotalPrice;
    db.IncomingCreditNoteRegisteredEvent.update({ _id: i._id }, i);
});