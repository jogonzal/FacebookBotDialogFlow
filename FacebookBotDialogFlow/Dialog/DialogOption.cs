using System;
using FacebookBotDialogFlow.Flow;

namespace FacebookBotDialogFlow.Dialog
{
	/// <summary>
	/// Represents each of the options that the user will be able to click/tap on
	/// </summary>
	[Serializable]
	public class DialogOption
	{
		public DialogOption(string option)
		{
			OptionString = option;
		}

		internal string OptionString { get; }

		internal string Url { get; set; }

		internal BotFlow NextFlow { get; set; }
	}
}
