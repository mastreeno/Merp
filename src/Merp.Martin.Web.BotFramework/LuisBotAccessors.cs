using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Merp.Martin.Web.BotFramework
{
    public class LuisBotAccessors
    {
        // The name of the dialog state.
        public static readonly string DialogStateName = $"{nameof(LuisBotAccessors)}.DialogState";

        /// <summary>
        /// Gets or Sets the DialogState accessor value.
        /// </summary>
        /// <value>
        /// A <see cref="DialogState"/> representing the state of the conversation.
        /// </value>
        public IStatePropertyAccessor<DialogState> ConversationDialogState { get; set; }
    }
}
