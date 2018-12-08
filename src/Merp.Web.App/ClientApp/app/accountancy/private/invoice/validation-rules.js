'use strict'

const issueOutgoingInvoiceValidationRules = {
    customer: 'required',
    date: 'required',
    currency: 'required',
    amount: 'required',
    taxes: 'required',
    totalPrice: 'required',
    description: 'required',
    lineItemDescription: 'required',
    lineItemTotalPrice: 'required'
}

const registerIncomingInvoiceValidationRules = {
    supplier: 'required',
    invoiceNumber: 'required',
    date: 'required',
    currency: 'required',
    amount: 'required',
    taxes: 'required',
    totalPrice: 'required',
    description: 'required',
    lineItemDescription: 'required',
    lineItemTotalPrice: 'required'
}

const registerOutgoingInvoiceValidationRules = {
    supplier: 'required',
    invoiceNumber: 'required',
    date: 'required',
    currency: 'required',
    amount: 'required',
    taxes: 'required',
    totalPrice: 'required',
    description: 'required',
    lineItemDescription: 'required',
    lineItemTotalPrice: 'required'
}

const linkInvoiceToAJobOrderValidationRules = {
    dateOfLink: 'required',
    jobOrderNumber: 'required'
}

export {
    issueOutgoingInvoiceValidationRules,
    registerIncomingInvoiceValidationRules,
    registerOutgoingInvoiceValidationRules,
    linkInvoiceToAJobOrderValidationRules
}