using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Merp.Web.App.Services.Url
{
    public class TimeTrackingEndpoints
    {
        public TaskManagementEndpoints TaskManagement { get; private set; }

        public TimeTrackingEndpoints(string timeTrackingBaseUrl)
        {
            if (string.IsNullOrWhiteSpace(timeTrackingBaseUrl))
            {
                throw new ArgumentException("value cannot be empty", nameof(timeTrackingBaseUrl));
            }

            TaskManagement = new TaskManagementEndpoints(timeTrackingBaseUrl);

        }

        public class TaskManagementEndpoints : UrlBuilder.Endpoints
        {
            private readonly string _baseUrl;

            public string Add { get; private set; }
            public string Cancel { get; private set; }
            public string MarkAsComplete { get; private set; }
            public string Update { get; private set; }

            public string Backlog { get; private set; }
            public string JobOrders { get; private set; }
            public string NextSevenDays { get; private set; }
            public string PriorityOptions { get; private set; }
            public string Today { get; private set; }

            public TaskManagementEndpoints(string timeTrackingBaseUrl)
            {
                _baseUrl = $"{timeTrackingBaseUrl}{ApiPrefix}/taskmanagement";

                Add = $"{_baseUrl}/Task/Add";
                Cancel = $"{_baseUrl}/Task/Cancel";
                MarkAsComplete = $"{_baseUrl}/Task/MarkAsComplete";
                Update = $"{_baseUrl}/Task/Update";

                Backlog = $"{_baseUrl}/Task/Backlog";
                JobOrders = $"{_baseUrl}/Task/JobOrders";
                NextSevenDays = $"{_baseUrl}/Task/NextSevenDays";
                PriorityOptions = $"{_baseUrl}/Task/PriorityOptions";
                Today = $"{_baseUrl}/Task/Today";
            }
        }

        public class Timesheet
        {

        }
    }
}
