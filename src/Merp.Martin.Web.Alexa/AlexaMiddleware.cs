using Merp.Martin.Intents.Accountancy;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Alexa.NET.Request;
using Alexa.NET.Request.Type;
using Alexa.NET.Response;
using Alexa.NET;

namespace Merp.Martin.Web.Alexa
{
    public class AlexaMiddleware
    {
        private readonly RequestDelegate _next;

        public AlexaMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, GrossIncomeIntentWorker grossIncomeIntentWorker, NetIncomeIntentWorker netIncomeIntentWorker, OutgoingInvoicePaymentCheckWorker outgoingInvoicePaymentCheckWorker)
        {
            var streamReader = new StreamReader(context.Request.Body);
            var rawBody = await streamReader.ReadToEndAsync();
            var skillRequest = JsonConvert.DeserializeObject<SkillRequest>(rawBody.ToString());
            SkillResponse response = null;
            if(skillRequest.Request is IntentRequest)
            {
                var request = (IntentRequest) skillRequest.Request;
                var intentName = request.Intent.Name;
                if (intentName == "Gross_Income")
                {
                    var queryParams = QueryParametersFactory
                                        .Accountancy
                                        .GrossIncome.Parse(request);
                    var msg = grossIncomeIntentWorker.Do(queryParams);
                    response = ResponseBuilder.Tell(msg);
                }
                else if (intentName == "Net_Income")
                {
                    var queryParams = QueryParametersFactory
                                        .Accountancy
                                        .NetIncome.Parse(request);
                    var msg = netIncomeIntentWorker.Do(queryParams);
                    response = ResponseBuilder.Tell(msg);
                }
                else if (intentName == "Outgoing_Invoice_Payment_Check")
                {
                    var queryParams = QueryParametersFactory
                                        .Accountancy
                                        .OutgoingInvoicePaymentCheck.Parse(request);
                    var msg = outgoingInvoicePaymentCheckWorker.Do(queryParams);
                    response = ResponseBuilder.Tell(msg);
                }
                else
                {
                    var msg = "Alas, I couldn't understand what you asked for.";
                    response = ResponseBuilder.Tell(msg);
                }
            }
            else if (skillRequest.Request is LaunchRequest)
            {
                var request = (LaunchRequest) skillRequest.Request;
                var msg = "Hi, I am Martin: how can I help you?";
                var reprompt = "Ground control to major Tom: Martin's still waiting for you here.";
                response = ResponseBuilder.Ask(msg, new Reprompt(reprompt));
            }
            var responseJson = JsonConvert.SerializeObject(response);
            await context.Response.WriteAsync(responseJson);
            await _next(context);
        }
    }
}
