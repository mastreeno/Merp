<template>
    <form class="col-md-12" role="form" v-on:submit.prevent="save">
        <b-card no-body>
            <b-tabs card vertical class="vertical-tabs">
                <b-tab active v-bind:title="labels.personInfo">
                    <div class="form-row">
                        <div class="form-group col-md-12">
                            <label for="firstName">{{ labels.firstName }}</label>
                            <input class="form-control" name="firstName" id="firstName" v-model="person.firstName" v-validate="validationRules.firstName" v-bind:data-vv-as="labels.firstName" />
                            <span class="text-danger" v-if="errors.first('firstName')">{{ errors.first('firstName') }}</span>
                        </div>
                    </div>
                    <div class="form-row">
                        <div class="form-group col-md-12">
                            <label for="lastName">{{ labels.lastName }}</label>
                            <input class="form-control" name="lastName" id="lastName" v-model="person.lastName" v-validate="validationRules.lastName" v-bind:data-vv-as="labels.lastName" />
                            <span class="text-danger" v-if="errors.first('lastName')">{{ errors.first('lastName') }}</span>
                        </div>
                    </div>
                    <div class="form-row">
                        <div class="form-group col-md-12">
                            <label for="nationalIdentificationNumber">{{ labels.nationalIdentificationNumber }}</label>
                            <input class="form-control" name="nationalIdentificationNumber" id="nationalIdentificationNumber" v-model="person.nationalIdentificationNumber" v-validate="validationRules.nationalIdentificationNumber" v-bind:data-vv-as="labels.nationalIdentificationNumber" />
                            <span class="text-danger" v-if="errors.first('nationalIdentificationNumber')">{{ errors.first('nationalIdentificationNumber') }}</span>
                        </div>
                    </div>
                    <div class="form-row">
                        <div class="form-group col-md-12">
                            <label for="vatNumber">{{ labels.vatNumber }}</label>
                            <vat-number-search v-model="person.vatNumber" v-bind:search="lookupPersonByVat" v-bind:labels="labels" v-on:vatnumber-found="setPersonInformation"></vat-number-search>
                        </div>
                    </div>

                    <fieldset>
                        <legend>{{ labels.legalAddress }}</legend>
                        <div class="form-row">
                            <div class="form-group col-md-12">
                                <label for="legalAddress">{{ labels.address }}</label>
                                <input class="form-control" name="legalAddress" id="legalAddress" v-model="person.legalAddress.address" />
                            </div>
                        </div>
                        <div class="form-row">
                            <div class="form-group col-md-6">
                                <label for="legalCity">{{ labels.city }}</label>
                                <input class="form-control" name="legalCity" id="legalCity" v-model="person.legalAddress.city" />
                            </div>
                            <div class="form-group col-md-6">
                                <label for="legalPostalCode">{{ labels.postalCode }}</label>
                                <input class="form-control" name="legalPostalCode" id="legalPostalCode" v-model="person.legalAddress.postalCode" />
                            </div>
                        </div>
                        <div class="form-row">
                            <div class="form-group col-md-6">
                                <label for="legalProvince">{{ labels.province }}</label>
                                <input class="form-control" name="legalProvince" id="legalProvince" v-model="person.legalAddress.province" />
                            </div>
                            <div class="form-group col-md-6">
                                <label for="legalCountry">{{ labels.country }}</label>
                                <input class="form-control" name="legalCountry" id="legalCountry" v-model="person.legalAddress.country" />
                            </div>
                        </div>
                    </fieldset>
                </b-tab>
                <b-tab v-bind:title="labels.shippingAddress">
                    <b-form-checkbox v-model="person.acquireShippingAddressFromLegalAddress">
                        {{ labels.useLegalAddress }}
                    </b-form-checkbox>
                    <hr />
                    <div class="form-row">
                        <div class="form-group col-md-12">
                            <label for="shippingAddress">{{ labels.address }}</label>
                            <input class="form-control" name="shippingAddress" id="shippingAddress" v-model="person.shippingAddress.address" v-bind:disabled="disableShippingAddress" />
                        </div>
                    </div>
                    <div class="form-row">
                        <div class="form-group col-md-6">
                            <label for="shippingCity">{{ labels.city }}</label>
                            <input class="form-control" name="shippingCity" id="shippingCity" v-model="person.shippingAddress.city" v-bind:disabled="disableShippingAddress" />
                        </div>
                        <div class="form-group col-md-6">
                            <label for="shippingPostalCode">{{ labels.postalCode }}</label>
                            <input class="form-control" name="shippingPostalCode" id="shippingPostalCode" v-model="person.shippingAddress.postalCode" v-bind:disabled="disableShippingAddress" />
                        </div>
                    </div>
                    <div class="form-row">
                        <div class="form-group col-md-6">
                            <label for="shippingProvince">{{ labels.province }}</label>
                            <input class="form-control" name="shippingProvince" id="shippingProvince" v-model="person.shippingAddress.province" v-bind:disabled="disableShippingAddress" />
                        </div>
                        <div class="form-group col-md-6">
                            <label for="shippingCountry">{{ labels.country }}</label>
                            <input class="form-control" name="shippingCountry" id="shippingCountry" v-model="person.shippingAddress.country" v-bind:disabled="disableShippingAddress" />
                        </div>
                    </div>
                </b-tab>
                <b-tab v-bind:title="labels.billingAddress">
                    <b-form-checkbox v-model="person.acquireBillingAddressFromLegalAddress">
                        {{ labels.useLegalAddress }}
                    </b-form-checkbox>
                    <hr />
                    <div class="form-row">
                        <div class="form-group col-md-12">
                            <label for="billingAddress">{{ labels.address }}</label>
                            <input class="form-control" name="billingAddress" id="billingAddress" v-model="person.billingAddress.address" v-bind:disabled="disableBillingAddress" />
                        </div>
                    </div>
                    <div class="form-row">
                        <div class="form-group col-md-6">
                            <label for="billingCity">{{ labels.city }}</label>
                            <input class="form-control" name="billingCity" id="billingCity" v-model="person.billingAddress.city" v-bind:disabled="disableBillingAddress" />
                        </div>
                        <div class="form-group col-md-6">
                            <label for="billingPostalCode">{{ labels.postalCode }}</label>
                            <input class="form-control" name="billingPostalCode" id="billingPostalCode" v-model="person.billingAddress.postalCode" v-bind:disabled="disableBillingAddress" />
                        </div>
                    </div>
                    <div class="form-row">
                        <div class="form-group col-md-6">
                            <label for="billingProvince">{{ labels.province }}</label>
                            <input class="form-control" name="billingProvince" id="billingProvince" v-model="person.billingAddress.province" v-bind:disabled="disableBillingAddress" />
                        </div>
                        <div class="form-group col-md-6">
                            <label for="billingCountry">{{ labels.country }}</label>
                            <input class="form-control" name="billingCountry" id="billingCountry" v-model="person.billingAddress.country" v-bind:disabled="disableBillingAddress" />
                        </div>
                    </div>
                </b-tab>
                <b-tab v-bind:title="labels.contactInfo">
                    <div class="form-row">
                        <div class="form-group col-md-6">
                            <label for="phoneNumber">{{ labels.phoneNumber }}</label>
                            <input class="form-control" id="phoneNumber" name="phoneNumber" v-model="person.contactInfo.phoneNumber" />
                        </div>
                        <div class="form-group col-md-6">
                            <label for="mobileNumber">{{ labels.mobileNumber }}</label>
                            <input class="form-control" id="mobileNumber" name="mobileNumber" v-model="person.contactInfo.mobileNumber" />
                        </div>
                    </div>
                    <div class="form-row">
                        <div class="form-group col-md-6">
                            <label for="faxNumber">{{ labels.faxNumber }}</label>
                            <input class="form-control" id="faxNumber" name="faxNumber" v-model="person.contactInfo.faxNumber" />
                        </div>
                        <div class="form-group col-md-6">
                            <label for="websiteAddress">{{ labels.websiteAddress }}</label>
                            <input class="form-control" id="websiteAddress" name="websiteAddress" v-model="person.contactInfo.websiteAddress" v-validate="validationRules.contactInfoWebsiteAddress" v-bind:data-vv-as="labels.websiteAddress" />
                            <span class="text-danger" v-if="errors.first('websiteAddress')">{{ errors.first('websiteAddress') }}</span>
                        </div>
                    </div>
                    <div class="form-row">
                        <div class="form-group col-md-6">
                            <label for="emailAddress">{{ labels.emailAddress }}</label>
                            <input type="email" class="form-control" id="emailAddress" name="emailAddress" v-model="person.contactInfo.emailAddress" v-validate="validationRules.contactInfoEmailAddress" v-bind:data-vv-as="labels.emailAddress" />
                            <span class="text-danger" v-if="errors.first('emailAddress')">{{ errors.first('emailAddress') }}</span>
                        </div>
                        <div class="form-group col-md-6">
                            <label for="instantMessaging">{{ labels.instantMessaging }}</label>
                            <input class="form-control" id="instantMessaging" name="instantMessaging" v-model="person.contactInfo.instantMessaging" />
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
    import { addPersonEntryValidationRules } from './validation-rules'
    import VatNumberSearch from '@/app/registry/shared/party/vatNumberSearch.vue'
    import EndpointConfigurationMixin from '@/app/mixins/endpointConfigurationMixin'

    import { httpClient } from '@/app/services/httpClient'

    export default {
        name: 'personEntryForm',
        mixins: [EndpointConfigurationMixin],
        components: {
            'vat-number-search': VatNumberSearch
        },
        props: ['personEntry', 'labels'],
        urls: {
            lookupPersonByVatNumber: window.endpoints.registryInternal.lookupPersonByVatNumber
        },
        data() {
            return {
                person: JSON.parse(JSON.stringify(this.personEntry)),
                disableShippingAddress: false,
                disableBillingAddress: false,
                validationRules: addPersonEntryValidationRules
            }
        },
        computed: {
            acquireShippingAddressFromLegalAddress() {
                return this.person.acquireShippingAddressFromLegalAddress
            },
            acquireBillingAddressFromLegalAddress() {
                return this.person.acquireBillingAddressFromLegalAddress
            },
            isValid() {
                return this.errors.items.length === 0
            }
        },
        methods: {
            save() {
                this.$emit('save-person', this.person)
            },
            cancel() {
                this.$validator.reset()
                this.person = JSON.parse(JSON.stringify(this.personEntry))
                this.$emit('cancel-save-person')
            },
            clearAddressFields(addressType) {
                this.person[addressType] = {
                    address: '',
                    city: '',
                    postalCode: '',
                    country: '',
                    province: ''
                }
            },
            lookupPersonByVat(vatNumber, country) {
                return httpClient.get(this.$urls.lookupPersonByVatNumber, {
                    vatNumber: vatNumber,
                    countryCode: country
                })
            },
            setPersonInformation(personInfo) {
                this.person.firstName = personInfo.firstName.trim()
                this.person.lastName = personInfo.lastName.trim()
                this.person.legalAddress.address = personInfo.address.trim()
                this.person.legalAddress.city = personInfo.city.trim()
                this.person.legalAddress.province = personInfo.province.trim()
                this.person.legalAddress.postalCode = personInfo.postalCode.trim()
                this.person.legalAddress.country = personInfo.country.trim()
            }
        },
        watch: {
            personEntry() {
                this.person = JSON.parse(JSON.stringify(this.personEntry))
            },
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
            }
        }
    }
</script>