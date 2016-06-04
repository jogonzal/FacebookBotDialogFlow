using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;

using FacebookBotDialogFlow.Dialog;

using FacebookBotDialogFlow.DisplayUtils;
using FacebookBotDialogFlow.Flow;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;


namespace InternalsTesTBot.Controllers
{
	[BotAuthentication]
	public class MessagesController : ApiController
	{
		/// <summary>
		/// POST: api/Messages
		/// Receive a message from a user and reply to it
		/// </summary>
		public async Task<Message> Post([FromBody]Message message)
		{
			if (message.Type == "Message")
			{
				if (message.Text.ToLowerInvariant() == "testui")
				{
					var reply = message.CreateReplyMessage();
					await DisplayUtils.AddActionsToMessage(reply, new BotFlow("Hi!", null)
					{
						Options = new List<DialogOption>()
						{
							new DialogOption("Option A")
						}
					});
					return reply;
				}
				else if (message.Text.ToLowerInvariant() == "countchars")
				{
					// calculate something for us to return
					int length = (message.Text ?? string.Empty).Length;

					// return our reply to the user
					return message.CreateReplyMessage($"You sent {length} characters");
				}
				else if (message.Text.ToLowerInvariant() == "testdialog")
				{
					return await Conversation.SendAsync(message, () => new OptionsDialog(new BotFlow("Hello", null)
					{
						Options = new List<DialogOption>()
						{
							new DialogOption("dog")
						}
					}));
				}
				else
				{
					// calculate something for us to return
					int length = (message.Text ?? string.Empty).Length;

					// return our reply to the user
					return message.CreateReplyMessage($"You sent {length} characters");
				}
			}
			else
			{
				return HandleSystemMessage(message);
			}
		}

		private Message HandleSystemMessage(Message message)
		{
			if (message.Type == "Ping")
			{
				Message reply = message.CreateReplyMessage();
				reply.Type = "Ping";
				return reply;
			}
			else if (message.Type == "DeleteUserData")
			{
				// Implement user deletion here
				// If we handle user deletion, return a real message
			}
			else if (message.Type == "BotAddedToConversation")
			{
			}
			else if (message.Type == "BotRemovedFromConversation")
			{
			}
			else if (message.Type == "UserAddedToConversation")
			{
			}
			else if (message.Type == "UserRemovedFromConversation")
			{
			}
			else if (message.Type == "EndOfConversation")
			{
			}

			return null;
		}
	}
}