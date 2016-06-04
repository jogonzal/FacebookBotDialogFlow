using System;
using FacebookBotDialogFlow.Flow;

namespace FacebookBotDialogFlow.Dialog
{
	[Serializable]
	public class DialogOption
	{
		public DialogOption(string option)
		{
			OptionString = option;
		}

		internal string OptionString { get; }

		internal BotFlow NextFlow { get; set; }
	}
}
