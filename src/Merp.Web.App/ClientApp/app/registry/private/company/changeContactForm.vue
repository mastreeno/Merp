<template>
    <div>
        <label class="font-weight-bold">{{ title }}</label>
        <div class="row">
            <div class="col-md-9">
                <person-info-autocomplete v-bind:person="contactModel" v-bind:disabled="!isEnabled" v-on:person-selected="changeContactModel"></person-info-autocomplete>
            </div>
            <div class="col-md-3" v-if="!isEnabled">
                <button class="btn btn-outline-primary" v-on:click="enableChanges">
                    <i class="fa fa-edit"></i>
                </button>
            </div>
        </div>
        <div class="btn-group mt-2" v-if="isEnabled">
            <button class="btn btn-primary" v-on:click="save">
                <i class="fa fa-save"></i> {{ labels.save }}
            </button>
            <button class="btn btn-outline-secondary" v-on:click="cancel">{{ labels.cancel }}</button>
        </div>
    </div>
</template>
<script>
    import PersonInfoAutocomplete from '@/app/registry/private/personInfoAutocomplete.vue'

    export default {
        name: 'changeCompanyContactForm',
        components: {
            'person-info-autocomplete': PersonInfoAutocomplete
        },
        props: ['contact', 'title', 'labels'],
        data() {
            return {
                contactModel: Object.assign({}, this.contact),
                isEnabled: false
            }
        },
        methods: {
            enableChanges() {
                this.isEnabled = true
            },
            cancel() {
                this.contactModel = Object.assign({}, this.contact)
                this.isEnabled = false
            },
            save() {
                this.isEnabled = false
                this.$emit('contact-changed', this.contactModel)
            },
            changeContactModel(contact) {
                this.contactModel = contact
            }
        },
        watch: {
            contact() {
                this.contactModel = Object.assign({}, this.contact)
            }
        }
    }
</script>