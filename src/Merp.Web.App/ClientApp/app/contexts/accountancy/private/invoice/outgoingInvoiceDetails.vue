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
                    <dt class="col-md-3 font-weight-bold">{{ uiTexts.invoiceNumber }}</dt>
                    <dd class="col-md-9">{{ invoice.invoiceNumber }}</dd>

                    <dt class="col-md-3 font-weight-bold">{{ uiTexts.date }}</dt>
                    <dd class="col-md-9">{{ invoice.date | dateFormat }}</dd>

                    <dt class="col-md-3 font-weight-bold">{{ uiTexts.dueDate }}</dt>
                    <dd class="col-md-9">{{ invoice.dueDate | dateFormat }}</dd>

                    <dt v-if="invoice.paymentDate" class="col-md-3 font-weight-bold">{{ uiTexts.paymentDate }}</dt>
                    <dd v-if="invoice.paymentDate" class="col-md-9">{{ invoice.paymentDate | dateFormat }}</dd>

                    <dt class="col-md-3 font-weight-bold">{{ uiTexts.currency }}</dt>
                    <dd class="col-md-9">{{ invoice.currency }}</dd>

                    <dt class="col-md-3 font-weight-bold">{{ uiTexts.purchaseOrderNumber }}</dt>
                    <dd class="col-md-9">{{ invoice.purchaseOrderNumber }}</dd>

                    <dt class="col-md-3 font-weight-bold">{{ uiTexts.paymentTerms }}</dt>
                    <dd class="col-md-9">{{ invoice.paymentTerms }}</dd>

                    <dt class="col-md-3 font-weight-bold">{{ uiTexts.description }}</dt>
                    <dd class="col-md-9">{{ invoice.description }}</dd>
                </dl>
            </div>
        </div>
        <div class="row">
            <div class="col-md-6">
                <dl class="row">
                    <dt class="col-md-6 font-weight-bold">{{ uiTexts.supplier }}</dt>
                    <dd class="col-md-6">
                        <address>
                            <strong>{{ invoice.supplier.name }}</strong><br />
                            {{ invoice.supplier.address }}<br />
                            {{ invoice.supplier.postalCode }} {{ invoice.supplier.city }}, {{ invoice.supplier.country }}
                        </address>
                    </dd>

                    <dt v-if="invoice.supplier.nationalIdentificationNumber" class="col-md-6 font-weight-bold">{{ uiTexts.nationalIdentificationNumber }}</dt>
                    <dd v-if="invoice.supplier.nationalIdentificationNumber" class="col-md-6">{{ invoice.supplier.nationalIdentificationNumber }}</dd>

                    <dt v-if="invoice.supplier.vatNumber" class="col-md-6 font-weight-bold">{{ uiTexts.vatNumber }}</dt>
                    <dd v-if="invoice.supplier.vatNumber" class="col-md-6">{{ invoice.supplier.vatNumber }}</dd>
                </dl>
            </div>
            <div class="col-md-6">
                <dl class="row">
                    <dt class="col-md-6 font-weight-bold">{{ uiTexts.customer }}</dt>
                    <dd class="col-md-6">
                        <address>
                            <strong>{{ invoice.customer.name }}</strong><br />
                            {{ invoice.customer.address }}<br />
                            {{ invoice.customer.postalCode }} {{ invoice.customer.city }}, {{ invoice.customer.country }}
                        </address>
                    </dd>

                    <dt v-if="invoice.customer.nationalIdentificationNumber" class="col-md-6 font-weight-bold">{{ uiTexts.nationalIdentificationNumber }}</dt>
                    <dd v-if="invoice.customer.nationalIdentificationNumber" class="col-md-6">{{ invoice.customer.nationalIdentificationNumber }}</dd>

                    <dt v-if="invoice.customer.vatNumber" class="col-md-6 font-weight-bold">{{ uiTexts.vatNumber }}</dt>
                    <dd v-if="invoice.customer.vatNumber" class="col-md-6">{{ invoice.customer.vatNumber }}</dd>
                </dl>
            </div>
        </div>
        <fieldset>
            <legend>{{ uiTexts.invoiceLines }}</legend>
            <div class="row">
                <div class="col-md-2 font-weight-bold">{{ uiTexts.lineItemCode }}</div>
                <div class="col-md-3 font-weight-bold">{{ uiTexts.description }}</div>
                <div class="col-md-1 font-weight-bold text-center">{{ uiTexts.lineItemQuantity }}</div>
                <div class="col-md-2 font-weight-bold text-right">{{ uiTexts.lineItemUnitPrice }}</div>
                <div class="col-md-2 font-weight-bold text-right">{{ uiTexts.lineItemTotalPrice }}</div>
                <div class="col-md-2 font-weight-bold text-right">{{ uiTexts.lineItemVat }}</div>
            </div>
            <div class="row" v-for="(item, index) in invoice.lineItems" v-bind:key="index">
                <div class="col-md-2">{{ item.code }}</div>
                <div class="col-md-3">{{ item.description }}</div>
                <div class="col-md-1 text-center">{{ item.quantity }}</div>
                <div class="col-md-2 text-right">{{ invoice.currency }} {{ item.unitPrice | currencyFormat(2) }}</div>
                <div class="col-md-2 text-right">{{ invoice.currency }} {{ item.totalPrice | currencyFormat(2) }}</div>
                <div class="col-md-2 text-right">{{ getVatDescription(item) }}</div>
            </div>
        </fieldset>
        <fieldset v-if="invoice.providenceFund">
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
                    <p class="form-control-plaintext">{{ invoice.providenceFund.description }}</p>
                </div>
                <div class="col-md-2">
                    <p class="form-control-plaintext text-right">{{ invoice.providenceFund.rate }}%</p>
                </div>
                <div class="col-md-3">
                    <p class="form-control-plaintext text-right">{{ invoice.currency }} {{ invoice.providenceFund.amount | currencyFormat(2) }}</p>
                </div>
            </div>
        </fieldset>
        <fieldset v-if="invoice.withholdingTax">
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
                    <p class="form-control-plaintext">{{ invoice.withholdingTax.description }}</p>
                </div>
                <div class="col-md-2">
                    <p class="form-control-plaintext text-right">{{ invoice.withholdingTax.rate }}%</p>
                </div>
                <div class="col-md-2">
                    <p class="form-control-plaintext text-right">{{ invoice.withholdingTax.taxableAmountRate }}%</p>
                </div>
                <div class="col-md-2 text-right">
                    <p class="form-control-plaintext text-right">{{ invoice.currency }} {{ invoice.withholdingTax.amount | currencyFormat(2) }}</p>
                </div>
            </div>
        </fieldset>
        <hr />
        <fieldset>
            <legend>{{ uiTexts.invoiceTotals }}</legend>
            <div class="row">
                <div class="col-md-2 font-weight-bold">{{ uiTexts.taxableAmount }}</div>
                <div class="col-md-10">{{ invoice.currency }} {{ invoice.amount | currencyFormat(2) }}</div>
            </div>
            <div class="row">
                <div class="col-md-2 font-weight-bold">{{ uiTexts.taxes }}</div>
                <div class="col-md-10">{{ invoice.currency }} {{ invoice.taxes | currencyFormat(2) }}</div>
            </div>
            <div class="row" v-if="invoice.nonTaxableItems.length" v-for="(item, index) in invoice.nonTaxableItems" v-bind:key="index">
                <div class="col-md-2 font-weight-bold">{{ item.description }}</div>
                <div class="col-md-10">{{ invoice.currency }} {{ item.amount | currencyFormat(2) }}</div>
            </div>
            <div class="row">
                <div class="col-md-2 font-weight-bold">{{ uiTexts.totalPrice }}</div>
                <div class="col-md-10">{{ invoice.currency }} {{ invoice.totalPrice | currencyFormat(2) }}</div>
            </div>
            <div class="row" v-if="invoice.withholdingTax">
                <div class="col-md-2 font-weight-bold">{{ uiTexts.withholdingTax }}</div>
                <div class="col-md-10">{{ invoice.currency }} {{ invoice.withholdingTax.amount | currencyFormat(2) }}</div>
            </div>
            <div class="row">
                <div class="col-md-2 font-weight-bold">{{ uiTexts.totalToPay }}</div>
                <div class="col-md-10">{{ invoice.currency }} {{ invoice.totalToPay | currencyFormat(2) }}</div>
            </div>
        </fieldset>
    </div>
</template>
<script>
    import EndpointConfigurationMixin from '@/app/shared/mixins/endpointConfigurationMixin'
    import { httpClient } from '@/app/shared/services/httpClient'
    import currencyFormat from '@/app/shared/filters/currencyFormat'

    export default {
        name: 'outgoingInvoiceDetails',
        mixins: [EndpointConfigurationMixin],
        urls: {
            outgoingInvoiceDetailsLocalization: window.endpoints.accountancy.outgoingInvoiceDetailsLocalization,
            outgoingInvoiceDetails: window.endpoints.accountancy.outgoingInvoiceDetails
        },
        data() {
            return {
                isLoading: true,
                invoice: {
                    customer: {},
                    supplier: {},
                    invoiceNumber: null,
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
            invoiceId() {
                return this.$route.params.id
            },
            pageTitle() {
                let pageTitle = this.uiTexts.outgoingInvoice
                if (this.invoice.invoiceNumber) {
                    pageTitle += ' ' + this.invoice.invoiceNumber
                }

                return pageTitle
            }
        },
        methods: {
            transformJsonToViewModel(invoiceJson) {
                let viewModel = JSON.parse(JSON.stringify(invoiceJson))
                return viewModel
            },
            loadInvoiceDetails() {
                let url = this.$urls.outgoingInvoiceDetails + '/' + this.invoiceId
                let self = this

                httpClient.get(url)
                    .then((response) => {
                        self.invoice = self.transformJsonToViewModel(response) || {}
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
                return httpClient.get(this.$urls.outgoingInvoiceDetailsLocalization)
            },
            onEndResourcesLoading() {
                this.loadInvoiceDetails()
            }
        }
    }
</script>