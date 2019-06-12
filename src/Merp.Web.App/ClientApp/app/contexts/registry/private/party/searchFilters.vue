<template>
    <form id="search-form" role="form" v-on:submit.prevent="filterParties">
        <div class="form-row">
            <div class="col-md-4">
                <div class="form-group row">
                    <label class="col-md-2 col-form-label" for="partyType">{{ labels.partySearchFilterType }}</label>
                    <div class="col-md-10">
                        <select id="partyType" name="partyType" class="form-control" v-model="type">
                            <option v-for="type in partyTypes" v-bind:value="type.value">{{ type.label }}</option>
                        </select>
                    </div>
                </div>
            </div>

            <div class="col-md-4">
                <div class="form-group row">
                    <label class="col-md-2 col-form-label" for="query">{{ labels.partySearchFilterName }}</label>
                    <div class="col-md-10">
                        <input type="search" id="query" name="query" v-model="query" class="form-control" v-bind:placeholder="labels.partySearchFilterName" />
                    </div>
                </div>
            </div>

            <div class="col-md-4">
                <div class="form-group row">
                    <label class="col-md-3 col-form-label" for="sort">{{ labels.partySearchFilterSort }}</label>
                    <div class="col-md-9">
                        <select id="sort" name="sort" class="form-control" v-model="ordering">
                            <option v-for="order in orderings" v-bind:value="order">{{ order.label }}</option>
                        </select>
                    </div>
                </div>
            </div>

            <div class="col-md-4">
                <div class="form-group row">
                    <label class="col-md-2 col-form-label" for="city">{{ labels.city }}</label>
                    <div class="col-md-10">
                        <input type="search" id="city" v-model="city" class="form-control" v-bind:placeholder="labels.city" />
                    </div>
                </div>
            </div>

            <div class="col-md-4">
                <div class="form-group row">
                    <label class="col-form-label col-md-4" for="city">{{ labels.postalCode }}</label>
                    <div class="col-md-8">
                        <input type="search" id="postalCode" v-model="postalCode" class="form-control" v-bind:placeholder="labels.postalCode" />
                    </div>
                </div>
            </div>
        </div>
        <div class="form-row">
            <div class="col-md-6">
                <button type="submit" class="btn btn-primary">
                    <i class="fa fa-search"></i>
                    {{ labels.search }}
                </button>
            </div>
            <div class="col-md-6">
                <div class="btn-group float-right" role="group">
                    <router-link to="/registry/company/add" class="btn btn-outline-secondary"><i class="fa fa-plus"></i> {{ labels.partyAddCompany }}</router-link>
                    <router-link to="/registry/person/add" class="btn btn-outline-secondary"><i class="fa fa-plus"></i> {{ labels.partyAddPerson }}</router-link>
                </div>
            </div>
        </div>
    </form>
</template>
<script>
    export default {
        name: 'partySearchFilters',
        props: ['filters', 'labels'],
        data() {
            return {
                type: this.filters.type,
                query: this.filters.query,
                ordering: this.filters.ordering,
                city: this.filters.city,
                postalCode: this.filters.postalCode
            }
        },
        computed: {
            orderings() {
                return [
                    { label: this.labels.partyFilterOrderingNameAsc, value: '+name', orderBy: 'name', orderDirection: 'asc' },
                    { label: this.labels.partyFilterOrderingNameDesc, value: '-name', orderBy: 'name', orderDirection: 'desc' }
                ]
            },
            partyTypes() {
                return [
                    { label: this.labels.partyFilterTypeAny, value: '' },
                    { label: this.labels.partyFilterTypeCompany, value: 'company' },
                    { label: this.labels.partyFilterTypePerson, value: 'person' }
                ]
            }
        },
        methods: {
            filterParties() {
                let _filters = {
                    type: this.type,
                    query: this.query,
                    ordering: this.ordering,
                    city: this.city,
                    postalCode: this.postalCode
                }

                this.$emit('filterparties', _filters)
            }
        },
        mounted() {
            if (!this.ordering) {
                this.ordering = this.orderings[0]
            }

            if (!this.type) {
                this.type = this.partyTypes[0].value
            }
        }
    }
</script>