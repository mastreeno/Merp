<template>
    <div>
        <vue-element-loading v-bind:active="isLoading"></vue-element-loading>
        <div class="row align-items-center">
            <div class="col">
                <h2>
                    {{ uiTexts.editDraftPageTitle }}
                </h2>
            </div>
            <div class="col">
                <router-link to="/accountancy/draft/search" class="btn btn-outline-secondary float-right">
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
            <outgoing-invoice-form v-bind:invoice="draft"
                                   v-bind:labels="uiTexts"
                                   v-bind:validation="validationRules"
                                   v-bind:vat-list="vatList"
                                   v-bind:disable-document-types="true"
                                   v-on:save="issueInvoiceFromDraft"
                                   v-on:save-draft="editDraft">
            </outgoing-invoice-form>
        </div>
    </div>
</template>
<script>
    import OutgoingInvoiceForm from '@/app/contexts/accountancy/private/outgoingInvoiceForm.vue'
    import EndpointConfigurationMixin from '@/app/shared/mixins/endpointConfigurationMixin'

    import { httpClient } from '@/app/shared/services/httpClient'
    import errorHelper from '@/app/shared/services/errorHelper'

    export default {
        name: 'editOutgoingInvoiceDraft',
        mixins: [EndpointConfigurationMixin],
        components: {
            'outgoing-invoice-form': OutgoingInvoiceForm
        },
        urls: {
            editOutgoingInvoiceDraft: window.endpoints.accountancy.editOutgoingInvoiceDraft,
            editOutgoingInvoiceDraftLocalization: window.endpoints.accountancy.editOutgoingInvoiceDraftLocalization,
            issueOutgoingInvoiceFromDraft: window.endpoints.accountancy.issueOutgoingInvoiceFromDraft,
            getAvailableVats: window.endpoints.accountancyInternal.getAvailableVats
        },
        data() {
            return {
                isLoading: true,
                isSaving: false,
                draft: {
                    customer: {},
                    date: null,
                    currency: '',
                    amount: 0.00,
                    taxes: 0.00,
                    totalPrice: 0.00,
                    totalToPay: 0.00,
                    totalTaxableAmount: 0.00,
                    purchaseOrderNumber: '',
                    paymentTerms: '',
                    description: '',
                    vatIncluded: false,
                    lineItems: [],
                    pricesByVat: [],
                    nonTaxableItems: [],
                    providenceFund: null,
                    withholdingTax: null
                },
                vatList: [],
                validationRules: {},
                serverErrors: []
            }
        },
        computed: {
            draftId() {
                return this.$route.params.id
            },
            editDraftUrl() {
                return this.$urls.editOutgoingInvoiceDraft + '/' + this.draftId
            }
        },
        methods: {
            transformJsonToViewModel(editOutgoingInvoiceDraftJson) {
                let viewModel = JSON.parse(JSON.stringify(editOutgoingInvoiceDraftJson))
                viewModel.lineItems = editOutgoingInvoiceDraftJson.lineItems.map((i) => {
                    return {
                        id: i.id,
                        code: i.code,
                        description: i.description,
                        quantity: i.quantity,
                        unitPrice: i.unitPrice,
                        totalPrice: i.totalPrice,
                        vat: this.vatList.filter(v => v.rate === i.vat && v.description === i.vatDescription)[0] || {},
                        vatIncluded: false
                    }
                })

                return viewModel
            },
            transformViewModelToEditOutgoingInvoiceDraft(viewModel) {
                return {
                    customer: viewModel.customer,
                    date: viewModel.date,
                    currency: viewModel.currency,
                    amount: viewModel.amount,
                    taxes: viewModel.taxes,
                    totalPrice: viewModel.totalPrice,
                    purchaseOrderNumber: viewModel.purchaseOrderNumber,
                    paymentTerms: viewModel.paymentTerms,
                    description: viewModel.description,
                    vatIncluded: viewModel.vatIncluded,
                    lineItems: viewModel.lineItems.map((i) => {
                        return {
                            id: i.id,
                            code: i.code,
                            description: i.description,
                            quantity: i.quantity,
                            unitPrice: i.unitPrice,
                            totalPrice: i.totalPrice,
                            vat: i.vat.rate,
                            vatDescription: i.vat.description,
                            vatIncluded: false
                        }
                    }),
                    pricesByVat: viewModel.pricesByVat.map((p) => {
                        return {
                            taxableAmount: p.taxableAmount,
                            vatRate: p.vatRate,
                            vatAmount: p.vatAmount,
                            totalPrice: p.totalPrice,
                            providenceFundAmount: p.providenceFundAmount || null
                        }
                    }),
                    nonTaxableItems: viewModel.nonTaxableItems.map((t) => {
                        return {
                            id: t.id,
                            description: t.description,
                            amount: t.amount
                        }
                    }),
                    providenceFund: !viewModel.providenceFund ? null : {
                        description: viewModel.providenceFund.description,
                        rate: viewModel.providenceFund.rate,
                        amount: viewModel.providenceFund.amount
                    },
                    withholdingTax: !viewModel.withholdingTax ? null : {
                        description: viewModel.withholdingTax.description,
                        rate: viewModel.withholdingTax.rate,
                        taxableAmountRate: viewModel.withholdingTax.taxableAmountRate,
                        amount: viewModel.withholdingTax.amount
                    },
                    totalToPay: viewModel.totalToPay
                }
            },
            transformViewModelToIssueOutgoingInvoiceFromDraft(viewModel) {
                return {
                    draftId: this.draftId,
                    customer: viewModel.customer,
                    date: viewModel.date,
                    currency: viewModel.currency,
                    amount: viewModel.amount,
                    taxes: viewModel.taxes,
                    totalPrice: viewModel.totalPrice,
                    purchaseOrderNumber: viewModel.purchaseOrderNumber,
                    paymentTerms: viewModel.paymentTerms,
                    description: viewModel.description,
                    vatIncluded: viewModel.vatIncluded,
                    lineItems: viewModel.lineItems.map((i) => {
                        return {
                            code: i.code,
                            description: i.description,
                            quantity: i.quantity,
                            unitPrice: i.unitPrice,
                            totalPrice: i.totalPrice,
                            vat: i.vat.rate,
                            vatDescription: i.vat.description,
                            vatIncluded: false
                        }
                    }),
                    pricesByVat: viewModel.pricesByVat.map((p) => {
                        return {
                            taxableAmount: p.taxableAmount,
                            vatRate: p.vatRate,
                            vatAmount: p.vatAmount,
                            totalPrice: p.totalPrice,
                            providenceFundAmount: p.providenceFundAmount || null
                        }
                    }),
                    nonTaxableItems: viewModel.nonTaxableItems.map((t) => {
                        return {
                            description: t.description,
                            amount: t.amount
                        }
                    }),
                    providenceFund: !viewModel.providenceFund ? null : {
                        description: viewModel.providenceFund.description,
                        rate: viewModel.providenceFund.rate,
                        amount: viewModel.providenceFund.amount
                    },
                    withholdingTax: !viewModel.withholdingTax ? null : {
                        description: viewModel.withholdingTax.description,
                        rate: viewModel.withholdingTax.rate,
                        taxableAmountRate: viewModel.withholdingTax.taxableAmountRate,
                        amount: viewModel.withholdingTax.amount
                    },
                    totalToPay: viewModel.totalToPay
                }
            },
            loadDraftDetail() {
                let self = this

                httpClient.get(this.editDraftUrl)
                    .then((response) => {
                        self.draft = self.transformJsonToViewModel(response) || {}
                        self.isLoading = false
                    })
            },
            issueInvoiceFromDraft(draft) {
                this.isSaving = true
                let self = this

                let issueOutgoingInvoiceFromDraftJson = this.transformViewModelToIssueOutgoingInvoiceFromDraft(draft)
                httpClient.post(this.$urls.issueOutgoingInvoiceFromDraft, issueOutgoingInvoiceFromDraftJson)
                    .then((response) => {
                        self.isSaving = false
                        self.$router.push('/accountancy/invoice/search')
                    })
                    .catch((errors) => {
                        self.serverErrors = errorHelper.buildErrorListFromModelState(errors) || []
                        self.isSaving = false
                    })
            },
            editDraft(draft) {
                this.isSaving = true
                let self = this

                let editOutgoingInvoiceDraftJson = this.transformViewModelToEditOutgoingInvoiceDraft(draft)
                httpClient.put(this.editDraftUrl, editOutgoingInvoiceDraftJson)
                    .then((response) => {
                        self.isSaving = false
                        self.$router.push('/accountancy/draft/search')
                    })
                    .catch((errors) => {
                        self.serverErrors = errorHelper.buildErrorListFromModelState(errors) || []
                        self.isSaving = false
                    })
            },
            loadResources() {
                return httpClient.get(this.$urls.editOutgoingInvoiceDraftLocalization)
            },
            onEndResourcesLoading() {
                this.loadVatList()
                this.loadDraftDetail()
            },
            loadVatList() {
                let self = this
                httpClient.get(this.$urls.getAvailableVats)
                    .then((response) => {
                        self.vatList = response || []
                        self.isLoading = false
                    })
                    .catch((errors) => {
                        self.vatList = []
                    })
            }
        }
    }
</script>