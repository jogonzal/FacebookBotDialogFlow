using System.Threading.Tasks;
using FacebookBotDialogFlow.Dialog;
using Microsoft.Bot.Builder.Dialogs;

namespace FacebookBotDialogFlow.Flow
{
	public static class FlowDialogBuilder
	{
		public static IDialog<string> BuildDialogChain(this BotFlow botflow)
		{
			return new OptionsDialog(botflow).ContinueWith(RecursiveCallToDialogs);
		}

		private static async Task<IDialog<string>> RecursiveCallToDialogs(IBotContext context, IAwaitable<DialogOption> item)
		{
			// Retrieve the dialog result
			DialogOption response = await item;

			if (response == null || response.NextFlow == null)
			{
				return Chain.Return("Done!");
			}
			else
			{
				// Recurse into data structure
				return new OptionsDialog(response.NextFlow).ContinueWith(RecursiveCallToDialogs);
			}
		}
	}
}
