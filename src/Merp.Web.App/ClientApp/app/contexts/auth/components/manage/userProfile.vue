<template>
    <div>
        <vue-element-loading v-bind:active="isLoading"></vue-element-loading>
        <div class="row align-items-center">
            <div class="col">
                <h2>{{ uiTexts.profilePageTitle }}</h2>
            </div>
            <div class="col">
                <router-link to="/" class="btn btn-outline-secondary float-right">
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
        <div class="row" v-else-if="isSavedSuccessfully">
            <div class="col-md-12">
                <b-alert show variant="success" dismissible>
                    <span>{{ successMessage }}</span>
                </b-alert>
            </div>
        </div>
        <div class="row">
            <form class="col-md-12" v-on:submit.prevent="saveUserProfile">
                <vue-element-loading v-bind:active="isSaving"></vue-element-loading>
                <b-card>
                    <div class="form-row form-group">
                        <label class="col-md-2 col-form-label font-weight-bold">{{ uiTexts.username }}</label>
                        <div class="col-md-10">
                            <input type="text" v-model="userProfile.username" disabled class="form-control-plaintext" />
                        </div>
                    </div>
                    <div class="form-row form-group">
                        <label class="col-md-2 col-form-label font-weight-bold" for="userEmail">{{ uiTexts.email }}</label>
                        <div class="col-md-10">
                            <div class="input-group">
                                <input type="email" name="userEmail" class="form-control" v-model="userProfile.email" v-validate="validationRules.email" data-vv-as="Email" />
                                <div class="input-group-append" v-if="userProfile.isEmailConfirmed">
                                    <span class="input-group-text">
                                        <i class="fa fa-check text-success"></i>
                                    </span>
                                </div>
                                <div class="input-group-append" v-else>
                                    <button class="btn btn-outline-primary" type="button" v-on:click="sendVerificationEmail">
                                        {{ uiTexts.sendVerificationEmail }}
                                    </button>
                                </div>
                            </div>
                            <span class="text-danger" v-if="errors.has('userEmail')">{{ errors.first('userEmail') }}</span>
                        </div>
                    </div>
                    <div class="form-row form-group">
                        <label class="col-md-2 col-form-label font-weight-bold" for="userPhone">{{ uiTexts.phoneNumber }}</label>
                        <div class="col-md-10">
                            <input type="text" name="userPhone" class="form-control" v-model="userProfile.phoneNumber" />
                        </div>
                    </div>
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
    import EndpointConfigurationMixin from '@/app/shared/mixins/endpointConfigurationMixin'
    import { httpClient } from '@/app/shared/services/httpClient'
    import errorHelper from '@/app/shared/services/errorHelper'

    import { manageProfileValidationRules } from './validation-rules'

    export default {
        name: 'userProfile',
        mixins: [EndpointConfigurationMixin],
        urls: {
            manageProfileLocalization: window.endpoints.auth.manageProfileLocalization,
            manageProfile: window.endpoints.auth.manageProfile,
            sendVerificationEmail: window.endpoints.auth.sendVerificationEmail
        },
        data() {
            return {
                isLoading: true,
                isSaving: false,
                userProfile: {},
                userProfileCopy: {},
                validationRules: manageProfileValidationRules,
                isSavedSuccessfully: false,
                serverErrors: [],
                successMessage: ''
            }
        },
        computed: {
            isValid() {
                return this.errors.items.length === 0
            }
        },
        methods: {
            transformJsonToViewModel(userProfileJson) {
                let viewModel = Object.assign({}, userProfileJson)
                return viewModel
            },
            transformViewModelToProfile() {
                return {
                    username: this.userProfile.username,
                    isEmailConfirmed: this.userProfile.isEmailConfirmed,
                    email: this.userProfile.email,
                    phoneNumber: this.userProfile.phoneNumber
                }
            },
            loadUserProfile() {
                let self = this
                httpClient.get(this.$urls.manageProfile)
                    .then((response) => {
                        self.userProfile = self.transformJsonToViewModel(response) || {}
                        self.userProfileCopy = Object.assign({}, self.userProfile)
                        self.isLoading = false
                    })
            },
            saveUserProfile() {
                this.isSaving = true;
                let self = this

                let userProfileJson = this.transformViewModelToProfile(this.userProfile)
                httpClient.put(this.$urls.manageProfile, userProfileJson)
                    .then((response) => {
                        self.successMessage = self.uiTexts.profileSavedSuccessfully
                        self.isSavedSuccessfully = true
                        self.userProfileCopy = Object.assign({}, self.userProfile)

                        self.isSaving = false
                    })
                    .catch((errors) => {
                        self.serverErrors = errorHelper.buildErrorListFromModelState(errors) || []
                        self.isSaving = false
                    })
            },
            cancel() {
                this.$validator.reset()
                this.userProfile = Object.assign({}, this.userProfileCopy)
            },
            sendVerificationEmail() {
                this.isSaving = true
                let self = this

                httpClient.post(this.$urls.sendVerificationEmail)
                    .then((response) => {
                        self.successMessage = self.uiTexts.verificationEmailSent
                        self.isSavedSuccessfully = true

                        self.isSaving = false
                    })
                    .catch((errors) => {
                        self.serverErrors = errorHelper.buildErrorListFromModelState(errors) || []
                        self.isSaving = false
                    })
            },
            loadResources() {
                return httpClient.get(this.$urls.manageProfileLocalization)
            },
            onEndResourcesLoading() {
                this.loadUserProfile()
            }
        }
    }
</script>