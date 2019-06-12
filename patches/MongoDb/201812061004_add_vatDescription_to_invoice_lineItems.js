db.OutgoingInvoiceIssuedEvent.find({}).forEach(function(i){
    i.LineItems.forEach(function(l){
        l.VatDescription = l.Vat.toString() + '%';
    });
    
    db.OutgoingInvoiceIssuedEvent.update({ _id: i._id }, i);
});

db.OutgoingCreditNoteIssuedEvent.find({}).forEach(function(i){
    i.LineItems.forEach(function(l){
        l.VatDescription = l.Vat.toString() + '%';
    });
    
    db.OutgoingCreditNoteIssuedEvent.update({ _id: i._id }, i);
});

db.IncomingInvoiceRegisteredEvent.find({}).forEach(function(i){
    i.LineItems.forEach(function(l){
        l.VatDescription = l.Vat.toString() + '%';
    });
    
    db.IncomingInvoiceRegisteredEvent.update({ _id: i._id }, i);
});

db.IncomingCreditNoteRegisteredEvent.find({}).forEach(function(i){
    i.LineItems.forEach(function(l){
        l.VatDescription = l.Vat.toString() + '%';
    });
    
    db.IncomingCreditNoteRegisteredEvent.update({ _id: i._id }, i);
});