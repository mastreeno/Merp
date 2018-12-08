<template>
    <div>
        <vue-element-loading v-bind:active="isLoading"></vue-element-loading>
        <div class="row align-items-center">
            <div class="col">
                <h2>
                    {{ uiTexts.createJobOrderPageTitle }}
                </h2>
            </div>
            <div class="col">
                <router-link to="/accountancy/joborder/search" class="btn btn-outline-secondary float-right">
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
            <form class="col-md-12" role="form" v-on:submit.prevent="createJobOrder">
                <b-card>
                    <div class="form-row">
                        <div class="form-group col-md-6">
                            <label for="customer">{{ uiTexts.customer }}</label>
                            <party-info-autocomplete v-bind:party="jobOrder.customer" name="customer" v-bind:field-label="uiTexts.customer" v-bind:disabled="false" v-on:party-selected="selectCustomer"></party-info-autocomplete>
                        </div>
                        <div class="form-group col-md-6">
                            <label for="name">{{ uiTexts.name }}</label>
                            <input type="text" name="name" v-model="jobOrder.name" class="form-control" v-validate="validationRules.name" v-bind:data-vv-as="uiTexts.name" />
                            <span class="text-danger" v-if="errors.first('name')">{{ errors.first('name') }}</span>
                        </div>
                    </div>
                    <div class="form-row">
                        <div class="form-group col-md-6">
                            <label for="contactPerson">{{ uiTexts.contactPerson }}</label>
                            <person-info-autocomplete v-bind:party="jobOrder.contactPerson" v-bind:disabled="false" v-on:person-selected="selectContactPerson"></person-info-autocomplete>
                        </div>
                        <div class="form-group col-md-6">
                            <label for="manager">{{ uiTexts.manager }}</label>
                            <person-info-autocomplete v-bind:party="jobOrder.manager" v-bind:disabled="false" v-on:person-selected="selectManager"></person-info-autocomplete>
                        </div>
                    </div>
                    <div class="form-row">
                        <div class="form-group col-md-6">
                            <label for="dateOfStart">{{ uiTexts.dateOfStart }}</label>
                            <datepicker v-bind:show-calendar-button="true"
                                        name="dateOfStart"
                                        v-model="jobOrder.dateOfStart"
                                        v-validate="validationRules.dateOfStart" 
                                        v-bind:data-vv-as="uiTexts.dateOfStart">
                            </datepicker>
                            <span class="text-danger" v-if="errors.first('dateOfStart')">{{ errors.first('dateOfStart') }}</span>
                        </div>
                        <div class="form-group col-md-6">
                            <label for="dueDate">{{ uiTexts.dueDate }}</label>
                            <datepicker v-bind:show-calendar-button="true"
                                        name="dueDate"
                                        v-model="jobOrder.dueDate"
                                        v-validate="validationRules.dueDate" 
                                        v-bind:data-vv-as="uiTexts.dueDate">
                            </datepicker>
                            <span class="text-danger" v-if="errors.first('dueDate')">{{ errors.first('dueDate') }}</span>
                        </div>
                    </div>
                    <div class="form-row">
                        <div class="form-group col-md-3">
                            <div class="mt-4">
                                <b-form-checkbox v-model="jobOrder.isTimeAndMaterial">
                                    {{ uiTexts.timeAndMaterials }}
                                </b-form-checkbox>
                            </div>
                        </div>
                        <div class="form-group col-md-3">
                            <label for="priceAmount">{{ uiTexts.amount }}</label>
                            <input type="number" step="0.01" min="0" class="form-control" name="priceAmount" v-model="jobOrder.price.amount" v-validate="validationRules.priceAmount" v-bind:data-vv-as="uiTexts.amount" />
                            <span class="text-danger" v-if="errors.first('priceAmount')">{{ errors.first('priceAmount') }}</span>
                        </div>
                        <div class="form-group col-md-2">
                            <label for="priceCurrency">{{ uiTexts.currency }}</label>
                            <input type="text" class="form-control" name="priceCurrency" v-model="jobOrder.price.currency" v-validate="validationRules.priceCurrency" v-bind:data-vv-as="uiTexts.currency" />
                            <span class="text-danger" v-if="errors.first('priceCurrency')">{{ errors.first('priceCurrency') }}</span>
                        </div>
                        <div class="form-group col-md-4">
                            <label for="purchaseOrderNumber">{{ uiTexts.purchaseOrderNumber }}</label>
                            <input type="text" name="purchaseOrderNumber" class="form-control" v-model="jobOrder.purchaseOrderNumber" />
                        </div>
                    </div>
                    <div class="form-row">
                        <div class="form-group col-md-12">
                            <label for="description">{{ uiTexts.description }}</label>
                            <input type="text" name="description" class="form-control" v-model="jobOrder.description" />
                        </div>
                    </div>
                    <div slot="footer">
                        <div class="btn-group float-right" role="group">
                            <button type="submit" class="btn btn-primary" v-bind:disabled="!isValid">{{ uiTexts.save }}</button>
                            <button type="button" v-on:click="cancel" class="btn btn-outline-secondary">{{ uiTexts.cancel }}</button>
                        </div>
                    </div>
                </b-card>
            </form>
        </div>
    </div>
</template>
<script>
    import PartyInfoAutocomplete from '@/app/registry/shared/party/partyInfoAutocomplete.vue'
    import PersonInfoAutocomplete from '@/app/registry/shared/person/personInfoAutocomplete.vue'
    import { createJobOrderValidationRules } from './validation-rules'
    import EndpointConfigurationMixin from '@/app/mixins/endpointConfigurationMixin'
    import Datepicker from '@/app/shared/widgets/datepicker.vue'

    import { httpClient } from '@/app/services/httpClient'
    import errorHelper from '@/app/services/errorHelper'

    export default {
        name: 'createJobOrder',
        mixins: [EndpointConfigurationMixin],
        components: {
            'party-info-autocomplete': PartyInfoAutocomplete,
            'person-info-autocomplete': PersonInfoAutocomplete,
            'datepicker': Datepicker
        },
        urls: {
            jobOrderCreateLocalization: window.endpoints.accountancy.jobOrderCreateLocalization,
            createNewJobOrder: window.endpoints.accountancy.createNewJobOrder
        },
        data() {
            return {
                isSaving: false,
                isLoading: true,
                serverErrors: [],
                validationRules: createJobOrderValidationRules,
                jobOrder: {
                    customer: {},
                    contactPerson: {},
                    manager: {},
                    name: null,
                    dateOfStart: null,
                    dueDate: null,
                    isTimeAndMaterial: false,
                    price: { amount: 0, currency: '' },
                    purchaseOrderNumber: '',
                    description: ''
                },
                jobOrderCopy: {
                    customer: {},
                    contactPerson: {},
                    manager: {},
                    name: null,
                    dateOfStart: null,
                    dueDate: null,
                    isTimeAndMaterial: false,
                    price: { amount: 0, currency: '' },
                    purchaseOrderNumber: '',
                    description: ''
                }
            }
        },
        computed: {
            isValid() {
                return this.errors.items.length === 0
            }
        },
        methods: {
            transformViewModelToCreate(viewModel) {
                return {
                    customer: viewModel.customer,
                    contactPerson: viewModel.contactPerson,
                    manager: viewModel.manager,
                    name: viewModel.name,
                    dateOfStart: viewModel.dateOfStart,
                    dueDate: viewModel.dueDate,
                    isTimeAndMaterial: viewModel.isTimeAndMaterial,
                    price: viewModel.price,
                    purchaseOrderNumber: viewModel.purchaseOrderNumber,
                    description: viewModel.description
                }
            },
            selectCustomer(customer) {
                this.jobOrder.customer = customer
            },
            selectContactPerson(contactPerson) {
                this.jobOrder.contactPerson = contactPerson
            },
            selectManager(manager) {
                this.jobOrder.manager = manager
            },
            createJobOrder() {
                this.isSaving = true
                let self = this
                let createJobOrderJson = this.transformViewModelToCreate(this.jobOrder)
                httpClient
                    .post(this.$urls.createNewJobOrder, createJobOrderJson)
                    .then((response) => {
                        self.isSaving = false
                        self.$router.push('/accountancy/joborder/search')
                    })
                    .catch((errors) => {
                        self.isSaving = false
                        self.serverErrors = errorHelper.buildErrorListFromModelState(errors) || []
                    })
            },
            cancel() {
                this.jobOrder = Object.assign({}, this.jobOrderCopy)
            },
            loadResources() {
                return httpClient.get(this.$urls.jobOrderCreateLocalization)
            },
            onEndResourcesLoading() {
                this.isLoading = false
            }
        }
    }
</script>