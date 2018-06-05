using Merp.Accountancy.QueryStack;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Luis.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace LuisBot.Managers
{
    public class InvoiceQueryIntentManager : IntentManager
    {
        public override async Task ProduceLuisResult(IDialogContext context, LuisResult result)
        {
            var answer = string.Empty;
            var invoice = new Invoice(result);
            if (!string.IsNullOrWhiteSpace(invoice.Number))
            {
                answer = QueryOutgoingInvoiceByNumber(invoice);
            }
            else
                answer = $"Can't answer your question about invoice number {invoice.Number} issued by {invoice.SupplierName} to {invoice.CustomerName}, I'm afraid.";
            await context.PostAsync(answer);
        }

        public string QueryOutgoingInvoiceByNumber(Invoice invoice)
        {
            var context = GetDbContext();
            using (var db = new Database(context))
            {
                var inv = db.OutgoingInvoices.Where(i => i.Number == invoice.Number).FirstOrDefault();
                if(inv == null)
                {
                    return $"Unable to find invoice number {invoice.Number} in the database.";
                }
                else
                {
                    if (inv.IsPaid)
                        return $"YAY! Invoice number {invoice.Number} was paid on {inv.PaymentDate:dd/MM/yyyy}";
                    else
                        return "Nope, it's still due :-(";
                }
            }
        }

        //public delegate string IntentManagerExecute(Invoice invoice);
        
        public class Invoice
        {
            public string Number { get; set; }
            public string CustomerName { get; set; }
            public string SupplierName { get; set; }

            public Invoice(LuisResult result)
            {
                Number = result.Entities.Any(e => e.Type == "invoiceNumber") ?
                            result.Entities.Where(e => e.Type == "invoiceNumber").First().Entity : null;
                if (int.TryParse(Number, out _))
                    Number = $"{Number}/{DateTime.Now.Year}";
                else
                    Number = Number.Replace(" ", "");

                CustomerName = result.Entities.Any(e => e.Type == "Party::Customer") ?
                            result.Entities.Where(e => e.Type == "Party::Customer").First().Entity : null;

                SupplierName = result.Entities.Any(e => e.Type == "Party::Supplier") ?
                            result.Entities.Where(e => e.Type == "Party::Supplier").First().Entity : null;
            }
        }
    }
}