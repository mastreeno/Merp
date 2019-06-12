db.OutgoingInvoiceIssuedEvent.find({}).forEach(function(i){
    i.TotalToPay = i.TotalPrice;
    db.OutgoingInvoiceIssuedEvent.update({ _id: i._id }, i);
});

db.OutgoingCreditNoteIssuedEvent.find({}).forEach(function(i){
    i.TotalToPay = i.TotalPrice;
    db.OutgoingCreditNoteIssuedEvent.update({ _id: i._id }, i);
});