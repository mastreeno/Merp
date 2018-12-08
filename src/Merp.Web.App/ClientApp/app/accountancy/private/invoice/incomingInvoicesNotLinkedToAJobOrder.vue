<template>
    <div>
        <vue-element-loading v-bind:active="isLoading"></vue-element-loading>
        <div class="row align-items-center">
            <div class="col-md-8">
                <h3>{{ uiTexts.incomingInvoicesNotLinkedToAJobOrderPageTitle }}</h3>
            </div>
            <div class="col">
                <div class="btn-group float-right" role="group">
                    <button type="button" class="btn btn-outline-primary" v-on:click="reloadInvoicesNotLinkedToAJobOrder">
                        <i class="fa fa-refresh"></i> {{ uiTexts.reload }}
                    </button>
                    <router-link to="/accountancy" class="btn btn-outline-secondary">
                        <i class="fa fa-angle-left"></i> {{ uiTexts.back }}
                    </router-link>
                </div>
            </div>
        </div>
        <hr />
        <div class="row">
            <div class="col-md-4">
                <div class="form-group form-row">
                    <label class="col-form-label col-md-5">{{ uiTexts.documentType }}</label>
                    <div class="col-md-7">
                        <select class="form-control" v-model="filters.type">
                            <option></option>
                            <option v-for="type in documentTypes" v-bind:key="type.value" v-bind:value="type.value">{{ type.displayText }}</option>
                        </select>
                    </div>
                </div>
            </div>
            <div class="col-md-6">
                <div class="form-group form-row">
                    <label class="col-form-label col-md-2">{{ uiTexts.supplier }}</label>
                    <div class="col-md-10">
                        <input type="text" class="form-control" v-model="filters.supplier" />
                    </div>
                </div>
            </div>
            <div class="form-group col-md-2">
                <button type="button" class="btn btn-primary" v-on:click="searchInvoicesNotLinkedToAJobOrder">
                    <i class="fa fa-search"></i> {{ uiTexts.search }}
                </button>
            </div>
        </div>
        <div class="row">
            <div class="col-md-12">
                <vue-element-loading v-bind:active="isSearching"></vue-element-loading>
                <table class="table table-striped">
                    <thead>
                        <tr>
                            <th>{{ uiTexts.documentType }}</th>
                            <th>{{ uiTexts.supplier }}</th>
                            <th>{{ uiTexts.invoiceNumber }}</th>
                            <th>&nbsp;</th>
                        </tr>
                    </thead>
                    <tbody>
                        <tr v-for="invoice in invoices" v-bind:key="invoice.originalId">
                            <td>{{ getInvoiceType(invoice) }}</td>
                            <td>{{ invoice.supplierName }}</td>
                            <td>{{ invoice.number }}</td>
                            <td>
                                <button type="button" class="btn btn-outline-secondary" v-b-tooltip.hover v-bind:title="uiTexts.linkInvoiceToJobOrder" v-on:click="openLinkToJobOrderModal(invoice)">
                                    <i class="fa fa-link fa-fw"></i>
                                </button>
                            </td>
                        </tr>
                    </tbody>
                </table>
                <b-pagination align="right" v-on:input="changePage" v-bind:total-rows="totalNumberOfInvoices" v-model="page" v-bind:per-page="size"></b-pagination>
            </div>
        </div>

        <b-modal v-model="showLinkToJobOrderModal" v-bind:title="uiTexts.linkIncomingInvoiceToJobOrder" hide-footer>
            <link-invoice-to-job-order-form v-bind:labels="uiTexts" v-bind:invoice="invoiceToLink" v-bind:saving="isSaving" v-bind:server-errors="serverErrors" v-on:close-linktojoborder-modal="closeModal" v-on:link-invoice-to-joborder="linkIncomingInvoiceToJobOrder"></link-invoice-to-job-order-form>
        </b-modal>
    </div>
</template>
<script>
    import _ from 'lodash'
    import LinkInvoiceToJobOrderForm from './linkInvoiceToJobOrderForm.vue'
    import EndpointConfigurationMixin from '@/app/mixins/endpointConfigurationMixin'

    import { httpClient } from '@/app/services/httpClient'
    import errorHelper from '@/app/services/errorHelper'
    
    export default {
        name: 'incomingInvoicesNotLinkedToAJobOrder',
        mixins: [EndpointConfigurationMixin],
        components: {
            'link-invoice-to-job-order-form': LinkInvoiceToJobOrderForm
        },
        urls: {
            incomingInvoicesNotLinkedToAJobOrderLocalization: window.endpoints.accountancy.incomingInvoicesNotLinkedToAJobOrderLocalization,
            getIncomingInvoicesNotLinkedToAJobOrder: window.endpoints.accountancy.getIncomingInvoicesNotLinkedToAJobOrder,
            linkIncomingInvoiceToJobOrder: window.endpoints.accountancy.linkIncomingInvoiceToJobOrder,
            getIncomingDocumentTypes: window.endpoints.accountancy.getIncomingDocumentTypes
        },
        data() {
            return {
                isLoading: true,
                isSearching: false,
                invoices: [],
                showLinkToJobOrderModal: false,
                invoiceToLink: {},
                isSaving: false,
                serverErrors: [],
                totalNumberOfInvoices: 0,
                page: 1,
                size: 20,
                documentTypesList: [],
                filters: {
                    type: null,
                    supplier: null
                }
            }
        },
        computed: {
            documentTypes() {
                return this.documentTypesList.map(d => {
                    let documentTypeKey = _.camelCase(d)
                    return {
                        value: d,
                        displayText: this.uiTexts[documentTypeKey]
                    }
                })
            }
        },
        methods: {
            getInvoiceType(invoice) {
                let invoiceType = _.camelCase(invoice.type)
                return this.uiTexts[invoiceType]
            },
            transformJsonToViewModel(incomingInvoicesNotLinkedToAJobOrderJson) {
                let viewModel = Object.assign({}, incomingInvoicesNotLinkedToAJobOrderJson)
                return viewModel
            },
            transformViewModelToLinkIncomingInvoiceToJobOrder(viewModel) {
                return {
                    type: viewModel.type,
                    dateOfLink: viewModel.dateOfLink,
                    jobOrderNumber: viewModel.jobOrderNumber,
                    amount: viewModel.amount
                }
            },
            loadDocumentTypes() {
                let self = this
                httpClient.get(this.$urls.getIncomingDocumentTypes)
                    .then((data) => {
                        self.documentTypesList = data || []
                    })
            },
            changePage(page) {
                this.page = page || 1
                this.loadInvoicesNotLinkedToAJobOrder()
            },
            reloadInvoicesNotLinkedToAJobOrder() {
                this.page = 1
                this.filters = {
                    type: null,
                    supplier: null
                }
                this.loadInvoicesNotLinkedToAJobOrder()
            },
            searchInvoicesNotLinkedToAJobOrder() {
                this.page = 1
                this.loadInvoicesNotLinkedToAJobOrder()
            },
            loadInvoicesNotLinkedToAJobOrder() {
                this.isSearching = true
                let self = this

                httpClient.get(this.$urls.getIncomingInvoicesNotLinkedToAJobOrder, {
                    type: this.filters.type,
                    supplier: this.filters.supplier,
                    page: this.page,
                    size: this.size
                }).then((data) => {
                    let viewModel = self.transformJsonToViewModel(data)
                    self.invoices = viewModel.invoices || []
                    self.totalNumberOfInvoices = viewModel.totalNumberOfInvoices
                    self.isSearching = false
                })
            },
            openLinkToJobOrderModal(invoice) {
                this.invoiceToLink = invoice
                this.showLinkToJobOrderModal = true
            },
            closeModal() {
                this.showLinkToJobOrderModal = false
            },
            linkIncomingInvoiceToJobOrder(link) {
                this.isSaving = true
                let self = this

                let url = this.$urls.linkIncomingInvoiceToJobOrder + '/' + this.invoiceToLink.originalId
                let linkIncomingInvoiceToJobOrderJson = this.transformViewModelToLinkIncomingInvoiceToJobOrder(link)
                httpClient.put(url, linkIncomingInvoiceToJobOrderJson)
                    .then((response) => {
                        self.isSaving = false
                        self.closeModal()
                        self.loadInvoicesNotLinkedToAJobOrder()
                    })
                    .catch((errors) => {
                        self.isSaving = false
                        self.serverErrors = errorHelper.buildErrorListFromModelState(errors) || []
                    })
            },
            loadResources() {
                return httpClient.get(this.$urls.incomingInvoicesNotLinkedToAJobOrderLocalization)
            },
            onEndResourcesLoading() {
                this.isLoading = false
                this.loadDocumentTypes()
            }
        }
    }
</script>