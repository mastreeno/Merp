<template>
    <div>
        <table class="table table-striped">
            <thead>
                <tr>
                    <th>{{ labels.documentType }}</th>
                    <th>{{ labels.date }}</th>
                    <th>{{ labels.customer }}</th>
                    <th>{{ labels.price }}</th>
                    <th>&nbsp;</th>
                </tr>
            </thead>
            <tbody>
                <tr v-for="draft in items" v-bind:key="draft.id">
                    <td>{{ labels[draft.documentType] }}</td>
                    <td>{{ draft.date | dateFormat }}</td>
                    <td>{{ draft.customerName }}</td>
                    <td>{{ draft.totalPrice | currencyFormat(2) }} {{ draft.currency }}</td>
                    <td>
                        <div class="btn-group" role="group">
                            <router-link v-bind:to="getDraftEditUrl(draft)" class="btn btn-outline-secondary">
                                <i class="fa fa-edit"></i>
                            </router-link>
                            <button type="button" class="btn btn-outline-secondary" v-on:click="deleteDraft(draft)">
                                <i class="fa fa-trash"></i>
                            </button>
                        </div>
                    </td>
                </tr>
            </tbody>
        </table>
    </div>
</template>
<script>
    import _ from 'lodash'

    export default {
        name: 'draftList',
        props: ['items', 'labels'],
        methods: {
            getDraftEditUrl(draft) {
                let documentType = _.upperFirst(draft.documentType)
                return '/accountancy/draft/edit' + documentType + 'Draft/' + draft.id
            },
            deleteDraft(draft) {
                this.$emit('delete-draft', draft)
            }
        }
    }
</script>