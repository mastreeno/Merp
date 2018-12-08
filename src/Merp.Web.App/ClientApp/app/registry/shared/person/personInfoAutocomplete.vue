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
            <button type="button" class="btn btn-outline-primary" v-b-tooltip.hover v-bind:title="uiTexts.registerNewPerson" v-bind:disabled="isDisabled" v-on:click="createNewPerson">
                <i class="fa fa-plus"></i>
            </button>
        </div>

        <b-modal id="createNewPersonModal" v-model="showCreateNewPersonModal" hide-footer v-bind:title="uiTexts.registerNewPerson" size="lg">
            <register-person-form v-bind:labels="uiTexts" v-on:new-person-form-closed="closeNewPersonModal" v-on:person-created="selectPerson"></register-person-form>
        </b-modal>
    </div>
</template>
<script>
    import vSelect from 'vue-select'
    import EndpointConfigurationMixin from '@/app/mixins/endpointConfigurationMixin'
    import RegisterPersonForm from './registerPersonForm.vue'

    import { httpClient } from '@/app/services/httpClient'

    export default {
        name: 'personInfoAutocomplete',
        mixins: [EndpointConfigurationMixin],
        components: {
            'v-select': vSelect,
            'register-person-form': RegisterPersonForm
        },
        props: ['person', 'disabled'],
        urls: {
            searchPeopleByPattern: window.endpoints.registryInternal.searchPeopleByPattern,
            getPersonInfoLocalization: window.endpoints.registryInternal.getPersonInfoLocalization
        },
        data() {
            return {
                personModel: this.person,
                isDisabled: this.disabled,
                people: [],
                showCreateNewPersonModal: false,
                isLoading: false,
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
                this.showCreateNewPersonModal = true
            },
            closeNewPersonModal() {
                this.showCreateNewPersonModal = false
            },
            loadResources() {
                this.isLoading = true
                return httpClient.get(this.$urls.getPersonInfoLocalization)
            },
            onEndResourcesLoading() {
                this.isLoading = false
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