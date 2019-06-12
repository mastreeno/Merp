'use strict'

const registerPersonValidationRules = {
    firstName: 'required',
    lastName: 'required',
    nationalIdentificationNumber: 'length:16'
}

export { registerPersonValidationRules }