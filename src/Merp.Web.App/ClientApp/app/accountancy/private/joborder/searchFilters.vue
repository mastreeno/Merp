<template>
    <form id="joborders-search-form" role="form" v-on:submit.prevent="filterJobOrders">
        <div class="form-row">
            <div class="col-md-5">
                <div class="form-group row">
                    <label class="col-md-2 col-form-label" for="customerFilter">{{ labels.customer }}</label>
                    <div class="col-md-10">
                        <select id="customerFilter" name="customerFilter" class="form-control" v-model="customer">
                            <option></option>
                            <option v-for="customer in customers" v-bind:key="customer.id" v-bind:value="customer.id">{{ customer.name }}</option>
                        </select>
                    </div>
                </div>
            </div>
            <div class="col-md-5">
                <div class="form-group row">
                    <label class="col-md-2 col-form-label" for="nameFilter">{{ labels.name }}</label>
                    <div class="col-md-10">
                        <input type="text" name="nameFilter" class="form-control" id="nameFilter" v-model="name" />
                    </div>
                </div>
            </div>
            <div class="col-md-2">
                <div class="form-group row ml-3">
                    <b-form-checkbox v-model="currentOnly">
                        {{ labels.currentOnly }}
                    </b-form-checkbox>
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
                <div class="btn-group float-right" role="group">
                    <router-link to="/accountancy/joborder/create" class="btn btn-outline-secondary"><i class="fa fa-plus"></i> {{ labels.newJobOrder }}</router-link>
                </div>
            </div>
        </div>
    </form>
</template>
<script>
    import EndpointConfigurationMixin from '@/app/mixins/endpointConfigurationMixin'
    import { httpClient } from '@/app/services/httpClient'

    export default {
        name: 'jobOrderSearchFilters',
        mixins: [EndpointConfigurationMixin],
        props: ['filters', 'labels'],
        urls: {
            getJobOrderCustomers: window.endpoints.accountancy.getJobOrderCustomers
        },
        data() {
            return {
                customer: this.filters.customer,
                name: this.filters.name,
                currentOnly: this.filters.currentOnly,
                customers: []
            }
        },
        methods: {
            transformCustomersJsonToViewModel(jobOrderCustomersJson) {
                let viewModel = jobOrderCustomersJson.slice()
                return viewModel
            },
            filterJobOrders() {
                let _filters = {
                    customer: this.customer,
                    name: this.name,
                    currentOnly: this.currentOnly
                }

                this.$emit('filterjoborders', _filters)
            }
        },
        mounted() {
            let self = this
            httpClient.get(this.$urls.getJobOrderCustomers)
                .then((data) => {
                    self.customers = self.transformCustomersJsonToViewModel(data) || []
                })
        }
    }
</script>