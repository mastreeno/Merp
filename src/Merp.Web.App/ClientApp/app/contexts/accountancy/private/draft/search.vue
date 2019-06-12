<template>
    <div>
        <vue-element-loading v-bind:active="isLoading"></vue-element-loading>
        <div class="row align-items-center">
            <div class="col">
                <h2>{{ uiTexts.searchDraftsPageTitle }}</h2>
            </div>
            <div class="col">
                <router-link to="/accountancy/invoice/search" class="btn btn-outline-secondary float-right">
                    <i class="fa fa-angle-left"></i> {{ uiTexts.back }}
                </router-link>
            </div>
        </div>
        <hr />
        <draft-search-filters v-bind:filters="filters" v-bind:labels="uiTexts" v-on:filterdrafts="filterDrafts"></draft-search-filters>
        <hr />
        <div class="row">
            <div class="col-md-12">
                <vue-element-loading v-bind:active="isSearching"></vue-element-loading>
                <p v-if="drafts.length">{{ drafts.length }} {{ uiTexts.itemsOf }} <span class="badge">{{ totalNumberOfDrafts }}</span></p>
                <draft-list v-bind:items="drafts" v-bind:labels="uiTexts" v-on:delete-draft="openDeleteDraftModal"></draft-list>
                <b-pagination align="right" v-on:input="changePage" v-bind:total-rows="totalNumberOfDrafts" v-model="page" v-bind:per-page="size"></b-pagination>
            </div>
        </div>

        <b-modal id="deleteDraftModal" v-model="showDeleteDraftModal" v-bind:ok-title="uiTexts.ok" v-bind:cancel-title="uiTexts.cancel" v-bind:title="uiTexts.deleteDraft" v-on:ok="deleteDraft">
            <div>
                {{ uiTexts.confirmDraftDeleteMessage }}.
            </div>
            <div>
                <h5>{{ uiTexts.confirmDraftDeleteQuestion }}</h5>
            </div>
        </b-modal>
    </div>
</template>
<script>
    import DraftSearchFilters from './searchFilters.vue'
    import DraftList from './list.vue'
    import EndpointConfigurationMixin from '@/app/shared/mixins/endpointConfigurationMixin'
    import { httpClient } from '@/app/shared/services/httpClient'

    export default {
        name: 'searchDrafts',
        mixins: [EndpointConfigurationMixin],
        components: {
            'draft-search-filters': DraftSearchFilters,
            'draft-list': DraftList
        },
        urls: {
            searchDrafts: window.endpoints.accountancy.searchDrafts,
            searchDraftsLocalization: window.endpoints.accountancy.searchDraftsLocalization,
            deleteOutgoingInvoiceDraft: window.endpoints.accountancy.deleteOutgoingInvoiceDraft,
            deleteOutgoingCreditNoteDraft: window.endpoints.accountancy.deleteOutgoingCreditNoteDraft
        },
        data() {
            return {
                isLoading: true,
                isSearching: false,
                drafts: [],
                totalNumberOfDrafts: 0,
                filters: {
                    customer: null,
                    dateFrom: null,
                    dateTo: null
                },
                page: 1,
                size: 20,
                draftToDelete: {},
                showDeleteDraftModal: false
            }
        },
        methods: {
            transformJsonToViewModel(searchDraftsJson) {
                let viewModel = Object.assign({}, searchDraftsJson)
                return viewModel
            },
            loadDrafts() {
                this.isSearching = true
                let self = this

                httpClient.get(this.$urls.searchDrafts, {
                    customerId: this.filters.customer,
                    dateFrom: this.filters.dateFrom,
                    dateTo: this.filters.dateTo,
                    page: this.page,
                    size: this.size
                }).then((data) => {
                    let viewModel = self.transformJsonToViewModel(data)
                    self.drafts = viewModel.drafts
                    self.totalNumberOfDrafts = viewModel.totalNumberOfDrafts

                    self.isSearching = false
                })
            },
            changePage(page) {
                this.page = page || 1
                this.loadDrafts()
            },
            filterDrafts(filters) {
                this.filters = filters || {
                    customer: null,
                    dateFrom: null,
                    dateTo: null
                }
                this.loadDrafts()
            },
            openDeleteDraftModal(draft) {
                this.draftToDelete = draft
                this.showDeleteDraftModal = true
            },
            deleteDraft(evt) {
                evt.preventDefault()
                this.isSearching = true
                let self = this

                let url = this.buildDeleteUrlForDraft(this.draftToDelete)
                httpClient.delete(url)
                    .then((response) => {
                        self.showDeleteDraftModal = false
                        self.loadDrafts()
                    })
                    .catch((errors) => {
                        self.showDeleteDraftModal = false
                        self.isSearching = false
                    })
            },
            buildDeleteUrlForDraft(draft) {
                switch (draft.documentType) {
                    case 'outgoingInvoice':
                        return this.$urls.deleteOutgoingInvoiceDraft + '/' + draft.id
                    case 'outgoingCreditNote':
                        return this.$urls.deleteOutgoingCreditNoteDraft + '/' + draft.id
                    default:
                        break;
                }
            },
            loadResources() {
                return httpClient.get(this.$urls.searchDraftsLocalization)
            },
            onEndResourcesLoading() {
                this.isLoading = false
            }
        }
    }
</script>