<template>
    <tr>
        <td><router-link v-bind:to="partyDetailUrl">{{ party.name }}</router-link></td>
        <td>{{ party.nationalIdentificationNumber }}</td>
        <td>{{ party.vatIndex }}</td>
        <td><i class="fa fa-phone"></i> {{ party.phoneNumber }}</td>
        <td>
            <button type="button" class="btn btn-outline-secondary" v-on:click="requestPartyUnlist(party)"><i class="fa fa-trash"></i></button>
        </td>
    </tr>
</template>
<script>
    export default {
        name: 'partyListRow',
        props: ['party'],
        computed: {
            partyDetailUrl() {
                return '/registry/' + this.party.partyType.toLowerCase() + '/info/' + this.party.uid
            }
        },
        methods: {
            requestPartyUnlist(party) {
                this.$emit('request-unlist', party)
            }
        }
    }
</script>