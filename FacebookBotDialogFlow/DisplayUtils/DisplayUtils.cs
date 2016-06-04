using System.Collections.Generic;
using System.Linq;

using FacebookBotDialogFlow.Dialog;
using FacebookBotDialogFlow.Flow;
using Microsoft.Bot.Connector;

namespace FacebookBotDialogFlow.DisplayUtils
{
	internal class DisplayUtils
	{
		/// <summary>
		/// Decorate a message so it displays options "facebook style"
		/// </summary>
		internal static void AddActionsToMessage(Message message, BotFlow flow)
		{
			List<Action> actions = flow.Options.Select(n => new Action()
			{
				Title = n.OptionString,
				Message = n.OptionString
			}).ToList();

			message.Attachments = new List<Attachment>()
				{
					new Attachment()
					{
						Text = flow.Message,
						Actions = actions,
						ThumbnailUrl = flow.ImageUrl
					}
				};

			message.Text = flow.Message;
		}
	}
}
