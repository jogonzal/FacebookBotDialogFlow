using System;
using System.Linq;
using System.Threading.Tasks;

using FacebookBotDialogFlow.Flow;

using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;

namespace FacebookBotDialogFlow.Dialog
{
	[Serializable]
	internal class OptionsDialog : IDialog<DialogOption>
	{
		private readonly BotFlow _botflow;

		public OptionsDialog(BotFlow botflow)
		{
			_botflow = botflow;
		}

		/// <summary>
		/// Display the dialog to the user
		/// </summary>
		/// <param name="context"></param>
		/// <returns></returns>
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

		public async Task MessageReceivedAsync(IDialogContext context, IAwaitable<Message> argument)
		{
			var message = await argument;
			DialogOption option;
			if (!_botflow.FindAnswer(message.Text, out option))
			{
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
