using System;
using System.Collections.Generic;
using System.Text;

namespace Merp.Martin.Intents.General
{
    public class LogoutWorker : IntentWorker
    {
        public LogoutWorker()
        {

        }

        public string Do()
        {
            return "You have been logged out from MERP.";
        }
    }
}
