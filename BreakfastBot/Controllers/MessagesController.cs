using System.Threading.Tasks;
using System.Web.Http;
using BreakfastBot.GoogleLookup;
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
						BotFlow.DisplayMessage("Hello I am breakfastbot and will do my best to serve you breakfast! Do you want milk?")
							.WithThumbnail("http://www.mysite.com/milk.png")
							.WithOption("Yes",
								BotFlow.DisplayMessage("Here's your milk."))
							.WithOption("No",
								BotFlow.DisplayMessage("Well, then what do you want? ;)")
								.WithOption("Cookies",
									BotFlow.DisplayMessage("Here are your cookies"))
								.WithOption("Waffles",
									BotFlow.DisplayMessage("Here are your waffles"))
								.WithOption("Nothing",
									BotFlow.CalculateMessage(async () =>
									{
										// Retrieve breakfast from database
										string top10Breakfasts = await DatabaseLookup.GetTop10Breakfasts();
										return "Maybe you want some ideas - here are the top 10 breakfasts according to our database: " + top10Breakfasts +
										". Do any of these sound good?";
									})
									.WithOption("Yes",
										BotFlow.DisplayMessage("Great! Click here to browse to our breakfast webpage to find them.")
										.WithOptionLink("Breakfast website", "http://www.microsoft.com")
									)
									.WithOption("No",
										BotFlow.DisplayMessage("You must be kidding - everybody likes breakfast!")
									)
							)
						)
						.FinishWith("Have a great day!");

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