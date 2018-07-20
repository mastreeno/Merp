using System;
using System.Collections.Generic;
using System.Linq;
using Merp.Web.Site.Areas.Sales.Models.Home;
using Merp.Sales.CommandStack.Commands;
using Merp.Sales.QueryStack;
using Merp.Sales.QueryStack.Model;
using MementoFX.Persistence;
using Microsoft.AspNetCore.Mvc.Rendering;
using Rebus.Bus;
using Merp.Domain.Model;

namespace Merp.Sales.Web.Areas.Sales.WorkerServices
{
    public class HomeControllerWorkerServices
    {
        public IBus Bus { get; private set; }
        public IDatabase Database { get; private set; }
        public IRepository Repository { get; private set; }
        public IEventStore EventStore { get; private set; }
        public HomeControllerWorkerServices(IBus bus, IDatabase database, IRepository repository, IEventStore eventStore)
        {
            this.Bus = bus ?? throw new ArgumentNullException(nameof(bus));
            this.Database = database ?? throw new ArgumentNullException(nameof(database));
            this.Repository = repository ?? throw new ArgumentNullException(nameof(repository));
            this.EventStore = eventStore ?? throw new ArgumentNullException(nameof(eventStore));
        }

        public IndexViewModel GetIndexViewModel()
        {
            //var customers = (from jo in Database.Projects
                               //orderby jo.CustomerName
                               //select new SelectListItem { Value = jo.CustomerId.ToString(), Text = jo.CustomerName })
                               //.Distinct()
                               //.ToList();
            //customers.Insert(0, new SelectListItem { Value = string.Empty, Text = string.Empty });
            var model = new IndexViewModel();
            //model.Customers = customers; 
            return model;
        }

        public IEnumerable<IndexViewModel.Project> GetList(bool currentOnly, Guid? customerId, string projectName)
        {
            var query = from jo in Database.Projects
                        orderby jo.Name
                        select new IndexViewModel.Project
                        {
                            CustomerId = jo.CustomerId,
                            IsCompleted = jo.IsCompleted,
                            Name = jo.Name,
                            Number = jo.Number,
                            Id = jo.Id
                        };
            if (currentOnly)
                query = query.Where(jo => jo.IsCompleted == false);
            if (customerId.HasValue)
                query = query.Where(jo => jo.CustomerId == customerId.Value);
            if (!string.IsNullOrEmpty(projectName))
                query = query.Where(jo => jo.Name.Contains(projectName));
            return query.Take(200).ToArray();
        }

        public IEnumerable<BalanceViewModel> GetBalanceViewModel(Guid projectId, DateTime dateFrom, DateTime dateTo, BalanceViewModel.Scale scale)
        {
            DateTime[] dates = null;          
            switch(scale)
            {
                case BalanceViewModel.Scale.Daily:
                case BalanceViewModel.Scale.Weekly:
                    var step = scale == BalanceViewModel.Scale.Weekly ? 7 : 1;
                    dates = EachDay(dateFrom, dateTo, step).ToArray();
                    break;
                case BalanceViewModel.Scale.Monthly:
                    dates = EachMonth(dateFrom, dateTo).ToArray();                  
                    break;
                case BalanceViewModel.Scale.Quarterly:
                    dates = EachQuarter(dateFrom, dateTo).ToArray();
                    break;
                case BalanceViewModel.Scale.Yearly:
                    dates = EachYear(dateFrom, dateTo).ToArray();
                    break;
            }
            var isTimeAndMaterial = (from jo in Database.Projects
                                     where jo.Id == projectId
                                     select jo.IsTimeAndMaterial).Single();
            Merp.Sales.CommandStack.Model.Project[] jobOrders = null;
            jobOrders = Repository.GetSeriesById<Merp.Sales.CommandStack.Model.Project>(projectId, dates);
            var model = new List<BalanceViewModel>();
            for(int i=0; i < dates.Count(); i++)
            {
                var balance = new BalanceViewModel()
                {
                    Date = dates[i],
                    Balance = jobOrders[i].Balance
                };
                model.Add(balance);
            }
            return model;
        }

        #region Projects
        public RegisterProjectViewModel GetRegisterProjectViewModel()
        {
            var model = new RegisterProjectViewModel();
            model.DateOfStart = DateTime.Now;
            model.DueDate = DateTime.Now;
            return model;
        }

        public ExtendProjectViewModel GetExtendJobOrderViewModel(Guid projectId)
        {
            var project = Repository.GetById<Merp.Sales.CommandStack.Model.Project>(projectId);
            var model = new ExtendProjectViewModel();
            if(project.DueDate.HasValue)
                model.NewDueDate = project.DueDate.Value;
            model.Price = project.Price.Amount;
            model.ProjectNumber = project.Number;
            model.ProjectId = project.Id;
            model.ProjectName = project.Name;
            return model;
        }

        public DetailViewModel GetProjectDetailViewModel(Guid projectId)
        {
            var project = Repository.GetById<Merp.Sales.CommandStack.Model.Project>(projectId);

            var model = new DetailViewModel();
            model.ManagerId = project.ManagerId;
            model.CustomerId = project.CustomerId;
            model.ContactPersonId = project.ContactPersonId;
            model.DateOfStart = project.DateOfStart;
            model.DueDate = project.DueDate;
            model.ProjectId = project.Id;
            model.ProjectNumber = project.Number;
            model.ProjectName = project.Name;
            model.Description = project.Description;
            model.Price = project?.Price?.Amount ?? 0;
            model.IsCompleted = project.IsCompleted;
            model.Balance = project.Balance;
                                     
            return model;
        }

        public MarkProjectAsCompletedViewModel GetMarkJobOrderAsCompletedViewModel(Guid projectId)
        {
            var project = Repository.GetById<Merp.Sales.CommandStack.Model.Project>(projectId);

            var model = new MarkProjectAsCompletedViewModel();
            model.DateOfCompletion = DateTime.Now;
            model.CustomerName = string.Empty;
            model.ProjectId = project.Id;
            model.ProjectNumber = project.Number;
            model.ProjectName = project.Name;
            return model;
        }

        public void CreateJobOrder(RegisterProjectViewModel model)
        {
            var price = new Money(model.Price, model.Currency);
            var command = new RegisterProjectCommand( 
                    model.Customer.OriginalId,
                    model.ContactPerson.OriginalId,
                    model.Manager.OriginalId,
                    price,
                    model.DateOfStart,
                    model.DueDate,
                    model.IsTimeAndMaterial,
                    model.Name, 
                    model.CustomerPurchaseOrderNumber,
                    model.Description
                );
            Bus.Send(command);
        }

        public void ExtendJobOrder(ExtendProjectViewModel model)
        {
            var command = new ExtendProjectCommand(model.ProjectId, model.NewDueDate, model.Price);
            Bus.Send(command);
        }

        public void MarkJobOrderAsCompleted(MarkProjectAsCompletedViewModel model)
        {
            var command = new MarkProjectAsCompletedCommand(model.ProjectId, model.DateOfCompletion);
            Bus.Send(command);
        }

        public decimal GetEvaluateProjectBalance(Guid projectId)
        {
            var jobOrder = Repository.GetById<Merp.Sales.CommandStack.Model.Project>(projectId, DateTime.Now);
            var balance = jobOrder.Balance;
            return balance;
        }
        #endregion

        private IEnumerable<DateTime> EachDay(DateTime from, DateTime thru)
        {
            return EachDay(from, thru, 1);
        }

        private IEnumerable<DateTime> EachDay(DateTime from, DateTime thru, int step)
        {
            for (var day = from.Date; day.Date <= thru.Date; day = day.AddDays(step))
                yield return day;
        }

        private IEnumerable<DateTime> EachMonth(DateTime from, DateTime thru)
        {
            for (var day = from.Date; day.Date <= thru.Date; day = day.AddMonths(1))
                yield return new DateTime(day.Year, day.Month, 1);
        }

        private IEnumerable<DateTime> EachQuarter(DateTime from, DateTime thru)
        {
            for (var day = from.Date; day.Date <= thru.Date; day = day.AddMonths(3))
                yield return new DateTime(day.Year, day.Month, 1);
        }

        private IEnumerable<DateTime> EachYear(DateTime from, DateTime thru)
        {
            for (var day = from.Date; day.Date <= thru.Date; day = day.AddYears(1))
                yield return new DateTime(day.Year, 12, 31);
        }
    }
}