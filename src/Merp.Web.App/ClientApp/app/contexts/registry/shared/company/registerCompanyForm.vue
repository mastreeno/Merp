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
                            <label class="col-form-label col-md-3" for="companyName">{{ labels.companyName }}</label>
                            <div class="col-md-9">
                                <input type="text" name="companyName" id="companyName" class="form-control" v-model="company.companyName" v-validate="validationRules.companyName" v-bind:data-vv-as="labels.companyName" />
                                <span class="text-danger" v-if="errors.has('companyName')">{{ errors.first('companyName') }}</span>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="form-row">
                    <div class="col-md-12">
                        <div class="form-group row">
                            <label class="col-form-label col-md-3" for="companyNationalIdentificationNumber">{{ labels.nationalIdentificationNumber }}</label>
                            <div class="col-md-9">
                                <input type="text" name="companyNationalIdentificationNumber" id="companyNationalIdentificationNumber" class="form-control" v-model="company.nationalIdentificationNumber" />
                            </div>
                        </div>
                    </div>
                </div>
                <div class="form-row">
                    <div class="col-md-12">
                        <div class="form-group row">
                            <label class="col-form-label col-md-3" for="companyVatNumber">{{ labels.vatNumber }}</label>
                            <div class="col-md-9">
                                <input type="text" name="companyVatNumber" id="companyVatNumber" class="form-control" v-model="company.vatNumber" v-validate="validationRules.vatNumber" v-bind:data-vv-as="labels.vatNumber" />
                                <span class="text-danger" v-if="errors.has('companyVatNumber')">{{ errors.first('companyVatNumber') }}</span>
                            </div>
                        </div>
                    </div>
                </div>
                <fieldset>
                    <legend>{{ labels.addressInfo }}</legend>
                    <div class="form-row">
                        <div class="col-md-6">
                            <div class="form-group row">
                                <label class="col-form-label col-md-3" for="companyAddress">{{ labels.address }}</label>
                                <div class="col-md-9">
                                    <input class="form-control" name="companyAddress" id="companyAddress" v-model="company.address.address" />
                                </div>
                            </div>
                        </div>
                        <div class="col-md-6">
                            <div class="form-group row">
                                <label class="col-form-label col-md-3" for="companyCity">{{ labels.city }}</label>
                                <div class="col-md-9">
                                    <input class="form-control" name="companyCity" id="companyCity" v-model="company.address.city" />
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="form-row">
                        <div class="col-md-4">
                            <div class="form-group row">
                                <label class="col-form-label col-md-3" for="companyPostalCode">{{ labels.postalCode }}</label>
                                <div class="col-md-9">
                                    <input class="form-control" name="companyPostalCode" id="companyPostalCode" v-model="company.address.postalCode" />
                                </div>
                            </div>
                        </div>
                        <div class="col-md-4">
                            <div class="form-group row">
                                <label class="col-form-label col-md-5" for="companyProvince">{{ labels.province }}</label>
                                <div class="col-md-7">
                                    <input class="form-control" name="companyProvince" id="companyProvince" v-model="company.address.province" />
                                </div>
                            </div>
                        </div>
                        <div class="col-md-4">
                            <div class="form-group row">
                                <label class="col-form-label col-md-4" for="companyCountry">{{ labels.country }}</label>
                                <div class="col-md-8">
                                    <input class="form-control" name="companyCountry" id="companyCountry" v-model="company.address.country" />
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

    import { registerCompanyValidationRules } from './validation-rules'

    export default {
        name: 'registerCompanyForm',
        props: ['labels'],
        mixins: [EndpointConfigurationMixin],
        urls: {
            registerNewCompany: window.endpoints.registryInternal.registerNewCompany,
            getPartyInfoByPattern: window.endpoints.registryInternal.getPartyInfoByPattern
        },
        data() {
            return {
                isSaving: false,
                serverErrors: [],
                validationRules: registerCompanyValidationRules,
                company: {
                    companyName: '',
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
                companyCopy: {
                    companyName: '',
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
                let registerCompanyJson = {
                    companyName: viewModel.companyName,
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

                return registerCompanyJson
            },
            loadCompanyInfoByPattern() {
                let self = this
                let companyDisplayName = (this.party.firstName + ' ' + this.party.lastNameOrCompanyName).trim()
                httpClient.get(this.$urls.getPartyInfoByPattern, {
                    text: companyDisplayName
                }).then((items) => {
                    if (items.length > 0) {
                        let companyCreated = items.map((i) => {
                            return {
                                id: i.id,
                                originalId: i.originalId,
                                name: i.name
                            }
                        })[0]

                        self.$emit('company-created', companyCreated)
                    }

                    self.isSaving = false
                    self.$emit('new-company-form-closed')
                })
            },
            saveNewCompany() {
                this.isSaving = true
                let self = this

                let registerCompanyJson = this.transformViewModelToRegister(this.company)
                httpClient.post(this.$urls.registerNewCompany, registerCompanyJson)
                    .then((response) => {
                        self.loadCompanyInfoByPattern()
                    })
                    .catch((errors) => {
                        self.serverErrors = errorHelper.buildErrorListFromModelState(errors) || []
                        self.isSaving = false
                    })
            },
            cancel() {
                this.$validator.reset()
                this.company = JSON.parse(JSON.stringify(this.companyCopy))
                this.$emit('new-company-form-closed')
            }
        }
    }
</script>