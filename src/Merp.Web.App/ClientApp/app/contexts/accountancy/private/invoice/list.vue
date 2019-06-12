<template>
    <div>
        <table class="table table-striped">
            <thead>
                <tr>
                    <th>{{ labels.documentType }}</th>
                    <th>{{ labels.dateOfIssue }}</th>
                    <th>{{ labels.dueDate }}</th>
                    <th>{{ labels.supplier }}</th>
                    <th>{{ labels.customer }}</th>
                    <th>{{ labels.invoiceNumber }}</th>
                    <th>{{ labels.price }}</th>
                    <th>&nbsp;</th>
                </tr>
            </thead>
            <tbody>
                <tr v-for="invoice in items" v-bind:key="invoice.uid">
                    <td>{{ labels[invoice.documentType] }}</td>
                    <td>{{ invoice.date | dateFormat }}</td>
                    <td>{{ invoice.dueDate | dateFormat }}</td>
                    <td>{{ invoice.supplierName }}</td>
                    <td>{{ invoice.customerName }}</td>
                    <td>{{ invoice.number }}</td>
                    <td>{{ invoice.totalPrice | currencyFormat(2) }} {{ invoice.currency }}</td>
                    <td>
                        <router-link v-bind:to="invoiceDetailsUrl(invoice.documentType, invoice.uid)" v-b-tooltip.hover v-bind:title="labels.invoiceDetails" class="btn btn-outline-secondary">
                            <i class="fa fa-file" v-bind:aria-label="labels.invoiceDetails"></i>
                        </router-link>
                    </td>
                </tr>
            </tbody>
        </table>
    </div>
</template>
<script>
    export default {
        name: 'invoiceList',
        props: ['items', 'labels'],
        methods: {
            invoiceDetailsUrl(invoiceDocumentType, invoiceId) {
                return '/accountancy/invoice/' + invoiceDocumentType + 'Details/' + invoiceId
            }
        }
    }
</script>