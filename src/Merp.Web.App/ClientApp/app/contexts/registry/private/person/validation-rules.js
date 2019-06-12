'use strict'

const addPersonEntryValidationRules = {
    firstName: 'required',
    lastName: 'required',
    nationalIdentificationNumber: 'length:16',
    contactInfoWebsiteAddress: 'url',
    contactInfoEmailAddress: 'email'
}

const changePersonContactInfoValidationRules = {
    contactInfoWebsiteAddress: 'url',
    contactInfoEmailAddress: 'email'
}

export {
    addPersonEntryValidationRules,
    changePersonContactInfoValidationRules
}