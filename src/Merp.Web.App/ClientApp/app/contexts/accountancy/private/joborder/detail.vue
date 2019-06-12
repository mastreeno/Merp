<template>
    <div>
        <vue-element-loading v-bind:active="isLoading"></vue-element-loading>
        <div class="row align-items-center">
            <div class="col">
                <h2>
                    {{ jobOrder.jobOrderName }}
                </h2>
            </div>
            <div class="col">
                <router-link to="/accountancy/joborder/search" class="btn btn-outline-secondary float-right">
                    <i class="fa fa-angle-left"></i> {{ uiTexts.back }}
                </router-link>
            </div>
        </div>
        <hr />
        <div class="row" v-if="!jobOrder.isCompleted">
            <div class="btn btn-group">
                <button class="btn btn-outline-primary" v-on:click="openMarkJobOrderAsCompletedModal">{{ uiTexts.markAsCompleted }}</button>
                <button class="btn btn-outline-primary" v-on:click="openExtendJobOrderModal">{{ uiTexts.extend }}</button>
            </div>
        </div>
        <div class="row">
            <div class="col-md-12">
                <b-card no-body>
                    <b-tabs card>
                        <b-tab active v-bind:title="uiTexts.general">
                            <div class="form-row">
                                <div class="col-md-6">
                                    <div class="form-group row">
                                        <label class="col-md-3 col-form-label font-weight-bold">{{ uiTexts.number }}</label>
                                        <div class="col-md-9">
                                            <p class="form-control-plaintext">{{ jobOrder.jobOrderNumber }}</p>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-md-6">
                                    <div class="form-group row">
                                        <label class="col-md-3 col-form-label font-weight-bold">{{ uiTexts.customer }}</label>
                                        <div class="col-md-9">
                                            <p class="form-control-plaintext">{{ customerName }}</p>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="form-row">
                                <div class="col-md-6">
                                    <div class="form-group row">
                                        <label class="col-md-4 col-form-label font-weight-bold">{{ uiTexts.contactPerson }}</label>
                                        <div class="col-md-8">
                                            <p class="form-control-plaintext">{{ contactPersonName }}</p>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-md-6">
                                    <div class="form-group row">
                                        <label class="col-md-3 col-form-label font-weight-bold">{{ uiTexts.manager }}</label>
                                        <div class="col-md-9">
                                            <p class="form-control-plaintext">{{ managerName }}</p>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="form-row">
                                <div class="col-md-6">
                                    <div class="form-group row">
                                        <label class="col-md-3 col-form-label font-weight-bold">{{ uiTexts.dateOfStart }}</label>
                                        <div class="col-md-9">
                                            <p class="form-control-plaintext">{{ jobOrder.dateOfStart | dateFormat }}</p>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-md-6">
                                    <div class="form-group row">
                                        <label class="col-md-4 col-form-label font-weight-bold">{{ uiTexts.dueDate }}</label>
                                        <div class="col-md-8">
                                            <p class="form-control-plaintext">{{ jobOrder.dueDate | dateFormat }}</p>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="form-row">
                                <div class="col-md-6">
                                    <div class="form-group row">
                                        <label class="col-md-3 col-form-label font-weight-bold">{{ uiTexts.price }}</label>
                                        <div class="col-md-9">
                                            <p class="form-control-plaintext">{{ jobOrder.price | currencyFormat(2) }}</p>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-md-6">
                                    <div class="form-group row">
                                        <label class="col-md-3 col-form-label font-weight-bold">{{ uiTexts.balance }}</label>
                                        <div class="col-md-9">
                                            <p class="form-control-plaintext">{{ jobOrder.balance | currencyFormat(2) }}</p>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </b-tab>
                        <b-tab v-bind:title="uiTexts.description">
                            <p>{{ jobOrder.description }}</p>
                        </b-tab>
                        <b-tab v-bind:title="uiTexts.trendAnalysis">
                            <job-order-trend-analysis v-bind:labels="uiTexts" v-bind:job-order-id="jobOrderId"></job-order-trend-analysis>
                        </b-tab>
                        <b-tab v-bind:title="uiTexts.incomingInvoices">
                            <div class="col-md-12">
                                <table class="table table-striped">
                                    <thead>
                                        <tr>
                                            <th>{{ uiTexts.dateOfIssue }}</th>
                                            <th>{{ uiTexts.invoiceNumber }}</th>
                                            <th>{{ uiTexts.amount }}</th>
                                            <th>{{ uiTexts.currency }}</th>
                                            <th>{{ uiTexts.supplier }}</th>
                                        </tr>
                                    </thead>
                                    <tbody>
                                        <tr v-for="invoice in incomingInvoices" v-bind:key="invoice.number">
                                            <td>{{ invoice.dateOfIssue | dateFormat }}</td>
                                            <td>{{ invoice.number }}</td>
                                            <td>{{ invoice.price | currencyFormat(2) }}</td>
                                            <td>{{ invoice.currency }}</td>
                                            <td>{{ invoice.supplierName }}</td>
                                        </tr>
                                    </tbody>
                                </table>
                            </div>
                        </b-tab>
                        <b-tab v-bind:title="uiTexts.outgoingInvoices">
                            <div class="col-md-12">
                                <table class="table table-striped">
                                    <thead>
                                        <tr>
                                            <th>{{ uiTexts.dateOfIssue }}</th>
                                            <th>{{ uiTexts.invoiceNumber }}</th>
                                            <th>{{ uiTexts.amount }}</th>
                                            <th>{{ uiTexts.currency }}</th>
                                            <th>{{ uiTexts.customer }}</th>
                                        </tr>
                                    </thead>
                                    <tbody>
                                        <tr v-for="invoice in outgoingInvoices" v-bind:key="invoice.number">
                                            <td>{{ invoice.dateOfIssue | dateFormat }}</td>
                                            <td>{{ invoice.number }}</td>
                                            <td>{{ invoice.price | currencyFormat(2) }}</td>
                                            <td>{{ invoice.currency }}</td>
                                            <td>{{ invoice.customerName }}</td>
                                        </tr>
                                    </tbody>
                                </table>
                            </div>
                        </b-tab>
                        <b-tab v-bind:title="uiTexts.incomingCreditNotes">
                            <div class="col-md-12">
                                <table class="table table-striped">
                                    <thead>
                                        <tr>
                                            <th>{{ uiTexts.dateOfIssue }}</th>
                                            <th>{{ uiTexts.invoiceNumber }}</th>
                                            <th>{{ uiTexts.amount }}</th>
                                            <th>{{ uiTexts.currency }}</th>
                                            <th>{{ uiTexts.supplier }}</th>
                                        </tr>
                                    </thead>
                                    <tbody>
                                        <tr v-for="creditNote in incomingCreditNotes" v-bind:key="creditNote.number">
                                            <td>{{ creditNote.dateOfIssue | dateFormat }}</td>
                                            <td>{{ creditNote.number }}</td>
                                            <td>{{ creditNote.price | currencyFormat(2) }}</td>
                                            <td>{{ creditNote.currency }}</td>
                                            <td>{{ creditNote.supplierName }}</td>
                                        </tr>
                                    </tbody>
                                </table>
                            </div>
                        </b-tab>
                        <b-tab v-bind:title="uiTexts.outgoingCreditNotes">
                            <div class="col-md-12">
                                <table class="table table-striped">
                                    <thead>
                                        <tr>
                                            <th>{{ uiTexts.dateOfIssue }}</th>
                                            <th>{{ uiTexts.invoiceNumber }}</th>
                                            <th>{{ uiTexts.amount }}</th>
                                            <th>{{ uiTexts.currency }}</th>
                                            <th>{{ uiTexts.customer }}</th>
                                        </tr>
                                    </thead>
                                    <tbody>
                                        <tr v-for="creditNote in outgoingCreditNotes" v-bind:key="creditNote.number">
                                            <td>{{ creditNote.dateOfIssue | dateFormat }}</td>
                                            <td>{{ creditNote.number }}</td>
                                            <td>{{ creditNote.price | currencyFormat(2) }}</td>
                                            <td>{{ creditNote.currency }}</td>
                                            <td>{{ creditNote.customerName }}</td>
                                        </tr>
                                    </tbody>
                                </table>
                            </div>
                        </b-tab>
                    </b-tabs>
                </b-card>
            </div>
        </div>

        <b-modal id="extendJobOrderModal" v-model="showExtendJobOrderModal" v-bind:ok-title="uiTexts.ok" v-bind:cancel-title="uiTexts.cancel" v-bind:title="uiTexts.extendJobOrder" v-on:ok="extendJobOrder" v-bind:ok-disabled="isExtendJobOrderFormValid">
            <vue-element-loading v-bind:active="isExtendingJobOrder"></vue-element-loading>
            <form role="form" v-on:submit.prevent="extendJobOrder" data-vv-scope="extendJobOrder">
                <div class="form-row">
                    <div class="col-md-12">
                        <div class="form-group row">
                            <label class="col-md-3 col-form-label font-weight-bold">{{ uiTexts.name }}</label>
                            <div class="col-md-9">
                                <p class="form-control-plaintext">{{ jobOrder.jobOrderName }}</p>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="form-row">
                    <div class="col-md-12">
                        <div class="form-group row">
                            <label class="col-md-3 col-form-label font-weight-bold">{{ uiTexts.number }}</label>
                            <div class="col-md-9">
                                <p class="form-control-plaintext">{{ jobOrder.jobOrderNumber }}</p>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="form-row">
                    <div class="col-md-12">
                        <div class="form-group row">
                            <label class="col-md-4 col-form-label font-weight-bold" for="newDueDate">{{ uiTexts.newDueDate }}</label>
                            <div class="col-md-8">
                                <datepicker v-bind:show-calendar-button="true"
                                            name="newDueDate"
                                            v-model="extendJobOrderModel.newDueDate"
                                            v-validate="extendJobOrderValidationRules.newDueDate" 
                                            v-bind:data-vv-as="uiTexts.newDueDate">
                                </datepicker>
                                <span class="text-danger" v-if="errors.first('extendJobOrder.newDueDate')">{{ errors.first('extendJobOrder.newDueDate') }}</span>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="form-row">
                    <div class="col-md-12">
                        <div class="form-group row">
                            <label class="col-md-4 col-form-label font-weight-bold" for="price">{{ uiTexts.price }}</label>
                            <div class="col-md-8">
                                <input type="number" step="0.01" min="0" name="price" v-model="extendJobOrderModel.price" class="form-control" v-validate="extendJobOrderValidationRules.price" v-bind:data-vv-as="uiTexts.price" />
                                <span class="text-danger" v-if="errors.first('extendJobOrder.price')">{{ errors.first('extendJobOrder.price') }}</span>
                            </div>
                        </div>
                    </div>
                </div>
            </form>
        </b-modal>

        <b-modal id="markJobOrderAsCompletedModal" v-model="showMarkJobOrderAsCompletedModal" v-bind:ok-title="uiTexts.ok" v-bind:cancel-title="uiTexts.cancel" v-bind:title="uiTexts.markJobOrderAsCompleted" v-on:ok="markJobOrderAsCompleted" v-bind:ok-disabled="isMarkJobOrderAsCompletedFormValid">
            <vue-element-loading v-bind:active="isMarkingJobOrderAsCompleted"></vue-element-loading>
            <form role="form" v-on:submit.prevent="markJobOrderAsCompleted" data-vv-scope="markJobOrderAsCompleted">
                <div class="form-row">
                    <div class="col-md-12">
                        <div class="form-group row">
                            <label class="col-md-3 col-form-label font-weight-bold">{{ uiTexts.name }}</label>
                            <div class="col-md-9">
                                <p class="form-control-plaintext">{{ jobOrder.jobOrderName }}</p>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="form-row">
                    <div class="col-md-12">
                        <div class="form-group row">
                            <label class="col-md-3 col-form-label font-weight-bold">{{ uiTexts.number }}</label>
                            <div class="col-md-9">
                                <p class="form-control-plaintext">{{ jobOrder.jobOrderNumber }}</p>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="form-row">
                    <div class="col-md-12">
                        <div class="form-group row">
                            <label class="col-md-5 col-form-label font-weight-bold" for="dateOfCompletion">{{ uiTexts.dateOfCompletion }}</label>
                            <div class="col-md-7">
                                <datepicker v-bind:show-calendar-button="true"
                                            name="dateOfCompletion"
                                            v-model="markJobOrderAsCompletedModel.dateOfCompletion"
                                            v-validate="markJobOrderAsCompletedValidationRules.dateOfCompletion" 
                                            v-bind:data-vv-as="uiTexts.dateOfCompletion">
                                </datepicker>
                                <span class="text-danger" v-if="errors.first('markJobOrderAsCompleted.dateOfCompletion')">{{ errors.first('markJobOrderAsCompleted.dateOfCompletion') }}</span>
                            </div>
                        </div>
                    </div>
                </div>
            </form>
        </b-modal>
    </div>
</template>
<script>
    import JobOrderTrendAnalysis from './trendAnalysis.vue'
    import { extendJobOrderValidationRules, markJobOrderAsCompletedValidationRules } from './validation-rules'
    import EndpointConfigurationMixin from '@/app/shared/mixins/endpointConfigurationMixin'
    import Datepicker from '@/app/shared/components/datepicker.vue'

    import { httpClient } from '@/app/shared/services/httpClient'

    export default {
        name: 'jobOrderDetail',
        mixins: [EndpointConfigurationMixin],
        components: {
            'job-order-trend-analysis': JobOrderTrendAnalysis,
            'datepicker': Datepicker
        },
        urls: {
            jobOrderDetailLocalization: window.endpoints.accountancy.jobOrderDetailLocalization,
            jobOrderDetail: window.endpoints.accountancy.jobOrderDetail,
            getPartyInfoById: window.endpoints.registry.getPartyInfoById,
            getOutgoingCreditNotesAssociatedToJobOrder: window.endpoints.accountancy.getOutgoingCreditNotesAssociatedToJobOrder,
            getOutgoingInvoicesAssociatedToJobOrder: window.endpoints.accountancy.getOutgoingInvoicesAssociatedToJobOrder,
            getIncomingCreditNotesAssociatedToJobOrder: window.endpoints.accountancy.getIncomingCreditNotesAssociatedToJobOrder,
            getIncomingInvoicesAssociatedToJobOrder: window.endpoints.accountancy.getIncomingInvoicesAssociatedToJobOrder,
            extendJobOrder: window.endpoints.accountancy.extendJobOrder,
            markJobOrderAsCompleted: window.endpoints.accountancy.markJobOrderAsCompleted
        },
        data() {
            return {
                isLoading: true,
                jobOrder: {},
                customerName: '',
                contactPersonName: '',
                managerName: '',
                showExtendJobOrderModal: false,
                isExtendingJobOrder: false,
                extendJobOrderModel: {},
                extendJobOrderValidationRules: extendJobOrderValidationRules,
                showMarkJobOrderAsCompletedModal: false,
                isMarkingJobOrderAsCompleted: false,
                markJobOrderAsCompletedModel: {
                    dateOfCompletion: new Date()
                },
                markJobOrderAsCompletedValidationRules: markJobOrderAsCompletedValidationRules,
                outgoingCreditNotes: [],
                outgoingInvoices: [],
                incomingCreditNotes: [],
                incomingInvoices: []
            }
        },
        computed: {
            jobOrderId() {
                return this.$route.params.id
            },
            isExtendJobOrderFormValid() {
                return this.errors.items.some((e) => e.scope === 'extendJobOrder')
            },
            isMarkJobOrderAsCompletedFormValid() {
                return this.errors.items.some((e) => e.scope === 'markJobOrderAsCompleted')
            },
            emptyGuid() {
                return '00000000-0000-0000-0000-000000000000'
            }
        },
        methods: {
            transformJsonToViewModel(jobOrderDetailJson) {
                let viewModel = Object.assign({}, jobOrderDetailJson)
                return viewModel
            },
            transformViewModelToExtend(extendJobOrderViewModel) {
                return {
                    newDueDate: extendJobOrderViewModel.newDueDate,
                    price: extendJobOrderViewModel.price
                }
            },
            transformViewModelToMarkAsCompleted(markJobOrderAsCompletedViewModel) {
                return {
                    dateOfCompletion: markJobOrderAsCompletedViewModel.dateOfCompletion
                }
            },
            loadJobOrderDetail() {
                let self = this
                let url = this.$urls.jobOrderDetail + '/' + this.jobOrderId
                httpClient.get(url)
                    .then((jobOrder) => {
                        self.jobOrder = self.transformJsonToViewModel(jobOrder) || {}

                        if (jobOrder.customerId && jobOrder.customerId !== self.emptyGuid) {
                            self.loadPartyInfo(jobOrder.customerId)
                                .then((response) => {
                                    self.customerName = response.name
                                })
                                .catch((errors) => {
                                    self.customerName = ''
                                })
                        }

                        if (jobOrder.contactPersonId && jobOrder.contactPersonId !== self.emptyGuid) {
                            self.loadPartyInfo(jobOrder.contactPersonId)
                                .then((response) => {
                                    self.contactPersonName = response.name
                                })
                                .catch((errors) => {
                                    self.contactPersonName = ''
                                })
                        }

                        if (jobOrder.managerId && jobOrder.managerId !== self.emptyGuid) {
                            self.loadPartyInfo(jobOrder.managerId)
                                .then((response) => {
                                    self.managerName = response.name
                                })
                                .catch((errors) => {
                                    self.managerName = ''
                                })
                        }

                        self.loadOutgoingInvoices()
                        self.loadIncomingInvoices()
                        self.loadOutgoingCreditNotes()
                        self.loadIncomingCreditNotes()

                        self.isLoading = false
                    })
            },
            loadOutgoingCreditNotes() {
                let self = this
                httpClient.get(this.$urls.getOutgoingCreditNotesAssociatedToJobOrder, {
                    jobOrderId: this.jobOrderId
                }).then((data) => {
                    self.outgoingCreditNotes = data.outgoingCreditNotes || []
                })
            },
            loadOutgoingInvoices() {
                let self = this
                httpClient.get(this.$urls.getOutgoingInvoicesAssociatedToJobOrder, {
                    jobOrderId: this.jobOrderId
                }).then((data) => {
                    self.outgoingInvoices = data.outgoingInvoices || []
                })
            },
            loadIncomingCreditNotes() {
                let self = this
                httpClient.get(this.$urls.getIncomingCreditNotesAssociatedToJobOrder, {
                    jobOrderId: this.jobOrderId
                }).then((data) => {
                    self.incomingCreditNotes = data.incomingCreditNotes || []
                })
            },
            loadIncomingInvoices() {
                let self = this
                httpClient.get(this.$urls.getIncomingInvoicesAssociatedToJobOrder, {
                    jobOrderId: this.jobOrderId
                }).then((data) => {
                    self.incomingInvoices = data.incomingInvoices || []
                })
            },
            loadPartyInfo(partyId) {
                let url = this.$urls.getPartyInfoById + '/' + partyId
                return httpClient.get(url)
            },
            openMarkJobOrderAsCompletedModal() {
                this.showMarkJobOrderAsCompletedModal = true
            },
            markJobOrderAsCompleted(evt) {
                evt.preventDefault()

                this.isMarkingJobOrderAsCompleted = true
                let self = this

                let url = this.$urls.markJobOrderAsCompleted + '/' + this.jobOrderId
                let markJobOrderAsCompletedJson = this.transformViewModelToMarkAsCompleted(this.markJobOrderAsCompletedModel)
                httpClient.put(url, markJobOrderAsCompletedJson)
                    .then((response) => {
                        self.showMarkJobOrderAsCompletedModal = false
                        self.isMarkingJobOrderAsCompleted = false
                    })
            },
            openExtendJobOrderModal() {
                this.showExtendJobOrderModal = true
            },
            extendJobOrder(evt) {
                evt.preventDefault()

                this.isExtendingJobOrder = true
                let self = this

                let url = this.$urls.extendJobOrder + '/' + this.jobOrderId
                let extendJobOrderJson = this.transformViewModelToExtend(this.extendJobOrderModel)
                httpClient.put(url, extendJobOrderJson)
                    .then((response) => {
                        this.jobOrder.dueDate = this.extendJobOrderModel.newDueDate
                        this.jobOrder.price = this.extendJobOrderModel.price

                        self.showExtendJobOrderModal = false
                        self.isExtendingJobOrder = false
                    })
            },
            loadResources() {
                return httpClient.get(this.$urls.jobOrderDetailLocalization)
            },
            onEndResourcesLoading() {
                this.loadJobOrderDetail()
            }
        },
        watch: {
            jobOrder() {
                this.extendJobOrderModel = {
                    newDueDate: this.jobOrder.dueDate,
                    price: this.jobOrder.price
                }
            }
        }
    }
</script>