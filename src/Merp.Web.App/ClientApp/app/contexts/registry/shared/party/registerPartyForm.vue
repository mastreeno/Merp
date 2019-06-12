<template>
    <div>
        <vue-element-loading v-bind:active="isSaving"></vue-element-loading>
        <form role="form" v-on:submit.prevent="saveNewParty">
            <b-card>
                <div class="row" v-if="serverErrors.length">
                    <div class="col-md-12">
                        <b-alert show variant="danger" dismissible>
                            <ul>
                                <li v-for="error in serverErrors">{{ error }}</li>
                            </ul>
                        </b-alert>
                    </div>
                </div>
                <div class="form-row">
                    <div class="col-md-12">
                        <div class="form-group row">
                            <label class="col-form-label col-md-4" for="partyLastNameOrCompanyName">{{ labels.lastNameOrCompanyName }}</label>
                            <div class="col-md-8">
                                <input type="text" name="partyLastNameOrCompanyName" id="partyLastNameOrCompanyName" class="form-control" v-model="party.lastNameOrCompanyName" v-validate="validationRules.lastNameOrCompanyName" v-bind:data-vv-as="labels.lastNameOrCompanyName" />
                                <span class="text-danger" v-if="errors.has('partyLastNameOrCompanyName')">{{ errors.first('partyLastNameOrCompanyName') }}</span>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="form-row">
                    <div class="col-md-12">
                        <div class="form-group row">
                            <label class="col-form-label col-md-4" for="partyFirstName">{{ labels.firstName }}</label>
                            <div class="col-md-8">
                                <input type="text" name="partyFirstName" id="partyFirstName" class="form-control" v-model="party.firstName" />
                            </div>
                        </div>
                    </div>
                </div>
                <div class="form-row">
                    <div class="col-md-12">
                        <div class="form-group row">
                            <label class="col-form-label col-md-4" for="partyNationalIdentificationNumber">{{ labels.nationalIdentificationNumber }}</label>
                            <div class="col-md-8">
                                <input type="text" name="partyNationalIdentificationNumber" id="partyNationalIdentificationNumber" class="form-control" v-model="party.nationalIdentificationNumber" />
                            </div>
                        </div>
                    </div>
                </div>
                <div class="form-row">
                    <div class="col-md-12">
                        <div class="form-group row">
                            <label class="col-form-label col-md-4" for="partyVatNumber">{{ labels.vatNumber }}</label>
                            <div class="col-md-8">
                                <input type="text" name="partyVatNumber" id="partyVatNumber" class="form-control" v-model="party.vatNumber" />
                            </div>
                        </div>
                    </div>
                </div>
                <fieldset>
                    <legend>{{ labels.addressInfo }}</legend>
                    <div class="form-row">
                        <div class="col-md-6">
                            <div class="form-group row">
                                <label class="col-form-label col-md-3" for="partyAddress">{{ labels.address }}</label>
                                <div class="col-md-9">
                                    <input class="form-control" name="partyAddress" id="partyAddress" v-model="party.address.address" />
                                </div>
                            </div>
                        </div>
                        <div class="col-md-6">
                            <div class="form-group row">
                                <label class="col-form-label col-md-3" for="partyCity">{{ labels.city }}</label>
                                <div class="col-md-9">
                                    <input class="form-control" name="partyCity" id="partyCity" v-model="party.address.city" />
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="form-row">
                        <div class="col-md-4">
                            <div class="form-group row">
                                <label class="col-form-label col-md-3" for="partyPostalCode">{{ labels.postalCode }}</label>
                                <div class="col-md-9">
                                    <input class="form-control" name="partyPostalCode" id="partyPostalCode" v-model="party.address.postalCode" />
                                </div>
                            </div>
                        </div>
                        <div class="col-md-4">
                            <div class="form-group row">
                                <label class="col-form-label col-md-5" for="partyProvince">{{ labels.province }}</label>
                                <div class="col-md-7">
                                    <input class="form-control" name="partyProvince" id="partyProvince" v-model="party.address.province" />
                                </div>
                            </div>
                        </div>
                        <div class="col-md-4">
                            <div class="form-group row">
                                <label class="col-form-label col-md-4" for="partyCountry">{{ labels.country }}</label>
                                <div class="col-md-8">
                                    <input class="form-control" name="partyCountry" id="partyCountry" v-model="party.address.country" />
                                </div>
                            </div>
                        </div>
                    </div>
                </fieldset>
                <div slot="footer">
                    <div class="btn-group float-right" role="group">
                        <button type="submit" class="btn btn-primary" v-bind:disabled="!isValid">{{ labels.save }}</button>
                        <button type="button" v-on:click="cancel" class="btn btn-outline-secondary">{{ labels.cancel }}</button>
                    </div>
                </div>
            </b-card>
        </form>
    </div>
</template>
<script>
    import EndpointConfigurationMixin from '@/app/shared/mixins/endpointConfigurationMixin'
    import { httpClient } from '@/app/shared/services/httpClient'
    import errorHelper from '@/app/shared/services/errorHelper'

    import { registerPartyValidationRules } from './validation-rules'

    export default {
        name: 'registerPartyForm',
        props: ['labels'],
        mixins: [EndpointConfigurationMixin],
        urls: {
            registerNewParty: window.endpoints.registryInternal.registerNewParty,
            getPartyInfoByPattern: window.endpoints.registryInternal.getPartyInfoByPattern
        },
        data() {
            return {
                isSaving: false,
                serverErrors: [],
                validationRules: registerPartyValidationRules,
                party: {
                    firstName: '',
                    lastNameOrCompanyName: '',
                    nationalIdentificationNumber: null,
                    vatNumber: '',
                    address: {
                        address: '',
                        city: '',
                        postalCode: '',
                        province: '',
                        country: ''
                    }
                },
                partyCopy: {
                    firstName: '',
                    lastNameOrCompanyName: '',
                    nationalIdentificationNumber: '',
                    vatNumber: '',
                    address: {
                        address: '',
                        city: '',
                        postalCode: '',
                        province: '',
                        country: ''
                    }
                }
            }
        },
        computed: {
            isValid() {
                return this.errors.items.length === 0
            }
        },
        methods: {
            transformViewModelToRegister(viewModel) {
                let registerPartyJson = {
                    firstName: viewModel.firstName,
                    lastNameOrCompanyName: viewModel.lastNameOrCompanyName,
                    nationalIdentificationNumber: viewModel.nationalIdentificationNumber,
                    vatNumber: viewModel.vatNumber,
                    address: {
                        address: viewModel.address.address,
                        city: viewModel.address.city,
                        postalCode: viewModel.address.postalCode,
                        province: viewModel.address.province,
                        country: viewModel.address.country
                    }
                }

                return registerPartyJson
            },
            loadPartyInfoByPattern() {
                let self = this
                let partyDisplayName = (this.party.firstName + ' ' + this.party.lastNameOrCompanyName).trim()
                httpClient.get(this.$urls.getPartyInfoByPattern, {
                    text: partyDisplayName
                }).then((items) => {
                    if (items.length > 0) {
                        let partyCreated = items.map((i) => {
                            return {
                                id: i.id,
                                originalId: i.originalId,
                                name: i.name
                            }
                        })[0]

                        self.$emit('party-created', partyCreated)
                    }

                    self.isSaving = false
                    self.$emit('new-party-form-closed')
                })
            },
            saveNewParty() {
                this.isSaving = true
                let self = this

                let registerPartyJson = this.transformViewModelToRegister(this.party)
                httpClient.post(this.$urls.registerNewParty, registerPartyJson)
                    .then((response) => {
                        self.loadPartyInfoByPattern()
                    })
                    .catch((errors) => {
                        self.serverErrors = errorHelper.buildErrorListFromModelState(errors) || []
                        self.isSaving = false
                    })
            },
            cancel() {
                this.$validator.reset()
                this.party = JSON.parse(JSON.stringify(this.partyCopy))
                this.$emit('new-party-form-closed')
            }
        }
    }
</script>