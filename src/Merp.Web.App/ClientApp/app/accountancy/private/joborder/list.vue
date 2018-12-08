<template>
    <div>
        <table class="table table-striped">
            <thead>
                <tr>
                    <th>{{ headers.customerName }}</th>
                    <th>{{ headers.jobOrderName }}</th>
                </tr>
            </thead>
            <tbody>
                <tr v-for="jobOrder in items" v-bind:key="jobOrder.originalId">
                    <td>{{ jobOrder.customerName }}</td>
                    <td><router-link v-bind:to="'/accountancy/joborder/detail/'+jobOrder.originalId">{{ jobOrder.name }}</router-link></td>
                </tr>
            </tbody>
        </table>
    </div>
</template>
<script>
    export default {
        name: 'jobOrderList',
        props: ['items', 'headers']
    }
</script>