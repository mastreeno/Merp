using Merp.TimeTracking.QueryStack.CRUDCommands;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace Merp.TimeTracking.CommandStack.Tests.Commands
{
    [TestFixture]
    public class ActivityCommandsFixture
    {
        [Test]
        public void reject_should_create_reject_and_set_activity_rejectid()
        {
            var cmd = new ActivityCommands();
            var entries = new List<Tuple<int, string>>();
            entries.Add(new Tuple<int, string>(3, "with errors"));
            entries.Add(new Tuple<int, string>(4, "not accepted"));

            //cmd.RejectEntries(entries, "global reason1");
        }
    }
}
