<template>
    <b-modal id="addPersonEntryModal" v-model="showModal" hide-footer v-bind:title="uiTexts.registerPersonPageTitle" size="lg">
        <vue-element-loading v-bind:active="isSaving || isLoading"></vue-element-loading>
        <person-entry-form v-bind:person-entry="person" v-bind:labels="uiTexts" v-on:save-person="addNewPerson" v-on:cancel-save-person="closeModal"></person-entry-form>
    </b-modal>
</template>
<script>
    import PersonEntryForm from './entryForm.vue'
    import EndpointConfigurationMixin from '@/app/mixins/endpointConfigurationMixin'

    import { httpClient } from '@/app/services/httpClient'
    import errorHelper from '@/app/services/errorHelper'

    export default {
        name: 'addPersonEntryModal',
        mixins: [EndpointConfigurationMixin],
        components: {
            'person-entry-form': PersonEntryForm
        },
        props: ['show'],
        urls: {
            personAddLocalization: window.endpoints.registry.personAddLocalization,
            registerNewPerson: window.endpoints.registry.registerNewPerson
        },
        data() {
            return {
                showModal: this.show,
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
                }
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
                        let fullName = person.firstName + ' ' + person.lastName
                        this.setPersonDefault()
                        this.$emit('person-saved', fullName.trim())
                    })
                    .catch((errors) => {
                        self.isSaving = false
                        self.serverErrors = errorHelper.buildErrorListFromModelState(errors) || []
                    })
            },
            closeModal() {
                this.showModal = false
            },
            setPersonDefault() {
                this.person = {
                    firstName: '',
                    lastName: '',
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
                        mobileNumber: null,
                        faxNumber: null,
                        websiteAddress: null,
                        emailAddress: null,
                        instantMessaging: null
                    }
                }
            },
            loadResources() {
                return httpClient.get(this.$urls.personAddLocalization)
            },
            onEndResourcesLoading() {
                this.isLoading = false
            }
        },
        watch: {
            show() {
                this.showModal = this.show
            },
            showModal() {
                if (!this.showModal) {
                    this.$emit('add-person-modal-closed')
                }
            }
        }
    }
</script>