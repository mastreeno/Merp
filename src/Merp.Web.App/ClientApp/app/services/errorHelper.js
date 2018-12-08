'use strict'

export default {
    buildErrorListFromModelState(modelState) {
        return Object.values(modelState).reduce((firstValue, secondValue) => firstValue.concat(secondValue))
    }
}