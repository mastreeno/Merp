'use strict'

const createJobOrderValidationRules = {
    name: 'required',
    dateOfStart: 'required',
    dueDate: 'required',
    priceAmount: 'required',
    priceCurrency: 'required'
}

const extendJobOrderValidationRules = {
    newDueDate: 'required',
    price: 'required'
}

const markJobOrderAsCompletedValidationRules = {
    dateOfCompletion: 'required'
}

export { createJobOrderValidationRules, extendJobOrderValidationRules, markJobOrderAsCompletedValidationRules }