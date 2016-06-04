using System.Linq;
using FacebookBotDialogFlow.Dialog;

namespace FacebookBotDialogFlow.Flow
{
	public static class BotFlowExtensions
	{
		public static bool FindAnswer(this BotFlow botflow, string text, out DialogOption result)
		{
			var resultOption = botflow.Options.Where(o => o.OptionString.ToLowerInvariant() == text.ToLowerInvariant());
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
