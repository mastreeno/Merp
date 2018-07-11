using Merp.Accountancy.CommandStack.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using Merp.Web.Site.Areas.ProjectManagement.Models.Project;
using Merp.Accountancy.QueryStack;
using Merp.Accountancy.QueryStack.Model;
using MementoFX.Persistence;
using Microsoft.AspNetCore.Mvc.Rendering;
using Rebus.Bus;

namespace Merp.Web.Site.Areas.Accountancy.WorkerServices
{
    public class ProjectControllerWorkerServices
    {
        public IBus Bus { get; private set; }
        public IDatabase Database { get; private set; }
        public IRepository Repository { get; private set; }
        public IEventStore EventStore { get; private set; }
        public ProjectControllerWorkerServices(IBus bus, IDatabase database, IRepository repository, IEventStore eventStore)
        {
            if(bus==null)
                throw new ArgumentNullException(nameof(bus));
            if (database == null)
                throw new ArgumentNullException(nameof(database));
            if (repository == null)
                throw new ArgumentNullException(nameof(repository));
            if (eventStore == null)
                throw new ArgumentNullException(nameof(eventStore));

            this.Bus = bus;
            this.Database = database;
            this.Repository = repository;
            this.EventStore = eventStore;
        }

        public IndexViewModel GetIndexViewModel()
        {
            var customers = (from jo in Database.JobOrders
                               orderby jo.CustomerName
                               select new SelectListItem { Value = jo.CustomerId.ToString(), Text = jo.CustomerName })
                               .Distinct()
                               .ToList();
            customers.Insert(0, new SelectListItem { Value = string.Empty, Text = string.Empty });
            var model = new IndexViewModel();
            model.Customers = customers; 
            return model;
        }

        public IEnumerable<IndexViewModel.Project> GetList(bool currentOnly, Guid? customerId, string jobOrderName)
        {
            var query = from jo in Database.JobOrders
                        orderby jo.CustomerName, jo.Name
                        select new IndexViewModel.Project
                        {
                            CustomerId = jo.CustomerId,
                            CustomerName = jo.CustomerName,
                            IsCompleted = jo.IsCompleted,
                            Name = jo.Name,
                            Number = jo.Number,
                            Id = jo.OriginalId
                        };
            if (currentOnly)
                query = query.Where(jo => jo.IsCompleted == false);
            if (customerId.HasValue)
                query = query.Where(jo => jo.CustomerId == customerId.Value);
            if (!string.IsNullOrEmpty(jobOrderName))
                query = query.Where(jo => jo.Name.Contains(jobOrderName));
            return query.Take(200).ToArray();
        }

        public IEnumerable<BalanceViewModel> GetBalanceViewModel(Guid jobOrderId, DateTime dateFrom, DateTime dateTo, BalanceViewModel.Scale scale)
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
            var isTimeAndMaterial = (from jo in Database.JobOrders
                                     where jo.OriginalId == jobOrderId
                                     select jo.IsTimeAndMaterial).Single();
            Merp.Accountancy.CommandStack.Model.JobOrder[] jobOrders = null;
            jobOrders = Repository.GetSeriesById<Merp.Accountancy.CommandStack.Model.JobOrder>(jobOrderId, dates);
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

        #region Job Orders
        public RegisterProjectViewModel GetCreateJobOrderViewModel()
        {
            var model = new RegisterProjectViewModel();
            model.DateOfStart = DateTime.Now;
            model.DueDate = DateTime.Now;
            return model;
        }

        public ExtendProjectViewModel GetExtendJobOrderViewModel(Guid jobOrderId)
        {
            var jobOrder = Repository.GetById<Merp.Accountancy.CommandStack.Model.JobOrder>(jobOrderId);
            var model = new ExtendProjectViewModel();
            model.NewDueDate = jobOrder.DueDate;
            model.Price = jobOrder.Price.Amount;
            model.ProjectNumber = jobOrder.Number;
            model.ProjectId = jobOrder.Id;
            model.ProjectName = jobOrder.Name;
            model.CustomerName = string.Empty;
            return model;
        }

        public DetailViewModel GetJobOrderDetailViewModel(Guid jobOrderId)
        {
            var jobOrder = Repository.GetById<Merp.Accountancy.CommandStack.Model.JobOrder>(jobOrderId);

            var model = new DetailViewModel();
            model.ManagerId = jobOrder.ManagerId;
            model.CustomerId = jobOrder.CustomerId;
            model.ContactPersonId = jobOrder.ContactPersonId;
            model.DateOfStart = jobOrder.DateOfStart;
            model.DueDate = jobOrder.DueDate;
            model.ProjectId = jobOrder.Id;
            model.ProjectNumber = jobOrder.Number;
            model.ProjectName = jobOrder.Name;
            model.Description = jobOrder.Description;
            model.Price = jobOrder?.Price?.Amount ?? 0;
            model.IsCompleted = jobOrder.IsCompleted;
            model.Balance = jobOrder.Balance;
                                     
            return model;
        }

        public MarkProjectAsCompletedViewModel GetMarkJobOrderAsCompletedViewModel(Guid jobOrderId)
        {
            var jobOrder = Repository.GetById<Merp.Accountancy.CommandStack.Model.JobOrder>(jobOrderId);

            var model = new MarkProjectAsCompletedViewModel();
            model.DateOfCompletion = DateTime.Now;
            model.CustomerName = string.Empty;
            model.ProjectId = jobOrder.Id;
            model.ProjectNumber = jobOrder.Number;
            model.ProjectName = jobOrder.Name;
            return model;
        }

        public void CreateJobOrder(RegisterProjectViewModel model)
        {
            var command = new RegisterJobOrderCommand( 
                    model.Customer.OriginalId,
                    model.Customer.Name,
                    model.ContactPerson.OriginalId,
                    model.Manager.OriginalId,
                    model.Price.Amount,
                    model.Price.Currency,
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
            var command = new ExtendJobOrderCommand(model.ProjectId, model.NewDueDate, model.Price);
            Bus.Send(command);
        }

        public void MarkJobOrderAsCompleted(MarkProjectAsCompletedViewModel model)
        {
            var command = new MarkJobOrderAsCompletedCommand(model.ProjectId, model.DateOfCompletion);
            Bus.Send(command);
        }

        public decimal GetEvaluateJobOrderBalance(Guid jobOrderId)
        {
            var jobOrder = Repository.GetById<Merp.Accountancy.CommandStack.Model.JobOrder>(jobOrderId, DateTime.Now);
            var balance = jobOrder.Balance; // jobOrder.CalculateBalance(EventStore);
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