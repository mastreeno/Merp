using Merp.Martin.Intents.Accountancy;
using Microsoft.Bot.Builder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Merp.Martin.Web.BotFramework
{
    public class QueryParametersFactory
    {
        public class Accountancy
        {
            public class OutgoingInvoicePaymentCheck
            {
                public static OutgoingInvoicePaymentCheckWorker.QueryParameters Parse(RecognizerResult result)
                {
                    var qp = new OutgoingInvoicePaymentCheckWorker.QueryParameters();

                    qp.CustomerName = result.Entities["Customer"]?.First()?.ToString();
                    qp.SupplierName = result.Entities["Supplier"]?.First()?.ToString();
                    qp.RawNumber = result.Entities["Invoice"]?.First()["invoiceNumber"]?.FirstOrDefault().ToString();

                    return qp;
                }
            }

            public class GrossIncome
            {
                public static GrossIncomeIntentWorker.QueryParameters Parse(RecognizerResult result)
                {
                    var qp = new GrossIncomeIntentWorker.QueryParameters();
                    qp.Year = result.Entities.ContainsKey("datetime") ? 
                        int.Parse((string)result.Entities["datetime"].First()["timex"].First()) :
                        (int?) null;
                    return qp;
                }
            }

            public class NetIncome
            {
                public static NetIncomeIntentWorker.QueryParameters Parse(RecognizerResult result)
                {
                    var qp = new NetIncomeIntentWorker.QueryParameters();
                    qp.Year = result.Entities.ContainsKey("datetime") ?
                        int.Parse((string)result.Entities["datetime"].First()["timex"].First()) :
                        (int?)null;
                    return qp;
                }
            }
        }
    }
}
