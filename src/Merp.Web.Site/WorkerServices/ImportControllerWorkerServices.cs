//using Feenpal.Tools.Import.Accountancy;
//using Feenpal.Tools.Import.Registry;
//using Merp.Registry.CommandStack.Commands;
//using Rebus.Bus;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading;
//using System.Threading.Tasks;

//namespace Merp.Web.Site.WorkerServices
//{
//    public class ImportControllerWorkerServices
//    {
//        public IBus Bus { get; set; }
//        public AccountancyImportEngine AccountancyImportEngine { get; private set; }
//        public RegistryImportEngine RegistryImportEngine { get; private set; }

//        public ImportControllerWorkerServices(IBus bus, AccountancyImportEngine accountancyImportEngine, RegistryImportEngine registryImportEngine)
//        {
//            this.Bus = bus ?? throw new ArgumentNullException(nameof(bus));
//            this.AccountancyImportEngine = accountancyImportEngine ?? throw new ArgumentNullException(nameof(accountancyImportEngine));
//            this.RegistryImportEngine = registryImportEngine ?? throw new ArgumentNullException(nameof(registryImportEngine));
//        }

//        public async Task Import()
//        {

//            var seconds = 1000;
//            var workers = 15;

//            //Registry
//            Bus.Advanced.Workers.SetNumberOfWorkers(workers);
//            RegistryImportEngine.RegisterPeople();
//            await Task.Delay(105 * seconds);

//            Bus.Advanced.Workers.SetNumberOfWorkers(1);
//            RegistryImportEngine.RegisterCompanies();
//            await Task.Delay(330 * seconds);

//            //Import job orders
//            Bus.Advanced.Workers.SetNumberOfWorkers(workers);
//            AccountancyImportEngine.RegisterJobOrders();
//            await Task.Delay(240 * seconds);

//            //Outgoinginvoices
//            Bus.Advanced.Workers.SetNumberOfWorkers(workers);
//            AccountancyImportEngine.ImportOutgoingInvoices();
//            await Task.Delay(360 * seconds);

//            Bus.Advanced.Workers.SetNumberOfWorkers(1);
//            AccountancyImportEngine.LinkOutgoingInvoicesToJobOrders();
//            await Task.Delay(360 * seconds);

//            Bus.Advanced.Workers.SetNumberOfWorkers(workers);
//            AccountancyImportEngine.MarkPaidOutgoingInvoices();
//            await Task.Delay(60 * seconds);

//            AccountancyImportEngine.MarkOutgoingInvoicesAsExpired();
//            await Task.Delay(120 * seconds);

//            //Incoming invoices
//            Bus.Advanced.Workers.SetNumberOfWorkers(workers);
//            AccountancyImportEngine.ImportIncomingInvoices();
//            await Task.Delay(360 * seconds);

//            Bus.Advanced.Workers.SetNumberOfWorkers(1);
//            AccountancyImportEngine.LinkIncomingInvoicesToJobOrders();
//            await Task.Delay(360 * seconds);

//            Bus.Advanced.Workers.SetNumberOfWorkers(workers);
//            AccountancyImportEngine.MarkPaidIncomingInvoices();
//            await Task.Delay(60 * seconds);

//            AccountancyImportEngine.MarkIncomingInvoicesAsExpired();
//            await Task.Delay(120 * seconds);

//            Bus.Advanced.Workers.SetNumberOfWorkers(workers);
//            AccountancyImportEngine.MarkJobOrdersAsCompleted();


//        }
//    }
//}
