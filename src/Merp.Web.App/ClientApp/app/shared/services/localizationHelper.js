'use strict'

import it from 'vee-validate/dist/locale/it'
import { Validator } from 'vee-validate'

import moment from 'moment'

const validationLocalized = {
    'it-IT': it
}

export default {
    localizeValidation() {
        let currentLanguage = navigator.language || navigator.userLanguage

        Validator.localize(validationLocalized)
        Validator.localize(currentLanguage)
    },

    localizeDate() {
        let currentLanguage = navigator.language || navigator.userLanguage
        moment.locale(currentLanguage)
    }
};