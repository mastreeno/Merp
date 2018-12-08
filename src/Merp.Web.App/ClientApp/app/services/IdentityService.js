'use strict'

export default class IdentityService {
    _userManager
    
    constructor(userManager) {
        this._userManager = userManager
    }

    getUser() {
        return this._userManager.getUser()
            .then((user) => user)
            .catch((err) => null)
    }

    async getAccessToken() {
        let user = await this.getUser()
        if (user) {
            return user.access_token
        }

        return undefined
    }

    signinRedirect() {
        return this._userManager.signinRedirect()
    }

    signoutRedirect() {
        return this._userManager.signoutRedirect()
    }

    signinRedirectCallback() {
        return this._userManager.signinRedirectCallback()
    }
}