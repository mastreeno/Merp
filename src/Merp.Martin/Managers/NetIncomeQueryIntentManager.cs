using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Luis.Models;
using System;
using System.Linq;
using System.Threading.Tasks;
using Merp.Accountancy.QueryStack;

namespace LuisBot.Managers
{
    public class NetIncomeQueryIntentManager : IntentManager
    {
        public override async Task ProduceLuisResult(IDialogContext context, LuisResult result)
        {
            var parameters = new QueryParameters(result);
            var dbContext = GetDbContext();
            using (var db = new Database(dbContext))
            {
                var grossIncome = db.OutgoingInvoices
                                    .Where(i => i.Date.Year == parameters.Year)
                                    .Sum(i => i.TaxableAmount);
                var costs = db.IncomingInvoices
                    .Where(i => i.Date.Year == parameters.Year)
                    .Sum(i => i.TaxableAmount);
                var netIncome = grossIncome - costs;

                var answer = "";
                if (parameters.Year == DateTime.Now.Year)
                    answer = $"The running net income is {netIncome:0.00} EUR";
                else
                    answer = $"We netted {netIncome:0.00} EUR in {parameters.Year}";
                await context.PostAsync(answer);
            }
        }

        public class QueryParameters
        {
            public int Year { get; set; }

            public QueryParameters(LuisResult result)
            {
                Year = result.Entities.Any(e => e.Type == "builtin.datetimeV2.daterange") ?
                            int.Parse(result.Entities.Where(e => e.Type == "builtin.datetimeV2.daterange").First().Entity) : DateTime.Now.Year;
            }
        }
    }
}