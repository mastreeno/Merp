<template>
    <div>
        <table class="table table-striped">
            <thead>
                <tr>
                    <th>{{ headers.partyListName }}</th>
                    <th>{{ headers.nationalIdentificationNumber }}</th>
                    <th>{{ headers.vatNumber }}</th>
                    <th>{{ headers.partyListPhoneNumber }}</th>
                    <th>&nbsp;</th>
                </tr>
            </thead>
            <tbody>
                <party-list-row v-for="party in items" v-bind:key="party.uid" v-bind:party="party" v-on:request-unlist="sendUnlistRequest"></party-list-row>
            </tbody>
        </table>
    </div>
</template>
<script>
    import PartyListRow from './listRow.vue'

    export default {
        name: 'partyList',
        components: {
            'party-list-row': PartyListRow
        },
        props: ['items', 'headers'],
        methods: {
            sendUnlistRequest(party) {
                this.$emit('unlist-party', party)
            }
        }
    }
</script>