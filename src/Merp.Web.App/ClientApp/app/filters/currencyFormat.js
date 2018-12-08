'use strict'

export default function (value, fractionDigits) {
    return parseFloat(value).toFixed(fractionDigits)
}