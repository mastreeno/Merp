<template>
    <div class="col-md-12">
        <div class="form-group row">
            <label for="companyName" class="col-md-3 col-form-label font-weight-bold">{{ labels.companyName }}</label>
            <div class="col-md-3">
                <input v-bind:class="{ 'form-control-plaintext': !isEnabled, 'form-control': isEnabled }" v-bind:disabled="!isEnabled" name="companyName" v-model="newCompanyName" v-validate="validationRules.companyName" v-bind:data-vv-as="labels.companyName" />
                <span class="text-danger" v-if="errors.first('companyName')">{{ errors.first('companyName') }}</span>
            </div>
            <label for="effectiveDate" class="col-md-2 col-form-label font-weight-bold" v-if="isEnabled">{{ labels.effectiveDate }}</label>
            <div class="col-md-3" v-if="isEnabled">
                <datepicker v-bind:show-calendar-button="true"
                            name="effectiveDate" 
                            v-model="effectiveDate"
                            v-bind:input-class="{ 'form-control-plaintext': !isEnabled, 'form-control': isEnabled }"
                            v-bind:disabled="!isEnabled"></datepicker>
            </div>
            <div class="col-md-2">
                <button class="btn btn-outline-primary" v-if="!isEnabled" v-on:click="enableChanges">
                    <i class="fa fa-edit"></i>
                </button>
                <div class="btn-group" v-if="isEnabled">
                    <button class="btn btn-primary" v-on:click="save">
                        <i class="fa fa-save"></i> {{ labels.save }}
                    </button>
                    <button class="btn btn-outline-secondary" v-on:click="cancel">{{ labels.cancel }}</button>
                </div>
            </div>
        </div>
    </div>
</template>
<script>
    import Datepicker from '@/app/shared/components/datepicker.vue'
    import { changeCompanyNameValidationRules } from './validation-rules'

    export default {
        name: 'changeCompanyNameForm',
        components: {
            'datepicker': Datepicker
        },
        props: ['companyName', 'labels'],
        data() {
            return {
                isEnabled: false,
                effectiveDate: new Date(),
                validationRules: changeCompanyNameValidationRules,
                newCompanyName: this.companyName
            }
        },
        methods: {
            enableChanges() {
                this.isEnabled = true
            },
            cancel() {
                this.$validator.reset()
                this.newCompanyName = this.companyName
                this.isEnabled = false
            },
            save() {
                this.$emit('change-company-name', {
                    newCompanyName: this.newCompanyName,
                    effectiveDate: this.effectiveDate
                })

                this.isEnabled = false
            }
        },
        watch: {
            companyName() {
                this.newCompanyName = this.companyName
            }
        }
    }
</script>