<template>
    <form role="form" class="col-md-12" v-on:submit.prevent="save">
        <b-card>
            <div class="form-row" v-if="!disableDocumentTypes">
                <div class="col-md-12">
                    <div class="row form-group">
                        <label class="col-form-label col">{{ labels.chooseDocumentType }}</label>
                        <div class="col">
                            <b-form-radio-group buttons
                                                v-bind:options="outgoingDocumentTypes"
                                                button-variant="outline-primary"
                                                v-model="invoiceModel.type"
                                                class="float-right"
                                                v-on:change="changeInvoiceType">
                            </b-form-radio-group>
                        </div>
                    </div>
                </div>
            </div>
            <div class="form-row">
                <div class="col-md-6 form-group">
                    <label for="customer">{{ labels.customer }}</label>
                    <party-info-autocomplete v-bind:party="invoiceModel.customer" v-bind:disabled="false" v-bind:validate="validation.customer" name="customer" v-bind:field-label="labels.customer" v-on:party-selected="selectCustomer"></party-info-autocomplete>
                </div>
                <div class="col-md-3 form-group">
                    <label for="date">{{ labels.date }}</label>
                    <datepicker v-bind:show-calendar-button="true"
                                name="date"
                                v-model="invoiceModel.date"
                                v-validate="validation.date"
                                v-bind:data-vv-as="labels.date">
                    </datepicker>
                    <span class="text-danger" v-if="errors.first('date')">{{ errors.first('date') }}</span>
                </div>
                <div class="col-md-3 form-group">
                    <label for="currency">{{ labels.currency }}</label>
                    <input type="text" class="form-control" name="currency" v-model="invoiceModel.currency" v-validate="validation.currency" v-bind:data-vv-as="labels.currency" />
                    <span class="text-danger" v-if="errors.first('currency')">{{ errors.first('currency') }}</span>
                </div>
            </div>
            <div class="form-row">
                <div class="col-md-6 form-group">
                    <label for="purchaseOrderNumber">{{ labels.purchaseOrderNumber }}</label>
                    <input type="text" name="purchaseOrderNumber" class="form-control" v-model="invoiceModel.purchaseOrderNumber" />
                </div>
                <div class="col-md-6 form-group">
                    <label for="paymentTerms">{{ labels.paymentTerms }}</label>
                    <input type="text" name="paymentTerms" class="form-control" v-model="invoiceModel.paymentTerms" />
                </div>
            </div>
            <div class="form-row">
                <div class="col-md-12 form-group">
                    <label for="description">{{ labels.description }}</label>
                    <input type="text" name="description" class="form-control" v-model="invoiceModel.description" v-validate="validation.description" v-bind:data-vv-as="labels.description" />
                    <span class="text-danger" v-if="errors.first('description')">{{ errors.first('description') }}</span>
                </div>
            </div>
            <div class="form-row">
                <div class="col-md-12 form-group">
                    <b-form-checkbox v-model="invoiceModel.vatIncluded" v-on:input="refreshPricesByVat">
                        {{ labels.vatIncluded }}
                    </b-form-checkbox>
                </div>
            </div>
            <fieldset>
                <legend>{{ labels.invoiceLines }}</legend>

                <div class="form-row">
                    <div class="col-md-1">
                        <label class="font-weight-bold">{{ labels.lineItemCode }}</label>
                    </div>
                    <div class="col-md-3">
                        <label class="font-weight-bold">{{ labels.description }}</label>
                    </div>
                    <div class="col-md-1">
                        <label class="font-weight-bold">{{ labels.lineItemQuantity }}</label>
                    </div>
                    <div class="col-md-2">
                        <label class="font-weight-bold">{{ labels.lineItemUnitPrice }}</label>
                    </div>
                    <div class="col-md-2">
                        <label class="font-weight-bold">{{ labels.lineItemTotalPrice }}</label>
                    </div>
                    <div class="col-md-3">
                        <label class="font-weight-bold">{{ labels.lineItemVat }}</label>
                    </div>
                </div>
                <invoice-line-item v-for="(item, index) in invoiceModel.lineItems" v-bind:key="index" v-bind:line-item.sync="item" v-bind:validation="validation" v-bind:index="index" v-bind:vat-list="vatList" v-on:remove-item="removeInvoiceLineItem(index)" v-on:refresh-totals="refreshPricesByVat"></invoice-line-item>
                <div class="form-row">
                    <div class="col-md-12 form-group">
                        <button type="button" class="btn btn-outline-primary" v-on:click="addNewInvoiceItem">
                            <i class="fa fa-plus"></i> {{ labels.addNew }}
                        </button>
                    </div>
                </div>
            </fieldset>
            <div class="row">
                <div class="col-md-6">
                    <fieldset>
                        <legend>{{ labels.pricesByVat }}</legend>
                        <div class="form-row">
                            <div class="col-md-3">
                                <label class="font-weight-bold">{{ labels.amount }}</label>
                            </div>
                            <div class="col-md-3 offset-md-2">
                                <label class="font-weight-bold">{{ labels.vat }}</label>
                            </div>
                            <div class="col-md-4">
                                <label class="font-weight-bold">{{ labels.totalPrice }}</label>
                            </div>
                        </div>
                        <div class="form-row form-group" v-for="(item, index) in invoiceModel.pricesByVat" v-bind:key="index">
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
                        <legend>{{ labels.nonTaxableItems }}</legend>
                        <div class="form-row">
                            <div class="col-md-8">
                                <label class="font-weight-bold">{{ labels.description }}</label>
                            </div>
                            <div class="col-md-4">
                                <label class="font-weight-bold">{{ labels.amount }}</label>
                            </div>
                        </div>
                        <div class="form-row form-group" v-for="(item, index) in invoiceModel.nonTaxableItems" v-bind:key="index">
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
                                    <i class="fa fa-plus"></i> {{ labels.addNew }}
                                </button>
                            </div>
                        </div>
                    </fieldset>
                </div>
            </div>
            <fieldset>
                <legend>{{ labels.invoiceTotals }}</legend>
                <div class="form-row">
                    <div class="col-md-12">
                        <div class="form-group row">
                            <label class="col-md-2 col-form-label font-weight-bold">{{ labels.amount }}</label>
                            <div class="col-md-10">
                                <p class="form-control-plaintext">{{ invoiceModel.currency }} {{ invoiceModel.amount | currencyFormat(2) }}</p>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="form-row">
                    <div class="col-md-12">
                        <div class="form-group row">
                            <label class="col-md-2 col-form-label font-weight-bold">{{ labels.taxes }}</label>
                            <div class="col-md-10">
                                <p class="form-control-plaintext">{{ invoiceModel.currency }} {{ invoiceModel.taxes | currencyFormat(2) }}</p>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="form-row" v-if="invoiceModel.nonTaxableItems.length">
                    <div class="col-md-12" v-for="(item, index) in invoiceModel.nonTaxableItems" v-bind:key="index">
                        <div class="form-group row">
                            <label class="col-md-2 col-form-label font-weight-bold">{{ item.description }}</label>
                            <div class="col-md-10">
                                <p class="form-control-plaintext">{{ invoiceModel.currency }} {{ item.amount | currencyFormat(2) }}</p>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="form-row">
                    <div class="col-md-12">
                        <div class="form-group row">
                            <label class="col-md-2 col-form-label font-weight-bold">{{ labels.totalPrice }}</label>
                            <div class="col-md-10">
                                <p class="form-control-plaintext">{{ invoiceModel.currency }} {{ invoiceModel.totalPrice | currencyFormat(2) }}</p>
                            </div>
                        </div>
                    </div>
                </div>
            </fieldset>
            <div slot="footer">
                <div class="btn-toolbar float-right">
                    <div class="btn-group mr-2" role="group">
                        <button type="button" class="btn btn-outline-primary" v-on:click="saveDraft">{{ labels.saveAsDraft }}</button>
                    </div>
                    <div class="btn-group" role="group">
                        <button type="submit" class="btn btn-primary" v-bind:disabled="!isValid">{{ labels.save }}</button>
                        <button type="button" v-on:click="cancel" class="btn btn-outline-secondary">{{ labels.cancel }}</button>
                    </div>
                </div>
            </div>
        </b-card>
    </form>
</template>
<script>
    import _ from 'lodash'
    import PartyInfoAutocomplete from '@/app/registry/shared/party/partyInfoAutocomplete.vue'
    import InvoiceLineItem from './invoiceLineItem.vue'
    import EndpointConfigurationMixin from '@/app/mixins/endpointConfigurationMixin'
    import Datepicker from '@/app/shared/widgets/datepicker.vue'

    import { httpClient } from '@/app/services/httpClient'

    export default {
        name: 'outgoingInvoiceForm',
        mixins: [EndpointConfigurationMixin],
        components: {
            'party-info-autocomplete': PartyInfoAutocomplete,
            'invoice-line-item': InvoiceLineItem,
            'datepicker': Datepicker
        },
        urls: {
            getOutgoingDocumentTypes: window.endpoints.accountancy.getOutgoingDocumentTypes
        },
        props: ['invoice', 'labels', 'validation', 'vatList', 'disableDocumentTypes'],
        data() {
            return {
                invoiceModel: JSON.parse(JSON.stringify(this.invoice)),
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
                            text: this.labels[labelKey]
                        }
                    })
            }
        },
        methods: {
            selectCustomer(customer) {
                this.invoiceModel.customer = customer
            },
            cancel() {
                this.$validator.reset()
                this.invoiceModel = JSON.parse(JSON.stringify(this.invoice))
            },
            save() {
                this.$emit('save', this.invoiceModel)
            },
            saveDraft() {
                this.$emit('save-draft', this.invoiceModel)
            },
            addNewInvoiceItem() {
                this.invoiceModel.lineItems.push({
                    code: '',
                    description: '',
                    quantity: 0,
                    unitPrice: 0,
                    totalPrice: 0,
                    vat: 0
                })
            },
            removeInvoiceLineItem(index) {
                this.invoiceModel.lineItems.splice(index, 1)
                this.refreshPricesByVat()
            },
            removeNonTaxableItem(index) {
                this.invoiceModel.nonTaxableItems.splice(index, 1)
            },
            addNonTaxableItem() {
                this.invoiceModel.nonTaxableItems.push({
                    description: '',
                    amount: 0.00
                })
            },
            refreshPricesByVat() {
                let uniqueVatRates = this.invoiceModel.lineItems
                    .map(i => i.vat)
                    .filter((item, index, self) => self.indexOf(item) === index)

                let vatIncluded = this.invoiceModel.vatIncluded
                let invoiceLineItems = this.invoiceModel.lineItems
                this.invoiceModel.pricesByVat = uniqueVatRates.map((v) => {
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
                this.invoiceModel.totalPrice = this.calculateInvoiceTotalPrice(this.invoiceModel.pricesByVat, this.invoiceModel.nonTaxableItems).toFixed(2)
                this.invoiceModel.taxes = this.calculateInvoiceTotalTaxes(this.invoiceModel.pricesByVat).toFixed(2)
                this.invoiceModel.amount = this.calculateTotalTaxableAmount(this.invoiceModel.pricesByVat).toFixed(2)
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
            },
            changeInvoiceType(type) {
                this.$emit('type-changed', type)
            }
        },
        watch: {
            invoice() {
                this.invoiceModel = JSON.parse(JSON.stringify(this.invoice))
            }
        },
        mounted() {
            this.loadOutgoingDocumentTypes()
        }
    }
</script>