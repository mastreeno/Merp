<template>
    <div class="navbar navbar-expand-lg navbar-dark bg-dark fixed-top">
        <div class="container">
            <a href="/" class="navbar-brand">MERP</a>
            <button type="button" class="navbar-toggler" data-toggle="collapse" data-target=".navbar-collapse">
                <span class="navbar-toggler-icon"></span>
            </button>
            <div class="navbar-collapse collapse">
                <ul class="navbar-nav mr-auto">
                    <li class="nav-item"><router-link class="nav-link" to="/" v-bind:exact="true">{{ uiTexts.home }}</router-link></li>
                    <li class="nav-item"><router-link class="nav-link" to="/about">{{ uiTexts.about }}</router-link></li>
                    <li class="nav-item"><router-link class="nav-link" to="/contact">{{ uiTexts.contact }}</router-link></li>
                    <li class="nav-item"><router-link class="nav-link" to="/accountancy">{{ uiTexts.accountancy }}</router-link></li>
                    <li class="nav-item"><router-link class="nav-link" to="/registry/party/search">{{ uiTexts.registry }}</router-link></li>
                    <li class="nav-item dropdown">
                        <a class="nav-link dropdown-toggle" href="#" id="navbarDropdown" role="button" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
                            {{uiTexts.timeTracking}}
                        </a>
                        <div class="dropdown-menu" aria-labelledby="navbarDropdown">
                            <router-link class="dropdown-item" to="/timetracking/tasks">{{uiTexts.timeTracking_TaskManagement}}</router-link>
                            <router-link class="dropdown-item" to="#">{{uiTexts.timeTracking_Timesheet}}</router-link>
                        </div>
                    </li>
                </ul>
                <ul class="navbar-nav mr-0">
                    <li class="nav-item">
                        <router-link to="/auth/manage/profile" class="nav-link">{{ uiTexts.hello }} {{ username }}!</router-link>
                    </li>
                    <li>
                        <button type="button" class="btn btn-link nav-link" v-on:click="signOut">{{ uiTexts.logOut }}</button>
                    </li>
                </ul>
            </div>
        </div>
    </div>
</template>
<script>
    import EndpointConfigurationMixin from '@/app/mixins/endpointConfigurationMixin'
    import { httpClient } from '@/app/services/httpClient'

    export default {
        name: 'MenuComponent',
        mixins: [EndpointConfigurationMixin],
        props: ['username'],
        urls: {
            getAppMenuLocalization: window.endpoints.client.getAppMenuLocalization
        },
        data() {
            return {
                isLoading: true
            }
        },
        methods: {
            signOut() {
                this.$identity.signoutRedirect()
            },
            loadResources() {
                return httpClient.get(this.$urls.getAppMenuLocalization)
            },
            onEndResourcesLoading() {
                this.isLoading = false
            }
        }
    }
</script>
<style src="./navmenu.css"></style>
