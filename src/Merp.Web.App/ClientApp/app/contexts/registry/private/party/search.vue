<template>
    <div>
        <vue-element-loading v-bind:active="isLoading"></vue-element-loading>
        <h2>{{ uiTexts.partySearchPageTitle }}</h2>
        <hr />
        <party-search-filters v-bind:filters="filters" v-bind:labels="uiTexts" v-on:filterparties="filterParties"></party-search-filters>
        <hr />
        <div class="row">
            <div class="col-md-12">
                <vue-element-loading v-bind:active="isSearching"></vue-element-loading>
                <p v-if="parties.length">{{ parties.length }} {{ uiTexts.itemsOf }} <span class="badge">{{ totalNumberOfParties }}</span></p>
                <party-list v-bind:items="parties" v-on:unlist-party="confirmUnlist" v-bind:headers="uiTexts"></party-list>
                <b-pagination align="right" v-on:input="changePage" v-bind:total-rows="totalNumberOfParties" v-model="page" v-bind:per-page="size"></b-pagination>
            </div>
        </div>

        <b-modal id="confirmUnlistParty" v-model="showUnlistConfirm" v-bind:ok-title="uiTexts.ok" v-bind:cancel-title="uiTexts.cancel" v-bind:title="uiTexts.deleteParty" v-on:ok="unlistParty">
            <div>
                {{ uiTexts.confirmPartyUnlistMessage }} <strong>{{ partyToUnlist.name }}</strong>.
            </div>
            <div>
                <h5>{{ uiTexts.confirmPartyUnlistQuestion }}</h5>
            </div>
        </b-modal>
    </div>
</template>
<script>
    import PartySearchFilters from './searchFilters.vue'
    import PartyList from './list.vue'
    import EndpointConfigurationMixin from '@/app/shared/mixins/endpointConfigurationMixin'

    import { httpClient } from '@/app/shared/services/httpClient'

    export default {
        name: 'partySearch',
        mixins: [EndpointConfigurationMixin],
        components: {
            'party-search-filters': PartySearchFilters,
            'party-list': PartyList
        },
        urls: {
            partySearchLocalization: window.endpoints.registry.partySearchLocalization,
            searchParties: window.endpoints.registry.searchParties,
            unlistParty: window.endpoints.registry.unlistParty
        },
        data() {
            return {
                parties: [],
                totalNumberOfParties: 0,
                filters: {
                    type: '',
                    query: '',
                    ordering: {
                        orderBy: 'name',
                        orderDirection: 'asc'
                    },
                    city: '',
                    postalCode: ''
                },
                page: 1,
                size: 20,
                isSearching: false,
                isLoading: true,
                partyToUnlist: {},
                showUnlistConfirm: false
            }
        },
        methods: {
            transformJsonToViewModel(searchPartiesJson) {
                let viewModel = Object.assign({}, searchPartiesJson)
                return viewModel
            },
            loadParties() {
                this.isSearching = true
                let self = this
                httpClient.get(this.$urls.searchParties, {
                    query: this.filters.query,
                    partyType: this.filters.type,
                    city: this.filters.city,
                    postalCode: this.filters.postalCode,
                    orderBy: this.filters.ordering.orderBy,
                    orderDirection: this.filters.ordering.orderDirection,
                    page: this.page,
                    size: this.size
                }).then((data) => {
                    let viewModel = self.transformJsonToViewModel(data)
                    self.totalNumberOfParties = viewModel.totalNumberOfParties || 0
                    self.parties = viewModel.parties || []

                    self.isSearching = false
                });
            },
            changePage(page) {
                this.page = page || 1
                this.loadParties()
            },
            filterParties(filters) {
                this.filters = filters || {
                    type: '',
                    query: '',
                    ordering: {
                        orderBy: 'name',
                        orderDirection: 'asc'
                    },
                    city: '',
                    postalCode: ''
                }
                this.loadParties()
            },
            confirmUnlist(party) {
                this.partyToUnlist = party
                this.showUnlistConfirm = true
            },
            unlistParty(evt) {
                evt.preventDefault()
                this.isSearching = true
                this.showUnlistConfirm = false

                let self = this
                let url = this.$urls.unlistParty + '/' + this.partyToUnlist.uid
                httpClient.delete(url)
                    .then((response) => {
                        self.removeUnlistedItem(self.partyToUnlist)

                        self.isSearching = false
                        self.partyToUnlist = {}
                    })
            },
            clearUnlistAndCloseModal() {
                this.partyToUnlist = {}
                this.showUnlistConfirm = false
            },
            removeUnlistedItem(party) {
                for (var i in this.parties) {
                    if (this.parties[i].id === party.id) {
                        this.parties.splice(i, 1)
                        return
                    }
                }
            },
            loadResources() {
                return httpClient.get(this.$urls.partySearchLocalization)
            },
            onEndResourcesLoading() {
                this.isLoading = false
            }
        }
    }
</script>