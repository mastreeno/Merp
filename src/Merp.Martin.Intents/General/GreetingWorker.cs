using System;
using System.Collections.Generic;
using System.Text;

namespace Merp.Martin.Intents.General
{
    public class GreetingWorker : IntentWorker
    {
        private readonly string[] PHRASES = new string[]
        {
            "Hi, I'm Martin and I'm here to help.",
            "Hi, ask me anyting about your accountancy.",
            "Hi, how can I help you?",
            "Hello, I'm hoping your day is running well; what are you asking for?",
            "Hi, once logged in, you can ask me about your accountancy; such as gross or net income, or query the status of an invoice. \nTry asking me: \"how much are we grossing?\"",
        };

        public GreetingWorker()
        {

        }

        public string Do()
        {
            var random = new Random();
            var index = random.Next(0, PHRASES.Length - 1);

            return PHRASES[index];
        }
    }
}
