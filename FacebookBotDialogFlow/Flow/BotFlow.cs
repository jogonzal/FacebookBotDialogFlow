using System;
using System.Collections.Generic;
using FacebookBotDialogFlow.Dialog;

namespace FacebookBotDialogFlow.Flow
{
	/// <summary>
	/// Fluent-like interface that allows for easy building of data structures
	/// </summary>
	public class BotFlow
	{
		internal string ImageUrl { get; }
		internal string Message { get; }

		internal IList<Tuple<DialogOption, BotFlow>> Options { get; set; } 

		public BotFlow(string message, string imageUrl)
		{
			this.Message = message;
			this.ImageUrl = imageUrl;
			this.Options = new List<Tuple<DialogOption, BotFlow>>();
		}

		public static BotFlow DisplayMessage(string message, string imageUrl = null)
		{
			return new BotFlow(message, imageUrl);
		}

		public BotFlow WithOption(string optionString, BotFlow displayMessage)
		{
			Options.Add(new Tuple<DialogOption, BotFlow>(new DialogOption() {OptionString = optionString }, displayMessage));
			return this;
		}
	}
}
