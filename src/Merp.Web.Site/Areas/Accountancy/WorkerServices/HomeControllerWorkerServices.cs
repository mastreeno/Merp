using Merp.Accountancy.QueryStack;
using Merp.Web.Site.Areas.Accountancy.Models.Home;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Merp.Web.Site.Areas.Accountancy.WorkerServices
{
    public class HomeControllerWorkerServices
    {
        public IDatabase Database { get; set; }

        public HomeControllerWorkerServices(IDatabase database)
        {
            Database = database ?? throw new ArgumentNullException(nameof(database));
        }

        public IndexViewModel GetIndexViewModel()
        {
            var model = new IndexViewModel();
            model.OutstandingIncomingInvoicesCount = Database.IncomingInvoices.Outstanding().Count();
            model.OverdueIncomingInvoicesCount = Database.IncomingInvoices.Due().Count();
            model.OutstandingIncomingInvoicesTotalPrice = Database.IncomingInvoices.Outstanding().Sum(i => i.TotalPrice);
            model.OverdueIncomingInvoicesTotalPrice = Database.IncomingInvoices.Due().Sum(i => i.TotalPrice);

            model.OutstandingOutgoingInvoicesCount = Database.OutgoingInvoices.Outstanding().Count();
            model.OverdueOutgoingInvoicesCount = Database.OutgoingInvoices.Due().Count();
            model.OutstandingOutgoingInvoicesTotalPrice = Database.OutgoingInvoices.Outstanding().Sum(i => i.TotalPrice);
            model.OverdueOutgoingInvoicesTotalPrice = Database.OutgoingInvoices.Due().Sum(i => i.TotalPrice);
            return model;
        }
    }
}
