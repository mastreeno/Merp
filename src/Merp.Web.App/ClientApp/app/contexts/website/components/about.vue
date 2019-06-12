<template>
    <div>
        <h2>MERP</h2>
        <h3>My Micro ERP</h3>

        <p>
            {{ uiTexts.aboutPageDescription }}
        </p>
    </div>
</template>
<script>
    import EndpointConfigurationMixin from '@/app/shared/mixins/endpointConfigurationMixin'
    import { httpClient } from '@/app/shared/services/httpClient'

    export default {
        name: 'about',
        mixins: [EndpointConfigurationMixin],
        urls: {
            getHomeAboutLocalization: window.endpoints.client.getHomeAboutLocalization
        },
        methods: {
            loadResources() {
                return httpClient.get(this.$urls.getHomeAboutLocalization)
            }
        }
    }
</script>