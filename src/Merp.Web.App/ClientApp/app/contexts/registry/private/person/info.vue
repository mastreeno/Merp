<template>
    <div>
        <vue-element-loading v-bind:active="isLoading"></vue-element-loading>
        <div class="row align-items-center">
            <div class="col">
                <h2>
                    <i class="fa fa-user"></i> {{ pageTitle }}
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
            <div class="col-md-12">
                <b-card no-body>
                    <b-tabs card vertical class="vertical-tabs">
                        <b-tab active v-bind:title="uiTexts.personInfo">
                            <section>
                                <div class="form-row">
                                    <div class="col-md-12">
                                        <div class="form-group row">
                                            <label class="col-md-3 col-form-label font-weight-bold">{{ uiTexts.fullName }}</label>
                                            <div class="col-md-9">
                                                <p class="form-control-plaintext">{{ personFullName }}</p>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="form-row">
                                    <div class="col-md-6">
                                        <div class="form-group row">
                                            <label class="col-md-6 col-form-label font-weight-bold">{{ uiTexts.nationalIdentificationNumber }}</label>
                                            <div class="col-md-6">
                                                <p class="form-control-plaintext">{{ person.nationalIdentificationNumber }}</p>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-md-6">
                                        <div class="form-group row">
                                            <label class="col-md-3 col-form-label font-weight-bold">{{ uiTexts.vatNumber }}</label>
                                            <div class="col-md-9">
                                                <p class="form-control-plaintext">{{ person.vatNumber }}</p>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </section>
                            <section>
                                <vue-element-loading v-bind:active="isChangingLegalAddress"></vue-element-loading>
                                <change-address-form v-bind:address="person.legalAddress || {}" v-bind:labels="uiTexts" v-bind:title="uiTexts.legalAddress" v-on:change-address="changeLegalAddress"></change-address-form>
                            </section>
                        </b-tab>
                        <b-tab v-bind:title="uiTexts.shippingAddress">
                            <vue-element-loading v-bind:active="isChangingShippingAddress"></vue-element-loading>
                            <change-address-form v-bind:address="person.shippingAddress || {}" v-bind:labels="uiTexts" v-bind:title="uiTexts.shippingAddress" v-on:change-address="changeShippingAddress"></change-address-form>
                        </b-tab>
                        <b-tab v-bind:title="uiTexts.billingAddress">
                            <vue-element-loading v-bind:active="isChangingBillingAddress"></vue-element-loading>
                            <change-address-form v-bind:address="person.billingAddress || {}" v-bind:labels="uiTexts" v-bind:title="uiTexts.billingAddress" v-on:change-address="changeBillingAddress"></change-address-form>
                        </b-tab>
                        <b-tab v-bind:title="uiTexts.contactInfo">
                            <vue-element-loading v-bind:active="isChangingContactInfo"></vue-element-loading>
                            <change-contact-info-form v-bind:contact-info="person.contactInfo || {}" v-bind:labels="uiTexts" v-on:change-contact-info="changeContactInfo"></change-contact-info-form>
                        </b-tab>
                    </b-tabs>
                </b-card>
            </div>
        </div>
    </div>
</template>
<script>
    import ChangeAddressForm from '@/app/contexts/registry/private/changeAddressForm.vue'
    import ChangePersonContactInfoForm from './changeContactInfoForm.vue'
    import EndpointConfigurationMixin from '@/app/shared/mixins/endpointConfigurationMixin'

    import { httpClient } from '@/app/shared/services/httpClient'
    import errorHelper from '@/app/shared/services/errorHelper'

    export default {
        name: 'personInfo',
        mixins: [EndpointConfigurationMixin],
        components: {
            'change-address-form': ChangeAddressForm,
            'change-contact-info-form': ChangePersonContactInfoForm
        },
        urls: {
            personInfoLocalization: window.endpoints.registry.personInfoLocalization,
            personInfo: window.endpoints.registry.personInfo,
            changePersonLegalAddress: window.endpoints.registry.changePersonLegalAddress,
            changePersonShippingAddress: window.endpoints.registry.changePersonShippingAddress,
            changePersonBillingAddress: window.endpoints.registry.changePersonBillingAddress,
            changePersonContactInfo: window.endpoints.registry.changePersonContactInfo
        },
        data() {
            return {
                isLoading: true,
                isChangingLegalAddress: false,
                isChangingShippingAddress: false,
                isChangingBillingAddress: false,
                isChangingContactInfo: false,
                person: {},
                serverErrors: []
            }
        },
        computed: {
            personId() {
                return this.$route.params.id
            },
            pageTitle() {
                if (!this.person || (!this.person.firstName && !this.person.lastName)) {
                    return 'Person info'
                }

                return (this.person.firstName + ' ' + this.person.lastName).trim()
            },
            personFullName() {
                return (this.person.firstName + ' ' + this.person.lastName).trim()
            }
        },
        methods: {
            transformJsonToViewModel(personInfoJson) {
                return {
                    id: personInfoJson.id,
                    originalId: personInfoJson.originalId,
                    firstName: personInfoJson.firstName,
                    lastName: personInfoJson.lastName,
                    nationalIdentificationNumber: personInfoJson.nationalIdentificationNumber,
                    vatNumber: personInfoJson.vatNumber,
                    legalAddress: personInfoJson.legalAddress,
                    shippingAddress: personInfoJson.shippingAddress,
                    billingAddress: personInfoJson.billingAddress,
                    contactInfo: {
                        phoneNumber: personInfoJson.phoneNumber,
                        mobileNumber: personInfoJson.mobileNumber,
                        faxNumber: personInfoJson.faxNumber,
                        websiteAddress: personInfoJson.websiteAddress,
                        emailAddress: personInfoJson.emailAddress,
                        instantMessaging: personInfoJson.instantMessaging
                    }
                }
            },
            transformViewModelToChangeLegalAddress(changeLegalAddressViewModel) {
                let changeLegalAddressJson = Object.assign({}, changeLegalAddressViewModel)
                return changeLegalAddressJson
            },
            transformViewModelToChangeShippingAddress(changeShippingAddressViewModel) {
                let changeShippingAddressJson = Object.assign({}, changeShippingAddressViewModel)
                return changeShippingAddressJson
            },
            transformViewModelToChangeBillingAddress(changeBillingAddressViewModel) {
                let changeBillingAddressJson = Object.assign({}, changeBillingAddressViewModel)
                return changeBillingAddressJson
            },
            transformViewModelToChangeContactInfo(changeContactInfoViewModel) {
                let changeContactInfoJson = Object.assign({}, changeContactInfoViewModel)
                return changeContactInfoJson
            },
            loadPersonInfo() {
                let self = this
                let url = this.$urls.personInfo + '/' + this.personId
                httpClient.get(url)
                    .then((person) => {
                        self.person = self.transformJsonToViewModel(person)
                        self.isLoading = false
                    })
            },
            changeLegalAddress(legalAddress) {
                this.isChangingLegalAddress = true
                let self = this

                let url = this.$urls.changePersonLegalAddress + '/' + this.personId
                let changeLegalAddressJson = this.transformViewModelToChangeLegalAddress(legalAddress)

                httpClient.put(url, changeLegalAddressJson)
                    .then((response) => {
                        self.person.legalAddress = legalAddress.address || {}
                        self.isChangingLegalAddress = false
                    })
                    .catch((errors) => {
                        self.isChangingLegalAddress = false
                        self.serverErrors = errorHelper.buildErrorListFromModelState(errors) || []
                    })
            },
            changeShippingAddress(shippingAddress) {
                this.isChangingShippingAddress = true
                let self = this

                let url = this.$urls.changePersonShippingAddress + '/' + this.personId
                let changeShippingAddressJson = this.transformViewModelToChangeShippingAddress(shippingAddress)

                httpClient.put(url, changeShippingAddressJson)
                    .then((response) => {
                        self.person.shippingAddress = shippingAddress.address || {}
                        self.isChangingShippingAddress = false
                    })
                    .catch((errors) => {
                        self.isChangingShippingAddress = false
                        self.serverErrors = errorHelper.buildErrorListFromModelState(errors) || []
                    })
            },
            changeBillingAddress(billingAddress) {
                this.isChangingBillingAddress = true
                let self = this

                let url = this.$urls.changePersonBillingAddress + '/' + this.personId
                let changeBillingAddressJson = this.transformViewModelToChangeBillingAddress(billingAddress)

                httpClient.put(url, changeBillingAddressJson)
                    .then((response) => {
                        self.person.billingAddress = billingAddress.address || {}
                        self.isChangingBillingAddress = false
                    })
                    .catch((errors) => {
                        self.isChangingBillingAddress = false
                        self.serverErrors = errorHelper.buildErrorListFromModelState(errors) || []
                    })
            },
            changeContactInfo(contactInfo) {
                this.isChangingContactInfo = true
                let self = this

                let url = this.$urls.changePersonContactInfo + '/' + this.personId
                let changeContactInfoJson = this.transformViewModelToChangeContactInfo(contactInfo)

                httpClient.put(url, changeContactInfoJson)
                    .then((response) => {
                        person.contactInfo = contactInfo || {}
                        self.isChangingContactInfo = false
                    })
                    .catch((errors) => {
                        self.isChangingContactInfo = false
                        self.serverErrors = errors || []
                    })
            },
            loadResources() {
                return httpClient.get(this.$urls.personInfoLocalization)
            },
            onEndResourcesLoading() {
                this.loadPersonInfo()
            }
        }
    }
</script>