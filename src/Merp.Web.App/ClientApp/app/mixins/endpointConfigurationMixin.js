'use strict'

export default {
    beforeCreate() {
        const options = this.$options
        if (!options.urls) {
            options.urls = {}
        }

        if (!options.computed.$urls) {
            options.computed.$urls = function () {
                return options.urls
            }
        }
    }
}