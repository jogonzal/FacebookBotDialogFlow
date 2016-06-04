using System.Threading.Tasks;
using System.Web.Http;
using FacebookBotDialogFlow.Flow;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;

namespace BreakfastBot.Controllers
{
	[BotAuthentication]
	public class MessagesController : ApiController
	{
		public async Task<Message> Post([FromBody] Message message)
		{
			if (message.Type == "Message")
			{
				BotFlow myBotFlow =
						BotFlow.DisplayMessage("Hello! Do you want milk?", "http://www.mysite.com/milk.png")
							.WithOption("Yes",
										BotFlow.DisplayMessage("Here's your milk."))
							.WithOption("No",
										BotFlow.DisplayMessage("Well, then what do you want?")
										.WithOption("Cookies",
											BotFlow.DisplayMessage("Here are your cookies"))
										.WithOption("Waffles",
											BotFlow.DisplayMessage("Here are your waffles"))
										.WithOption("Nothing"))
						.FinishWith("You have been served breakfast!");

				return await Conversation.SendAsync(message, () => myBotFlow.BuildDialogChain());
			}

			return HandleSystemMessage(message);
		}

		private Message HandleSystemMessage(Message message)
		{
			if (message.Type == "Ping")
			{
				var reply = message.CreateReplyMessage();
				reply.Type = "Ping";
				return reply;
			}
			if (message.Type == "DeleteUserData")
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