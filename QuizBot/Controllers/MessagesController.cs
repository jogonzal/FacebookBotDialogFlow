using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using FacebookBotDialogFlow.Flow;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using Newtonsoft.Json;

namespace QuizBot
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
				var wrongAnswerBotFlow = BotFlow.DisplayMessage("Wrong answer!")
										.Do(() =>
										{
											/* Report score to the server */
										});
				var finishedBotFlow = BotFlow.DisplayMessage("Very nice! You got all questions right!")
										.Do(() =>
										{
											/* Report score to the server */
										});

				var thirdQuestionBotFlow = BotFlow.DisplayMessage("Nice answer! Now a tough one: How much is 7 * 7?")
					.WithOption("49", finishedBotFlow)
					.WithOption("54", wrongAnswerBotFlow)
					.WithOption("11", wrongAnswerBotFlow);

				var secondQuestion = BotFlow.DisplayMessage("Now a tough one: How much is 10 / 5?")
					.WithOption("2", thirdQuestionBotFlow)
					.Do(() =>
					{
						/* Report score to the server */
					})
					.WithOption("5", wrongAnswerBotFlow)
					.WithOption("1", wrongAnswerBotFlow);

				var firstQuestion = BotFlow.DisplayMessage("Let's do this! How much is 2 + 1?")
					.WithOption("1", wrongAnswerBotFlow)
					.WithOption("3", secondQuestion)
					.WithOption("4", wrongAnswerBotFlow);

				var botflow = BotFlow.DisplayMessage("This is a small math quiz - are you ready?")
					.WithOption("Yes", firstQuestion)
					.WithOption("No", BotFlow.DisplayMessage("Call me when you're ready!"))
					.FinishWith("Thanks for playing!");

				return await Conversation.SendAsync(message, () => botflow.BuildDialogChain());
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