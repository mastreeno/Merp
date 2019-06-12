'use strict'

import Bot from './bot/bot.vue'
import WebChat from './webchat/webchat.vue'

export default [
    { path: '/bot', component: Bot },
    { path: '/webchat', component: WebChat },
]