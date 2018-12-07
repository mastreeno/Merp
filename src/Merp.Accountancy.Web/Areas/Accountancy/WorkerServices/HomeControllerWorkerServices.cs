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
            model.OverdueIncomingInvoicesCount = Database.IncomingInvoices.Overdue().Count();
            model.OutstandingIncomingInvoicesTotalPrice = Database.IncomingInvoices.Outstanding().Sum(i => i.TotalPrice);
            model.OverdueIncomingInvoicesTotalPrice = Database.IncomingInvoices.Overdue().Sum(i => i.TotalPrice);

            model.OutstandingOutgoingInvoicesCount = Database.OutgoingInvoices.Outstanding().Count();
            model.OverdueOutgoingInvoicesCount = Database.OutgoingInvoices.Overdue().Count();
            model.OutstandingOutgoingInvoicesTotalPrice = Database.OutgoingInvoices.Outstanding().Sum(i => i.TotalPrice);
            model.OverdueOutgoingInvoicesTotalPrice = Database.OutgoingInvoices.Overdue().Sum(i => i.TotalPrice);
            return model;
        }
    }
}
