<template>
    <datepicker v-bind:bootstrap-styling="true"
                v-bind:input-class="inputClassName"
                v-bind:disabled="disabled"
                v-bind:calendar-button="showCalendarButton"
                calendar-button-icon="fa fa-calendar fa-fw"
                v-bind:clear-button="showClearButton"
                clear-button-icon="fa fa-close fa-fw"
                v-bind:language="currentLanguage"
                v-bind:format="applyFormatter"
                v-bind:name="name"
                v-bind:value="value"
                v-on:input="updateDate">
    </datepicker>
</template>
<script>
    import Datepicker from 'vuejs-datepicker'
    import { en, it } from 'vuejs-datepicker/dist/locale'
    import dateFormat from '@/app/shared/filters/dateFormat'

    const localization = {
        'it': it,
        'it-IT': it
    }

    export default {
        name: 'merpDatepicker',
        components: {
            'datepicker': Datepicker
        },
        props: ['value', 'name', 'format', 'showCalendarButton', 'showClearButton', 'inputClass', 'disabled'],
        computed: {
            currentLanguage() {
                let userLanguage = navigator.language || navigator.userLanguage
                return localization[userLanguage] || en
            },
            inputClassName() {
                return this.inputClass || 'form-control'
            }
        },
        methods: {
            applyFormatter(date) {
                let format = this.format || null
                return dateFormat(date, format)
            },
            updateDate(date) {
                this.$emit('input', date)
            }
        }
    }
</script>