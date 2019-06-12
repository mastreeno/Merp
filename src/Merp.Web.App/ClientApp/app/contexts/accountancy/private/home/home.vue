<template>
    <div>
        <vue-element-loading v-bind:active="isLoading"></vue-element-loading>

        <h2>{{ uiTexts.homePageTitle }}</h2>
        <hr />

        <div class="row">
            <div class="col-md-4">
                <section>
                    <h3>{{ uiTexts.jobOrders }}</h3>
                    <ul class="list-unstyled">
                        <li><router-link to="/accountancy/joborder/search">{{ uiTexts.viewJobOrders }}</router-link></li>
                        <li><router-link to="/accountancy/joborder/create">{{ uiTexts.createJobOrder }}</router-link></li>
                    </ul>
                </section>
                <section>
                    <h3>{{ uiTexts.invoices }}</h3>
                    <ul class="list-unstyled">
                        <li><router-link to="/accountancy/invoice/search">{{ uiTexts.searchInvoices }}</router-link></li>
                        <li><router-link to="/accountancy/invoice/issue/outgoingInvoice">{{ uiTexts.issueInvoice }}</router-link></li>
                        <li><router-link to="/accountancy/invoice/issue/outgoingCreditNote">{{ uiTexts.issueCreditNote }}</router-link></li>
                        <li><router-link to="/accountancy/invoice/register/incomingInvoice">{{ uiTexts.registerIncomingInvoice }}</router-link></li>
                        <li><router-link to="/accountancy/invoice/register/incomingCreditNote">{{ uiTexts.registerIncomingCreditNote }}</router-link></li>
                        <li><router-link to="/accountancy/invoice/registerOutgoing/outgoingInvoice">{{ uiTexts.registerOutgoingInvoice }}</router-link></li>
                        <li><router-link to="/accountancy/invoice/registerOutgoing/outgoingCreditNote">{{ uiTexts.registerOutgoingCreditNote }}</router-link></li>
                        <li><router-link to="/accountancy/invoice/outgoingInvoicesNotLinkedToAJobOrder">{{ uiTexts.linkOutgoingInvoiceToJobOrder }}</router-link></li>
                        <li><router-link to="/accountancy/invoice/incomingInvoicesNotLinkedToAJobOrder">{{ uiTexts.linkIncomingInvoiceToJobOrder }}</router-link></li>
                    </ul>
                </section>
            </div>
            <div class="col-md-8">
                <vue-element-loading v-bind:active="isLoadingStats"></vue-element-loading>
                <ul class="list-unstyled list-inline">
                    <li class="col-md-6">
                        <h3>{{ uiTexts.outgoingInvoices }}</h3>
                        <p>
                            {{ uiTexts.outstanding }}: {{ invoiceStats.outstandingOutgoingInvoicesCount }} ({{ uiTexts.with }} {{ invoiceStats.overdueOutgoingInvoicesCount }} {{ uiTexts.ofThem }} {{ uiTexts.beingOverdue }})
                        </p>
                        <p>
                            {{ uiTexts.owed }}: {{ invoiceStats.outstandingOutgoingInvoicesTotalPrice }} ({{ uiTexts.with }} {{ invoiceStats.overdueOutgoingInvoicesTotalPrice }} {{ uiTexts.beingOverdue }})
                        </p>
                    </li>
                    <li class="col-md-6">
                        <h3>{{ uiTexts.incomingInvoices }}</h3>
                        <p>
                            {{ uiTexts.outstanding }}: {{ invoiceStats.outstandingIncomingInvoicesCount }} ({{ uiTexts.with }} {{ invoiceStats.overdueIncomingInvoicesCount }} {{ uiTexts.ofThem }} {{ uiTexts.beingOverdue }})
                        </p>
                        <p>
                            {{ uiTexts.owed }}: {{ invoiceStats.outstandingIncomingInvoicesTotalPrice }} ({{ uiTexts.with }} {{ invoiceStats.overdueIncomingInvoicesTotalPrice }} {{ uiTexts.beingOverdue }})
                        </p>
                    </li>
                </ul>
            </div>
        </div>
    </div>
</template>
<script>
    import { httpClient } from '@/app/shared/services/httpClient'
    import EndpointConfigurationMixin from '@/app/shared/mixins/endpointConfigurationMixin'

    export default {
        name: 'accountancyHome',
        mixins: [EndpointConfigurationMixin],
        urls: {
            homePageLocalization: window.endpoints.accountancy.homePageLocalization,
            getInvoicesStats: window.endpoints.accountancy.getInvoicesStats
        },
        data() {
            return {
                isLoading: true,
                isLoadingStats: false,
                invoiceStats: {
                    outstandingOutgoingInvoicesCount: 0,
                    overdueOutgoingInvoicesCount: 0,
                    outstandingOutgoingInvoicesTotalPrice: 0,
                    overdueOutgoingInvoicesTotalPrice: 0,
                    outstandingIncomingInvoicesCount: 0,
                    overdueIncomingInvoicesCount: 0,
                    outstandingIncomingInvoicesTotalPrice: 0,
                    overdueIncomingInvoicesTotalPrice: 0
                }
            }
        },
        methods: {
            transformJsonToViewModel(invoiceStatsJson) {
                let viewModel = Object.assign({}, invoiceStatsJson)
                return viewModel
            },
            loadInvoicesStats() {
                this.isLoadingStats = true
                let self = this
                httpClient.get(this.$urls.getInvoicesStats)
                    .then((response) => {
                        let viewModel = self.transformJsonToViewModel(response)
                        self.invoiceStats = viewModel || {
                            outstandingOutgoingInvoicesCount: 0,
                            overdueOutgoingInvoicesCount: 0,
                            outstandingOutgoingInvoicesTotalPrice: 0,
                            overdueOutgoingInvoicesTotalPrice: 0,
                            outstandingIncomingInvoicesCount: 0,
                            overdueIncomingInvoicesCount: 0,
                            outstandingIncomingInvoicesTotalPrice: 0,
                            overdueIncomingInvoicesTotalPrice: 0
                        }
                        
                        self.isLoadingStats = false
                    })
            },
            loadResources() {
                return httpClient.get(this.$urls.homePageLocalization)
            },
            onEndResourcesLoading() {
                this.loadInvoicesStats()
                this.isLoading = false
            }
        }
    }
</script>