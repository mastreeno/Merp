<template>
    <div>
        <vue-element-loading v-bind:active="isLoading"></vue-element-loading>
        <div class="row align-items-center">
            <div class="col">
                <h2>
                    <i class="fa fa-building"></i> {{ pageTitle }}
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
                        <b-tab active v-bind:title="uiTexts.companyInfo">
                            <section>
                                <div class="form-row">
                                    <vue-element-loading v-bind:active="isChangingCompanyName"></vue-element-loading>
                                    <change-company-name-form v-bind:company-name="company.companyName" v-bind:labels="uiTexts" v-on:change-company-name="changeCompanyName"></change-company-name-form>
                                </div>
                                <div class="form-row">
                                    <div class="col-md-6">
                                        <div class="form-group row">
                                            <label class="col-md-6 col-form-label font-weight-bold">{{ uiTexts.nationalIdentificationNumber }}</label>
                                            <div class="col-md-6">
                                                <p class="form-control-plaintext">{{ company.nationalIdentificationNumber }}</p>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-md-6">
                                        <div class="form-group row">
                                            <label class="col-md-3 col-form-label font-weight-bold">{{ uiTexts.vatNumber }}</label>
                                            <div class="col-md-9">
                                                <p class="form-control-plaintext">{{ company.vatNumber }}</p>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </section>
                            <section>
                                <vue-element-loading v-bind:active="isChangingLegalAddress"></vue-element-loading>
                                <change-address-form v-bind:address="company.legalAddress || {}" v-on:change-address="changeLegalAddress" v-bind:labels="uiTexts" v-bind:title="uiTexts.legalAddress"></change-address-form>
                            </section>
                        </b-tab>
                        <b-tab v-bind:title="uiTexts.shippingAddress">
                            <vue-element-loading v-bind:active="isChangingShippingAddress"></vue-element-loading>
                            <change-address-form v-bind:address="company.shippingAddress || {}" v-on:change-address="changeShippingAddress" v-bind:labels="uiTexts" v-bind:title="uiTexts.shippingAddress"></change-address-form>
                        </b-tab>
                        <b-tab v-bind:title="uiTexts.billingAddress">
                            <vue-element-loading v-bind:active="isChangingBillingAddress"></vue-element-loading>
                            <change-address-form v-bind:address="company.billingAddress || {}" v-on:change-address="changeBillingAddress" v-bind:labels="uiTexts" v-bind:title="uiTexts.billingAddress"></change-address-form>
                        </b-tab>
                        <b-tab v-bind:title="uiTexts.contactInfo">
                            <section>
                                <div class="form-row">
                                    <div class="form-group col-md-12">
                                        <vue-element-loading v-bind:active="isChangingMainContact"></vue-element-loading>
                                        <change-contact-form v-bind:title="uiTexts.mainContact" v-bind:contact="company.mainContact" v-bind:labels="uiTexts" v-on:contact-changed="changeMainContact"></change-contact-form>
                                    </div>
                                </div>
                                <div class="form-row">
                                    <div class="form-group col-md-12">
                                        <vue-element-loading v-bind:active="isChangingAdministrativeContact"></vue-element-loading>
                                        <change-contact-form v-bind:title="uiTexts.administrativeContact" v-bind:contact="company.administrativeContact" v-bind:labels="uiTexts" v-on:contact-changed="changeAdministrativeContact"></change-contact-form>
                                    </div>
                                </div>
                            </section>
                            <section>
                                <vue-element-loading v-bind:active="isChangingContactInfo"></vue-element-loading>
                                <change-contact-info-form v-bind:contact-info="company.contactInfo" v-bind:labels="uiTexts" v-on:change-contact-info="changeContactInfo"></change-contact-info-form>
                            </section>
                        </b-tab>
                    </b-tabs>
                </b-card>
            </div>
        </div>
    </div>
</template>
<script>
    import ChangeAddressForm from '@/app/registry/private/changeAddressForm.vue'
    import ChangeCompanyNameForm from './changeNameForm.vue'
    import ChangeCompanyContactInfoForm from './changeContactInfoForm.vue'
    import ChangeCompanyContactForm from './changeContactForm.vue'
    import EndpointConfigurationMixin from '@/app/mixins/endpointConfigurationMixin'

    import { httpClient } from '@/app/services/httpClient'
    import errorHelper from '@/app/services/errorHelper'

    export default {
        name: 'companyInfo',
        mixins: [EndpointConfigurationMixin],
        components: {
            'change-address-form': ChangeAddressForm,
            'change-company-name-form': ChangeCompanyNameForm,
            'change-contact-info-form': ChangeCompanyContactInfoForm,
            'change-contact-form': ChangeCompanyContactForm
        },
        urls: {
            companyInfoLocalization: window.endpoints.registry.companyInfoLocalization,
            companyInfo: window.endpoints.registry.companyInfo,
            changeCompanyName: window.endpoints.registry.changeCompanyName,
            changeCompanyLegalAddress: window.endpoints.registry.changeCompanyLegalAddress,
            changeCompanyShippingAddress: window.endpoints.registry.changeCompanyShippingAddress,
            changeCompanyBillingAddress: window.endpoints.registry.changeCompanyBillingAddress,
            changeCompanyContactInfo: window.endpoints.registry.changeCompanyContactInfo,
            associateMainContactToCompany: window.endpoints.registry.associateMainContactToCompany,
            associateAdministrativeContactToCompany: window.endpoints.registry.associateAdministrativeContactToCompany
        },
        data() {
            return {
                isLoading: true,
                isChangingCompanyName: false,
                isChangingLegalAddress: false,
                isChangingShippingAddress: false,
                isChangingBillingAddress: false,
                isChangingContactInfo: false,
                isChangingMainContact: false,
                isChangingAdministrativeContact: false,
                company: {},
                serverErrors: []
            }
        },
        computed: {
            companyId() {
                return this.$route.params.id
            },
            pageTitle() {
                if (!this.company || !this.company.companyName) {
                    return this.uiTexts.companyInfoLabel
                }

                return this.company.companyName.trim()
            }
        },
        methods: {
            transformJsonToViewModel(companyInfoJson) {
                return {
                    companyName: companyInfoJson.companyName,
                    vatNumber: companyInfoJson.vatNumber,
                    nationalIdentificationNumber: companyInfoJson.nationalIdentificationNumber,
                    legalAddress: companyInfoJson.legalAddress,
                    acquireShippingAddressFromLegalAddress: companyInfoJson.acquireShippingAddressFromLegalAddress,
                    shippingAddress: companyInfoJson.shippingAddress,
                    acquireBillingAddressFromLegalAddress: companyInfoJson.acquireBillingAddressFromLegalAddress,
                    billingAddress: companyInfoJson.billingAddress,
                    mainContact: companyInfoJson.mainContact,
                    administrativeContact: companyInfoJson.administrativeContact,
                    contactInfo: {
                        phoneNumber: companyInfoJson.phoneNumber,
                        faxNumber: companyInfoJson.faxNumber,
                        websiteAddress: companyInfoJson.websiteAddress,
                        emailAddress: companyInfoJson.emailAddress
                    }
                }
            },
            transformViewModelToChangeCompanyName(changeCompanyNameViewModel) {
                return {
                    newCompanyName: changeCompanyNameViewModel.newCompanyName,
                    effectiveDate: changeCompanyNameViewModel.effectiveDate
                }
            },
            transformViewModelChangeLegalAddress(changeLegalAddressViewModel) {
                return {
                    legalAddress: changeLegalAddressViewModel.address,
                    effectiveDate: changeLegalAddressViewModel.effectiveDate
                }
            },
            transformViewModelChangeShippingAddress(changeShippingAddressViewModel) {
                return {
                    shipping: changeShippingAddressViewModel.address,
                    effectiveDate: changeShippingAddressViewModel.effectiveDate
                }
            },
            transformViewModelChangeBillingAddress(changeBillingAddressViewModel) {
                return {
                    billingAddress: changeBillingAddressViewModel.address,
                    effectiveDate: changeBillingAddressViewModel.effectiveDate
                }
            },
            transformViewModelToChangeContactInfo(changeContactInfoViewModel) {
                let changeContactInfoJson = Object.assign({}, changeContactInfoViewModel)
                return changeContactInfoJson
            },
            transformViewModelToAssociateMainContact(contactViewModel) {
                return {
                    mainContact: contactViewModel
                }
            },
            transformViewModelToAssociateAdministrativeContact(contactViewModel) {
                return {
                    administrativeContact: contactViewModel
                }
            },
            loadCompanyInfo() {
                let self = this
                let url = this.$urls.companyInfo + '/' + this.companyId

                httpClient.get(url)
                    .then((company) => {
                        self.company = self.transformJsonToViewModel(company)
                        self.isLoading = false
                    })
            },
            changeCompanyName(changeCompanyNameViewModel) {
                this.isChangingCompanyName = true
                let self = this

                let url = this.$urls.changeCompanyName + '/' + this.companyId
                let changeCompanyNameJson = this.transformViewModelToChangeCompanyName(changeCompanyNameViewModel)

                httpClient.put(url, changeCompanyNameJson)
                    .then((response) => {
                        self.company.companyName = changeCompanyNameViewModel.newCompanyName
                        self.isChangingCompanyName = false
                    })
                    .catch((errors) => {
                        self.isChangingCompanyName = false
                        self.serverErrors = errorHelper.buildErrorListFromModelState(errors) || []
                    })
            },
            changeLegalAddress(legalAddress) {
                this.isChangingLegalAddress = true
                let self = this

                let url = this.$urls.changeCompanyLegalAddress + '/' + this.companyId
                let changeLegalAddressJson = this.transformViewModelChangeLegalAddress(legalAddress)

                httpClient.put(url, changeLegalAddressJson)
                    .then((response) => {
                        self.company.legalAddress = legalAddress.address || {}
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

                let url = this.$urls.changeCompanyShippingAddress + '/' + this.companyId
                let changeShippingAddressJson = this.transformViewModelChangeShippingAddress(shippingAddress)

                httpClient.put(url, changeShippingAddressJson)
                    .then((response) => {
                        self.company.shippingAddress = shippingAddress.address || {}
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

                let url = this.$urls.changeCompanyBillingAddress + '/' + this.companyId
                let changeBillingAddressJson = this.transformViewModelChangeBillingAddress(billingAddress)

                httpClient.put(url, changeBillingAddressJson)
                    .then((response) => {
                        self.company.billingAddress = billingAddress.address || {}
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

                let url = this.$urls.changeCompanyContactInfo + '/' + this.companyId
                let changeContactInfoJson = this.transformViewModelToChangeContactInfo(contactInfo)

                httpClient.put(url, changeContactInfoJson)
                    .then((response) => {
                        self.company.contactInfo = contactInfo || {}
                        self.isChangingContactInfo = false
                    })
                    .catch((errors) => {
                        self.isChangingContactInfo = false
                        self.serverErrors = errorHelper.buildErrorListFromModelState(errors) || []
                    })
            },
            changeMainContact(contact) {
                this.isChangingMainContact = true
                let self = this

                let url = this.$urls.associateMainContactToCompany + '/' + this.companyId
                let associateMainContactJson = this.transformViewModelToAssociateMainContact(contact)

                httpClient.put(url, associateMainContactJson)
                    .then((response) => {
                        self.isChangingMainContact = false
                    })
                    .catch((errors) => {
                        self.isChangingMainContact = false
                        self.serverErrors = errorHelper.buildErrorListFromModelState(errors) || []
                    })
            },
            changeAdministrativeContact(contact) {
                console.log(contact)
                this.isChangingAdministrativeContact = true
                let self = this

                let url = this.$urls.associateAdministrativeContactToCompany + '/' + this.companyId
                let associateAdministrativeContactJson = this.transformViewModelToAssociateAdministrativeContact(contact)

                httpClient.put(url, associateAdministrativeContactJson)
                    .then((response) => {
                        self.isChangingAdministrativeContact = false
                    })
                    .catch((errors) => {
                        self.isChangingAdministrativeContact = false
                        self.serverErrors = errorHelper.buildErrorListFromModelState(errors) || []
                    })
            },
            loadResources() {
                return httpClient.get(this.$urls.companyInfoLocalization)
            },
            onEndResourcesLoading() {
                this.loadCompanyInfo()
            }
        }
    }
</script>