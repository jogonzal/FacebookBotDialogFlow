using System.Collections.Generic;
using System.Linq;

using FacebookBotDialogFlow.Dialog;

using Microsoft.Bot.Connector;

namespace FacebookBotDialogFlow.DisplayUtils
{
	internal class DisplayUtils
	{
		/// <summary>
		/// Decorate a message so it displays options "facebook style"
		/// </summary>
		internal static void AddActionsToMessage(Message message, string question, IList<DialogOption> dialogOptions)
		{
			List<Action> actions = dialogOptions.Select(n => new Action()
			{
				Title = n.OptionString,
				Message = n.OptionString,
				Url = null // framework does not support linking URLs yet
			}).ToList();

			message.Attachments = new List<Attachment>()
				{
					new Attachment()
					{
						Text = question,
						Actions = actions
					}
				};

			message.Text = question;
		}
	}
}
