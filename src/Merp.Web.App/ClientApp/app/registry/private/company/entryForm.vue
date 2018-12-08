<template>
    <form class="col-md-12" role="form" v-on:submit.prevent="save">
        <b-card no-body>
            <b-tabs card vertical class="vertical-tabs">
                <b-tab active v-bind:title="labels.companyInfo">
                    <div class="form-row">
                        <div class="form-group col-md-12">
                            <label for="companyName">{{ labels.companyName }}</label>
                            <input class="form-control" name="companyName" id="companyName" v-model="company.companyName" v-validate="validationRules.companyName" v-bind:data-vv-as="labels.companyName" />
                            <span class="text-danger" v-if="errors.first('companyName')">{{ errors.first('companyName') }}</span>
                        </div>
                    </div>
                    <div class="form-row">
                        <div class="form-group col-md-12">
                            <label for="nationalIdentificationNumber">{{ labels.nationalIdentificationNumber }}</label>
                            <input class="form-control" name="nationalIdentificationNumber" id="nationalIdentificationNumber" v-model="company.nationalIdentificationNumber" />
                        </div>
                    </div>
                    <div class="form-row">
                        <div class="form-group col-md-12">
                            <label for="vatNumber">{{ labels.vatNumber }}</label>
                            <vat-number-search v-model="company.vatNumber" v-bind:search="lookupCompanyByVat" v-bind:labels="labels" v-on:vatnumber-found="setCompanyInformation"></vat-number-search>
                            <span class="text-danger" v-if="errors.first('vatNumber')">{{ errors.first('vatNumber') }}</span>
                        </div>
                    </div>

                    <fieldset>
                        <legend>{{ labels.legalAddress }}</legend>
                        <div class="form-row">
                            <div class="form-group col-md-12">
                                <label for="legalAddress">{{ labels.address }}</label>
                                <input class="form-control" name="legalAddress" id="legalAddress" v-model="company.legalAddress.address" />
                            </div>
                        </div>
                        <div class="form-row">
                            <div class="form-group col-md-6">
                                <label for="legalCity">{{ labels.city }}</label>
                                <input class="form-control" name="legalCity" id="legalCity" v-model="company.legalAddress.city" />
                            </div>
                            <div class="form-group col-md-6">
                                <label for="legalPostalCode">{{ labels.postalCode }}</label>
                                <input class="form-control" name="legalPostalCode" id="legalPostalCode" v-model="company.legalAddress.postalCode" />
                            </div>
                        </div>
                        <div class="form-row">
                            <div class="form-group col-md-6">
                                <label for="legalProvince">{{ labels.province }}</label>
                                <input class="form-control" name="legalProvince" id="legalProvince" v-model="company.legalAddress.province" />
                            </div>
                            <div class="form-group col-md-6">
                                <label for="legalCountry">{{ labels.country }}</label>
                                <input class="form-control" name="legalCountry" id="legalCountry" v-model="company.legalAddress.country" />
                            </div>
                        </div>
                    </fieldset>
                </b-tab>
                <b-tab v-bind:title="labels.shippingAddress">
                    <b-form-checkbox v-model="company.acquireShippingAddressFromLegalAddress">
                        {{ labels.useLegalAddress }}
                    </b-form-checkbox>
                    <hr />
                    <div class="form-row">
                        <div class="form-group col-md-12">
                            <label for="shippingAddress">{{ labels.address }}</label>
                            <input class="form-control" name="shippingAddress" id="shippingAddress" v-model="company.shippingAddress.address" v-bind:disabled="disableShippingAddress" />
                        </div>
                    </div>
                    <div class="form-row">
                        <div class="form-group col-md-6">
                            <label for="shippingCity">{{ labels.city }}</label>
                            <input class="form-control" name="shippingCity" id="shippingCity" v-model="company.shippingAddress.city" v-bind:disabled="disableShippingAddress" />
                        </div>
                        <div class="form-group col-md-6">
                            <label for="shippingPostalCode">{{ labels.postalCode }}</label>
                            <input class="form-control" name="shippingPostalCode" id="shippingPostalCode" v-model="company.shippingAddress.postalCode" v-bind:disabled="disableShippingAddress" />
                        </div>
                    </div>
                    <div class="form-row">
                        <div class="form-group col-md-6">
                            <label for="shippingProvince">{{ labels.province }}</label>
                            <input class="form-control" name="shippingProvince" id="shippingProvince" v-model="company.shippingAddress.province" v-bind:disabled="disableShippingAddress" />
                        </div>
                        <div class="form-group col-md-6">
                            <label for="shippingCountry">{{ labels.country }}</label>
                            <input class="form-control" name="shippingCountry" id="shippingCountry" v-model="company.shippingAddress.country" v-bind:disabled="disableShippingAddress" />
                        </div>
                    </div>
                </b-tab>
                <b-tab v-bind:title="labels.billingAddress">
                    <b-form-checkbox v-model="company.acquireBillingAddressFromLegalAddress">
                        {{ labels.useLegalAddress }}
                    </b-form-checkbox>
                    <hr />
                    <div class="form-row">
                        <div class="form-group col-md-12">
                            <label for="billingAddress">{{ labels.address }}</label>
                            <input class="form-control" name="billingAddress" id="billingAddress" v-model="company.billingAddress.address" v-bind:disabled="disableBillingAddress" />
                        </div>
                    </div>
                    <div class="form-row">
                        <div class="form-group col-md-6">
                            <label for="billingCity">{{ labels.city }}</label>
                            <input class="form-control" name="billingCity" id="billingCity" v-model="company.billingAddress.city" v-bind:disabled="disableBillingAddress" />
                        </div>
                        <div class="form-group col-md-6">
                            <label for="billingPostalCode">{{ labels.postalCode }}</label>
                            <input class="form-control" name="billingPostalCode" id="billingPostalCode" v-model="company.billingAddress.postalCode" v-bind:disabled="disableBillingAddress" />
                        </div>
                    </div>
                    <div class="form-row">
                        <div class="form-group col-md-6">
                            <label for="billingProvince">{{ labels.province }}</label>
                            <input class="form-control" name="billingProvince" id="billingProvince" v-model="company.billingAddress.province" v-bind:disabled="disableBillingAddress" />
                        </div>
                        <div class="form-group col-md-6">
                            <label for="billingCountry">{{ labels.country }}</label>
                            <input class="form-control" name="billingCountry" id="billingCountry" v-model="company.billingAddress.country" v-bind:disabled="disableBillingAddress" />
                        </div>
                    </div>
                </b-tab>
                <b-tab v-bind:title="labels.contactInfo">
                    <div class="form-row">
                        <div class="form-group col-md-12">
                            <label for="mainContact">{{ labels.mainContact }}</label>
                            <person-info-autocomplete v-bind:person="company.mainContact" v-on:person-selected="setMainContact"></person-info-autocomplete>
                        </div>
                    </div>
                    <div class="form-row">
                        <div class="form-group col-md-12">
                            <label for="administrativeContact">{{ labels.administrativeContact }}</label>
                            <person-info-autocomplete v-bind:person="company.administrativeContact" v-on:person-selected="setAdministrativeContact"></person-info-autocomplete>
                        </div>
                    </div>
                    <div class="form-row">
                        <div class="form-group col-md-6">
                            <label for="phoneNumber">{{ labels.phoneNumber }}</label>
                            <input class="form-control" id="phoneNumber" name="phoneNumber" v-model="company.contactInfo.phoneNumber" />
                        </div>
                        <div class="form-group col-md-6">
                            <label for="faxNumber">{{ labels.faxNumber }}</label>
                            <input class="form-control" id="faxNumber" name="faxNumber" v-model="company.contactInfo.faxNumber" />
                        </div>
                    </div>
                    <div class="form-row">
                        <div class="form-group col-md-6">
                            <label for="websiteAddress">{{ labels.websiteAddress }}</label>
                            <input class="form-control" id="websiteAddress" name="websiteAddress" v-model="company.contactInfo.websiteAddress" v-validate="validationRules.contactInfoWebsiteAddress" v-bind:data-vv-as="labels.websiteAddress" />
                            <span class="text-danger" v-if="errors.first('websiteAddress')">{{ errors.first('websiteAddress') }}</span>
                        </div>
                        <div class="form-group col-md-6">
                            <label for="emailAddress">{{ labels.emailAddress }}</label>
                            <input type="email" class="form-control" id="emailAddress" name="emailAddress" v-model="company.contactInfo.emailAddress" v-validate="validationRules.contactInfoEmailAddress" v-bind:data-vv-as="labels.emailAddress" />
                            <span class="text-danger" v-if="errors.first('emailAddress')">{{ errors.first('emailAddress') }}</span>
                        </div>
                    </div>
                </b-tab>
            </b-tabs>
            <div slot="footer">
                <div class="btn-group float-right" role="group">
                    <button type="submit" class="btn btn-primary" v-bind:disabled="!isValid">{{ labels.save }}</button>
                    <button type="button" v-on:click="cancel" class="btn btn-outline-secondary">{{ labels.cancel }}</button>
                </div>
            </div>
        </b-card>
    </form>
</template>
<script>
    import VatNumberSearch from '@/app/registry/shared/party/vatNumberSearch.vue'
    import PersonInfoAutocomplete from '@/app/registry/private/personInfoAutocomplete.vue'
    import EndpointConfigurationMixin from '@/app/mixins/endpointConfigurationMixin'

    import { httpClient } from '@/app/services/httpClient'
    import { addCompanyEntryValidationRules } from './validation-rules'

    export default {
        name: 'companyEntryForm',
        mixins: [EndpointConfigurationMixin],
        components: {
            'vat-number-search': VatNumberSearch,
            'person-info-autocomplete': PersonInfoAutocomplete
        },
        urls: {
            lookupCompanyByVatNumber: window.endpoints.registryInternal.lookupCompanyByVatNumber
        },
        props: ['companyEntry', 'labels'],
        data() {
            return {
                company: JSON.parse(JSON.stringify(this.companyEntry)),
                disableShippingAddress: false,
                disableBillingAddress: false,
                validationRules: addCompanyEntryValidationRules
            }
        },
        computed: {
            acquireShippingAddressFromLegalAddress() {
                return this.company.acquireShippingAddressFromLegalAddress
            },
            acquireBillingAddressFromLegalAddress() {
                return this.company.acquireBillingAddressFromLegalAddress
            },
            isValid() {
                return this.errors.items.length === 0
            }
        },
        methods: {
            save() {
                this.$emit('save-company', this.company)
            },
            cancel() {
                this.$validator.reset()
                this.company = JSON.parse(JSON.stringify(this.companyEntry))
            },
            clearAddressFields(addressType) {
                this.company[addressType] = {
                    address: '',
                    city: '',
                    postalCode: '',
                    country: '',
                    province: ''
                }
            },
            setMainContact(contact) {
                this.company.mainContact = contact
            },
            setAdministrativeContact(contact) {
                this.company.administrativeContact = contact
            },
            lookupCompanyByVat(vatNumber, country) {
                return httpClient.get(this.$urls.lookupCompanyByVatNumber, {
                    vatNumber: vatNumber,
                    countryCode: country
                })
            },
            setCompanyInformation(companyInfo) {
                this.company.companyName = companyInfo.companyName.trim()
                this.company.legalAddress.address = companyInfo.address.trim()
                this.company.legalAddress.city = companyInfo.city.trim()
                this.company.legalAddress.province = companyInfo.province.trim()
                this.company.legalAddress.postalCode = companyInfo.postalCode.trim()
                this.company.legalAddress.country = companyInfo.country.trim()
            }
        },
        watch: {
            acquireShippingAddressFromLegalAddress() {
                if (this.acquireShippingAddressFromLegalAddress) {
                    this.clearAddressFields('shippingAddress')
                }

                this.disableShippingAddress = this.acquireShippingAddressFromLegalAddress
            },
            acquireBillingAddressFromLegalAddress() {
                if (this.acquireBillingAddressFromLegalAddress) {
                    this.clearAddressFields('billingAddress')
                }

                this.disableBillingAddress = this.acquireBillingAddressFromLegalAddress
            },
            companyEntry() {
                this.company = JSON.parse(JSON.stringify(this.companyEntry))
            }
        }
    }
</script>