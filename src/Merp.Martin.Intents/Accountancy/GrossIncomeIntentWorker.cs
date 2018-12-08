using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Merp.Accountancy.QueryStack;
using Microsoft.EntityFrameworkCore;

namespace Merp.Martin.Intents.Accountancy
{
    public class GrossIncomeIntentWorker : IntentWorker
    {
        public AccountancyDbContext ActiveDbContext { get; private set; }

        public GrossIncomeIntentWorker(AccountancyDbContext context)
        {
            ActiveDbContext = context ?? throw new ArgumentNullException(nameof(context));
        }

        public string Do(QueryParameters qp)
        {
            using (var db = new Database(ActiveDbContext))
            {
                var year = qp.Year ?? DateTime.Now.Year;
                var grossIncome = db.OutgoingInvoices
                                    .Where(i => i.Date.Year == year)
                                    .Sum(i => i.TaxableAmount);
                var answer = "";
                if (year == DateTime.Now.Year)
                    answer = $"The running gross income is {grossIncome:0.00} EUR";
                else
                    answer = $"We grossed {grossIncome:0.00} EUR in {year}";
                return answer;
            }
        }

        public class QueryParameters
        {
            public DateTime? Date { get; set; }
            public int? Year { get; set; }
        }
    }
}
