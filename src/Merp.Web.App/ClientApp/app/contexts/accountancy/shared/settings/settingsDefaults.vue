<template>
    <div class="row">
        <vue-element-loading v-bind:active="isLoading"></vue-element-loading>
        <form class="col-md-12" role="form" v-on:submit.prevent="save">
            <vue-element-loading v-bind:active="isSaving"></vue-element-loading>
            <div class="form-row">
                <div class="col-md-12 form-group">
                    <b-form-checkbox v-model="settings.minimumTaxpayerRegime" v-on:input="applyMinimumTaxpayerRegime">
                        {{ uiTexts.minimumTaxpayerRegime }}
                    </b-form-checkbox>
                </div>
            </div>
            <div class="form-row">
                <div class="col-md-12">
                    <div class="form-group row">
                        <label class="col-md-3 col-form-label">{{ uiTexts.vat }}</label>
                        <div class="col-md-9">
                            <select class="form-control" v-model="settings.vatRate" v-bind:disabled="!isChoiceEnabled">
                                <option></option>
                                <option v-for="vat in vatRates" v-bind:key="vat.id" v-bind:value="vat">
                                    {{ vat.description }}
                                </option>
                            </select>
                        </div>
                    </div>
                </div>
            </div>
            <div class="form-row">
                <div class="col-md-12">
                    <div class="form-group row">
                        <label class="col-md-3 col-form-label">{{ uiTexts.providenceFund }}</label>
                        <div class="col-md-9">
                            <select class="form-control" v-model="settings.providenceFund">
                                <option></option>
                                <option v-for="fund in providenceFunds" v-bind:key="fund.id" v-bind:value="fund">
                                    {{ fund.description }}
                                </option>
                            </select>
                        </div>
                    </div>
                </div>
            </div>
            <div class="form-row">
                <div class="col-md-12">
                    <div class="form-group row">
                        <label class="col-md-3 col-form-label">{{ uiTexts.withholdingTax }}</label>
                        <div class="col-md-9">
                            <select class="form-control" v-model="settings.withholdingTax">
                                <option></option>
                                <option v-for="withholding in withholdingTaxes" v-bind:key="withholding.id" v-bind:value="withholding">
                                    {{ withholding.description }}
                                </option>
                            </select>
                        </div>
                    </div>
                </div>
            </div>
            <div class="form-row">
                <div class="col-md-12 form-group">
                    <b-form-checkbox v-model="settings.electronicInvoiceEnabled">
                        {{ uiTexts.enableElectronicInvoice }}
                    </b-form-checkbox>
                </div>
            </div>
            <div class="form-row">
                <div class="col-md-12 form-group">
                    <b-form-checkbox v-model="settings.splitPayment">
                        {{ uiTexts.applySplitPayment }}
                    </b-form-checkbox>
                </div>
            </div>

            <div class="form-row">
                <div class="btn-group" role="group" aria-label="Settings buttons">
                    <button type="submit" class="btn btn-primary">{{ uiTexts.save }}</button>
                    <button type="button" class="btn btn-outline-secondary" v-on:click="cancel">{{ uiTexts.cancel }}</button>
                </div>
            </div>
        </form>
    </div>
</template>
<script>
    import EndpointConfigurationMixin from '@/app/shared/mixins/endpointConfigurationMixin'
    import { httpClient } from '@/app/shared/services/httpClient'

    export default {
        name: 'accountancySettingsDefaults',
        mixins: [EndpointConfigurationMixin],
        urls: {
            getAvailableVats: window.endpoints.accountancyInternal.getAvailableVats,
            getProvidenceFunds: window.endpoints.accountancyInternal.getProvidenceFunds,
            getWithholdingTaxes: window.endpoints.accountancyInternal.getWithholdingTaxes,
            getSettingsDefaults: window.endpoints.accountancyInternal.getSettingsDefaults,
            saveSettingsDefaults: window.endpoints.accountancyInternal.saveSettingsDefaults,
            settingsDefaultsLocalization: window.endpoints.accountancyInternal.settingsDefaultsLocalization
        },
        data() {
            return {
                isChoiceEnabled: true,
                isLoading: true,
                isSaving: false,
                settings: {
                    minimumTaxpayerRegime: false,
                    electronicInvoiceEnabled: false,
                    splitPayment: false,
                    vatRate: '',
                    withholdingTax: '',
                    providenceFund: ''
                },
                settingsCopy: {
                    minimumTaxpayerRegime: false,
                    electronicInvoiceEnabled: false,
                    splitPayment: false,
                    vatRate: '',
                    withholdingTax: '',
                    providenceFund: ''
                },
                vatRates: [],
                providenceFunds: [],
                withholdingTaxes: []
            }
        },
        methods: {
            transformJsonToVatRatesViewModel(vats) {
                return vats.map((v) => {
                    return {
                        id: v.id,
                        rate: v.rate,
                        description: v.description,
                        appliedForMinimumTaxPayer: v.appliedForMinimumTaxPayer
                    }
                })
            },
            transformJsonToProvidenceFundsViewModel(providenceFunds) {
                return providenceFunds.map((p) => {
                    return {
                        id: p.id,
                        description: p.description,
                        rate: p.rate,
                        appliedInWithholdingTax: p.appliedInWithholdingTax
                    }
                })
            },
            transformJsonToWithholdingTaxesViewModel(withholdingTaxes) {
                return withholdingTaxes.map((w) => {
                    return {
                        id: w.id,
                        description: w.description,
                        rate: w.rate,
                        taxableAmountRate: w.taxableAmountRate
                    }
                })
            },
            transformJsonToSettingsViewModel(settings) {
                let vatRate = ''
                if (settings.vatRate) {
                    vatRate = this.vatRates.filter(v => v.id == settings.vatRate.id)[0] || ''
                }

                let withholdingTax = ''
                if (settings.withholdingTax) {
                    withholdingTax = this.withholdingTaxes.filter(w => w.id == settings.withholdingTax.id)[0] || ''
                }

                let providenceFund = ''
                if (settings.providenceFund) {
                    providenceFund = this.providenceFunds.filter(p => p.id == settings.providenceFund.id)[0] || ''
                }

                return {
                    minimumTaxpayerRegime: settings.minimumTaxpayerRegime || false,
                    electronicInvoiceEnabled: settings.electronicInvoiceEnabled || false,
                    splitPayment: settings.splitPayment || false,
                    vatRate: vatRate,
                    withholdingTax: withholdingTax,
                    providenceFund: providenceFund
                }
            },
            transformViewModelToSaveDefaults(viewModel) {
                return {
                    minimumTaxpayerRegime: viewModel.minimumTaxpayerRegime || false,
                    electronicInvoiceEnabled: viewModel.electronicInvoiceEnabled || false,
                    splitPayment: viewModel.splitPayment || false,
                    vatRate: viewModel.vatRate || null,
                    providenceFund: viewModel.providenceFund || null,
                    withholdingTax: viewModel.withholdingTax || null
                }
            },
            loadSettings() {
                let self = this

                httpClient.all([
                    httpClient.get(this.$urls.getAvailableVats),
                    httpClient.get(this.$urls.getProvidenceFunds),
                    httpClient.get(this.$urls.getWithholdingTaxes),
                    httpClient.get(this.$urls.getSettingsDefaults)
                ]).then(([vats, providenceFunds, withholdingTaxes, defaultSettings]) => {
                    self.vatRates = self.transformJsonToVatRatesViewModel(vats)
                    self.providenceFunds = self.transformJsonToProvidenceFundsViewModel(providenceFunds)
                    self.withholdingTaxes = self.transformJsonToWithholdingTaxesViewModel(withholdingTaxes)

                    if (defaultSettings) {
                        self.settings = self.transformJsonToSettingsViewModel(defaultSettings)
                        self.settingsCopy = JSON.parse(JSON.stringify(self.settings))
                    }

                    self.isLoading = false
                })
            },
            save() {
                this.isSaving = true

                let settingsJson = this.transformViewModelToSaveDefaults(this.settings)
                let self = this

                httpClient
                    .post(this.$urls.saveSettingsDefaults, settingsJson)
                    .then((response) => {
                        self.isSaving = false
                        self.settingsCopy = JSON.parse(JSON.stringify(self.settings))
                    })
                    .catch((errors) => {
                        self.isSaving = false
                    })
            },
            cancel() {
                this.settings = JSON.parse(JSON.stringify(this.settingsCopy))
            },
            loadResources() {
                return httpClient.get(this.$urls.settingsDefaultsLocalization)
            },
            onEndResourcesLoading() {
                this.loadSettings()
            },
            applyMinimumTaxpayerRegime() {
                if (this.settings.minimumTaxpayerRegime) {
                    this.settings.vatRate = this.vatRates.filter((v) => v.appliedForMinimumTaxPayer)[0] || ''
                }
                else {
                    this.settings.vatRate = this.settingsCopy.vatRate ? Object.assign({}, this.settingsCopy.vatRate) : ''
                }

                this.isChoiceEnabled = !this.settings.minimumTaxpayerRegime
            }
        }
    }
</script>