<template>
    <form id="invoices-search-form" v-on:submit.prevent="filterInvoices">
        <div class="form-row">
            <div class="col-md-6">
                <div class="form-group row">
                    <label class="col-md-2 col-form-label">{{ labels.kind }}</label>
                    <div class="col-md-10">
                        <select v-model="kind" class="form-control">
                            <option v-for="k in invoiceKinds" v-bind:key="k.value" v-bind:value="k.value">{{ k.displayName }}</option>
                        </select>
                    </div>
                </div>
            </div>
            <div class="col-md-6">
                <div class="form-group row">
                    <label class="col-md-2 col-form-label">{{ labels.status }}</label>
                    <div class="col-md-10">
                        <select v-model="status" class="form-control">
                            <option></option>
                            <option v-for="s in invoiceStatusList" v-bind:key="s.value" v-bind:value="s.value">{{ s.displayName }}</option>
                        </select>
                    </div>
                </div>
            </div>
        </div>
        <div class="form-row">
            <div class="col-md-6">
                <div class="form-group row">
                    <label class="col-md-2 col-form-label">{{ labels.customer }}</label>
                    <div class="col-md-10">
                        <select v-model="customer" class="form-control">
                            <option></option>
                            <option v-for="c in customers" v-bind:key="c.id" v-bind:value="c.id">{{ c.name }}</option>
                        </select>
                    </div>
                </div>
            </div>
            <div class="col-md-6">
                <div class="form-group row">
                    <label class="col-md-2 col-form-label">{{ labels.supplier }}</label>
                    <div class="col-md-10">
                        <select v-model="supplier" class="form-control">
                            <option></option>
                            <option v-for="s in suppliers" v-bind:key="s.id" v-bind:value="s.id">{{ s.name }}</option>
                        </select>
                    </div>
                </div>
            </div>
        </div>
        <div class="form-row">
            <div class="col-md-6">
                <div class="form-group row">
                    <label class="col-md-3 col-form-label">{{ labels.dateFrom }}</label>
                    <div class="col-md-9">
                        <datepicker v-bind:show-calendar-button="true"
                                    v-bind:show-clear-button="true"
                                    name="dateFrom"
                                    v-model="dateFrom">
                        </datepicker>
                    </div>
                </div>
            </div>
            <div class="col-md-6">
                <div class="form-group row">
                    <label class="col-md-3 col-form-label">{{ labels.dateTo }}</label>
                    <div class="col-md-9">
                        <datepicker v-bind:show-calendar-button="true"
                                    v-bind:show-clear-button="true"
                                    name="dateTo"
                                    v-model="dateTo">
                        </datepicker>
                    </div>
                </div>
            </div>
        </div>
        <div class="form-row">
            <div class="col-md-6">
                <button type="submit" class="btn btn-primary">
                    <i class="fa fa-search"></i>
                    {{ labels.search }}
                </button>
            </div>
            <div class="col-md-6">
                <div class="btn-toolbar float-right" role="toolbar">
                    <div class="btn-group mr-2">
                        <router-link class="btn btn-outline-primary" to="/accountancy/draft/search">{{ labels.viewDrafts }}</router-link>
                    </div>
                    
                    <div class="btn-group" role="group">
                        <div class="btn-group" role="group">
                            <button type="button" class="btn btn-outline-secondary dropdown-toggle" id="salesDropdownButton" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
                                {{ labels.sales }}
                            </button>
                            <div class="dropdown-menu" aria-labelledby="salesDropdownButton">
                                <router-link class="dropdown-item" to="/accountancy/invoice/issue/outgoingInvoice">{{ labels.issueInvoice }}</router-link>
                                <router-link class="dropdown-item" to="/accountancy/invoice/issue/outgoingCreditNote">{{ labels.issueCreditNote }}</router-link>
                                <router-link class="dropdown-item" to="/accountancy/invoice/registerOutgoing/outgoingInvoice">{{ labels.registerOutgoingInvoice }}</router-link>
                                <router-link class="dropdown-item" to="/accountancy/invoice/registerOutgoing/outgoingCreditNote">{{ labels.registerOutgoingCreditNote }}</router-link>
                            </div>
                        </div>
                        <div class="btn-group" role="group">
                            <button type="button" class="btn btn-outline-secondary dropdown-toggle" id="purchaseDropdownButton" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
                                {{ labels.purchases }}
                            </button>
                            <div class="dropdown-menu dropdown-menu-right" aria-labelledby="purchaseDropdownButton">
                                <router-link class="dropdown-item" to="/accountancy/invoice/register/incomingInvoice">{{ labels.registerIncomingInvoice }}</router-link>
                                <router-link class="dropdown-item" to="/accountancy/invoice/register/incomingCreditNote">{{ labels.registerIncomingCreditNote }}</router-link>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </form>
</template>
<script>
    import _ from 'lodash'
    import EndpointConfigurationMixin from '@/app/mixins/endpointConfigurationMixin'
    import Datepicker from '@/app/shared/widgets/datepicker.vue'
    import { httpClient } from '@/app/services/httpClient'

    export default {
        name: 'invoiceSearchFilters',
        components: {
            'datepicker': Datepicker
        },
        mixins: [EndpointConfigurationMixin],
        props: ['filters', 'labels'],
        urls: {
            getInvoiceCustomers: window.endpoints.accountancy.getInvoiceCustomers,
            getInvoiceSuppliers: window.endpoints.accountancy.getInvoiceSuppliers,
            getSearchInvoiceKinds: window.endpoints.accountancy.getSearchInvoiceKinds,
            getSearchInvoiceStates: window.endpoints.accountancy.getSearchInvoiceStates
        },
        data() {
            return {
                kind: this.filters.kind,
                status: this.filters.status,
                customer: this.filters.customer,
                supplier: this.filters.supplier,
                dateFrom: this.filters.dateFrom,
                dateTo: this.filters.dateTo,
                customers: [],
                suppliers: [],
                invoiceKindList: [],
                invoiceStates: []
            }
        },
        computed: {
            invoiceKinds() {
                return this.invoiceKindList.map(k => {
                    let labelKey = _.camelCase(k)
                    return {
                        value: k,
                        displayName: this.labels[labelKey]
                    }
                })
            },
            invoiceStatusList() {
                return this.invoiceStates.map(k => {
                    let labelKey = _.camelCase(k)
                    return {
                        value: k,
                        displayName: this.labels[labelKey]
                    }
                })
            }
        },
        methods: {
            transformCustomersJsonToViewModel(customersJson) {
                let viewModel = customersJson.slice()
                return viewModel
            },
            transformSuppliersJsonToViewModel(suppliersJson) {
                let viewModel = suppliersJson.slice()
                return viewModel
            },
            filterInvoices() {
                let _filters = {
                    kind: this.kind,
                    status: this.status,
                    customer: this.customer,
                    supplier: this.supplier,
                    dateFrom: this.dateFrom,
                    dateTo: this.dateTo
                }

                this.$emit('filterinvoices', _filters)
            },
            loadInvoiceKinds() {
                let self = this
                httpClient.get(this.$urls.getSearchInvoiceKinds)
                    .then((response) => {
                        self.invoiceKindList = response || []
                        self.kind = self.invoiceKindList[0]
                    })
            },
            loadInvoiceStates() {
                let self = this
                httpClient.get(this.$urls.getSearchInvoiceStates)
                    .then((response) => {
                        self.invoiceStates = response || []
                    })
            },
            loadInvoiceCustomers() {
                let self = this
                httpClient.get(this.$urls.getInvoiceCustomers)
                    .then((data) => {
                        self.customers = self.transformCustomersJsonToViewModel(data) || []
                    })
            },
            loadInvoiceSuppliers() {
                let self = this
                httpClient.get(this.$urls.getInvoiceSuppliers)
                    .then((data) => {
                        self.suppliers = self.transformSuppliersJsonToViewModel(data) || []
                    })
            }
        },
        mounted() {
            this.loadInvoiceKinds()
            this.loadInvoiceStates()
            this.loadInvoiceCustomers()
            this.loadInvoiceSuppliers()
        }
    }
</script>