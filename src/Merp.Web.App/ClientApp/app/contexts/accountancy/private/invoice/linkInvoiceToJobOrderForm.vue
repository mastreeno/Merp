<template>
    <div>
        <vue-element-loading v-bind:active="saving"></vue-element-loading>
        <div v-if="errors.length">
            <div class="col-md-12">
                <b-alert show variant="danger" dismissible>
                    <ul>
                        <li v-for="error in serverErrors">{{ error }}</li>
                    </ul>
                </b-alert>
            </div>
        </div>
        <div class="form-row">
            <div class="col-md-12">
                <div class="form-group row">
                    <label class="col-md-5 col-form-label font-weight-bold">{{ labels.invoiceNumber }}</label>
                    <div class="col-md-7">
                        <p class="form-control-plaintext">{{ invoice.number }}</p>
                    </div>
                </div>
            </div>
        </div>
        <div class="form-row">
            <div class="col-md-12">
                <div class="form-group row">
                    <label class="col-md-5 col-form-label font-weight-bold">{{ labels.amount }}</label>
                    <div class="col-md-7">
                        <p class="form-control-plaintext">{{ invoice.amount | currencyFormat(2) }}</p>
                    </div>
                </div>
            </div>
        </div>
        <div class="form-row">
            <div class="col-md-12">
                <div class="form-group row">
                    <label class="col-md-5 col-form-label font-weight-bold" for="dateOfLink">{{ labels.dateOfLink }}</label>
                    <div class="col-md-7">
                        <datepicker v-bind:show-calendar-button="true"
                                    name="dateOfLink"
                                    v-model="dateOfLink"
                                    v-validate="validationRules.dateOfLink" 
                                    v-bind:data-vv-as="labels.dateOfLink">
                        </datepicker>
                        <span class="text-danger" v-if="errors.first('dateOfLink')">{{ errors.first('dateOfLink') }}</span>
                    </div>
                </div>
            </div>
        </div>
        <div class="form-row">
            <div class="col-md-12">
                <div class="form-group row">
                    <label class="col-md-5 col-form-label font-weight-bold" for="jobOrderNumber">{{ labels.jobOrderNumber }}</label>
                    <div class="col-md-7">
                        <input type="text" name="jobOrderNumber" class="form-control" v-model="jobOrderNumber" v-validate="validationRules.jobOrderNumber" v-bind:data-vv-as="labels.jobOrderNumber" />
                        <span class="text-danger" v-if="errors.first('jobOrderNumber')">{{ errors.first('jobOrderNumber') }}</span>
                    </div>
                </div>
            </div>
        </div>
        <hr />
        <div class="form-row">
            <div class="col-md-12">
                <div class="btn-group float-right" role="group">
                    <button type="button" class="btn btn-primary" v-on:click="linkInvoiceToJobOrder" v-bind:disabled="!isValid">{{ labels.save }}</button>
                    <button type="button" class="btn btn-outline-secondary" v-on:click="cancel">{{ labels.cancel }}</button>
                </div>
            </div>
        </div>
    </div>
</template>
<script>
    import Datepicker from '@/app/shared/components/datepicker.vue'
    import { linkInvoiceToAJobOrderValidationRules } from './validation-rules'

    export default {
        name: 'linkInvoiceToJobOrderForm',
        components: {
            'datepicker': Datepicker
        },
        props: ['labels', 'invoice', 'saving', 'serverErrors'],
        data() {
            return {
                dateOfLink: new Date(),
                jobOrderNumber: null,
                validationRules: linkInvoiceToAJobOrderValidationRules
            }
        },
        computed: {
            isValid() {
                return this.errors.items.length === 0
            }
        },
        methods: {
            linkInvoiceToJobOrder(evt) {
                evt.preventDefault()
                this.$emit('link-invoice-to-joborder', {
                    dateOfLink: this.dateOfLink,
                    jobOrderNumber: this.jobOrderNumber,
                    amount: this.invoice.amount,
                    type: this.invoice.type
                })
            },
            cancel(evt) {
                evt.preventDefault()
                this.$emit('close-linktojoborder-modal')
            }
        }
    }
</script>