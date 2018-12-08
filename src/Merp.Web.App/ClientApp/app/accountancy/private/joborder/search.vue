<template>
    <div>
        <vue-element-loading v-bind:active="isLoading"></vue-element-loading>
        <h2>{{ uiTexts.searchJobOrdersTitle }}</h2>
        <hr />
        <job-order-search-filters v-bind:filters="filters" v-bind:labels="uiTexts" v-on:filterjoborders="filterJobOrders"></job-order-search-filters>
        <hr />
        <div class="row">
            <div class="col-md-12">
                <vue-element-loading v-bind:active="isSearching"></vue-element-loading>
                <p v-if="jobOrders.length">{{ jobOrders.length }} {{ uiTexts.itemsOf }} <span class="badge">{{ totalNumberOfJobOrders }}</span></p>
                <job-order-list v-bind:items="jobOrders" v-bind:headers="uiTexts"></job-order-list>
                <b-pagination align="right" v-on:input="changePage" v-bind:total-rows="totalNumberOfJobOrders" v-model="page" v-bind:per-page="size"></b-pagination>
            </div>
        </div>
    </div>
</template>
<script>
    import JobOrderSearchFilters from './searchFilters.vue'
    import JobOrderList from './list.vue'
    import EndpointConfigurationMixin from '@/app/mixins/endpointConfigurationMixin'

    import { httpClient } from '@/app/services/httpClient'

    export default {
        name: 'searchJobOrders',
        mixins: [EndpointConfigurationMixin],
        components: {
            'job-order-search-filters': JobOrderSearchFilters,
            'job-order-list': JobOrderList
        },
        urls: {
            jobOrderSearchLocalization: window.endpoints.accountancy.jobOrderSearchLocalization,
            searchJobOrders: window.endpoints.accountancy.searchJobOrders
        },
        data() {
            return {
                isLoading: true,
                isSearching: false,
                jobOrders: [],
                totalNumberOfJobOrders: 0,
                filters: {
                    customer: null,
                    name: null,
                    currentOnly: false
                },
                page: 1,
                size: 20
            }
        },
        methods: {
            transformJsonToViewModel(searchJobOrdersJson) {
                let viewModel = Object.assign({}, searchJobOrdersJson)
                return viewModel
            },
            loadJobOrders() {
                this.isSearching = true
                let self = this

                httpClient.get(this.$urls.searchJobOrders, {
                    currentOnly: this.filters.currentOnly,
                    customerId: this.filters.customer,
                    jobOrderName: this.filters.name,
                    page: this.page,
                    size: this.size
                }).then((data) => {
                    let viewModel = self.transformJsonToViewModel(data)
                    self.totalNumberOfJobOrders = viewModel.totalNumberOfJobOrders || 0
                    self.jobOrders = viewModel.jobOrders || []

                    self.isSearching = false
                })
            },
            changePage(page) {
                this.page = page || 1
                this.loadJobOrders()
            },
            filterJobOrders(filters) {
                this.filters = filters || {
                    customer: null,
                    name: null,
                    currentOnly: false
                }
                this.loadJobOrders()
            },
            loadResources() {
                return httpClient.get(this.$urls.jobOrderSearchLocalization)
            },
            onEndResourcesLoading() {
                this.isLoading = false
            }
        }
    }
</script>