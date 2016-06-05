using System;
using System.Linq;
using System.Threading.Tasks;

using FacebookBotDialogFlow.Flow;

using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;

namespace FacebookBotDialogFlow.Dialog
{
	/// <summary>
	/// Represents each of the questions and options we display to the user when asking him a question
	/// </summary>
	[Serializable]
	internal class OptionsDialog : IDialog<DialogOption>
	{
		/// <summary>
		/// Constains all the information relevant to the question
		/// </summary>
		private readonly BotFlow _botflow;

		public OptionsDialog(BotFlow botflow)
		{
			_botflow = botflow;
		}

		/// <summary>
		/// Display the dialog to the user - called when dialog starts
		/// </summary>
		public async Task StartAsync(IDialogContext context)
		{
			var msg = context.MakeMessage();
			await DisplayUtils.DisplayUtils.AddActionsToMessage(msg, _botflow);
			await context.PostAsync(msg);
			if (_botflow.Options != null && _botflow.Options.Where(o => o.Url == null).Count() > 0)
			{
				context.Wait(MessageReceivedAsync);
			}
			else
			{
				context.Done<DialogOption>(null);
			}
		}

		/// <summary>
		/// Called when a message is received to this dialog
		/// </summary>
		public async Task MessageReceivedAsync(IDialogContext context, IAwaitable<Message> argument)
		{
			var message = await argument;
			DialogOption option;
			if (!_botflow.TryGetAnswer(message.Text, out option))
			{
				// This should never happen, unless the user explicitly types something that is not recognized
				await context.PostAsync("I didn't understand your answer.");
				context.Wait(MessageReceivedAsync);
			}
			else
			{
				context.Done(option);
			}
		}
	}
}
