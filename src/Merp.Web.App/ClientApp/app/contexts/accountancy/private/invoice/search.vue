<template>
    <div>
        <vue-element-loading v-bind:active="isLoading"></vue-element-loading>
        <h2>{{ uiTexts.searchInvoicesPageTitle }}</h2>
        <hr />
        <invoice-search-filters v-bind:filters="filters" v-bind:labels="uiTexts" v-on:filterinvoices="filterInvoices"></invoice-search-filters>
        <hr />
        <div class="row">
            <div class="col-md-12">
                <vue-element-loading v-bind:active="isSearching"></vue-element-loading>
                <p v-if="invoices.length">{{ invoices.length }} {{ uiTexts.itemsOf }} <span class="badge">{{ totalNumberOfInvoices }}</span></p>
                <invoice-list v-bind:items="invoices" v-bind:labels="uiTexts"></invoice-list>
                <b-pagination align="right" v-on:input="changePage" v-bind:total-rows="totalNumberOfInvoices" v-model="page" v-bind:per-page="size"></b-pagination>
            </div>
        </div>
    </div>
</template>
<script>
    import InvoiceSearchFilters from './searchFilters.vue'
    import InvoiceList from './list.vue'
    import EndpointConfigurationMixin from '@/app/shared/mixins/endpointConfigurationMixin'

    import { httpClient } from '@/app/shared/services/httpClient'

    export default {
        name: 'searchInvoices',
        mixins: [EndpointConfigurationMixin],
        components: {
            'invoice-search-filters': InvoiceSearchFilters,
            'invoice-list': InvoiceList
        },
        urls: {
            invoiceSearchLocalization: window.endpoints.accountancy.invoiceSearchLocalization,
            searchInvoices: window.endpoints.accountancy.searchInvoices
        },
        data() {
            return {
                isLoading: true,
                isSearching: false,
                invoices: [],
                filters: {
                    kind: null,
                    status: null,
                    customer: null,
                    supplier: null,
                    dateFrom: null,
                    dateTo: null
                },
                totalNumberOfInvoices: 0,
                page: 1,
                size: 20
            }
        },
        methods: {
            transformJsonToViewModel(searchInvoicesJson) {
                let viewModel = Object.assign({}, searchInvoicesJson)
                return viewModel
            },
            loadInvoices() {
                this.isSearching = true
                let self = this

                httpClient.get(this.$urls.searchInvoices, {
                    kind: this.filters.kind,
                    status: this.filters.status,
                    customerId: this.filters.customer,
                    supplierId: this.filters.supplier,
                    dateFrom: this.filters.dateFrom,
                    dateTo: this.filters.dateTo,
                    page: this.page,
                    size: this.size
                }).then((data) => {
                    let viewModel = self.transformJsonToViewModel(data)
                    self.invoices = viewModel.invoices
                    self.totalNumberOfInvoices = viewModel.totalNumberOfInvoices

                    self.isSearching = false
                })
            },
            changePage(page) {
                this.page = page || 1
                this.loadInvoices()
            },
            filterInvoices(filters) {
                this.filters = filters || {
                    kind: null,
                    status: null,
                    customer: null,
                    supplier: null,
                    dateFrom: null,
                    dateTo: null
                }
                this.loadInvoices()
            },
            loadResources() {
                return httpClient.get(this.$urls.invoiceSearchLocalization)
            },
            onEndResourcesLoading() {
                this.isLoading = false
            }
        }
    }
</script>