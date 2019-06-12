'use strict'

const addCompanyEntryValidationRules = {
    companyName: 'required',
    vatNumber: 'required',
    contactInfoWebsiteAddress: 'url',
    contactInfoEmailAddress: 'email'
}

const changeCompanyNameValidationRules = {
    companyName: 'required'
}

const changeCompanyContactInfoValidationRules = {
    contactInfoWebsiteAddress: 'url',
    contactInfoEmailAddress: 'email'
}

export {
    addCompanyEntryValidationRules,
    changeCompanyNameValidationRules,
    changeCompanyContactInfoValidationRules
}