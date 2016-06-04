# FacebookBotDialogFlow
A framework for building simple Facebook bots that communicate based with dialogs

Facebook provides relatively APIs that allow bots to display options as buttons. Keeping state for those options, knowing how to build the payload for displaying options and how to implement dialogs can be hard. Usually, what we know is the dialog flow - like this:

```
StartMessage: "Hello! Do you want milk?" - (also, display an image of milk)
Options "Yes"
	Message "Here's your milk!"
Options "No"
	Message "Well, maybe you want to google something?"
	Options "Cookies"
		Message "Here are your cookies!"
	Options "Waffles"
		Message "Here are your waffles"
	Options "Nothing"
		Message "I tried!" - (also, display the top 10 google results for breakfast)
```

With botflow, you can easily build a conversation diagram literally by writing this code:

```csharp
[BotAuthentication]
public class MessagesController : ApiController
{
	public async Task<Message> Post([FromBody] Message message)
	{
		if (message.Type == "Message")
		{
			BotFlow myBotFlow =
					BotFlow.DisplayMessage("Hello! Do you want milk?", "http://www.wifss.ucdavis.edu/wp-content/uploads/2015/03/Milk-Pouring-istock-6x4.jpg")
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

	...
}
return myBotFlow;
									
public static class MyOwnClass{
	public static async Task<BotDialogMessage> GoogleSearchForTop10Breakfasts(){
		// Make a web request here...
	}
}
```