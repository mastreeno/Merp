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
                                   v-on:save="issueCreditNoteFromDraft"
                                   v-on:save-draft="editDraft">
            </outgoing-invoice-form>
        </div>
    </div>
</template>
<script>
    import OutgoingInvoiceForm from '@/app/accountancy/private/outgoingInvoiceForm.vue'
    import EndpointConfigurationMixin from '@/app/mixins/endpointConfigurationMixin'

    import { httpClient } from '@/app/services/httpClient'
    import errorHelper from '@/app/services/errorHelper'

    export default {
        name: 'editOutgoingInvoiceDraft',
        mixins: [EndpointConfigurationMixin],
        components: {
            'outgoing-invoice-form': OutgoingInvoiceForm
        },
        urls: {
            editOutgoingCreditNoteDraft: window.endpoints.accountancy.editOutgoingCreditNoteDraft,
            editOutgoingCreditNoteDraftLocalization: window.endpoints.accountancy.editOutgoingCreditNoteDraftLocalization,
            getInvoiceVatList: window.endpoints.accountancy.getInvoiceVatList,
            issueOutgoingCreditNoteFromDraft: window.endpoints.accountancy.issueOutgoingCreditNoteFromDraft
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
                    totalTaxableAmount: 0.00,
                    purchaseOrderNumber: '',
                    paymentTerms: '',
                    description: '',
                    vatIncluded: false,
                    lineItems: [],
                    pricesByVat: [],
                    nonTaxableItems: []
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
                return this.$urls.editOutgoingCreditNoteDraft + '/' + this.draftId
            }
        },
        methods: {
            transformJsonToViewModel(editOutgoingCreditNoteDraftJson) {
                let viewModel = JSON.parse(JSON.stringify(editOutgoingCreditNoteDraftJson))
                return viewModel
            },
            transformViewModelToEditOutgoingCreditNoteDraft(viewModel) {
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
                            vat: i.vat,
                            vatIncluded: false
                        }
                    }),
                    pricesByVat: viewModel.pricesByVat.map((p) => {
                        return {
                            taxableAmount: p.taxableAmount,
                            vatRate: p.vatRate,
                            vatAmount: p.vatAmount,
                            totalPrice: p.totalPrice
                        }
                    }),
                    nonTaxableItems: viewModel.nonTaxableItems.map((t) => {
                        return {
                            id: t.id,
                            description: t.description,
                            amount: t.amount
                        }
                    })
                }
            },
            transformViewModelToIssueOutgoingCreditNoteFromDraft(viewModel) {
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
                            vat: i.vat,
                            vatIncluded: false
                        }
                    }),
                    pricesByVat: viewModel.pricesByVat.map((p) => {
                        return {
                            taxableAmount: p.taxableAmount,
                            vatRate: p.vatRate,
                            vatAmount: p.vatAmount,
                            totalPrice: p.totalPrice
                        }
                    }),
                    nonTaxableItems: viewModel.nonTaxableItems.map((t) => {
                        return {
                            description: t.description,
                            amount: t.amount
                        }
                    })
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
            issueCreditNoteFromDraft(draft) {
                this.isSaving = true
                let self = this

                let issueOutgoingCreditNoteFromDraftJson = this.transformViewModelToIssueOutgoingCreditNoteFromDraft(draft)
                httpClient.post(this.$urls.issueOutgoingCreditNoteFromDraft, issueOutgoingCreditNoteFromDraftJson)
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

                let editOutgoingCreditNoteDraftJson = this.transformViewModelToEditOutgoingCreditNoteDraft(draft)
                httpClient.put(this.editDraftUrl, editOutgoingCreditNoteDraftJson)
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
                return httpClient.get(this.$urls.editOutgoingCreditNoteDraftLocalization)
            },
            onEndResourcesLoading() {
                this.loadVatList()
                this.loadDraftDetail()
            },
            loadVatList() {
                let self = this
                httpClient.get(this.$urls.getInvoiceVatList)
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