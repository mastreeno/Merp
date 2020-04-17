using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Schema;
using Merp.Martin.Intents.Accountancy;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using Microsoft.Bot.Builder.Dialogs;
using Merp.Martin.Intents.General;
using System.Text.RegularExpressions;

namespace Merp.Martin.Web.BotFramework
{
    /// <summary>
    /// For each interaction from the user, an instance of this class is created and
    /// the OnTurnAsync method is called.
    /// This is a transient lifetime service. Transient lifetime services are created
    /// each time they're requested. For each <see cref="Activity"/> received, a new instance of this
    /// class is created. Objects that are expensive to construct, or have a lifetime
    /// beyond the single turn, should be carefully managed.
    /// </summary>
    /// <seealso cref="https://docs.microsoft.com/en-us/aspnet/core/fundamentals/dependency-injection?view=aspnetcore-2.1"/>
    /// <seealso cref="https://docs.microsoft.com/en-us/dotnet/api/microsoft.bot.ibot?view=botbuilder-dotnet-preview"/>
    public class LuisBot : IBot
    {
        /// <summary>
        /// Key in the bot config (.bot file) for the LUIS instance.
        /// In the .bot file, multiple instances of LUIS can be configured.
        /// </summary>
        public static readonly string LuisKey = "MartinAtMastreeno-en";

        private const string WelcomeText = "Hi, once logged in, you can ask me about your accountancy; such as gross or net income, or query the status of an invoice. \nTry asking me: \"how much are we grossing?\"";

        /// <summary>
        /// Services configured from the ".bot" file.
        /// </summary>
        private readonly BotServices _services;
        private readonly LuisBotAccessors _stateAccessors;

        private readonly GrossIncomeIntentWorker _grossIncomeIntentWorker;
        private readonly NetIncomeIntentWorker _netIncomeIntentWorker;
        private readonly OutgoingInvoicePaymentCheckWorker _outgoingInvoicePaymentCheckWorker;
        private readonly LogoutWorker _logoutWorker;
        private readonly HelpWorker _helpWorker;
        private readonly GreetingWorker _greetingWorker;
        private readonly ILoggerFactory _loggerFactory;

        private readonly DialogSet _dialogs;

        /// <summary>
        /// Initializes a new instance of the <see cref="LuisBot"/> class.
        /// </summary>
        /// <param name="services">Services configured from the ".bot" file.</param>
        public LuisBot(BotServices services, LuisBotAccessors accessors, GrossIncomeIntentWorker grossIncomeIntentWorker, NetIncomeIntentWorker netIncomeIntentWorker, OutgoingInvoicePaymentCheckWorker outgoingInvoicePaymentCheckWorker, LogoutWorker logoutWorker, HelpWorker helpWorker, GreetingWorker greetingWorker, ILoggerFactory loggerFactory)
        {
            _services = services ?? throw new System.ArgumentNullException(nameof(services));

            if (!_services.LuisServices.ContainsKey(LuisKey))
            {
                throw new System.ArgumentException($"Invalid configuration. Please check your '.bot' file for a LUIS service named '{LuisKey}'.");
            }

            _stateAccessors = accessors ?? throw new ArgumentNullException(nameof(accessors));

            _grossIncomeIntentWorker = grossIncomeIntentWorker ?? throw new System.ArgumentNullException(nameof(grossIncomeIntentWorker));
            _netIncomeIntentWorker = netIncomeIntentWorker ?? throw new System.ArgumentNullException(nameof(netIncomeIntentWorker));
            _outgoingInvoicePaymentCheckWorker = outgoingInvoicePaymentCheckWorker ?? throw new System.ArgumentNullException(nameof(outgoingInvoicePaymentCheckWorker));
            _logoutWorker = logoutWorker ?? throw new System.ArgumentNullException(nameof(logoutWorker));
            _helpWorker = helpWorker ?? throw new System.ArgumentNullException(nameof(helpWorker));
            _greetingWorker = greetingWorker ?? throw new System.ArgumentNullException(nameof(greetingWorker));
            _loggerFactory = loggerFactory ?? throw new ArgumentNullException(nameof(loggerFactory));

            _dialogs = new DialogSet(_stateAccessors.ConversationDialogState);

            // Add the OAuth prompts and related dialogs into the dialog set
            _dialogs.Add(LoginPrompt("MastreenoAuth"));
        }

        /// <summary>
        /// Every conversation turn for our LUIS Bot will call this method.
        /// There are no dialogs used, the sample only uses "single turn" processing,
        /// meaning a single request and response, with no stateful conversation.
        /// </summary>
        /// <param name="turnContext">A <see cref="ITurnContext"/> containing all the data needed
        /// for processing this conversation turn. </param>
        /// <param name="cancellationToken">(Optional) A <see cref="CancellationToken"/> that can be used by other objects
        /// or threads to receive notice of cancellation.</param>
        /// <returns>A <see cref="Task"/> that represents the work queued to execute.</returns>
        public async Task OnTurnAsync(ITurnContext turnContext, CancellationToken cancellationToken = default(CancellationToken))
        {
            try
            {
                if (turnContext.Activity.Type == ActivityTypes.Message)
                {
                    Regex regex = new Regex(@"^\d{6}$");
                    if (regex.IsMatch(turnContext.Activity.Text))
                    {
                        await ContinueLoginDialog(turnContext.Activity.Text, turnContext, cancellationToken);
                    }
                    else
                    {
                        var recognizerResult = await _services.LuisServices[LuisKey].RecognizeAsync(turnContext, cancellationToken);
                        var topIntent = recognizerResult?.GetTopScoringIntent();
                        if (topIntent != null && topIntent.HasValue && topIntent.Value.intent != "None")
                        {
                            var intentName = topIntent.Value.intent;
                            if (intentName == "Gross_Income")
                            {
                                var isUserSignedIn = await IsUserSignedIn(turnContext, cancellationToken);
                                if (isUserSignedIn == true)
                                {
                                    var queryParams = QueryParametersFactory
                                                    .Accountancy
                                                    .GrossIncome.Parse(recognizerResult);
                                    var msg = _grossIncomeIntentWorker.Do(queryParams);
                                    await turnContext.SendActivityAsync(msg);
                                }
                                else
                                {
                                    await StartLoginDialog(turnContext, cancellationToken);
                                }
                            }
                            else if (intentName == "Net_Income")
                            {
                                var isUserSignedIn = await IsUserSignedIn(turnContext, cancellationToken);
                                if (isUserSignedIn == true)
                                {
                                    var queryParams = QueryParametersFactory
                                                    .Accountancy
                                                    .NetIncome.Parse(recognizerResult);
                                    var msg = _netIncomeIntentWorker.Do(queryParams);
                                    await turnContext.SendActivityAsync(msg);
                                }
                                else
                                {
                                    await StartLoginDialog(turnContext, cancellationToken);
                                }
                            }
                            else if (intentName == "Outgoing_Invoice_Payment_Check")
                            {
                                var isUserSignedIn = await IsUserSignedIn(turnContext, cancellationToken);
                                if (isUserSignedIn == true)
                                {
                                    var queryParams = QueryParametersFactory
                                                    .Accountancy
                                                    .OutgoingInvoicePaymentCheck.Parse(recognizerResult);
                                    var msg = _outgoingInvoicePaymentCheckWorker.Do(queryParams);
                                    await turnContext.SendActivityAsync(msg);
                                }
                                else
                                {
                                    await StartLoginDialog(turnContext, cancellationToken);
                                }
                            }
                            else if (intentName == "Logout")
                            {
                                var msg = _logoutWorker.Do();
                                await LogoutFromMastreenoAuth(msg, turnContext, cancellationToken);
                            }
                            else if (intentName == "Help")
                            {
                                var msg = _helpWorker.Do();
                                await turnContext.SendActivityAsync(msg);
                            }
                            else if (intentName == "Greeting")
                            {
                                var msg = _greetingWorker.Do();
                                await turnContext.SendActivityAsync(msg);
                            }
                            else
                            {
                                await turnContext.SendActivityAsync($"==>LUIS Top Scoring Intent: {topIntent.Value.intent}, Score: {topIntent.Value.score}\n");
                            }
                        }
                        else
                        {
                            var msg = @"No LUIS intents were found.";
                            await turnContext.SendActivityAsync(msg);
                        }
                    }
                }
                else if (turnContext.Activity.Type == ActivityTypes.ConversationUpdate)
                {
                    // Send a welcome message to the user and tell them what actions they may perform to use this bot
                    await SendWelcomeMessageAsync(turnContext, cancellationToken);
                }
                else
                {
                    await turnContext.SendActivityAsync($"{turnContext.Activity.Type} event detected", cancellationToken: cancellationToken);
                }
            }
            catch (Exception ex)
            {
                ILogger logger = _loggerFactory.CreateLogger<LuisBot>();
                logger.LogError($"\r\nException caught : {ex.StackTrace}\r\n");
            }

        }

        /// <summary>
        /// On a conversation update activity sent to the bot, the bot will
        /// send a message to the any new user(s) that were added.
        /// </summary>
        /// <param name="turnContext">Provides the <see cref="ITurnContext"/> for the turn of the bot.</param>
        /// <param name="cancellationToken" >(Optional) A <see cref="CancellationToken"/> that can be used by other objects
        /// or threads to receive notice of cancellation.</param>
        /// <returns>>A <see cref="Task"/> representing the operation result of the Turn operation.</returns>
        private static async Task SendWelcomeMessageAsync(ITurnContext turnContext, CancellationToken cancellationToken)
        {
            foreach (var member in turnContext.Activity.MembersAdded)
            {
                if (member.Id != turnContext.Activity.Recipient.Id)
                {
                    await turnContext.SendActivityAsync(
                        $"Welcome to Martin {member.Name}. {WelcomeText}",
                        cancellationToken: cancellationToken);
                }
            }
        }

        private async Task<bool> IsUserSignedIn(ITurnContext turnContext, CancellationToken cancellationToken)
        {
            var userToken = await GetMastreenoAuthUserToken(turnContext, cancellationToken: cancellationToken);

            return userToken != null;
        }

        private async Task<TokenResponse> GetMastreenoAuthUserToken(ITurnContext turnContext, CancellationToken cancellationToken)
        {
            var botAdapter = (BotFrameworkAdapter)turnContext.Adapter;

            var userToken = await botAdapter.GetUserTokenAsync(turnContext, "MastreenoAuth", magicCode: string.Empty, cancellationToken: cancellationToken);

            return userToken;
        }

        private async Task StartLoginDialog(ITurnContext turnContext, CancellationToken cancellationToken)
        {
            var dc = await _dialogs.CreateContextAsync(turnContext, cancellationToken);

            await dc.BeginDialogAsync("Login", cancellationToken: cancellationToken);
        }

        private async Task ContinueLoginDialog(string magicCode, ITurnContext turnContext, CancellationToken cancellationToken)
        {
            var botAdapter = (BotFrameworkAdapter)turnContext.Adapter;

            var userToken = await botAdapter.GetUserTokenAsync(turnContext, "MastreenoAuth", magicCode: magicCode, cancellationToken: cancellationToken);
            await turnContext.SendActivityAsync("You have successfully signed in", cancellationToken: cancellationToken);
        }

        private static OAuthPrompt LoginPrompt(string connectionName)
        {
            return new OAuthPrompt(
                "Login",
                new OAuthPromptSettings
                {
                    ConnectionName = connectionName,
                    Text = "You have to login with your MERP account to continue.",
                    Title = "Pleas Sign In",
                    Timeout = 300000, // User has 5 minutes to login (1000 * 60 * 5)
                });
        }

        private static async Task LogoutFromMastreenoAuth(string message, ITurnContext turnContext, CancellationToken cancellationToken)
        {
            var botAdapter = (BotFrameworkAdapter)turnContext.Adapter;

            // The bot adapter encapsulates the authentication processes.
            await botAdapter.SignOutUserAsync(turnContext, "MastreenoAuth", cancellationToken: cancellationToken);
            await turnContext.SendActivityAsync(message, cancellationToken: cancellationToken);
        }

        
    }
}
