<template>
    <div id='app-root' class="container-fluid">
        <div v-if="!loading" class="row">
            <menu-component v-bind:username="currentUserName" />
            <div class="container body-content">
                <router-view></router-view>
            </div>
            <div class="container">
                <hr />
                <footer>
                    <p>&copy; {{ currentYear }} - Mastreeno ltd</p>
                </footer>
            </div>
        </div>
        <vue-element-loading v-bind:active="loading" is-full-screen></vue-element-loading>
    </div>
</template>
<script>
    import Vue from 'vue'
    import MenuComponent from '@/app/shared/components/navmenu.vue'
    import VueElementLoading from 'vue-element-loading'

    Vue.component('vue-element-loading', VueElementLoading)
    Vue.component('menu-component', MenuComponent)

    export default {
        name: 'App',
        data() {
            return {
                loading: true,
                currentUserName: ''
            }
        },
        computed: {
            currentYear() {
                return (new Date()).getFullYear()
            }
        },
        mounted() {
            let self = this
            this.$identity
                .getUser()
                .then((user) => {
                    if (!user) {
                        self.$identity.signinRedirect()
                    }
                    else {
                        self.currentUserName = user.profile.name
                        self.loading = false
                    }
                })
        }
    }
</script>
