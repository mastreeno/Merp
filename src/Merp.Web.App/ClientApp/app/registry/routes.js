'use strict'

import RegistryHome from './private/home/home.vue'
import PartySearch from './private/party/search.vue'
import AddPersonEntry from './private/person/addEntry.vue'
import AddCompanyEntry from './private/company/addEntry.vue'
import PersonInfo from './private/person/info.vue'
import CompanyInfo from './private/company/info.vue'

export default [
    { path: '/registry', component: RegistryHome },
    { path: '/registry/party/search', component: PartySearch },
    { path: '/registry/person/add', component: AddPersonEntry },
    { path: '/registry/company/add', component: AddCompanyEntry },
    { path: '/registry/person/info/:id', component: PersonInfo },
    { path: '/registry/company/info/:id', component: CompanyInfo }
]