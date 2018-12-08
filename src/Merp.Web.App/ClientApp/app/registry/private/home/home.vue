<template>
    <div>
        <h2>Registry</h2>
        <hr />

        <ul class="list-unstyled">
            <li><router-link to="/registry/party/search">Find an item</router-link></li>
            <li><router-link to="/registry/person/add">Register a company</router-link></li>
            <li><router-link to="/registry/company/add">Register a person</router-link></li>
        </ul>
    </div>
</template>
<script>
    export default {
        name: 'registryHome'
    }
</script>