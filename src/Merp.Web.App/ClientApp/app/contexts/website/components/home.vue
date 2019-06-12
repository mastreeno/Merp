<template>
    <div>
        <div class="jumbotron">
            <h1>
                [My|Micro]ERP
            </h1>
        </div>

        <div class="row">
            {{ uiTexts.homePageDescription }}
        </div>
    </div>
</template>
<script>
    import EndpointConfigurationMixin from '@/app/shared/mixins/endpointConfigurationMixin'
    import { httpClient } from '@/app/shared/services/httpClient'

    export default {
        name: 'Home',
        mixins: [EndpointConfigurationMixin],
        urls: {
            getHomeIndexLocalization: window.endpoints.client.getHomeIndexLocalization
        },
        methods: {
            loadResources() {
                return httpClient.get(this.$urls.getHomeIndexLocalization)
            }
        }
    }
</script>