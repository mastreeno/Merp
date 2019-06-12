<template>
    <div>
        <vue-element-loading v-bind:active="isLoadingBalance"></vue-element-loading>
        <div class="row">
            <form class="form-inline" v-on:submit.prevent="refreshBalance">
                <div class="form-group">
                    <label class="sr-only" for="dateFrom">{{ labels.dateFrom }}</label>
                    <datepicker v-bind:show-calendar-button="true"
                                v-bind:show-clear-button="true"
                                name="dateFrom"
                                v-model="dateFrom">
                    </datepicker>
                </div>
                <div class="form-group">
                    <label class="sr-only" for="dateTo">{{ labels.dateTo }}</label>
                    <datepicker v-bind:show-calendar-button="true"
                                v-bind:show-clear-button="true"
                                name="dateTo"
                                v-model="dateTo">
                    </datepicker>
                </div>
                <div class="form-group">
                    <label class="sr-only" for="scale">Scale</label>
                    <select v-model="scale" class="form-control">
                        <option v-for="s in scales" v-bind:value="s.value">{{ s.displayName }}</option>
                    </select>
                </div>
                <button type="submit" class="btn btn-outline-primary">{{ labels.apply }}</button>
            </form>
        </div>
        <div class="row">
            <canvas id="line" class="chart chart-line" chart-data="chartData"
                    chart-labels="chartLabels" chart-legend="false" chart-series="chartSeries"></canvas>
        </div>
        <div class="row">
            <table class="table table-striped">
                <tr>
                    <td>{{ labels.date }}</td>
                    <td>{{ labels.balance }}</td>
                </tr>
                <tr v-for="balance in balances" v-bind:key="balance.date">
                    <td>{{ balance.date }}</td>
                    <td>{{ balance.balance }}</td>
                </tr>
            </table>
        </div>
    </div>
</template>
<script>
    import Chart from 'chart.js'
    import dateFormat from '@/app/shared/filters/dateFormat'
    import EndpointConfigurationMixin from '@/app/shared/mixins/endpointConfigurationMixin'
    import Datepicker from '@/app/shared/components/datepicker.vue'

    import { httpClient } from '@/app/shared/services/httpClient'

    export default {
        name: 'jobOrderTrendAnalysis',
        components: {
            'datepicker': Datepicker
        },
        mixins: [EndpointConfigurationMixin],
        props: ['labels', 'jobOrderId'],
        urls: {
            getJobOrderBalance: window.endpoints.accountancy.getJobOrderBalance
        },
        data() {
            return {
                isLoadingBalance: false,
                dateFrom: null,
                dateTo: null,
                scale: null,
                chartLabels: [],
                chartData: [[]],
                balances: []
            }
        },
        computed: {
            chartSeries() {
                return ['balance']
            },
            scales() {
                return [
                    {
                        value: 'Daily',
                        displayName: this.labels.dailyScale
                    },
                    {
                        value: 'Weekly',
                        displayName: this.labels.weeklyScale
                    },
                    {
                        value: 'Monthly',
                        displayName: this.labels.monthlyScale
                    },
                    {
                        value: 'Yearly',
                        displayName: this.labels.yearlyScale
                    }
                ]
            }
        },
        methods: {
            transformJsonToViewModel(jobOrderBalanceJson) {
                let viewModel = jobOrderBalanceJson.map(b => {
                    return {
                        date: dateFormat(b.date),
                        balance: b.balance
                    }
                })
                return viewModel
            },
            refreshBalance() {
                this.isLoadingBalance = true
                this.chartLabels = []
                this.chartData = [[]]

                let self = this

                httpClient
                    .get(this.$urls.getJobOrderBalance, {
                        jobOrderId: this.jobOrderId,
                        dateFrom: this.dateFrom,
                        dateTo: this.dateTo,
                        scale: this.scale
                    })
                    .then((data) => {
                        self.balances = self.transformJsonToViewModel(data)
                        self.chartLabels = self.balances.map((b) => b.date)
                        self.chartData[0] = self.balances.map((b) => b.balance)

                        self.renderChart()

                        self.isLoadingBalance = false
                    })
            },
            renderChart() {
                var canvas = document.getElementById('line')
                new Chart(canvas, {
                    type: 'line',
                    data: {
                        labels: this.chartLabels,
                        datasets: this.chartData.map((data) => {
                            return {
                                data: data
                            }
                        })
                    },
                    options: {
                        legend: false
                    }
                })
            }
        },
        mounted() {
            this.scale = this.scales[0].value
        }
    }
</script>