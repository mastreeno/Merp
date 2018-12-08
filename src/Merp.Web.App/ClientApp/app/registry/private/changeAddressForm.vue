<template>
    <div>
        <div class="row">
            <div class="col-md-6">
                <h5>{{ title }}</h5>
            </div>
            <div class="col-md-6">
                <button class="btn btn-outline-primary" v-on:click="enableChanges" v-if="!isEnabled">
                    <i class="fa fa-edit"></i> {{ labels.change }} {{ title }}
                </button>
                <div class="btn-group" role="group" v-if="isEnabled">
                    <button class="btn btn-primary" v-on:click="save"><i class="fa fa-save"></i> {{ labels.save }}</button>
                    <button class="btn btn-outline-secondary" v-on:click="cancel">{{ labels.cancel }}</button>
                </div>
            </div>
        </div>
        <br />
        <div class="form-row">
            <div class="col-md-12">
                <div class="form-group row">
                    <label for="address" class="col-form-label col-md-3 font-weight-bold">{{ labels.address }}</label>
                    <div class="col-md-9">
                        <input v-bind:class="{ 'form-control-plaintext': !isEnabled, 'form-control': isEnabled }" v-bind:disabled="!isEnabled" name="address" v-model="addressModel.address" />
                    </div>
                </div>
            </div>
            
        </div>
        <div class="form-row">
            <div class="col-md-6">
                <div class="form-group row">
                    <label for="city" class="col-form-label col-md-2 font-weight-bold">{{ labels.city }}</label>
                    <div class="col-md-10">
                        <input v-bind:class="{ 'form-control-plaintext': !isEnabled, 'form-control': isEnabled }" v-bind:disabled="!isEnabled" name="city" v-model="addressModel.city" />
                    </div>
                </div>
            </div>
            <div class="col-md-6">
                <div class="form-group row">
                    <label for="postalCode" class="col-md-4 col-form-label font-weight-bold">{{ labels.postalCode }}</label>
                    <div class="col-md-8">
                        <input v-bind:class="{ 'form-control-plaintext': !isEnabled, 'form-control': isEnabled }" v-bind:disabled="!isEnabled" name="postalCode" v-model="addressModel.postalCode" />
                    </div>
                </div>
            </div>
        </div>
        <div class="form-row">
            <div class="col-md-6">
                <div class="form-group row">
                    <label for="province" class="col-md-4 col-form-label font-weight-bold">{{ labels.province }}</label>
                    <div class="col-md-8">
                        <input v-bind:class="{ 'form-control-plaintext': !isEnabled, 'form-control': isEnabled }" v-bind:disabled="!isEnabled" name="province" v-model="addressModel.province" />
                    </div>
                </div>
            </div>
            <div class="col-md-6">
                <div class="form-group row">
                    <label for="country" class="col-md-4 col-form-label font-weight-bold">{{ labels.country }}</label>
                    <div class="col-md-8">
                        <input v-bind:class="{ 'form-control-plaintext': !isEnabled, 'form-control': isEnabled }" v-bind:disabled="!isEnabled" name="country" v-model="addressModel.country" />
                    </div>
                </div>
            </div>
        </div>
        <div class="form-row" v-if="isEnabled">
            <div class="col-md-6">
                <div class="form-group row">
                    <label class="col-md-4 col-form-label font-weight-bold" for="effectiveDate">{{ labels.effectiveDate }}</label>
                    <div class="col-md-8">
                        <!--<input class="form-control" name="effectiveDate" type="date" v-model="effectiveDate" />-->
                        <datepicker v-bind:show-calendar-button="true"
                                    name="effectiveDate"
                                    v-model="effectiveDate">
                        </datepicker>
                    </div>
                </div>
            </div>
        </div>
    </div>
</template>
<script>
    import Datepicker from '@/app/shared/widgets/datepicker.vue'

    export default {
        name: 'changeAddressForm',
        components: {
            'datepicker': Datepicker
        },
        props: ['address', 'title', 'labels'],
        data() {
            return {
                isEnabled: false,
                addressModel: Object.assign({}, this.address),
                effectiveDate: new Date()
            }
        },
        methods: {
            enableChanges() {
                this.isEnabled = true
            },
            cancel() {
                this.addressModel = Object.assign({}, this.address)
                this.isEnabled = false
            },
            save() {
                this.$emit('change-address', { address: this.addressModel, effectiveDate: this.effectiveDate })
                this.isEnabled = false
            }
        },
        watch: {
            address() {
                this.addressModel = Object.assign({}, this.address)
            }
        }
    }
</script>