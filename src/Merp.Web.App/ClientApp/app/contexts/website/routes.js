'use strict'

import Home from './components/home.vue'
import About from './components/about.vue'
import Contact from './components/contact.vue'

export default [
    { path: '/', component: Home },
    { path: '/about', component: About },
    { path: '/contact', component: Contact }
]