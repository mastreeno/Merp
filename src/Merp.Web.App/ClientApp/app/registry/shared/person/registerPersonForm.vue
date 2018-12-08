<template>
    <div>
        <vue-element-loading v-bind:active="isSaving"></vue-element-loading>
        <form role="form" v-on:submit.prevent="saveNewPerson">
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
                            <label class="col-form-label col-md-3" for="personFirstName">{{ labels.firstName }}</label>
                            <div class="col-md-9">
                                <input type="text" name="personFirstName" id="personFirstName" class="form-control" v-model="person.firstName" v-validate="validationRules.firstName" v-bind:data-vv-as="labels.firstName" />
                                <span class="text-danger" v-if="errors.has('personFirstName')">{{ errors.first('personFirstName') }}</span>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="form-row">
                    <div class="col-md-12">
                        <div class="form-group row">
                            <label class="col-form-label col-md-3" for="personLastName">{{ labels.lastName }}</label>
                            <div class="col-md-9">
                                <input type="text" name="personLastName" id="personLastName" class="form-control" v-model="person.lastName" v-validate="validationRules.lastName" v-bind:data-vv-as="labels.lastName" />
                                <span class="text-danger" v-if="errors.has('personLastName')">{{ errors.first('personLastName') }}</span>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="form-row">
                    <div class="col-md-12">
                        <div class="form-group row">
                            <label class="col-form-label col-md-3" for="personNationalIdentificationNumber">{{ labels.nationalIdentificationNumber }}</label>
                            <div class="col-md-9">
                                <input type="text" name="personNationalIdentificationNumber" id="personNationalIdentificationNumber" class="form-control" v-model="person.nationalIdentificationNumber" v-validate="validationRules.nationalIdentificationNumber" v-bind:data-vv-as="labels.nationalIdentificationNumber" />
                                <span class="text-danger" v-if="errors.has('personNationalIdentificationNumber')">{{ errors.first('personNationalIdentificationNumber') }}</span>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="form-row">
                    <div class="col-md-12">
                        <div class="form-group row">
                            <label class="col-form-label col-md-3" for="personVatNumber">{{ labels.vatNumber }}</label>
                            <div class="col-md-9">
                                <input type="text" name="personVatNumber" id="personVatNumber" class="form-control" v-model="person.vatNumber" />
                            </div>
                        </div>
                    </div>
                </div>
                <fieldset>
                    <legend>{{ labels.addressInfo }}</legend>
                    <div class="form-row">
                        <div class="col-md-6">
                            <div class="form-group row">
                                <label class="col-form-label col-md-3" for="personAddress">{{ labels.address }}</label>
                                <div class="col-md-9">
                                    <input class="form-control" name="personAddress" id="personAddress" v-model="person.address.address" />
                                </div>
                            </div>
                        </div>
                        <div class="col-md-6">
                            <div class="form-group row">
                                <label class="col-form-label col-md-3" for="personCity">{{ labels.city }}</label>
                                <div class="col-md-9">
                                    <input class="form-control" name="personCity" id="personCity" v-model="person.address.city" />
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="form-row">
                        <div class="col-md-4">
                            <div class="form-group row">
                                <label class="col-form-label col-md-3" for="personPostalCode">{{ labels.postalCode }}</label>
                                <div class="col-md-9">
                                    <input class="form-control" name="personPostalCode" id="personPostalCode" v-model="person.address.postalCode" />
                                </div>
                            </div>
                        </div>
                        <div class="col-md-4">
                            <div class="form-group row">
                                <label class="col-form-label col-md-5" for="personProvince">{{ labels.province }}</label>
                                <div class="col-md-7">
                                    <input class="form-control" name="personProvince" id="personProvince" v-model="person.address.province" />
                                </div>
                            </div>
                        </div>
                        <div class="col-md-4">
                            <div class="form-group row">
                                <label class="col-form-label col-md-4" for="personCountry">{{ labels.country }}</label>
                                <div class="col-md-8">
                                    <input class="form-control" name="personCountry" id="personCountry" v-model="person.address.country" />
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
    import EndpointConfigurationMixin from '@/app/mixins/endpointConfigurationMixin'
    import { httpClient } from '@/app/services/httpClient'
    import errorHelper from '@/app/services/errorHelper'

    import { registerPersonValidationRules } from './validation-rules'

    export default {
        name: 'registerPersonForm',
        props: ['labels'],
        mixins: [EndpointConfigurationMixin],
        urls: {
            registerNewPerson: window.endpoints.registryInternal.registerNewPerson,
            searchPeopleByPattern: window.endpoints.registryInternal.searchPeopleByPattern
        },
        data() {
            return {
                isSaving: false,
                serverErrors: [],
                validationRules: registerPersonValidationRules,
                person: {
                    firstName: '',
                    lastName: '',
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
                personCopy: {
                    firstName: '',
                    lastName: '',
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
                let registerPersonJson = {
                    firstName: viewModel.firstName,
                    lastName: viewModel.lastName,
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

                return registerPersonJson
            },
            loadPersonInfoByPattern() {
                let self = this
                let personDisplayName = (this.person.firstName + ' ' + this.person.lastName).trim()
                httpClient.get(this.$urls.searchPeopleByPattern, {
                    query: personDisplayName
                }).then((items) => {
                    if (items.length > 0) {
                        let personCreated = items.map((i) => {
                            return {
                                id: i.id,
                                originalId: i.originalId,
                                name: i.name
                            }
                        })[0]

                        self.$emit('person-created', personCreated)
                    }

                    self.isSaving = false
                    self.$emit('new-person-form-closed')
                })
            },
            saveNewPerson() {
                this.isSaving = true
                let self = this

                let registerPersonJson = this.transformViewModelToRegister(this.person)
                httpClient.post(this.$urls.registerNewPerson, registerPersonJson)
                    .then((response) => {
                        self.loadPersonInfoByPattern()
                    })
                    .catch((errors) => {
                        self.serverErrors = errorHelper.buildErrorListFromModelState(errors) || []
                        self.isSaving = false
                    })
            },
            cancel() {
                this.$validator.reset()
                this.person = JSON.parse(JSON.stringify(this.personCopy))
                this.$emit('new-person-form-closed')
            }
        }
    }
</script>