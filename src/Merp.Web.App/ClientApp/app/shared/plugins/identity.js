'use strict'

import Oidc from 'oidc-client'

import IdentityService from '@/app/shared/services/IdentityService'

export default {
    install(Vue, options) {
        let identityService = new IdentityService(new Oidc.UserManager(options));

        Vue.identity = identityService;
        Vue.prototype.$identity = identityService;
    }
}