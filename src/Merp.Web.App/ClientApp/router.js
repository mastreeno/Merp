'use strict';

import Vue from 'vue'
import VueRouter from 'vue-router'

import websiteRoutes from './app/contexts/website/routes'
import authRoutes from './app/contexts/auth/routes'
import accountancyRoutes from './app/contexts/accountancy/routes'
import registryRoutes from './app/contexts/registry/routes'
import timeTrackingRoutes from './app/contexts/timetracking/routes'
import martinRoutes from './app/contexts/martin/routes'

Vue.use(VueRouter)

const routes = websiteRoutes
    .concat(martinRoutes)
    .concat(authRoutes)
    .concat(accountancyRoutes)
    .concat(registryRoutes)
    .concat(timeTrackingRoutes)

const router = new VueRouter({ mode: 'history', routes: routes })

export { routes, router }

