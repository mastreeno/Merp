<template>
    <div>
        <vue-element-loading v-bind:active="isLoading"></vue-element-loading>
        <div class="row align-items-center">
            <div class="col">
                <h2>
                    <i class="fa fa-user"></i> {{ uiTexts.registerPersonPageTitle }}
                </h2>
            </div>
            <div class="col">
                <router-link to="/registry/party/search" class="btn btn-outline-secondary float-right">
                    <i class="fa fa-angle-left"></i> {{ uiTexts.back }}
                </router-link>
            </div>
        </div>
        <hr />
        <div class="row" v-if="serverErrors.length">
            <div class="col-md-12">
                <b-alert show variant="danger" dismissible>
                    <ul>
                        <li v-for="error in serverErrors">{{ error }}</li>
                    </ul>
                </b-alert>
            </div>
        </div>
        <div class="row">
            <vue-element-loading v-bind:active="isSaving"></vue-element-loading>
            <person-entry-form v-bind:person-entry="person" v-bind:labels="uiTexts" v-on:save-person="addNewPerson"></person-entry-form>
        </div>
    </div>
</template>
<script>
    import PersonEntryForm from './entryForm.vue'
    import EndpointConfigurationMixin from '@/app/mixins/endpointConfigurationMixin'

    import { httpClient } from '@/app/services/httpClient'
    import errorHelper from '@/app/services/errorHelper'

    export default {
        name: 'addPersonEntry',
        mixins: [EndpointConfigurationMixin],
        components: {
            'person-entry-form': PersonEntryForm
        },
        urls: {
            personAddLocalization: window.endpoints.registry.personAddLocalization,
            registerNewPerson: window.endpoints.registry.registerNewPerson
        },
        data() {
            return {
                isSaving: false,
                isLoading: true,
                person: {
                    firstName: '',
                    lastName: '',
                    nationalIdentificationNumber: null,
                    vatNumber: '',
                    legalAddress: {
                        address: '',
                        city: '',
                        postalCode: '',
                        country: '',
                        province: ''
                    },
                    acquireShippingAddressFromLegalAddress: false,
                    shippingAddress: {
                        address: '',
                        city: '',
                        postalCode: '',
                        country: '',
                        province: ''
                    },
                    acquireBillingAddressFromLegalAddress: false,
                    billingAddress: {
                        address: '',
                        city: '',
                        postalCode: '',
                        country: '',
                        province: ''
                    },
                    contactInfo: {
                        phoneNumber: null,
                        mobileNumber: null,
                        faxNumber: null,
                        websiteAddress: null,
                        emailAddress: null,
                        instantMessaging: null
                    }
                },
                serverErrors: []
            }
        },
        methods: {
            transformViewModelToAddEntry(viewModel) {
                return {
                    firstName: viewModel.firstName,
                    lastName: viewModel.lastName,
                    nationalIdentificationNumber: viewModel.nationalIdentificationNumber,
                    vatNumber: viewModel.vatNumber,
                    legalAddress: viewModel.legalAddress,
                    acquireShippingAddressFromLegalAddress: viewModel.acquireShippingAddressFromLegalAddress,
                    shippingAddress: viewModel.shippingAddress,
                    acquireBillingAddressFromLegalAddress: viewModel.acquireBillingAddressFromLegalAddress,
                    billingAddress: viewModel.billingAddress,
                    phoneNumber: viewModel.contactInfo.phoneNumber,
                    mobileNumber: viewModel.contactInfo.mobileNumber,
                    faxNumber: viewModel.contactInfo.faxNumber,
                    websiteAddress: viewModel.contactInfo.websiteAddress,
                    emailAddress: viewModel.contactInfo.emailAddress,
                    instantMessaging: viewModel.contactInfo.instantMessaging
                }
            },
            addNewPerson(person) {
                this.isSaving = true
                let self = this
                let addPersonEntryJson = this.transformViewModelToAddEntry(person)

                httpClient.post(this.$urls.registerNewPerson, addPersonEntryJson)
                    .then((response) => {
                        self.isSaving = false
                        self.$router.push('/registry/party/search')
                    })
                    .catch((errors) => {
                        self.isSaving = false
                        self.serverErrors = errorHelper.buildErrorListFromModelState(errors) || []
                    })
            },
            loadResources() {
                return httpClient.get(this.$urls.personAddLocalization)
            },
            onEndResourcesLoading() {
                this.isLoading = false
            }
        }
    }
</script>