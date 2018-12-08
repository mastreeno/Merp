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
            <form role="form" class="col-md-12" v-on:submit.prevent="register">
                <b-card>
                    <div class="form-row">
                        <div class="col-md-12">
                            <div class="row form-group">
                                <label class="col-form-label col">{{ uiTexts.chooseDocumentType }}</label>
                                <div class="col">
                                    <b-form-radio-group buttons
                                                        v-bind:options="outgoingDocumentTypes"
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
                            <label for="customer">{{ uiTexts.customer }}</label>
                            <party-info-autocomplete v-bind:party="invoice.customer" v-bind:disabled="false" v-bind:validate="validationRules.customer" name="customer" v-bind:field-label="uiTexts.customer" v-on:party-selected="selectCustomer"></party-info-autocomplete>
                        </div>
                        <div class="col-md-6 form-group">
                            <label for="invoiceNumber">{{ uiTexts.invoiceNumber }}</label>
                            <input type="text" class="form-control" name="invoiceNumber" v-model="invoice.number" v-validate="validationRules.invoiceNumber" v-bind:data-vv-as="uiTexts.invoiceNumber" />
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
                    <div class="row">
                        <div class="col-md-6">
                            <fieldset>
                                <legend>{{ uiTexts.pricesByVat }}</legend>
                                <div class="form-row">
                                    <div class="col-md-3">
                                        <label class="font-weight-bold">{{ uiTexts.amount }}</label>
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
                        <legend>{{ uiTexts.invoiceTotals }}</legend>
                        <div class="form-row">
                            <div class="col-md-12">
                                <div class="form-group row">
                                    <label class="col-md-2 col-form-label font-weight-bold">{{ uiTexts.amount }}</label>
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
    import PartyInfoAutocomplete from '@/app/registry/shared/party/partyInfoAutocomplete.vue'
    import InvoiceLineItem from '@/app/accountancy/private/invoiceLineItem.vue'
    import { registerOutgoingInvoiceValidationRules } from './validation-rules'
    import EndpointConfigurationMixin from '@/app/mixins/endpointConfigurationMixin'
    import Datepicker from '@/app/shared/widgets/datepicker.vue'

    import { httpClient } from '@/app/services/httpClient'
    import errorHelper from '@/app/services/errorHelper'

    export default {
        name: 'registerOutgoingInvoice',
        mixins: [EndpointConfigurationMixin],
        components: {
            'party-info-autocomplete': PartyInfoAutocomplete,
            'invoice-line-item': InvoiceLineItem,
            'datepicker': Datepicker
        },
        urls: {
            registerOutgoingInvoiceLocalization: window.endpoints.accountancy.registerOutgoingInvoiceLocalization,
            registerOutgoingInvoice: window.endpoints.accountancy.registerOutgoingInvoice,
            getInvoiceVatList: window.endpoints.accountancy.getInvoiceVatList,
            getOutgoingDocumentTypes: window.endpoints.accountancy.getOutgoingDocumentTypes
        },
        data() {
            return {
                isLoading: true,
                isSaving: false,
                validationRules: registerOutgoingInvoiceValidationRules,
                invoice: {
                    type: _.upperFirst(this.$route.params.type) || 'OutgoingInvoice',
                    customer: {},
                    number: null,
                    date: new Date(),
                    dueDate: null,
                    currency: '',
                    amount: 0.00,
                    taxes: 0.00,
                    totalPrice: 0.00,
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
                        vat: 0
                    }],
                    pricesByVat: [{}],
                    nonTaxableItems: []
                },
                invoiceCopy: {
                    type: 'OutgoingInvoice',
                    customer: {},
                    number: null,
                    date: new Date(),
                    dueDate: null,
                    currency: '',
                    amount: 0.00,
                    taxes: 0.00,
                    totalPrice: 0.00,
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
                        vat: 0
                    }],
                    pricesByVat: [{}],
                    nonTaxableItems: []
                },
                serverErrors: [],
                vatList: [],
                outgoingDocumentTypeList: []
            }
        },
        computed: {
            isValid() {
                return this.errors.items.length === 0
            },
            outgoingDocumentTypes() {
                return this.outgoingDocumentTypeList
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
                    case 'OutgoingCreditNote':
                        return this.uiTexts.registerCreditNotePageTitle
                    case 'OutgoingInvoice':
                    default:
                        return this.uiTexts.registerInvoicePageTitle
                }
            },
            showDueDate() {
                return !(this.invoice.type === 'OutgoingCreditNote')
            }
        },
        methods: {
            transformViewModelToRegisterOutgoingInvoice(viewModel) {
                return {
                    type: viewModel.type,
                    customer: viewModel.customer,
                    number: viewModel.number,
                    date: viewModel.date,
                    dueDate: viewModel.dueDate,
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
            selectCustomer(customer) {
                this.invoice.customer = customer
            },
            register() {
                this.isSaving = true
                let self = this

                let registerOutgoingInvoiceJson = this.transformViewModelToRegisterOutgoingInvoice(this.invoice)
                httpClient.post(this.$urls.registerOutgoingInvoice, registerOutgoingInvoiceJson)
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
            loadResources() {
                return httpClient.get(this.$urls.registerOutgoingInvoiceLocalization)
            },
            onEndResourcesLoading() {
                this.loadOutgoingDocumentTypes()
                this.loadVatList()
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
                        self.isLoading = false
                    })
            },
            addNewInvoiceItem() {
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
                            let vat = i.vat / 100.00
                            let amount = i.totalPrice / (1 + vat)

                            return amount
                        }

                        return i.totalPrice
                    }).reduce((amount, secondAmount) => amount + secondAmount)

                    let vatAmount = itemsWithVat.map((i) => {
                        let vat = i.vat / 100.00

                        if (vatIncluded) {
                            let total = i.totalPrice / (1 + vat)

                            return total * vat
                        }

                        return i.totalPrice * vat
                    }).reduce((tax, secondTax) => tax + secondTax)

                    let totalPrice = itemsWithVat.map((i) => {
                        if (vatIncluded) {
                            return i.totalPrice
                        }

                        let vat = i.totalPrice * (i.vat / 100.00)
                        return i.totalPrice + vat
                    }).reduce((price, secondPrice) => price + secondPrice)

                    return {
                        taxableAmount: parseFloat(taxableAmount.toFixed(2)),
                        vatRate: v,
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
            loadOutgoingDocumentTypes() {
                let self = this
                httpClient.get(this.$urls.getOutgoingDocumentTypes)
                    .then((response) => {
                        self.outgoingDocumentTypeList = response || []
                    })
            }
        }
    }
</script>