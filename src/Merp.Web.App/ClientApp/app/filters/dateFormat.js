'use strict'

import moment from 'moment'

export default function (value, format) {
    if (!format) {
        format = 'L'
    }

    if (!value) {
        return null
    }

    return moment(value).format(format)
}