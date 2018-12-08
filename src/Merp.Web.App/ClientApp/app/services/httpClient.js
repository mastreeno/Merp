'use strict'

import Vue from 'vue'
import axios from 'axios'

class HttpClient {
    accessToken

    async setAuthorizationHeader() {
        let accessToken = await Vue.identity.getAccessToken()
        if (accessToken) {
            this.accessToken = accessToken
            axios.defaults.headers.common['Authorization'] = 'Bearer ' + accessToken
        }
    }

    get(url, params) {
        let config = {}
        if (params) {
            config.params = params
        }

        return axios
            .get(url, config)
            .then((response) => response.data)
            .catch((error) => {
                var errors = {}
                if (error.response) {
                    errors = error.response.data || {}
                }

                return Promise.reject(errors)
            })
    }

    post(url, model) {
        return axios
            .post(url, model)
            .then((response) => response.data)
            .catch((error) => {
                var errors = {}
                if (error.response) {
                    errors = error.response.data || {}
                }

                return Promise.reject(errors)
            })
    }

    put(url, model) {
        return axios
            .put(url, model)
            .then((response) => response.data)
            .catch((error) => {
                var errors = {}
                if (error.response) {
                    errors = error.response.data || {}
                }

                return Promise.reject(errors)
            })
    }

    patch(url, model) {
        return axios
            .patch(url, model)
            .then((response) => response.data)
            .catch((error) => {
                var errors = {}
                if (error.response) {
                    errors = error.response.data || {}
                }

                return Promise.reject(errors)
            })
    }

    delete(url) {
        return axios
            .delete(url)
            .then((response) => response.data)
            .catch((error) => {
                var errors = {}
                if (error.response) {
                    errors = error.response.data || {}
                }

                return Promise.reject(errors)
            })
    }
}

const httpClient = new HttpClient()
export { httpClient }