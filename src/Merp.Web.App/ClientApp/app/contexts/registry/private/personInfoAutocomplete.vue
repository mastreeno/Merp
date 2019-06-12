<template>
    <div class="row">
        <vue-element-loading v-bind:active="isLoading"></vue-element-loading>
        <div class="col-md-10">
            <v-select v-model="personModel" label="name" v-bind:filterable="false" v-bind:options="people" v-on:search="searchPeople" v-on:input="selectPerson" v-bind:disabled="isDisabled">
                <template slot="option" scope="option">
                    <div>
                        {{ option.name }}
                    </div>
                </template>
                <template slot="selected-option" scope="option">
                    <div>
                        {{ option.name }}
                    </div>
                </template>
            </v-select>
        </div>
        <div class="col-md-2">
            <button type="button" class="btn btn-outline-primary" v-b-tooltip.hover title="Create new person" v-bind:disabled="isDisabled" v-on:click="createNewPerson">
                <i class="fa fa-plus"></i>
            </button>
        </div>

        <add-person-entry-modal v-bind:show="showNewPersonForm" v-on:add-person-modal-closed="resetShowModal" v-on:person-saved="selectCreatedPerson"></add-person-entry-modal>
    </div>
</template>
<script>
    import vSelect from 'vue-select'
    import AddPersonEntryModal from '@/app/contexts/registry/private/person/addEntryModal.vue'
    import EndpointConfigurationMixin from '@/app/shared/mixins/endpointConfigurationMixin'

    import { httpClient } from '@/app/shared/services/httpClient'

    export default {
        name: 'personInfoAutocomplete',
        mixins: [EndpointConfigurationMixin],
        components: {
            'v-select': vSelect,
            'add-person-entry-modal': AddPersonEntryModal
        },
        props: ['person', 'disabled'],
        urls: {
            searchPeopleByPattern: window.endpoints.registry.searchPeopleByPattern
        },
        data() {
            return {
                personModel: this.person,
                isDisabled: this.disabled,
                people: [],
                showNewPersonForm: false,
                isLoading: false
            }
        },
        methods: {
            transformJsonToViewModel(peopleByPatternJson) {
                let viewModel = peopleByPatternJson.slice()
                return viewModel
            },
            searchPeople(query, loading) {
                loading(true)
                let self = this
                httpClient.get(this.$urls.searchPeopleByPattern, {
                    query: query
                }).then((items) => {
                    self.people = self.transformJsonToViewModel(items) || []
                    loading(false)
                })
            },
            selectPerson(item) {
                this.$emit('person-selected', item)
            },
            createNewPerson() {
                this.showNewPersonForm = true
            },
            resetShowModal() {
                if (this.showNewPersonForm) {
                    this.showNewPersonForm = false
                }
            },
            selectCreatedPerson(fullName) {
                this.showNewPersonForm = false
                this.isLoading = true
                let self = this

                httpClient
                    .get(this.$urls.searchPeopleByPattern, {
                        query: fullName
                    })
                    .then((people) => {
                        if (people && people.length) {
                            let viewModel = self.transformJsonToViewModel(people)
                            self.selectPerson(viewModel[0])
                        }

                        self.isLoading = false
                    })
            }
        },
        watch: {
            person() {
                this.personModel = this.person
            },
            disabled() {
                this.isDisabled = this.disabled
            }
        }
    }
</script>
<style>
    .v-select .dropdown-toggle {
        display: flex !important;
        flex-wrap: wrap;
    }

    .v-select input[type=search], .v-select input[type=search]:focus {
        flex-basis: 20px;
        flex-grow: 1;
        height: 33px;
        padding: 0 20px 0 10px;
        width: 100% !important;
    }
</style>