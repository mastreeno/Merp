<template>
    <form id="drafts-search-form" v-on:submit.prevent="filterDrafts">
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
            <div class="col-md-3">
                <div class="form-group row">
                    <label class="col-md-3 col-form-label">{{ labels.dateFrom }}</label>
                    <div class="col-md-9">
                        <datepicker v-bind:show-clear-button="true"
                                    name="dateFrom"
                                    v-model="dateFrom">
                        </datepicker>
                    </div>
                </div>
            </div>
            <div class="col-md-3">
                <div class="form-group row">
                    <label class="col-md-3 col-form-label">{{ labels.dateTo }}</label>
                    <div class="col-md-9">
                        <datepicker v-bind:show-clear-button="true"
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
        </div>
    </form>
</template>
<script>
    import EndpointConfigurationMixin from '@/app/shared/mixins/endpointConfigurationMixin'
    import Datepicker from '@/app/shared/components/datepicker.vue'
    import { httpClient } from '@/app/shared/services/httpClient'

    export default {
        name: 'draftSearchFilters',
        components: {
            'datepicker': Datepicker
        },
        mixins: [EndpointConfigurationMixin],
        props: ['filters', 'labels'],
        urls: {
            getDraftCustomers: window.endpoints.accountancy.getDraftCustomers
        },
        data() {
            return {
                customer: this.filters.customer,
                supplier: this.filters.supplier,
                dateFrom: this.filters.dateFrom,
                dateTo: this.filters.dateTo,
                customers: []
            }
        },
        methods: {
            transformCustomersJsonToViewModel(customersJson) {
                let viewModel = customersJson.slice()
                return viewModel
            },
            filterDrafts() {
                let _filters = {
                    customer: this.customer,
                    dateFrom: this.dateFrom,
                    dateTo: this.dateTo
                }

                this.$emit('filterdrafts', _filters)
            },
            loadDraftCustomers() {
                let self = this
                httpClient.get(this.$urls.getDraftCustomers)
                    .then((data) => {
                        self.customers = self.transformCustomersJsonToViewModel(data) || []
                    })
            }
        },
        mounted() {
            this.loadDraftCustomers()
        }
    }
</script>