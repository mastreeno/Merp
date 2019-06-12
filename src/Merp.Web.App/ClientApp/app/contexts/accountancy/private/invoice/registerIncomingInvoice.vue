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
            <form class="col-md-12" role="form" v-on:submit.prevent="registerInvoice">
                <b-card>
                    <div class="form-row">
                        <div class="col-md-12">
                            <div class="row form-group">
                                <label class="col-form-label col">{{ uiTexts.chooseDocumentType }}</label>
                                <div class="col">
                                    <b-form-radio-group buttons
                                                        v-bind:options="incomingDocumentTypes"
                                                        button-variant="outline-primary"
                                                        v-model="invoice.type"
                                                        class="float-right">
                                    </b-form-radio-group>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="form-row">
                        <div class="col-md-6 form-group">
                            <label for="customer">{{ uiTexts.supplier }}</label>
                            <party-info-autocomplete v-bind:party="invoice.supplier" v-bind:disabled="false" v-bind:validate="validationRules.supplier" name="supplier" v-bind:field-label="uiTexts.supplier" v-on:party-selected="selectSupplier"></party-info-autocomplete>
                        </div>
                        <div class="col-md-6 form-group">
                            <label for="invoiceNumber">{{ uiTexts.invoiceNumber }}</label>
                            <input type="text" class="form-control" name="invoiceNumber" v-model="invoice.invoiceNumber" v-validate="validationRules.invoiceNumber" v-bind:data-vv-as="uiTexts.invoiceNumber" />
                            <span class="text-danger" v-if="errors.first('invoiceNumber')">{{ errors.first('invoiceNumber') }}</span>
                        </div>
                    </div>
                    <div class="form-row">
                        <div class="col form-group">
                            <label for="currency">{{ uiTexts.currency }}</label>
                            <input type="text" class="form-control" name="currency" v-model="invoice.currency" v-validate="validationRules.currency" v-bind:data-vv-as="uiTexts.currency" />
                            <span class="text-danger" v-if="errors.first('currency')">{{ errors.first('currency') }}</span>
                        </div>
                        <div class="col form-group">
                            <label for="date">{{ uiTexts.date }}</label>
                            <datepicker v-bind:show-calendar-button="true"
                                        name="date"
                                        v-model="invoice.date"
                                        v-validate="validationRules.date"
                                        v-bind:data-vv-as="uiTexts.date">
                            </datepicker>
                            <span class="text-danger" v-if="errors.first('date')">{{ errors.first('date') }}</span>
                        </div>
                        <div class="col form-group" v-if="showDueDate">
                            <label for="dueDate">{{ uiTexts.dueDate }}</label>
                            <datepicker v-bind:show-calendar-button="true"
                                        v-bind:show-clear-button="true"
                                        name="dueDate"
                                        v-model="invoice.dueDate">
                            </datepicker>
                        </div>
                    </div>
                    <div class="form-row">
                        <div class="col-md-6 form-group">
                            <label for="purchaseOrderNumber">{{ uiTexts.purchaseOrderNumber }}</label>
                            <input type="text" name="purchaseOrderNumber" class="form-control" v-model="invoice.purchaseOrderNumber" />
                        </div>
                        <div class="col-md-6 form-group">
                            <label for="paymentTerms">{{ uiTexts.paymentTerms }}</label>
                            <input type="text" name="paymentTerms" class="form-control" v-model="invoice.paymentTerms" />
                        </div>
                    </div>
                    <div class="form-row">
                        <div class="col-md-12 form-group">
                            <label for="description">{{ uiTexts.description }}</label>
                            <input type="text" name="description" class="form-control" v-model="invoice.description" v-validate="validationRules.description" v-bind:data-vv-as="uiTexts.description" />
                            <span class="text-danger" v-if="errors.first('description')">{{ errors.first('description') }}</span>
                        </div>
                    </div>
                    <div class="form-row">
                        <div class="col-md-12 form-group">
                            <b-form-checkbox v-model="invoice.vatIncluded" v-on:input="refreshPricesByVat">
                                {{ uiTexts.vatIncluded }}
                            </b-form-checkbox>
                        </div>
                    </div>
                    <fieldset>
                        <legend>{{ uiTexts.invoiceLines }}</legend>

                        <div class="form-row">
                            <div class="col-md-1">
                                <label class="font-weight-bold">{{ uiTexts.lineItemCode }}</label>
                            </div>
                            <div class="col-md-3">
                                <label class="font-weight-bold">{{ uiTexts.description }}</label>
                            </div>
                            <div class="col-md-1">
                                <label class="font-weight-bold">{{ uiTexts.lineItemQuantity }}</label>
                            </div>
                            <div class="col-md-2">
                                <label class="font-weight-bold">{{ uiTexts.lineItemUnitPrice }}</label>
                            </div>
                            <div class="col-md-2">
                                <label class="font-weight-bold">{{ uiTexts.lineItemTotalPrice }}</label>
                            </div>
                            <div class="col-md-3">
                                <label class="font-weight-bold">{{ uiTexts.lineItemVat }}</label>
                            </div>
                        </div>
                        <invoice-line-item v-for="(item, index) in invoice.lineItems" v-bind:key="index" v-bind:line-item.sync="item" v-bind:validation="validationRules" v-bind:index="index" v-bind:vat-list="vatList" v-on:remove-item="removeInvoiceLineItem(index)" v-on:refresh-totals="refreshPricesByVat"></invoice-line-item>
                        <div class="form-row">
                            <div class="col-md-12 form-group">
                                <button type="button" class="btn btn-outline-primary" v-on:click="addNewInvoiceItem">
                                    <i class="fa fa-plus"></i> {{ uiTexts.addNew }}
                                </button>
                            </div>
                        </div>
                    </fieldset>
                    <fieldset>
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
                                <select class="form-control" v-model="invoice.providenceFund.description" v-on:change="changeProvidenceFundValues" v-b-tooltip.hover v-bind:title="invoice.providenceFund.description">
                                    <option v-bind:value="null"></option>
                                    <option v-for="providenceFund in providenceFunds" v-bind:key="providenceFund.id" v-bind:value="providenceFund.description">{{ providenceFund.description }}</option>
                                </select>
                            </div>
                            <div class="col-md-2">
                                <p class="form-control-plaintext text-right" v-if="invoice.providenceFund.rate">{{ invoice.providenceFund.rate }}%</p>
                            </div>
                            <div class="col-md-3">
                                <p class="form-control-plaintext text-right" v-if="invoice.providenceFund.amount">{{ invoice.currency }} {{ invoice.providenceFund.amount | currencyFormat(2) }}</p>
                            </div>
                        </div>
                    </fieldset>
                    <div class="row">
                        <div class="col-md-6">
                            <fieldset>
                                <legend>{{ uiTexts.pricesByVat }}</legend>
                                <div class="form-row">
                                    <div class="col-md-3">
                                        <label class="font-weight-bold">{{ uiTexts.taxableAmount }}</label>
                                    </div>
                                    <div class="col-md-3 offset-md-2">
                                        <label class="font-weight-bold">{{ uiTexts.vat }}</label>
                                    </div>
                                    <div class="col-md-4">
                                        <label class="font-weight-bold">{{ uiTexts.totalPrice }}</label>
                                    </div>
                                </div>
                                <div class="form-row form-group" v-for="(item, index) in invoice.pricesByVat" v-bind:key="index">
                                    <div class="col-md-3">
                                        <input type="number" class="form-control" v-model="item.taxableAmount" step="0.01" v-on:change="calculateInvoiceTotals" />
                                    </div>
                                    <div class="col-md-2">
                                        <span class="form-control-plaintext text-right">{{ item.vatRate }}%</span>
                                    </div>
                                    <div class="col-md-3">
                                        <input type="number" class="form-control" v-model="item.vatAmount" step="0.01" min="0" v-on:change="calculateInvoiceTotals" />
                                    </div>
                                    <div class="col-md-4">
                                        <input type="number" class="form-control" v-model="item.totalPrice" step="0.01" min="0" v-on:change="calculateInvoiceTotals" />
                                    </div>
                                </div>
                            </fieldset>
                        </div>
                        <div class="col-md-6">
                            <fieldset>
                                <legend>{{ uiTexts.nonTaxableItems }}</legend>
                                <div class="form-row">
                                    <div class="col-md-8">
                                        <label class="font-weight-bold">{{ uiTexts.description }}</label>
                                    </div>
                                    <div class="col-md-4">
                                        <label class="font-weight-bold">{{ uiTexts.amount }}</label>
                                    </div>
                                </div>
                                <div class="form-row form-group" v-for="(item, index) in invoice.nonTaxableItems" v-bind:key="index">
                                    <div class="col-md-8">
                                        <input type="text" class="form-control" v-model="item.description" />
                                    </div>
                                    <div class="col-md-3">
                                        <input type="number" step="0.01" min="0" class="form-control" v-model="item.amount" v-on:change="calculateInvoiceTotals" />
                                    </div>
                                    <div class="col-md-1">
                                        <button type="button" class="btn btn-outline-secondary" v-on:click="removeNonTaxableItem(index)">
                                            <i class="fa fa-remove"></i>
                                        </button>
                                    </div>
                                </div>
                                <div class="form-row form-group">
                                    <div class="col-md-12">
                                        <button type="button" v-on:click="addNonTaxableItem" class="btn btn-outline-primary">
                                            <i class="fa fa-plus"></i> {{ uiTexts.addNew }}
                                        </button>
                                    </div>
                                </div>
                            </fieldset>
                        </div>
                    </div>
                    <fieldset>
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
                                <select class="form-control" v-model="invoice.withholdingTax.description" v-on:change="changeWithholdingTaxValues" v-b-tooltip.hover v-bind:title="invoice.withholdingTax.description">
                                    <option v-bind:value="null"></option>
                                    <option v-for="withholdingTax in withholdingTaxes" v-bind:key="withholdingTax.id" v-bind:value="withholdingTax.description">{{ withholdingTax.description }}</option>
                                </select>
                            </div>
                            <div class="col-md-2">
                                <p class="form-control-plaintext text-right" v-if="invoice.withholdingTax.rate">{{ invoice.withholdingTax.rate }}%</p>
                            </div>
                            <div class="col-md-2">
                                <p class="form-control-plaintext text-right" v-if="invoice.withholdingTax.taxableAmountRate">{{ invoice.withholdingTax.taxableAmountRate }}%</p>
                            </div>
                            <div class="col-md-2 text-right">
                                <p class="form-control-plaintext text-right" v-if="invoice.withholdingTax.amount">{{ invoice.currency }} {{ invoice.withholdingTax.amount | currencyFormat(2) }}</p>
                            </div>
                        </div>
                    </fieldset>
                    <fieldset>
                        <legend>{{ uiTexts.invoiceTotals }}</legend>
                        <div class="form-row">
                            <div class="col-md-12">
                                <div class="form-group row">
                                    <label class="col-md-2 col-form-label font-weight-bold">{{ uiTexts.taxableAmount }}</label>
                                    <div class="col-md-10">
                                        <p class="form-control-plaintext">{{ invoice.currency }} {{ invoice.amount | currencyFormat(2) }}</p>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="form-row">
                            <div class="col-md-12">
                                <div class="form-group row">
                                    <label class="col-md-2 col-form-label font-weight-bold">{{ uiTexts.taxes }}</label>
                                    <div class="col-md-10">
                                        <p class="form-control-plaintext">{{ invoice.currency }} {{ invoice.taxes | currencyFormat(2) }}</p>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="form-row" v-if="invoice.nonTaxableItems.length">
                            <div class="col-md-12" v-for="(item, index) in invoice.nonTaxableItems" v-bind:key="index">
                                <div class="form-group row">
                                    <label class="col-md-2 col-form-label font-weight-bold">{{ item.description }}</label>
                                    <div class="col-md-10">
                                        <p class="form-control-plaintext">{{ invoice.currency }} {{ item.amount | currencyFormat(2) }}</p>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="form-row">
                            <div class="col-md-12">
                                <div class="form-group row">
                                    <label class="col-md-2 col-form-label font-weight-bold">{{ uiTexts.totalPrice }}</label>
                                    <div class="col-md-10">
                                        <p class="form-control-plaintext">{{ invoice.currency }} {{ invoice.totalPrice | currencyFormat(2) }}</p>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="form-row" v-if="invoice.withholdingTax && invoice.withholdingTax.amount">
                            <div class="col-md-12">
                                <div class="form-group row">
                                    <label class="col-md-2 col-form-label font-weight-bold">{{ uiTexts.withholdingTax }}</label>
                                    <div class="col-md-10">
                                        <p class="form-control-plaintext">{{ invoice.currency }} {{ invoice.withholdingTax.amount | currencyFormat(2) }}</p>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="form-row">
                            <div class="col-md-12">
                                <div class="form-group row">
                                    <label class="col-md-2 col-form-label font-weight-bold">{{ uiTexts.totalToPay }}</label>
                                    <div class="col-md-10">
                                        <p class="form-control-plaintext">{{ invoice.currency }} {{ invoice.totalToPay | currencyFormat(2) }}</p>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </fieldset>
                    <div slot="footer">
                        <div class="btn-group float-right" role="group">
                            <button type="submit" class="btn btn-primary" v-bind:disabled="!isValid">{{ uiTexts.save }}</button>
                            <button type="button" v-on:click="cancel" class="btn btn-outline-secondary">{{ uiTexts.cancel }}</button>
                        </div>
                    </div>
                </b-card>
            </form>
        </div>
    </div>
</template>
<script>
    import _ from 'lodash'
    import PartyInfoAutocomplete from '@/app/contexts/registry/shared/party/partyInfoAutocomplete.vue'
    import InvoiceLineItem from '@/app/contexts/accountancy/private/invoiceLineItem.vue'
    import { registerIncomingInvoiceValidationRules } from './validation-rules'
    import EndpointConfigurationMixin from '@/app/shared/mixins/endpointConfigurationMixin'
    import Datepicker from '@/app/shared/components/datepicker.vue'

    import { httpClient } from '@/app/shared/services/httpClient'
    import errorHelper from '@/app/shared/services/errorHelper'

    export default {
        name: 'registerIncomingInvoice',
        mixins: [EndpointConfigurationMixin],
        components: {
            'party-info-autocomplete': PartyInfoAutocomplete,
            'invoice-line-item': InvoiceLineItem,
            'datepicker': Datepicker
        },
        urls: {
            registerIncomingInvoiceLocalization: window.endpoints.accountancy.registerIncomingInvoiceLocalization,
            registerInvoice: window.endpoints.accountancy.registerInvoice,
            getIncomingDocumentTypes: window.endpoints.accountancy.getIncomingDocumentTypes,
            getAvailableVats: window.endpoints.accountancyInternal.getAvailableVats,
            getSettingsDefaults: window.endpoints.accountancyInternal.getSettingsDefaults,
            getProvidenceFunds: window.endpoints.accountancyInternal.getProvidenceFunds,
            getWithholdingTaxes: window.endpoints.accountancyInternal.getWithholdingTaxes
        },
        data() {
            return {
                isLoading: true,
                isSaving: false,
                serverErrors: [],
                validationRules: registerIncomingInvoiceValidationRules,
                vatList: [],
                invoice: {
                    type: _.upperFirst(this.$route.params.type) || 'IncomingInvoice',
                    supplier: {},
                    invoiceNumber: null,
                    date: new Date(),
                    dueDate: null,
                    currency: '',
                    amount: 0.00,
                    taxes: 0.00,
                    totalPrice: 0.00,
                    totalToPay: 0.00,
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
                        vat: '',
                    }],
                    pricesByVat: [{}],
                    nonTaxableItems: [],
                    providenceFund: {},
                    withholdingTax: {},
                },
                invoiceCopy: {
                    type: 'IncomingInvoice',
                    supplier: {},
                    invoiceNumber: null,
                    date: new Date(),
                    dueDate: null,
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
                    pricesByVat: [{}],
                    nonTaxableItems: [],
                    providenceFund: {},
                    withholdingTax: {},
                },
                incomingDocumentTypeList: [],
                providenceFunds: [],
                withholdingTaxes: [],
            }
        },
        computed: {
            isValid() {
                return this.errors.items.length === 0
            },
            incomingDocumentTypes() {
                return this.incomingDocumentTypeList
                    .map(d => {
                        let labelKey = _.camelCase(d)
                        return {
                            value: d,
                            text: this.uiTexts[labelKey]
                        }
                    })
            },
            pageTitle() {
                switch (this.invoice.type) {
                    case 'IncomingCreditNote':
                        return this.uiTexts.registerCreditNotePageTitle
                    case 'IncomingInvoice':
                    default:
                        return this.uiTexts.registerInvoicePageTitle
                }
            },
            showDueDate() {
                return !(this.invoice.type === 'IncomingCreditNote')
            }
        },
        methods: {
            transformViewModelToRegisterIncomingInvoice(viewModel) {
                return {
                    type: viewModel.type,
                    supplier: viewModel.supplier,
                    invoiceNumber: viewModel.invoiceNumber,
                    date: viewModel.date,
                    dueDate: viewModel.dueDate || null,
                    currency: viewModel.currency,
                    amount: viewModel.amount,
                    taxes: viewModel.taxes,
                    totalPrice: viewModel.totalPrice,
                    totalToPay: viewModel.totalToPay,
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
                    providenceFund: (!viewModel.providenceFund || !viewModel.providenceFund.description) ? null : {
                        description: viewModel.providenceFund.description,
                        rate: viewModel.providenceFund.rate,
                        amount: viewModel.providenceFund.amount
                    },
                    withholdingTax: !viewModel.withholdingTax ? null : {
                        description: viewModel.withholdingTax.description,
                        rate: viewModel.withholdingTax.rate,
                        taxableAmountRate: viewModel.withholdingTax.taxableAmountRate,
                        amount: viewModel.withholdingTax.amount
                    }
                }
            },
            registerInvoice() {
                this.isSaving = true
                let self = this

                let registerIncomingInvoiceJson = this.transformViewModelToRegisterIncomingInvoice(this.invoice)
                httpClient.post(this.$urls.registerInvoice, registerIncomingInvoiceJson)
                    .then((response) => {
                        self.isSaving = false
                        self.$router.push('/accountancy/invoice/search')
                    })
                    .catch((errors) => {
                        self.serverErrors = errorHelper.buildErrorListFromModelState(errors) || []
                        self.isSaving = false
                    })
            },
            cancel() {
                this.$validator.reset()
                this.invoice = JSON.parse(JSON.stringify(this.invoiceCopy))
            },
            selectSupplier(supplier) {
                this.invoice.supplier = supplier
            },
            loadResources() {
                return httpClient.get(this.$urls.registerIncomingInvoiceLocalization)
            },
            onEndResourcesLoading() {
                this.loadSettings()
            },
            loadSettings() {
                let self = this

                httpClient.all([
                    httpClient.get(this.$urls.getAvailableVats),
                    httpClient.get(this.$urls.getIncomingDocumentTypes),
                    httpClient.get(this.$urls.getProvidenceFunds),
                    httpClient.get(this.$urls.getWithholdingTaxes),
                ]).then(([vats, documentTypes, providenceFunds, withholdingTaxes]) => {
                    self.vatList = vats || []
                    self.incomingDocumentTypeList = documentTypes || []
                    self.providenceFunds = providenceFunds || []
                    self.withholdingTaxes = withholdingTaxes || []

                    self.isLoading = false
                })
            },
            setInvoiceProvidenceFund(providenceFund) {
                this.invoice.providenceFund = {
                    rate: providenceFund.rate,
                    description: providenceFund.description,
                    amount: (0.00).toFixed(2),
                    appliedInWithholdingTax: providenceFund.appliedInWithholdingTax
                }

                if (this.invoice.lineItems.length > 0) {
                    let totalTaxableAmount = this.invoice.lineItems
                        .map((l) => l.totalPrice)
                        .reduce((firstPrice, secondPrice) => firstPrice + secondPrice) || 0.00

                    this.invoice.providenceFund.amount = totalTaxableAmount.toFixed(2)
                }
            },
            setInvoiceWithholdingTax(withholdingTax) {
                this.invoice.withholdingTax = {
                    description: withholdingTax.description,
                    rate: withholdingTax.rate,
                    taxableAmountRate: withholdingTax.taxableAmountRate,
                    amount: (0.00).toFixed(2)
                }
            },
            changeProvidenceFundValues(evt) {
                let providenceFundDescription = evt.target.value
                let providenceFund = this.providenceFunds.filter(p => p.description === providenceFundDescription)[0] || null

                if (providenceFund) {
                    this.setInvoiceProvidenceFund(providenceFund)
                }
                else {
                    this.invoice.providenceFund = {}
                }

                this.refreshPricesByVat()
            },
            changeWithholdingTaxValues(evt) {
                let withholdingTaxDescription = evt.target.value
                let withholdingTax = this.withholdingTaxes.filter(w => w.description === withholdingTaxDescription)[0] || null

                if (withholdingTax) {
                    this.setInvoiceWithholdingTax(withholdingTax)
                }
                else {
                    this.invoice.withholdingTax = {}
                }

                this.refreshPricesByVat()
            },
            addNewInvoiceItem() {
                console.log(this.invoice.lineItems)
                this.invoice.lineItems.push({
                    code: '',
                    description: '',
                    quantity: 0,
                    unitPrice: 0,
                    totalPrice: 0,
                    vat: 0
                })
            },
            removeInvoiceLineItem(index) {
                this.invoice.lineItems.splice(index, 1)
                this.calculateInvoiceTotals()
            },
            removeNonTaxableItem(index) {
                this.invoice.nonTaxableItems.splice(index, 1)
            },
            addNonTaxableItem() {
                this.invoice.nonTaxableItems.push({
                    description: '',
                    amount: 0.00
                })
            },
            calculateProvidenceFundForAmount(amount) {
                if (!this.invoice.providenceFund || !this.invoice.providenceFund.rate) {
                    return 0.00
                }

                return amount * (this.invoice.providenceFund.rate / 100.00)
            },
            refreshPricesByVat() {
                let uniqueVatRates = this.invoice.lineItems
                    .map(i => i.vat)
                    .filter((item, index, self) => self.indexOf(item) === index)

                let vatIncluded = this.invoice.vatIncluded
                let invoiceLineItems = this.invoice.lineItems
                this.invoice.pricesByVat = uniqueVatRates.map((v) => {
                    let itemsWithVat = invoiceLineItems.filter((i) => i.vat === v)

                    let taxableAmount = itemsWithVat.map((i) => {
                        if (vatIncluded) {
                            let vat = i.vat.rate / 100.00
                            let amount = i.totalPrice / (1 + vat)

                            return amount
                        }

                        let providenceFundAmount = this.calculateProvidenceFundForAmount(i.totalPrice)
                        return i.totalPrice + providenceFundAmount
                    }).reduce((amount, secondAmount) => amount + secondAmount) || 0.00

                    let providenceFundAmount = itemsWithVat.map((i) => {
                        let amount = i.totalPrice
                        if (vatIncluded) {
                            let vat = i.vat.rate / 100.00
                            amount = i.totalPrice / (1 + vat)
                        }

                        return this.calculateProvidenceFundForAmount(amount)
                    }).reduce((amount, secondAmount) => amount + secondAmount) || 0.00

                    let vatAmount = itemsWithVat.map((i) => {
                        let vat = 0
                        if (i.vat && i.vat.rate) {
                            vat = i.vat.rate / 100.00
                        }

                        if (vatIncluded) {
                            let total = i.totalPrice / (1 + vat)

                            return total * vat
                        }

                        let providenceFundAmount = this.calculateProvidenceFundForAmount(i.totalPrice)
                        return (i.totalPrice + providenceFundAmount) * vat
                    }).reduce((tax, secondTax) => tax + secondTax)

                    let totalPrice = itemsWithVat.map((i) => {
                        if (vatIncluded) {
                            return i.totalPrice
                        }

                        let providenceFundAmount = this.calculateProvidenceFundForAmount(i.totalPrice)
                        let taxableAmount = i.totalPrice + providenceFundAmount

                        let vat = taxableAmount * (i.vat.rate / 100.00)
                        return taxableAmount + vat
                    }).reduce((price, secondPrice) => price + secondPrice) || 0.00

                    return {
                        taxableAmount: parseFloat(taxableAmount.toFixed(2)),
                        providenceFundAmount: parseFloat(providenceFundAmount.toFixed(2)),
                        vatRate: v.rate,
                        vatAmount: parseFloat(vatAmount.toFixed(2)),
                        totalPrice: parseFloat(totalPrice.toFixed(2))
                    }
                })

                this.calculateInvoiceTotals()
            },
            calculateInvoiceTotals() {
                this.invoice.totalPrice = this.calculateInvoiceTotalPrice(this.invoice.pricesByVat, this.invoice.nonTaxableItems).toFixed(2)
                this.invoice.taxes = this.calculateInvoiceTotalTaxes(this.invoice.pricesByVat).toFixed(2)
                this.invoice.amount = this.calculateTotalTaxableAmount(this.invoice.pricesByVat).toFixed(2)

                if (this.invoice.providenceFund) {
                    this.invoice.providenceFund.amount = this.calculateTotalProvidenceFundAmount(this.invoice.pricesByVat).toFixed(2)
                }

                if (this.invoice.withholdingTax) {
                    this.invoice.withholdingTax.amount = this.calculateWithholdingTaxAmount(this.invoice.amount).toFixed(2)
                }

                this.invoice.totalToPay = this.calculateTotalToPay().toFixed(2)
            },
            calculateInvoiceTotalPrice(pricesByVat, nonTaxableItems) {
                let vatTotalPrice = 0.00
                let nonTaxablePrice = 0.00

                vatTotalPrice = pricesByVat
                    .map((p) => p.totalPrice)
                    .reduce((price, secondPrice) => price + secondPrice)

                if (nonTaxableItems.length > 0) {
                    nonTaxablePrice = nonTaxableItems
                        .map((t) => parseFloat(t.amount))
                        .reduce((price, secondPrice) => price + secondPrice)
                }

                return vatTotalPrice + nonTaxablePrice
            },
            calculateInvoiceTotalTaxes(pricesByVat) {
                return pricesByVat
                    .map((p) => p.vatAmount)
                    .reduce((tax, secondTax) => tax + secondTax)
            },
            calculateTotalTaxableAmount(pricesByVat) {
                return pricesByVat
                    .map((p) => p.taxableAmount)
                    .reduce((amount, secondAmount) => amount + secondAmount)
            },
            calculateTotalProvidenceFundAmount(pricesByVat) {
                let providenceFundAmount = pricesByVat
                    .map((p) => p.providenceFundAmount)
                    .reduce((amount, secondAmount) => amount + secondAmount)

                return providenceFundAmount || 0.00
            },
            calculateWithholdingTaxAmount(taxableAmount) {
                if (this.invoice.providenceFund && !this.invoice.providenceFund.appliedInWithholdingTax) {
                    taxableAmount -= this.invoice.providenceFund.amount
                }

                let withholdingTax = this.invoice.withholdingTax

                let withholdingTaxableAmount = taxableAmount * (withholdingTax.taxableAmountRate / 100.00)
                let withholdingTaxAmount = withholdingTaxableAmount * (withholdingTax.rate / 100.00)

                return withholdingTaxAmount || 0.00
            },
            calculateTotalToPay() {
                let withholdingTaxAmount = 0.00
                if (this.invoice.withholdingTax) {
                    withholdingTaxAmount = this.invoice.withholdingTax.amount
                }

                return (this.invoice.totalPrice - withholdingTaxAmount) || 0.00
            }
        }
    }
</script>