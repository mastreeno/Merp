using System;
using System.Collections.Generic;
using System.Text;

namespace Merp.Martin.Intents.General
{
    public class HelpWorker : IntentWorker
    {
        public HelpWorker()
        {

        }

        public string Do()
        {
            return "Once logged in, you can ask me about your accountancy; such as gross or net income, or query the status of an invoice. \nTry asking me: \"how much are we grossing?\"";
        }
    }
}
