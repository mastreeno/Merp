'use strict';

import './css/site.css'
import 'bootstrap'
import Vue from 'vue'
import BootstrapVue from 'bootstrap-vue'
import VeeValidate from 'vee-validate'

import { router } from './router'
import Identity from './app/shared/plugins/identity'
import Localization from './app/shared/plugins/localization'
import { httpClient } from './app/shared/services/httpClient'
import localizationHelper from './app/shared/services/localizationHelper'
import dateFormat from './app/shared/filters/dateFormat'
import currencyFormat from './app/shared/filters/currencyFormat'
import App from './app/app.vue'

Vue.use(BootstrapVue)
Vue.use(VeeValidate)

localizationHelper.localizeValidation()
localizationHelper.localizeDate()

Vue.use(Identity, {
    authority: window.endpoints.authority.authorityBaseUrl,
    client_id: "merp.webapp",
    redirect_uri: window.endpoints.authority.clientCallbackUrl,
    response_type: "id_token token",
    scope: "openid profile merp.accountancy.api merp.accountancy.api.internal merp.registry.api merp.registry.api.internal merp.timetracking.api merp.auth.api merp.webapp.api",
    post_logout_redirect_uri: window.endpoints.authority.clientBaseUrl
})

Vue.use(Localization)

Vue.filter('dateFormat', dateFormat)
Vue.filter('currencyFormat', currencyFormat)

const app = new Vue({
    router,
    ...App
})

httpClient.setAuthorizationHeader()

app.$mount('#app-root')