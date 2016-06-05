using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using FacebookBotDialogFlow.Dialog;

using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using Action = System.Action;

namespace FacebookBotDialogFlow.Flow
{
	/// <summary>
	/// Fluent-like interface that allows for easy building of data structures
	/// </summary>
	[Serializable]
	public class BotFlow
	{
		/// <summary>
		/// This function is called to retrieve the message - this might be done asynchronously
		/// </summary>
		private readonly Func<Task<string>> _messageCalculatingFunction;

		/// <summary>
		/// If there's a static message, it will be stored here
		/// </summary>
		private readonly string _message;

		/// <summary>
		/// The image url, if any
		/// </summary>
		internal string ImageUrl { get; set; }
		/// <summary>
		/// Message displayed at the end of the flow
		/// </summary>
		internal string CompletionMessage { get; set; }

		/// <summary>
		/// Options for this flow the user can click on
		/// </summary>
		internal IList<DialogOption> Options { get; set; }

		/// <summary>
		/// These will be called when the botflow starts
		/// </summary>
		internal List<Action> ActionsToPerformWhenCalled;

		/// <summary>
		/// Constructor
		/// </summary>
		internal BotFlow(string message, string imageUrl)
		{
			this._message = message;
			this._messageCalculatingFunction = RetrieveMessageSyncrhonously;

			this.ImageUrl = imageUrl;
			this.Options = new List<DialogOption>();
		}

		/// <summary>
		/// Constructor
		/// </summary>
		private BotFlow(Func<Task<string>> messageCalculatingFunction)
		{
			_messageCalculatingFunction = messageCalculatingFunction;
			this.Options = new List<DialogOption>();
		}

		/// <summary>
		/// Retrieves the message
		/// </summary>
		/// <returns></returns>
		internal async Task<string> GetMessage()
		{
			return await _messageCalculatingFunction();
		}

		/// <summary>
		/// Retrieves the stored message (For serialization purposes
		/// </summary>
		private Task<string> RetrieveMessageSyncrhonously()
		{
			return Task.FromResult(_message);
		}

		public void PerformAtions()
		{
			foreach (var action in ActionsToPerformWhenCalled)
			{
				try
				{
					action();
				}
				catch (Exception ex)
				{
					// Tolerate exceptions
					continue;
				}
			}
		}

		/// <summary>
		/// Display a message to the user in a bot message
		/// </summary>
		public static BotFlow DisplayMessage(string message, string imageUrl = null)
		{
			return new BotFlow(message, imageUrl);
		}

		/// <summary>
		/// Display a message to the user that will be calculated
		/// </summary>
		public static BotFlow CalculateMessage(Func<Task<string>> messageCalculatingFunction)
		{
			return new BotFlow(messageCalculatingFunction);
		}

		#region Fluent API

		/// <summary>
		/// Add an option to a message as a reply button
		/// </summary>
		public BotFlow WithOption(string optionString, BotFlow nextFlow = null)
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

		public BotFlow WithOptionLink(string message, string optionLink)
		{
			Options.Add(new DialogOption(message)
			{
				NextFlow = null,
				Url = optionLink
			});
			return this;
		}

		public BotFlow WithThumbnail(string thumbNailUrl)
		{
			ImageUrl = thumbNailUrl;
			return this;
		}

		public BotFlow Do(System.Action action)
		{
			return this;
		}

		#endregion

		/// <summary>
		/// Transforms a BotFlow into a dialog chain for the Microsoft bot framework
		/// </summary>
		/// <returns></returns>
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

		/// <summary>
		/// Recursively calls dialogs based on user input - basically asyncrhonously traverses the botflow graph
		/// </summary>
		public async Task<IDialog<string>> RecursiveCallToDialogs(IBotContext context, IAwaitable<DialogOption> item)
		{
			// Retrieve the dialog result
			DialogOption response = await item;

			if (response?.NextFlow == null)
			{
				return Chain.Return(CompletionMessage ?? "The dialog is complete!");
			}
			else
			{
				// Recurse into data structure
				return new OptionsDialog(response.NextFlow).ContinueWith(RecursiveCallToDialogs);
			}
		}

		/// <summary>
		/// Given a user input, try to get the next option (if possible)
		/// </summary>
		public bool TryGetAnswer(string text, out DialogOption result)
		{
			var resultOption = Options.Where(o => o.OptionString.ToLowerInvariant() == text.ToLowerInvariant());
			if (!resultOption.Any())
			{
				result = null;
				return false;
			}
			else
			{
				result = resultOption.First();
				return true;
			}
		}
	}
}
