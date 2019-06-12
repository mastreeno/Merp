<template>
    <div class="row">
        <vue-element-loading v-bind:active="isLoading"></vue-element-loading>
        <div class="col-md-8">
            <vat-list v-bind:vat-list="vatList" 
                      v-bind:labels="uiTexts" 
                      v-bind:filter-text="filter"
                      v-bind:current-page="page"
                      v-bind:page-size="size"
                      v-on:edit-vat="prepareVatForEdit" 
                      v-on:unlist-vat="confirmVatUnlist" 
                      v-on:filter-vat="filterVat"
                      v-on:paginate="paginate">
            </vat-list>
        </div>
        <div class="col-md-4">
            <vat-management-form v-bind:vat="vatModel" 
                                 v-bind:labels="uiTexts"
                                 v-on:vat-saved="vatSaved">
            </vat-management-form>
        </div>

        <b-modal v-model="showUnlistVatConfirmModal" v-bind:title="uiTexts.deleteVat" v-bind:ok-title="uiTexts.ok" v-bind:cancel-title="uiTexts.cancel" v-on:ok="unlistVat">
            <div>
                {{ uiTexts.confirmVatUnlistMessage }} <strong>{{ vatToUnlist.rate }}% {{ vatToUnlist.description }}</strong>
            </div>
            <div>
                <h5>{{ uiTexts.confirmVatUnlistQuestion }}</h5>
            </div>
        </b-modal>
    </div>
</template>
<script>
    import VatList from './vatList.vue'
    import VatManagementForm from './vatManagementForm.vue'
    import EndpointConfigurationMixin from '@/app/shared/mixins/endpointConfigurationMixin'
    import { httpClient } from '@/app/shared/services/httpClient'
    
    export default {
        name: 'accountancyVatManagement',
        components: {
            'vat-list': VatList,
            'vat-management-form': VatManagementForm
        },
        mixins: [EndpointConfigurationMixin],
        urls: {
            settingsVatManagementLocalization: window.endpoints.accountancyInternal.settingsVatManagementLocalization,
            getVatList: window.endpoints.accountancyInternal.getVatList,
            unlistVat: window.endpoints.accountancyInternal.unlistVat
        },
        data() {
            return {
                isLoading: true,
                filter: null,
                page: 1,
                size: 20,
                totalNumberOfVats: 0,
                vatList: [],
                vatModel: { rate: 0.00, description: '' },
                showUnlistVatConfirmModal: false,
                vatToUnlist: {}
            }
        },
        methods: {
            loadVatList() {
                let self = this
                httpClient.get(this.$urls.getVatList, {
                    filter: this.filter,
                    page: this.page,
                    size: this.size
                }).then((response) => {
                    self.vatList = response.vats || []
                    self.totalNumberOfVats = response.totalNumberOfVats || 0
                    self.isLoading = false
                })
            },
            filterVat(filter) {
                this.filter = filter || null
                this.loadVatList()
            },
            paginate(pagination) {
                this.page = pagination.page || 1
                this.size = pagination.size || 20
                this.loadVatList()
            },
            prepareVatForEdit(vat) {
                this.vatModel = Object.assign({}, vat)
            },
            vatSaved(vat) {
                this.isLoading = true
                this.vatList.push(vat)
                this.loadVatList()
            },
            confirmVatUnlist(vat) {
                this.showUnlistVatConfirmModal = true
                this.vatToUnlist = vat
            },
            unlistVat(evt) {
                evt.preventDefault()
                this.showUnlistVatConfirmModal = false
                this.isLoading = true

                let self = this
                let vatId = this.vatToUnlist.id

                httpClient
                    .delete(this.$urls.unlistVat + '/' + vatId)
                    .then((response) => {
                        self.vatList = self.vatList.filter(v => v.id !== vatId)
                        self.vatToUnlist = {}

                        self.loadVatList()
                    })
                    .catch((errors) => {
                        self.isLoading = false
                        self.vatToUnlist = {}
                    })
            },
            loadResources() {
                return httpClient.get(this.$urls.settingsVatManagementLocalization)
            },
            onEndResourcesLoading() {
                this.loadVatList()
            }
        }
    }
</script>