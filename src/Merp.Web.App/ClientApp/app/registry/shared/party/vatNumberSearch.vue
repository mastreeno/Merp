<template>
    <div>
        <div class="input-group mb-3">
            <div class="input-group-prepend">
                <select class="form-control" name="searchVatCountry" v-model="countryCode">
                    <option value="">{{ labels.country }}</option>
                    <option v-for="c in countries" v-bind:value="c.code">{{ c.displayName }}</option>
                </select>
            </div>
            <input class="form-control" name="vatNumber" id="vatNumber" v-bind:value="value" v-on:input="updateVatNumber" />
            <div class="input-group-append">
                <vue-element-loading v-bind:active="isSearching"></vue-element-loading>
                <button class="btn btn-outline-primary" type="button" v-on:click="loadByVatNumber">
                    <i class="fa fa-search"></i>
                    {{ labels.search }}
                </button>
            </div>
        </div>
        <span class="text-danger" v-if="error">{{ error }}</span>
    </div>
</template>
<script>
    import EndpointConfigurationMixin from '@/app/mixins/endpointConfigurationMixin'
    import { httpClient } from '@/app/services/httpClient'

    export default {
        name: 'vatNumberSearch',
        mixins: [EndpointConfigurationMixin],
        props: ['value', 'search', 'labels', 'country'],
        urls: {
            getCountries: window.endpoints.registryInternal.getCountries
        },
        data() {
            return {
                countryCode: this.country || '',
                countries: [],
                isSearching: false,
                error: ''
            }
        },
        methods: {
            updateVatNumber(evt) {
                this.$emit('input', evt.target.value)
            },
            loadByVatNumber(evt) {
                evt.preventDefault()

                let vatNumber = this.parseVatNumberValue(this.value)

                this.isSearching = true
                let self = this
                this.search(vatNumber, this.countryCode)
                    .then((response) => {
                        self.isSearching = false
                        self.$emit('vatnumber-found', response)
                    }, (error) => {
                        self.isSearching = false
                        self.error = error
                    })
                    .catch((error) => {
                        self.isSearching = false
                        self.error = error
                    })
            },
            parseVatNumberValue(value) {
                let vatNumberStartsWithCountry = this.countries.some((c) => {
                    return value.indexOf(c.code) === 0
                })

                if (!vatNumberStartsWithCountry) {
                    return value
                }

                this.countryCode = value.substring(0, 2)
                return value.substring(2)
            }
        },
        mounted() {
            let self = this
            httpClient.get(this.$urls.getCountries)
                .then((countries) => {
                    self.countries = countries || []
                })
                .catch((errors) => {
                    self.countries = []
                })
        }
    }
</script>