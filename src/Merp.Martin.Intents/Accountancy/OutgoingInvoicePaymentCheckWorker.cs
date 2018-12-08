using Merp.Accountancy.QueryStack;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace Merp.Martin.Intents.Accountancy
{
    public class OutgoingInvoicePaymentCheckWorker : IntentWorker
    {
        public AccountancyDbContext ActiveDbContext { get; private set; }

        public OutgoingInvoicePaymentCheckWorker(AccountancyDbContext context)
        {
            ActiveDbContext = context ?? throw new ArgumentNullException(nameof(context));
        }

        public string Do(QueryParameters parameters)
        {
            using (var db = new Database(ActiveDbContext))
            {
                var invoice = db.OutgoingInvoices.Where(i => i.Number == parameters.Number).FirstOrDefault();
                if (invoice == null)
                {
                    return $"Unable to find invoice number {parameters.Number} in the database.";
                }
                else
                {
                    if (invoice.IsPaid)
                        return $"YAY! Invoice number {invoice.Number} was paid on {invoice.PaymentDate:dd/MM/yyyy}";
                    else
                        return "Nope, it's still due :-(";
                }
            }
        }

        public class QueryParameters
        {
            public string RawNumber { get; set; }
            public string CustomerName { get; set; }
            public string SupplierName { get; set; }
            public string Month { get; set; }
            public string Ordinal { get; set; }

            public string Number
            {
                get
                {
                    if (int.TryParse(RawNumber, out _))
                        return $"{RawNumber}/{DateTime.Now.Year}";
                    else
                        return RawNumber.Replace(" ", "");
                }
            }
        }
    }
}
