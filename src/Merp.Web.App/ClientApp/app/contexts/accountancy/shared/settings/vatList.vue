<template>
    <fieldset>
        <legend>{{ labels.vatList }}</legend>
        <div class="row">
            <div class="col-md-12">
                <div class="input-group">
                    <input class="form-control" type="text" v-model="filter" />
                    <div class="input-group-append">
                        <button type="button" class="input-group-btn btn btn-primary" v-on:click="filterVat">
                            <i class="fa fa-search fa-fw"></i> {{ labels.search }}
                        </button>
                    </div>
                </div>
            </div>
        </div>
        <div v-if="vatList.length > 0" class="list-group">
            <div class="list-group-item" v-for="vat in vatList" v-bind:key="vat.id">
                <span class="align-baseline">{{ vat.description }} ({{ vat.rate | currencyFormat(2) }}%)</span>
                <div v-if="!vat.isSystemVat" class="btn-group float-right" role="group">
                    <button type="button" v-on:click="editVat(vat)" class="btn btn-outline-primary">
                        <i class="fa fa-edit"></i>
                    </button>
                    <button type="button" v-on:click="unlistVat(vat)" class="btn btn-outline-secondary">
                        <i class="fa fa-trash"></i>
                    </button>
                </div>
                <div v-else class="float-right" v-b-tooltip.hover.left v-bind:title="labels.systemVatMessage">
                    <i class="fa fa-lock fa-lg"></i>
                </div>
            </div>
        </div>
        <div v-else class="row">
            <div class="col-md-12 font-weight-bold">{{ labels.emptyVatListMessage }}</div>
        </div>
        <div class="row float-right" v-if="vatList.length < totals">
            <div class="col-md-12">
                <b-pagination v-bind:total-rows="totals" v-model="page" v-bind:per-page="size" v-on:input="changePage"></b-pagination>
            </div>
        </div>
    </fieldset>
</template>
<script>
    export default {
        name: 'accountancyVatList',
        props: ['vatList', 'labels', 'filterText', 'currentPage', 'pageSize', 'totals'],
        data() {
            return {
                filter: this.filterText || '',
                page: this.currentPage || 1,
                size: this.pageSize || 20
            }
        },
        methods: {
            editVat(vat) {
                if (vat.isSystemVat) {
                    return;
                }

                this.$emit('edit-vat', vat)
            },
            unlistVat(vat) {
                if (vat.isSystemVat) {
                    return;
                }

                this.$emit('unlist-vat', vat)
            },
            filterVat() {
                this.$emit('filter-vat', this.filter)
            },
            changePage() {
                this.$emit('paginate', { page: this.page, size: this.size })
            }
        }
    }
</script>