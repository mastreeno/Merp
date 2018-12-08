<template>
    <div>
        <div class="row">
            <div class="col-md-6">
                <h5>{{ labels.contactInfo }}</h5>
            </div>
            <div class="col-md-6">
                <button class="btn btn-outline-primary" v-on:click="enableChanges" v-if="!isEnabled">
                    <i class="fa fa-edit"></i> {{ labels.change }} {{ labels.contactInfo }}
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
                    <label for="phoneNumber" class="col-md-3 col-form-label font-weight-bold">{{ labels.phoneNumber }}</label>
                    <div class="col-md-9">
                        <input v-bind:class="{ 'form-control-plaintext': !isEnabled, 'form-control': isEnabled }" v-bind:disabled="!isEnabled" name="phoneNumber" v-model="contactInfoModel.phoneNumber" />
                    </div>
                </div>
            </div>
        </div>
        <div class="form-row">
            <div class="col-md-12">
                <div class="form-group row">
                    <label for="faxNumber" class="col-md-3 col-form-label font-weight-bold">{{ labels.faxNumber }}</label>
                    <div class="col-md-9">
                        <input v-bind:class="{ 'form-control-plaintext': !isEnabled, 'form-control': isEnabled }" v-bind:disabled="!isEnabled" name="faxNumber" v-model="contactInfoModel.faxNumber" />
                    </div>
                </div>
            </div>
        </div>
        <div class="form-row">
            <div class="col-md-12">
                <div class="form-group row">
                    <label for="websiteAddress" class="col-md-3 col-form-label font-weight-bold">{{ labels.websiteAddress }}</label>
                    <div class="col-md-9">
                        <input v-bind:class="{ 'form-control-plaintext': !isEnabled, 'form-control': isEnabled }" v-bind:disabled="!isEnabled" name="websiteAddress" v-model="contactInfoModel.websiteAddress" v-validate="validationRules.contactInfoWebsiteAddress" v-bind:data-vv-as="labels.websiteAddress" />
                        <span class="text-danger" v-if="errors.first('websiteAddress')">{{ errors.first('websiteAddress') }}</span>
                    </div>
                </div>
            </div>
        </div>
        <div class="form-row">
            <div class="col-md-12">
                <div class="form-group row">
                    <label for="emailAddress" class="col-form-label col-md-3 font-weight-bold">{{ labels.emailAddress }}</label>
                    <div class="col-md-9">
                        <input type="email" v-bind:class="{ 'form-control-plaintext': !isEnabled, 'form-control': isEnabled }" v-bind:disabled="!isEnabled" name="emailAddress" v-model="contactInfoModel.emailAddress" v-validate="validationRules.contactInfoEmailAddress" v-bind:data-vv-as="labels.emailAddress" />
                        <span class="text-danger" v-if="errors.first('emailAddress')">{{ errors.first('emailAddress') }}</span>
                    </div>
                </div>
            </div>
        </div>
    </div>
</template>
<script>
    import { changeCompanyContactInfoValidationRules } from './validation-rules'

    export default {
        name: 'changeCompanyContactInfoForm',
        props: ['contactInfo', 'labels'],
        data() {
            return {
                isEnabled: false,
                contactInfoModel: Object.assign({}, this.contactInfo),
                validationRules: changeCompanyContactInfoValidationRules
            }
        },
        methods: {
            enableChanges() {
                this.isEnabled = true
            },
            cancel() {
                this.$validator.reset()
                this.contactInfoModel = Object.assign({}, this.contactInfo)
                this.isEnabled = false
            },
            save() {
                this.$emit('change-contact-info', this.contactInfoModel)
                this.isEnabled = false
            }
        },
        watch: {
            contactInfo() {
                this.contactInfoModel = Object.assign({}, this.contactInfo)
            }
        }
    }
</script>