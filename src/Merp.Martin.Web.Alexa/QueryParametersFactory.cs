using Alexa.NET.Request.Type;
using Merp.Martin.Intents.Accountancy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Merp.Martin.Web.Alexa
{
    public class QueryParametersFactory
    {
        public class Accountancy
        {
            public class OutgoingInvoicePaymentCheck
            {
                public static OutgoingInvoicePaymentCheckWorker.QueryParameters Parse(IntentRequest request)
                {
                    var qp = new OutgoingInvoicePaymentCheckWorker.QueryParameters();

                    //qp.CustomerName = result.Entities["Customer"]?.First()?.ToString();
                    //qp.SupplierName = result.Entities["Supplier"]?.First()?.ToString();
                    //qp.RawNumber = result.Entities["Invoice"]?.First()["invoiceNumber"]?.FirstOrDefault().ToString();

                    return qp;
                }
            }

            public class GrossIncome
            {
                public static GrossIncomeIntentWorker.QueryParameters Parse(IntentRequest request)
                {
                    var qp = new GrossIncomeIntentWorker.QueryParameters();
                    qp.Date = request.Intent.Slots["date"].Value == null ? (DateTime?)null : DateTime.Parse(request.Intent.Slots["date"].Value);
                    qp.Year = request.Intent.Slots["year"].Value == null ? (int?) null : int.Parse(request.Intent.Slots["year"].Value);
                    return qp;
                }
            }

            public class NetIncome
            {
                public static NetIncomeIntentWorker.QueryParameters Parse(IntentRequest request)
                {
                    var qp = new NetIncomeIntentWorker.QueryParameters();
                    qp.Date = request.Intent.Slots["date"].Value == null ? (DateTime?)null : DateTime.Parse(request.Intent.Slots["date"].Value);
                    qp.Year = request.Intent.Slots["year"].Value == null ? (int?)null : int.Parse(request.Intent.Slots["year"].Value);
                    return qp;
                }
            }
        }
    }
}
