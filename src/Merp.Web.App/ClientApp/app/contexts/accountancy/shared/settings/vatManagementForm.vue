<template>
    <form role="form" v-on:submit.prevent="save">
        <fieldset>
            <legend>{{ pageTitle }}</legend>
            <div class="form-row">
                <div class="col-md-12 form-group">
                    <label>{{ labels.rate }}</label>
                    <div class="input-group">
                        <input type="number" step="0.01" min="0" class="form-control" v-model="vatModel.rate" />
                        <div class="input-group-append">
                            <span class="input-group-text">%</span>
                        </div>
                    </div>
                </div>
            </div>
            <div class="form-row">
                <div class="col-md-12 form-group">
                    <label>{{ labels.description }}</label>
                    <input type="text" class="form-control" v-model="vatModel.description" />
                </div>
            </div>
            <div class="form-row float-right">
                <div class="btn-group" role="group">
                    <button type="submit" class="btn btn-primary">{{ labels.save }}</button>
                    <button type="button" v-on:click="cancel" class="btn btn-outline-secondary">{{ labels.cancel }}</button>
                </div>
            </div>
        </fieldset>
    </form>
</template>
<script>
    import EndpointConfigurationMixin from '@/app/shared/mixins/endpointConfigurationMixin'
    import { httpClient } from '@/app/shared/services/httpClient'

    export default {
        name: 'accountancyVatManagementForm',
        mixins: [EndpointConfigurationMixin],
        props: ['vat', 'labels'],
        urls: {
            createVat: window.endpoints.accountancyInternal.createVat,
            editVat: window.endpoints.accountancyInternal.editVat
        },
        data() {
            return {
                vatModel: Object.assign({}, this.vat) || { rate: 0.00, description: '' },
                isSaving: false
            }
        },
        computed: {
            emptyGuid() {
                return '00000000-0000-0000-0000-000000000000'
            },
            isEditMode() {
                return (this.vatModel.id && this.vatModel.id !== this.emptyGuid)
            },
            pageTitle() {
                if (this.isEditMode) {
                    return this.vatModel.description
                }

                return this.labels.addNewVat
            }
        },
        methods: {
            save() {
                this.isSaving = true
                let self = this

                let saveResult = this.isEditMode ? this._edit(this.vatModel) : this._add(this.vatModel)
                saveResult
                    .then((response) => {
                        self.$emit('vat-saved', self.vatModel)
                        self.cleanVat()
                        self.isSaving = false
                    })
                    .catch((errors) => {
                        self.isSaving = false
                    })
            },
            cleanVat() {
                this.vatModel = { rate: 0.00, description: '' }
            },
            cancel() {
                this.vatModel = Object.assign({}, { rate: 0.00, description: '' })
            },
            _add(vat) {
                return httpClient.post(this.$urls.createVat, vat)
            },
            _edit(vat) {
                return httpClient.put(this.$urls.editVat + '/' + vat.id, vat)
            }
        },
        watch: {
            vat() {
                this.vatModel = Object.assign({}, this.vat)
            }
        }
    }
</script>