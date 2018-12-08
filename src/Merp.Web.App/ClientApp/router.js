'use strict';

import Vue from 'vue'
import VueRouter from 'vue-router'

import Home from './app/components/home/home.vue'
import About from './app/components/about/about.vue'
import Bot from './app/components/bot/bot.vue'
import Contact from './app/components/contact/contact.vue'

import authRoutes from './app/auth/routes'
import accountancyRoutes from './app/accountancy/routes'
import registryRoutes from './app/registry/routes'
import timeTrackingRoutes from './app/timetracking/routes'

Vue.use(VueRouter)

const routes = [
    { path: '/', component: Home },
    { path: '/about', component: About },
    { path: '/bot', component: Bot },
    { path: '/contact', component: Contact }
]
    .concat(authRoutes)
    .concat(accountancyRoutes)
    .concat(registryRoutes)
    .concat(timeTrackingRoutes)

const router = new VueRouter({ mode: 'history', routes: routes })

export { routes, router }

