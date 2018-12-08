<template>
    <div>
        <h2>{{ uiTexts.contact }}</h2>
        <h3>{{ uiTexts.contactPageDescription }}</h3>
        <address>
            <strong>Web:</strong> <a href="http://www.mastreeno.com">{{ uiTexts.website }}</a><br />
            <strong>Marketing:</strong> <a href="mailto:info@mastreeno.com">info@mastreeno.com</a>
        </address>
    </div>
</template>
<script>
    import EndpointConfigurationMixin from '@/app/mixins/endpointConfigurationMixin'
    import { httpClient } from '@/app/services/httpClient'

    export default {
        name: 'contact',
        mixins: [EndpointConfigurationMixin],
        urls: {
            getHomeContactLocalization: window.endpoints.client.getHomeContactLocalization
        },
        methods: {
            loadResources() {
                return httpClient.get(this.$urls.getHomeContactLocalization)
            }
        }
    }
</script>