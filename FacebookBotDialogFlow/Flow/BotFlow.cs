using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using FacebookBotDialogFlow.Dialog;

using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;

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
		internal string CompletionMessage { get; set; }

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

		/// <summary>
		/// Add a completion message for the top level dialog
		/// </summary>
		public BotFlow FinishWith(string completionMessage)
		{
			CompletionMessage = completionMessage;
			return this;
		}

		public IDialog<string> BuildDialogChain()
		{
			return Chain.PostToChain()
				.Switch(
					new Case<Message, IDialog<string>>((msg) => true, (IBotContext ctx, Message msg) =>
					{
						return Chain.ContinueWith(new OptionsDialog(this), RecursiveCallToDialogs);
					})
				)
				.Unwrap()
				.PostToUser();
		}

		public async Task<IDialog<string>> RecursiveCallToDialogs(IBotContext context, IAwaitable<DialogOption> item)
		{
			// Retrieve the dialog result
			DialogOption response = await item;

			if (response?.NextFlow == null)
			{
				return Chain.Return(CompletionMessage);
			}
			else
			{
				// Recurse into data structure
				return new OptionsDialog(response.NextFlow).ContinueWith(RecursiveCallToDialogs);
			}
		}
	}
}
