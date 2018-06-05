using System;
using System.Configuration;
using System.Text;
using System.Threading.Tasks;
using LuisBot.Managers;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Luis;
using Microsoft.Bot.Builder.Luis.Models;

namespace Microsoft.Bot.Sample.LuisBot
{
    // For more information about this template visit http://aka.ms/azurebots-csharp-luis
    [Serializable]
    public class BasicLuisDialog : LuisDialog<object>
    {
        public BasicLuisDialog() : base(new LuisService(new LuisModelAttribute(
            ConfigurationManager.AppSettings["LuisAppId"], 
            ConfigurationManager.AppSettings["LuisAPIKey"], 
            domain: ConfigurationManager.AppSettings["LuisAPIHostName"])))
        {
        }

        [LuisIntent("None")]
        public async Task NoneIntent(IDialogContext context, LuisResult result)
        {
            await this.ShowLuisResult(context, result);
        }

        [LuisIntent("Cancel")]
        public async Task CancelIntent(IDialogContext context, LuisResult result)
        {
            await this.ShowLuisResult(context, result);
        }

        [LuisIntent("Help")]
        public async Task HelpIntent(IDialogContext context, LuisResult result)
        {
            await this.ShowLuisResult(context, result);
        }

        // Go to https://luis.ai and create a new intent, then train/publish your luis app.
        // Finally replace "Gretting" with the name of your newly created intent in the following handler
        [LuisIntent("Greeting")]
        public async Task GreetingIntent(IDialogContext context, LuisResult result)
        {
            await this.ShowLuisResult(context, result);
        }

        [LuisIntent("InvoiceQuery")]
        public async Task InvoiceQueryIntent(IDialogContext context, LuisResult result)
        {
            try
            {
                var manager = new InvoiceQueryIntentManager();
                await manager.ProduceLuisResult(context, result);
            }
            catch(Exception ex)
            {
                var message = $"{ex.GetType().FullName} - {ex.Message}";
                await context.PostAsync(message);
                await this.ShowLuisResult(context, result);
            }
        }

        [LuisIntent("Gross Income")]
        public async Task GrossIncomeQueryIntent(IDialogContext context, LuisResult result)
        {
            try
            {
                //await this.ShowLuisResult(context, result);
                var manager = new GrossIncomeQueryIntentManager();
                await manager.ProduceLuisResult(context, result);
            }
            catch (Exception ex)
            {
                var message = $"{ex.GetType().FullName} - {ex.Message}";
                await context.PostAsync(message);
                await this.ShowLuisResult(context, result);
            }
        }

        [LuisIntent("Net Income")]
        public async Task NetIncomeQueryIntent(IDialogContext context, LuisResult result)
        {
            try
            {
                //await this.ShowLuisResult(context, result);
                var manager = new NetIncomeQueryIntentManager();
                await manager.ProduceLuisResult(context, result);
            }
            catch (Exception ex)
            {
                var message = $"{ex.GetType().FullName} - {ex.Message}";
                await context.PostAsync(message);
                await this.ShowLuisResult(context, result);
            }
        }

        private async Task ShowLuisResult(IDialogContext context, LuisResult result) 
        {
            await context.PostAsync($"You said: {result.Query}");
            foreach (var intent in result.Intents)
            {
                await context.PostAsync($"You have reached {intent.Intent}.");
            }
            foreach (var entity in result.Entities)
            {
                await context.PostAsync($"Entity {entity.Type} was set to {entity.Entity}.");
            }
            context.Wait(MessageReceived);
        }
    }
}