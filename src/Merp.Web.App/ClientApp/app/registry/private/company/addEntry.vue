<template>
    <div>
        <vue-element-loading v-bind:active="isLoading"></vue-element-loading>
        <div class="row align-items-center">
            <div class="col">
                <h2>
                    <i class="fa fa-building"></i> {{ uiTexts.registerCompanyPageTitle }}
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
            <company-entry-form v-bind:company-entry="company" v-bind:labels="uiTexts" v-on:save-company="addNewCompany"></company-entry-form>
        </div>
    </div>
</template>
<script>
    import CompanyEntryForm from './entryForm.vue'
    import EndpointConfigurationMixin from '@/app/mixins/endpointConfigurationMixin'

    import { httpClient } from '@/app/services/httpClient'
    import errorHelper from '@/app/services/errorHelper'

    export default {
        name: 'addCompanyEntry',
        mixins: [EndpointConfigurationMixin],
        components: {
            'company-entry-form': CompanyEntryForm
        },
        urls: {
            companyAddLocalization: window.endpoints.registry.companyAddLocalization,
            registerNewCompany: window.endpoints.registry.registerNewCompany
        },
        data() {
            return {
                isSaving: false,
                isLoading: true,
                company: {
                    companyName: '',
                    nationalIdentificationNumber: '',
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
                        faxNumber: null,
                        websiteAddress: null,
                        emailAddress: null
                    },
                    mainContact: {},
                    administrativeContact: {}
                },
                serverErrors: []
            }
        },
        methods: {
            transformViewModelToAddEntry(viewModel) {
                return {
                    companyName: viewModel.companyName,
                    vatNumber: viewModel.vatNumber,
                    nationalIdentificationNumber: viewModel.nationalIdentificationNumber,
                    legalAddress: viewModel.legalAddress,
                    acquireShippingAddressFromLegalAddress: viewModel.acquireShippingAddressFromLegalAddress,
                    shippingAddress: viewModel.shippingAddress,
                    acquireBillingAddressFromLegalAddress: viewModel.acquireBillingAddressFromLegalAddress,
                    billingAddress: viewModel.billingAddress,
                    mainContact: viewModel.mainContact,
                    administrativeContact: viewModel.administrativeContact,
                    phoneNumber: viewModel.contactInfo.phoneNumber,
                    faxNumber: viewModel.contactInfo.faxNumber,
                    websiteAddress: viewModel.contactInfo.websiteAddress,
                    emailAddress: viewModel.contactInfo.emailAddress
                }
            },
            addNewCompany(company) {
                this.isSaving = true
                let self = this
                let addCompanyEntryJson = this.transformViewModelToAddEntry(company)

                httpClient
                    .post(this.$urls.registerNewCompany, addCompanyEntryJson)
                    .then((response) => {
                        self.isSaving = false
                        self.$router.push('/registry/party/search')
                    }).catch((errors) => {
                        self.isSaving = false
                        self.serverErrors = errorHelper.buildErrorListFromModelState(errors) || []
                    })
            },
            loadResources() {
                return httpClient.get(this.$urls.companyAddLocalization)
            },
            onEndResourcesLoading() {
                this.isLoading = false
            }
        }
    }
</script>