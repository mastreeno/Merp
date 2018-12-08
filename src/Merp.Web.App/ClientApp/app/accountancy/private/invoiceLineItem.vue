<template>
    <div class="form-row form-group">
        <div class="col-md-1">
            <input type="text" class="form-control" v-model="lineItem.code" />
        </div>
        <div class="col-md-3">
            <input type="text" class="form-control" v-bind:class="{'is-invalid': errors.has(lineItemDescriptionInputName)}" v-bind:name="lineItemDescriptionInputName" v-model="lineItem.description" key="lineItemDescription" v-validate="validation.lineItemDescription" />
        </div>
        <div class="col-md-1">
            <input type="number" class="form-control" min="0" v-model="lineItem.quantity" v-on:change="fixQuantity" />
        </div>
        <div class="col-md-2">
            <input type="number" step="0.01" min="0" class="form-control" v-model="lineItem.unitPrice" v-bind:disabled="isUnitPriceDisabled" v-on:change="fixUnitPrice" />
        </div>
        <div class="col-md-2">
            <input type="number" v-bind:name="lineItemTotalPriceInputName" step="0.01" class="form-control" v-bind:class="{'is-invalid': errors.has(lineItemTotalPriceInputName)}" v-model="lineItem.totalPrice" v-on:change="fixTotalPrice" v-validate="validation.lineItemTotalPrice" />
        </div>
        <div class="col-md-2">
            <select class="form-control" v-model="lineItem.vat" v-on:change="fixVat">
                <option value="0"></option>
                <option v-for="vat in vatList" v-bind:key="vat.rate" v-bind:value="vat.rate">{{ vat.description }}</option>
            </select>
        </div>
        <div class="col-md-1">
            <button type="button" class="btn btn-outline-secondary" v-on:click="removeItem">
                <i class="fa fa-remove" aria-label="Remove item"></i>
            </button>
        </div>
    </div>
</template>
<script>
    export default {
        name: 'invoiceLineItem',
        props: ['lineItem', 'validation', 'index', 'vatList'],
        computed: {
            isUnitPriceDisabled() {
                return this.lineItem.quantity === 0
            },
            lineItemDescriptionInputName() {
                return 'lineItemDescription' + this.index
            },
            lineItemTotalPriceInputName() {
                return 'lineItemTotalPrice' + this.index
            }
        },
        methods: {
            fixUnitPrice() {
                if (isNaN(this.lineItem.unitPrice) || this.lineItem.unitPrice === "" || this.lineItem.unitPrice === null) {
                    return
                }

                let unitPrice = parseFloat(this.lineItem.unitPrice)

                this.lineItem.unitPrice = parseFloat(unitPrice.toFixed(2))
                this.calculateTotalPrice()
            },
            fixQuantity() {
                if (isNaN(this.lineItem.quantity) || this.lineItem.quantity === "" || this.lineItem.quantity === null) {
                    return
                }

                this.lineItem.quantity = parseInt(this.lineItem.quantity, 10)
                this.calculateTotalPrice()
            },
            fixTotalPrice() {
                if (isNaN(this.lineItem.totalPrice) || this.lineItem.totalPrice === "" || this.lineItem.totalPrice === null) {
                    return
                }

                let totalPrice = parseFloat(this.lineItem.totalPrice)
                this.lineItem.totalPrice = parseFloat(totalPrice.toFixed(2))

                this.$emit('refresh-totals')
            },
            calculateTotalPrice() {
                let totalPrice = parseFloat(this.lineItem.quantity * this.lineItem.unitPrice)
                this.lineItem.totalPrice = parseFloat(totalPrice.toFixed(2))

                this.$emit('refresh-totals')
            },
            fixVat() {
                if (isNaN(this.lineItem.vat) || this.lineItem.vat === "" || this.lineItem.vat === null) {
                    this.lineItem.vat = 0
                }

                let vat = parseFloat(this.lineItem.vat)
                this.lineItem.vat = parseFloat(vat.toFixed(2))

                this.$emit('refresh-totals')
            },
            removeItem() {
                this.$emit('remove-item')
            },
            refreshPricesAsVatIncluded() {
                this.$emit('refresh-totals')
            }
        }
    }
</script>