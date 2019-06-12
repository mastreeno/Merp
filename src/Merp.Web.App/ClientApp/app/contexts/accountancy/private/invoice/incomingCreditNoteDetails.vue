<template>
    <div>
        <vue-element-loading v-bind:active="isLoading"></vue-element-loading>
        <div class="row align-items-center">
            <div class="col">
                <h2>
                    {{ pageTitle }}
                </h2>
            </div>
            <div class="col">
                <router-link to="/accountancy/invoice/search" class="btn btn-outline-secondary float-right">
                    <i class="fa fa-angle-left"></i> {{ uiTexts.back }}
                </router-link>
            </div>
        </div>
        <hr />
        <div class="row">
            <div class="col-md-12">
                <dl class="row">
                    <dt class="col-md-3 font-weight-bold">{{ uiTexts.creditNoteNumber }}</dt>
                    <dd class="col-md-9">{{ creditNote.creditNoteNumber }}</dd>

                    <dt class="col-md-3 font-weight-bold">{{ uiTexts.date }}</dt>
                    <dd class="col-md-9">{{ creditNote.date | dateFormat }}</dd>

                    <dt class="col-md-3 font-weight-bold">{{ uiTexts.dueDate }}</dt>
                    <dd class="col-md-9">{{ creditNote.dueDate | dateFormat }}</dd>

                    <dt v-if="creditNote.paymentDate" class="col-md-3 font-weight-bold">{{ uiTexts.paymentDate }}</dt>
                    <dd v-if="creditNote.paymentDate" class="col-md-9">{{ creditNote.paymentDate | dateFormat }}</dd>

                    <dt class="col-md-3 font-weight-bold">{{ uiTexts.currency }}</dt>
                    <dd class="col-md-9">{{ creditNote.currency }}</dd>

                    <dt class="col-md-3 font-weight-bold">{{ uiTexts.purchaseOrderNumber }}</dt>
                    <dd class="col-md-9">{{ creditNote.purchaseOrderNumber }}</dd>

                    <dt class="col-md-3 font-weight-bold">{{ uiTexts.paymentTerms }}</dt>
                    <dd class="col-md-9">{{ creditNote.paymentTerms }}</dd>

                    <dt class="col-md-3 font-weight-bold">{{ uiTexts.description }}</dt>
                    <dd class="col-md-9">{{ creditNote.description }}</dd>
                </dl>
            </div>
        </div>
        <div class="row">
            <div class="col-md-6">
                <dl class="row">
                    <dt class="col-md-6 font-weight-bold">{{ uiTexts.supplier }}</dt>
                    <dd class="col-md-6">
                        <address>
                            <strong>{{ creditNote.supplier.name }}</strong><br />
                            {{ creditNote.supplier.address }}<br />
                            {{ creditNote.supplier.postalCode }} {{ creditNote.supplier.city }}, {{ creditNote.supplier.country }}
                        </address>
                    </dd>

                    <dt v-if="creditNote.supplier.nationalIdentificationNumber" class="col-md-6 font-weight-bold">{{ uiTexts.nationalIdentificationNumber }}</dt>
                    <dd v-if="creditNote.supplier.nationalIdentificationNumber" class="col-md-6">{{ creditNote.supplier.nationalIdentificationNumber }}</dd>

                    <dt v-if="creditNote.supplier.vatNumber" class="col-md-6 font-weight-bold">{{ uiTexts.vatNumber }}</dt>
                    <dd v-if="creditNote.supplier.vatNumber" class="col-md-6">{{ creditNote.supplier.vatNumber }}</dd>
                </dl>
            </div>
            <div class="col-md-6">
                <dl class="row">
                    <dt class="col-md-6 font-weight-bold">{{ uiTexts.customer }}</dt>
                    <dd class="col-md-6">
                        <address>
                            <strong>{{ creditNote.customer.name }}</strong><br />
                            {{ creditNote.customer.address }}<br />
                            {{ creditNote.customer.postalCode }} {{ creditNote.customer.city }}, {{ creditNote.customer.country }}
                        </address>
                    </dd>

                    <dt v-if="creditNote.customer.nationalIdentificationNumber" class="col-md-6 font-weight-bold">{{ uiTexts.nationalIdentificationNumber }}</dt>
                    <dd v-if="creditNote.customer.nationalIdentificationNumber" class="col-md-6">{{ creditNote.customer.nationalIdentificationNumber }}</dd>

                    <dt v-if="creditNote.customer.vatNumber" class="col-md-6 font-weight-bold">{{ uiTexts.vatNumber }}</dt>
                    <dd v-if="creditNote.customer.vatNumber" class="col-md-6">{{ creditNote.customer.vatNumber }}</dd>
                </dl>
            </div>
        </div>
        <fieldset>
            <legend>{{ uiTexts.creditNoteLines }}</legend>
            <div class="row">
                <div class="col-md-2 font-weight-bold">{{ uiTexts.lineItemCode }}</div>
                <div class="col-md-3 font-weight-bold">{{ uiTexts.description }}</div>
                <div class="col-md-1 font-weight-bold text-center">{{ uiTexts.lineItemQuantity }}</div>
                <div class="col-md-2 font-weight-bold text-right">{{ uiTexts.lineItemUnitPrice }}</div>
                <div class="col-md-2 font-weight-bold text-right">{{ uiTexts.lineItemTotalPrice }}</div>
                <div class="col-md-2 font-weight-bold text-right">{{ uiTexts.lineItemVat }}</div>
            </div>
            <div class="row" v-for="(item, index) in creditNote.lineItems" v-bind:key="index">
                <div class="col-md-2">{{ item.code }}</div>
                <div class="col-md-3">{{ item.description }}</div>
                <div class="col-md-1 text-center">{{ item.quantity }}</div>
                <div class="col-md-2 text-right">{{ creditNote.currency }} {{ item.unitPrice | currencyFormat(2) }}</div>
                <div class="col-md-2 text-right">{{ creditNote.currency }} {{ item.totalPrice | currencyFormat(2) }}</div>
                <div class="col-md-2 text-right">{{ getVatDescription(item) }}</div>
            </div>
        </fieldset>
        <fieldset v-if="creditNote.providenceFund">
            <legend>{{ uiTexts.providenceFund }}</legend>

            <div class="form-row">
                <div class="col-md-7">
                    <label class="font-weight-bold">{{ uiTexts.description }}</label>
                </div>
                <div class="col-md-2 text-right">
                    <label class="font-weight-bold">{{ uiTexts.rate }}</label>
                </div>
                <div class="col-md-3 text-right">
                    <label class="font-weight-bold">{{ uiTexts.amount }}</label>
                </div>
            </div>
            <div class="form-row">
                <div class="col-md-7">
                    <p class="form-control-plaintext">{{ creditNote.providenceFund.description }}</p>
                </div>
                <div class="col-md-2">
                    <p class="form-control-plaintext text-right">{{ creditNote.providenceFund.rate }}%</p>
                </div>
                <div class="col-md-3">
                    <p class="form-control-plaintext text-right">{{ creditNote.currency }} {{ creditNote.providenceFund.amount | currencyFormat(2) }}</p>
                </div>
            </div>
        </fieldset>
        <fieldset v-if="creditNote.withholdingTax">
            <legend>{{ uiTexts.withholdingTax }}</legend>
            <div class="form-row">
                <div class="col-md-6">
                    <label class="font-weight-bold">{{ uiTexts.description }}</label>
                </div>
                <div class="col-md-2 text-right">
                    <label class="font-weight-bold">{{ uiTexts.rate }}</label>
                </div>
                <div class="col-md-2 text-right">
                    <label class="font-weight-bold">{{ uiTexts.taxableAmountRate }}</label>
                </div>
                <div class="col-md-2 text-right">
                    <label class="font-weight-bold">{{ uiTexts.amount }}</label>
                </div>
            </div>
            <div class="form-row">
                <div class="col-md-6">
                    <p class="form-control-plaintext">{{ creditNote.withholdingTax.description }}</p>
                </div>
                <div class="col-md-2">
                    <p class="form-control-plaintext text-right">{{ creditNote.withholdingTax.rate }}%</p>
                </div>
                <div class="col-md-2">
                    <p class="form-control-plaintext text-right">{{ creditNote.withholdingTax.taxableAmountRate }}%</p>
                </div>
                <div class="col-md-2 text-right">
                    <p class="form-control-plaintext text-right">{{ creditNote.currency }} {{ creditNote.withholdingTax.amount | currencyFormat(2) }}</p>
                </div>
            </div>
        </fieldset>
        <hr />
        <fieldset>
            <legend>{{ uiTexts.creditNoteTotals }}</legend>
            <div class="row">
                <div class="col-md-2 font-weight-bold">{{ uiTexts.taxableAmount }}</div>
                <div class="col-md-10">{{ creditNote.currency }} {{ creditNote.amount | currencyFormat(2) }}</div>
            </div>
            <div class="row">
                <div class="col-md-2 font-weight-bold">{{ uiTexts.taxes }}</div>
                <div class="col-md-10">{{ creditNote.currency }} {{ creditNote.taxes | currencyFormat(2) }}</div>
            </div>
            <div class="row" v-if="creditNote.nonTaxableItems.length" v-for="(item, index) in creditNote.nonTaxableItems" v-bind:key="index">
                <div class="col-md-2 font-weight-bold">{{ item.description }}</div>
                <div class="col-md-10">{{ creditNote.currency }} {{ item.amount | currencyFormat(2) }}</div>
            </div>
            <div class="row">
                <div class="col-md-2 font-weight-bold">{{ uiTexts.totalPrice }}</div>
                <div class="col-md-10">{{ creditNote.currency }} {{ creditNote.totalPrice | currencyFormat(2) }}</div>
            </div>
            <div class="row" v-if="creditNote.withholdingTax">
                <div class="col-md-2 font-weight-bold">{{ uiTexts.withholdingTax }}</div>
                <div class="col-md-10">{{ creditNote.currency }} {{ creditNote.withholdingTax.amount | currencyFormat(2) }}</div>
            </div>
            <div class="row">
                <div class="col-md-2 font-weight-bold">{{ uiTexts.totalToPay }}</div>
                <div class="col-md-10">{{ creditNote.currency }} {{ creditNote.totalToPay | currencyFormat(2) }}</div>
            </div>
        </fieldset>
    </div>
</template>
<script>
    import EndpointConfigurationMixin from '@/app/shared/mixins/endpointConfigurationMixin'
    import { httpClient } from '@/app/shared/services/httpClient'
    import currencyFormat from '@/app/shared/filters/currencyFormat'

    export default {
        name: 'incomingCreditNoteDetails',
        mixins: [EndpointConfigurationMixin],
        urls: {
            incomingCreditNoteDetailsLocalization: window.endpoints.accountancy.incomingCreditNoteDetailsLocalization,
            incomingCreditNoteDetails: window.endpoints.accountancy.incomingCreditNoteDetails
        },
        data() {
            return {
                isLoading: true,
                creditNote: {
                    customer: {},
                    supplier: {},
                    creditNoteNumber: null,
                    date: null,
                    dueDate: null,
                    paymentDate: null,
                    currency: null,
                    amount: null,
                    taxes: null,
                    totalPrice: null,
                    description: null,
                    paymentTerms: null,
                    purchaseOrderNumber: null,
                    isOverdue: false,
                    lineItems: [],
                    nonTaxableItems: [],
                    providenceFund: null,
                    withholdingTax: null
                }
            }
        },
        computed: {
            creditNoteId() {
                return this.$route.params.id
            },
            pageTitle() {
                let pageTitle = this.uiTexts.incomingCreditNote
                if (this.creditNote.creditNoteNumber) {
                    pageTitle += ' ' + this.creditNote.creditNoteNumber
                }

                return pageTitle
            }
        },
        methods: {
            transformJsonToViewModel(creditNoteJson) {
                let viewModel = JSON.parse(JSON.stringify(creditNoteJson))
                return viewModel
            },
            loadCreditNoteDetails() {
                let url = this.$urls.incomingCreditNoteDetails + '/' + this.creditNoteId
                let self = this

                httpClient.get(url)
                    .then((response) => {
                        self.creditNote = self.transformJsonToViewModel(response) || {}
                        self.isLoading = false
                    })
            },
            getVatDescription(item) {
                let defaultDescription = currencyFormat(item.vat, 2) + '%'
                if (!item.vatDescription) {
                    return defaultDescription
                }

                return item.vatDescription
            },
            loadResources() {
                return httpClient.get(this.$urls.incomingCreditNoteDetailsLocalization)
            },
            onEndResourcesLoading() {
                this.loadCreditNoteDetails()
            }
        }
    }
</script>