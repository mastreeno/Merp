using Merp.Accountancy.QueryStack;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Luis.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LuisBot.Managers
{
    public abstract class IntentManager
    {
        public abstract Task ProduceLuisResult(IDialogContext context, LuisResult result);

        protected AccountancyDbContext GetDbContext()
        {
            var readModelConnectionString = ConfigurationManager.ConnectionStrings["Merp-Accountancy-ReadModel"].ConnectionString;
            var optionsBuilder = new DbContextOptionsBuilder<AccountancyDbContext>();
            optionsBuilder.UseSqlServer(readModelConnectionString);
            var context = new AccountancyDbContext(optionsBuilder.Options);
            return context;
        }
    }
}
