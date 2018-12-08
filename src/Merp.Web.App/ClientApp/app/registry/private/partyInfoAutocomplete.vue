<template>
    <div>
        <v-select v-model="partyModel" label="name" v-bind:filterable="false" v-bind:options="parties" v-on:search="searchParties" v-on:input="selectParty" v-bind:disabled="isDisabled" v-validate="validate" v-bind:data-vv-name="name" v-bind:data-vv-as="fieldLabel">
            <template slot="option" scope="option">
                <div>
                    {{ option.name }}
                </div>
            </template>
            <template slot="selected-option" scope="option">
                <div>
                    {{ option.name }}
                </div>
            </template>
        </v-select>
        <span class="text-danger" v-if="errors.first(name)">{{ errors.first(name) }}</span>
    </div>
</template>
<script>
    import vSelect from 'vue-select'
    import EndpointConfigurationMixin from '@/app/mixins/endpointConfigurationMixin'

    import { httpClient } from '@/app/services/httpClient'

    export default {
        name: 'partyInfoAutcomplete',
        mixins: [EndpointConfigurationMixin],
        components: {
            'v-select': vSelect
        },
        props: ['party', 'disabled', 'validate', 'name', 'fieldLabel'],
        urls: {
            getPartyInfoByPattern: window.endpoints.registry.getPartyInfoByPattern
        },
        data() {
            return {
                partyModel: this.party,
                isDisabled: this.disabled,
                parties: []
            }
        },
        methods: {
            transformJsonToViewModel(partyInfoJson) {
                let viewModel = partyInfoJson.slice()
                return viewModel
            },
            searchParties(query, loading) {
                loading(true)
                let self = this
                httpClient.get(this.$urls.getPartyInfoByPattern, {
                    text: query
                }).then((items) => {
                    self.parties = self.transformJsonToViewModel(items) || []
                    loading(false)
                })
            },
            selectParty(item) {
                this.$emit('party-selected', item)
            },
        },
        watch: {
            party() {
                this.partyModel = this.party
            },
            disabled() {
                this.isDisabled = this.disabled
            }
        }
    }
</script>
<style>
    .v-select .dropdown-toggle {
        display: flex !important;
        flex-wrap: wrap;
    }

    .v-select input[type=search], .v-select input[type=search]:focus {
        flex-basis: 20px;
        flex-grow: 1;
        height: 33px;
        padding: 0 20px 0 10px;
        width: 100% !important;
    }
</style>