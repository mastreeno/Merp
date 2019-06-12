<template>
    <div class="row">
        <vue-element-loading v-bind:active="isLoading"></vue-element-loading>
        <div class="col-md-10">
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
        <div class="col-md-2">
            <button type="button" class="btn btn-outline-primary" v-b-tooltip.hover v-bind:title="uiTexts.registerNewParty" v-bind:disabled="isDisabled" v-on:click="createNewParty">
                <i class="fa fa-plus"></i>
            </button>
        </div>

        <b-modal id="createNewPartyModal" v-model="showCreateNewPartyModal" hide-footer v-bind:title="uiTexts.registerNewParty" size="lg">
            <register-party-form v-bind:labels="uiTexts" v-on:new-party-form-closed="closeNewPartyModal" v-on:party-created="selectParty"></register-party-form>
        </b-modal>
    </div>
</template>
<script>
    import vSelect from 'vue-select'
    import EndpointConfigurationMixin from '@/app/shared/mixins/endpointConfigurationMixin'
    import RegisterPartyForm from './registerPartyForm.vue'

    import { httpClient } from '@/app/shared/services/httpClient'

    export default {
        name: 'partyInfoAutcomplete',
        mixins: [EndpointConfigurationMixin],
        components: {
            'v-select': vSelect,
            'register-party-form': RegisterPartyForm
        },
        props: ['party', 'disabled', 'validate', 'name', 'fieldLabel'],
        urls: {
            getPartyInfoByPattern: window.endpoints.registryInternal.getPartyInfoByPattern,
            getPartyInfoLocalization: window.endpoints.registryInternal.getPartyInfoLocalization
        },
        data() {
            return {
                partyModel: this.party,
                isDisabled: this.disabled,
                parties: [],
                isLoading: false,
                showCreateNewPartyModal: false
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
            createNewParty() {
                this.showCreateNewPartyModal = true
            },
            closeNewPartyModal() {
                this.showCreateNewPartyModal = false
            },
            loadResources() {
                this.isLoading = true
                return httpClient.get(this.$urls.getPartyInfoLocalization)
            },
            onEndResourcesLoading() {
                this.isLoading = false
            }
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