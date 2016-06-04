using System;
using System.Collections.Generic;
using System.Linq;

using FacebookBotDialogFlow.Dialog;

namespace FacebookBotDialogFlow.Flow
{
	/// <summary>
	/// Fluent-like interface that allows for easy building of data structures
	/// </summary>
	[Serializable]
	public class BotFlow
	{
		internal string ImageUrl { get; }
		internal string Message { get; }

		internal IList<DialogOption> Options { get; set; } 

		internal BotFlow(string message, string imageUrl)
		{
			this.Message = message;
			this.ImageUrl = imageUrl;
			this.Options = new List<DialogOption>();
		}

		/// <summary>
		/// Display a message to the user in a bot message
		/// </summary>
		public static BotFlow DisplayMessage(string message, string imageUrl = null)
		{
			return new BotFlow(message, imageUrl);
		}

		/// <summary>
		/// Add an option to a message as a reply button
		/// </summary>
		public BotFlow WithOption(string optionString, BotFlow nextFlow)
		{
			Options.Add(new DialogOption(optionString)
			{
				NextFlow = nextFlow
			});
			return this;
		}
	}
}
