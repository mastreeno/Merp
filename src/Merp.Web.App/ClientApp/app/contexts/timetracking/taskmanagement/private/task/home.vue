<template>
    <div id="OnTime" class="row">
        <div class="col-md-3">
            <p>
                &nbsp;
            </p>
            <ul class="list-unstyled">
                <li><button type="button" class="btn btn-link" v-on:click="refresh('backlog')">Backlog</button></li>
                <li><button type="button" class="btn btn-link" v-on:click="refresh('today')">Today</button></li>
                <li><button type="button" class="btn btn-link" v-on:click="refresh('next')">Next 7 days</button></li>
            </ul>
            <div class="row" v-if="jobOrders.length">
                <h3>Job Orders</h3>
                <ul class="list-unstyled"></ul>
            </div>
        </div>
        <div class="col-md-9">
            <form v-on:submit.prevent="addTask()" class="form-inline">
                <input type="text" class="form-control" v-model="taskName" size="30" placeholder="add new task here">
                <input class="btn btn-primary" type="submit" value="add">
            </form>
            <h2>{{viewType}}</h2>
            <ul class="list-unstyled">
                <li v-for="task in tasks">
                    <div v-if="task.edit === false">
                        <input type="checkbox" v-model="task.done" v-on:change="markTaskAsComplete(task)" :value="task" /> <span v-on:click="editTask(task)">{{task.name}}</span>
                    </div>
                    <div v-else>
                        <div class="row">
                            <input type="text" v-model="task.name" class="form-control" v-on:keyup.enter="updateTask(task)" v-on:keyup.esc="cancelEdit(task)" />
                        </div>
                        <div class="row form-inline">
                            <div class="form-group">
                                <button type="button" class="btn btn-default" aria-label="Left Align" v-on:click="cancelTask(task)">
                                    <i class="fa fa-trash"></i>
                                </button>&nbsp;
                                <button type="button" class="btn" v-on:click="updateTask(task)"><i class="fa fa-check"></i></button>&nbsp;
                                <button type="button" class="btn" v-on:click="cancelEdit(task)"><i class="fa fa-undo"></i></button>&nbsp;
                                <input type="text" class="form-control" v-model="task.jobOrderId" v-on:keyup.esc="cancelEdit(task)" />
                                <select v-model="task.priority">
                                    <option v-for="option in priorityOptions" v-bind:value="option">{{ option }}</option>
                                </select>
                            </div>
                        </div>
                    </div>
                </li>
            </ul>
        </div>
    </div>
</template>

<script>
    import { httpClient } from '@/app/shared/services/httpClient'
    import EndpointConfigurationMixin from '@/app/shared/mixins/endpointConfigurationMixin'

    export default {
        name: 'taskManagementHome',
        mixins: [EndpointConfigurationMixin],
        urls: {            
            add: window.endpoints.timeTracking.taskManagement.add,
            cancel: window.endpoints.timeTracking.taskManagement.cancel,
            markAsComplete: window.endpoints.timeTracking.taskManagement.markAsComplete,
            update: window.endpoints.timeTracking.taskManagement.update,
            backlog: window.endpoints.timeTracking.taskManagement.backlog,
            jobOrders: window.endpoints.timeTracking.taskManagement.jobOrders,
            nextSevenDays: window.endpoints.timeTracking.taskManagement.nextSevenDays,
            priorityOptions: window.endpoints.timeTracking.taskManagement.priorityOptions,
            today: window.endpoints.timeTracking.taskManagement.today
        },
        data() {
            return {
                viewType: '',
                taskName: '',
                priorityOptions: [],
                tasks: [],
                originalTask: {},
                jobOrders: []
            }
        },
        methods: {
            editTask: function (task) {
                this.tasks.forEach((item) => { if (item.edit) this.cancelEdit(item); });
                this.originalTask = Object.assign({}, task);
                task.edit = true;
            },
            cancelEdit: function (task) {
                Object.assign(task, this.originalTask);
                task.edit = false;
                this.originalTask = null;
            },
            addTask: function () {
                if (this.taskName) {
                    httpClient.post(this.$urls.add, { name: this.taskName })
                        .then((data) => {
                            var task = { taskId: data, name: this.taskName, done: false, edit: false, priority: 0 };
                            this.tasks.push(task);
                            this.taskName = '';
                        })
                        .catch(function (errors) {
                            alert("Couldn't add the task, retry later.");
                        });
                }
                return false;
            },
            cancelTask: function (task) {
                var url = this.$urls.cancel + '/' + task.taskId;
                httpClient.get(url)
                    .then((data) => {
                        var index = this.tasks.indexOf(task);
                        this.tasks.splice(index, 1);
                    })
                    .catch((errors) => {
                        alert("Can't cancel the task, retry later.");
                    });
            },
            updateTask: function (task) {
                var url = this.$urls.update + '/' + task.taskId;
                var model = { taskId: task.taskId, name: task.name, priority: task.priority, jobOrderId: task.jobOrderId };
                httpClient.post(url, model)
                    .then(() => {
                        task.edit = false;
                    })
                    .catch((errors) => {
                        window.alert("Can't update the task, retry later.");
                    });
            },
            markTaskAsComplete: function (task) {
                var url = this.$urls.markAsComplete + '/' + task.taskId;
                httpClient.get(url)
                    .then((data) => {
                        var index = this.tasks.indexOf(task);
                        this.tasks.splice(index, 1);
                    })
                    .catch((errors) => {
                        alert("Can't mark task as complete, retry later.");
                    });
            },
            refresh: function (viewType) {
                var url = '';
                switch (viewType) {
                    case 'backlog':
                        url = this.$urls.backlog;
                        this.viewType = 'Backlog';
                        break;
                    case 'next':
                        url = this.$urls.nextSevenDays;
                        this.viewType = 'Next 7 days';
                        break;
                    case 'today':
                        url = this.$urls.today;
                        this.viewType = 'Today';
                        break;
                }
                httpClient.get(url)
                    .then((data) => {
                        this.tasks = data.map((item, index) => {
                            return {
                                taskId: item.taskId,
                                name: item.name,
                                done: item.done,
                                edit: false,
                                priority: item.priority,
                                jobOrderId: item.jobOrderId
                            };
                        });
                    })
            }
        },
        mounted: function () {
            httpClient.get(this.$urls.priorityOptions)
            .then((data) => {
                this.priorityOptions = data;
            });
            httpClient.get(this.$urls.jobOrders)
            .then((data) => {
                this.jobOrders = data;
            });
            this.refresh('today');
        }
    }
</script>
