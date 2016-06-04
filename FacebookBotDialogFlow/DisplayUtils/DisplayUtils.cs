using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FacebookBotDialogFlow.Flow;

using Microsoft.Bot.Connector;

namespace FacebookBotDialogFlow.DisplayUtils
{
	internal class DisplayUtils
	{
		/// <summary>
		/// Decorate a message so it displays options "facebook style"
		/// </summary>
		internal static async Task AddActionsToMessage(Message message, BotFlow flow)
		{
			List<Action> actions = null;
			if (flow.Options != null)
			{
				actions = flow.Options.Select(n => new Action()
				{
					Title = n.OptionString,
					Message = n.Url == null ? n.OptionString : null,
					Url = n.Url
				}).ToList();
			}

			var messageString = await flow.GetMessage();

			message.Attachments = new List<Attachment>()
				{
					new Attachment()
					{
						Text = messageString,
						Actions = actions,
						ThumbnailUrl = flow.ImageUrl
					}
				};

			message.Text = messageString;
		}
	}
}
