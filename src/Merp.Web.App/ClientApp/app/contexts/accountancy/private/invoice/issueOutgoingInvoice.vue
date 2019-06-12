<template>
    <div>
        <vue-element-loading v-bind:active="isLoading"></vue-element-loading>
        <div class="row align-items-center">
            <div class="col">
                <h2>{{ pageTitle }}</h2>
            </div>
            <div class="col">
                <router-link to="/accountancy/invoice/search" class="btn btn-outline-secondary float-right">
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
            <outgoing-invoice-form v-bind:invoice="invoice" 
                                v-bind:labels="uiTexts" 
                                v-bind:validation="validationRules" 
                                v-bind:vat-list="vatList" 
                                v-on:save="issueInvoice"
                                v-on:save-draft="createInvoiceDraft"
                                v-on:type-changed="updateInvoiceType">
            </outgoing-invoice-form>
        </div>
    </div>
</template>
<script>
    import _ from 'lodash'
    import OutgoingInvoiceForm from '@/app/contexts/accountancy/private/outgoingInvoiceForm.vue'
    import { issueOutgoingInvoiceValidationRules } from './validation-rules'
    import EndpointConfigurationMixin from '@/app/shared/mixins/endpointConfigurationMixin'

    import { httpClient } from '@/app/shared/services/httpClient'
    import errorHelper from '@/app/shared/services/errorHelper'

    export default {
        name: 'issueOutgoingInvoice',
        mixins: [EndpointConfigurationMixin],
        components: {
            'outgoing-invoice-form': OutgoingInvoiceForm
        },
        urls: {
            getOutgoingDocumentTypes: window.endpoints.accountancy.getOutgoingDocumentTypes,
            issueOutgoingInvoiceLocalization: window.endpoints.accountancy.issueOutgoingInvoiceLocalization,
            issueInvoice: window.endpoints.accountancy.issueInvoice,
            createOutgoingDraft: window.endpoints.accountancy.createOutgoingDraft,
            getAvailableVats: window.endpoints.accountancyInternal.getAvailableVats
        },
        data() {
            return {
                isLoading: true,
                isSaving: false,
                serverErrors: [],
                validationRules: issueOutgoingInvoiceValidationRules,
                vatList: [],
                invoice: {
                    type: _.upperFirst(this.$route.params.type) || 'OutgoingInvoice',
                    customer: {},
                    date: new Date(),
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
                    lineItems: [{
                        code: '',
                        description: '',
                        quantity: 0,
                        unitPrice: 0,
                        totalPrice: 0,
                        vat: ''
                    }],
                    providenceFund: null,
                    withholdingTax: null,
                    pricesByVat: [{}],
                    nonTaxableItems: []
                }
            }
        },
        computed: {
            pageTitle() {
                switch (this.invoice.type) {
                    case 'OutgoingCreditNote':
                        return this.uiTexts.issueCreditNotePageTitle
                    case 'OutgoingInvoice':
                    default:
                        return this.uiTexts.issueInvoicePageTitle
                }
            }
        },
        methods: {
            transformViewModelToIssueOutgoingInvoice(viewModel) {
                return {
                    type: viewModel.type,
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
            transformViewModelToCreateOutgoingDraft(viewModel) {
                return {
                    type: viewModel.type,
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
            issueInvoice(invoice) {
                this.isSaving = true
                let self = this

                let issueOutgoingInvoice = this.transformViewModelToIssueOutgoingInvoice(invoice)
                httpClient.post(this.$urls.issueInvoice, issueOutgoingInvoice)
                    .then((response) => {
                        self.isSaving = false
                        self.$router.push('/accountancy/invoice/search')
                    })
                    .catch((errors) => {
                        self.serverErrors = errorHelper.buildErrorListFromModelState(errors) || []
                        self.isSaving = false
                    })
            },
            createInvoiceDraft(invoice) {
                this.isSaving = true
                let self = this

                let outgoingInvoiceDraft = this.transformViewModelToCreateOutgoingDraft(invoice)
                httpClient.post(this.$urls.createOutgoingDraft, outgoingInvoiceDraft)
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
                return httpClient.get(this.$urls.issueOutgoingInvoiceLocalization)
            },
            onEndResourcesLoading() {
                this.loadVatList()
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
                        self.isLoading = false
                    })
            },
            updateInvoiceType(type) {
                this.invoice.type = type || 'OutgoingInvoice'
            }
        }
    }
</script>