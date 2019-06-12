'use strict'

export default {
    install(Vue, options) {
        Vue.mixin({
            data() {
                return {
                    uiTexts: {}
                }
            },
            methods: {
                loadResources() {
                    return null;
                },
                onEndResourcesLoading() {
                }
            },
            mounted() {
                let loadResourcesResult = this.loadResources()
                if (loadResourcesResult) {
                    let self = this
                    loadResourcesResult
                        .then((data) => {
                            self.uiTexts = data || {}
                            self.onEndResourcesLoading()
                        })
                }
            }
        })
    }
}