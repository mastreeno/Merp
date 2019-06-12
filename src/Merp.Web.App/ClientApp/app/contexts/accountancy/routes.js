'use strict'

import AccountancyHome from './private/home/home.vue'

import SearchJobOrders from './private/joborder/search.vue'
import CreateJobOrder from './private/joborder/create.vue'
import JobOrderDetail from './private/joborder/detail.vue'

import SearchInvoices from './private/invoice/search.vue'
import IssueOutgoingInvoice from './private/invoice/issueOutgoingInvoice.vue'
import RegisterIncomingInvoice from './private/invoice/registerIncomingInvoice.vue'
import RegisterOutgoingInvoice from './private/invoice/registerOutgoingInvoice.vue'
import OutgoingInvoicesNotLinkedToAJobOrder from './private/invoice/outgoingInvoicesNotLinkedToAJobOrder.vue'
import IncomingInvoicesNotLinkedToAJobOrder from './private/invoice/incomingInvoicesNotLinkedToAJobOrder.vue'
import OutgoingCreditNoteDetails from './private/invoice/outgoingCreditNoteDetails.vue'
import OutgoingInvoiceDetails from './private/invoice/outgoingInvoiceDetails.vue'
import IncomingCreditNoteDetails from './private/invoice/incomingCreditNoteDetails.vue'
import IncomingInvoiceDetails from './private/invoice/incomingInvoiceDetails.vue'

import SearchDrafts from './private/draft/search.vue'
import EditOutgoingCreditNoteDraft from './private/draft/editOutgoingCreditNoteDraft.vue'
import EditOutgoingInvoiceDraft from './private/draft/editOutgoingInvoiceDraft.vue'

export default [
    { path: '/accountancy', component: AccountancyHome },
    { path: '/accountancy/joborder/search', component: SearchJobOrders },
    { path: '/accountancy/joborder/create', component: CreateJobOrder },
    { path: '/accountancy/joborder/detail/:id', component: JobOrderDetail },
    { path: '/accountancy/invoice/search', component: SearchInvoices },
    { path: '/accountancy/invoice/issue/:type?', component: IssueOutgoingInvoice },
    { path: '/accountancy/invoice/registerOutgoing/:type?', component: RegisterOutgoingInvoice },
    { path: '/accountancy/invoice/register/:type?', component: RegisterIncomingInvoice },
    { path: '/accountancy/invoice/outgoingInvoicesNotLinkedToAJobOrder', component: OutgoingInvoicesNotLinkedToAJobOrder },
    { path: '/accountancy/invoice/incomingInvoicesNotLinkedToAJobOrder', component: IncomingInvoicesNotLinkedToAJobOrder },
    { path: '/accountancy/invoice/outgoingCreditNoteDetails/:id', component: OutgoingCreditNoteDetails },
    { path: '/accountancy/invoice/outgoingInvoiceDetails/:id', component: OutgoingInvoiceDetails },
    { path: '/accountancy/invoice/incomingCreditNoteDetails/:id', component: IncomingCreditNoteDetails },
    { path: '/accountancy/invoice/incomingInvoiceDetails/:id', component: IncomingInvoiceDetails },
    { path: '/accountancy/draft/search', component: SearchDrafts },
    { path: '/accountancy/draft/editOutgoingCreditNoteDraft/:id', component: EditOutgoingCreditNoteDraft },
    { path: '/accountancy/draft/editOutgoingInvoiceDraft/:id', component: EditOutgoingInvoiceDraft }
]