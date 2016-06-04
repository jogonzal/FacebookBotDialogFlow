# FacebookBotDialogFlow
A framework for building simple Facebook bots that communicate based on dialogs that use buttons like these:

![Alt text](/dialog.png?raw=true "Facebook dialog example")

Facebook provides APIs that allow bots to display options as buttons. Keeping state for those options, knowing how to build the payload for displaying them interpreting answers can be hard. Usually, when we design a bot we have clarity on the following:

```
StartMessage: "Hello I am breakfastbot and will do my best to serve you breakfast! Do you want milk?"
Options "Yes"
	Message "Here's your milk!"
Options "No"
	Message "Well, maybe you want to google something?"
	Options "Cookies"
		Message "Here are your cookies!"
	Options "Waffles"
		Message "Here are your waffles"
	Options "Nothing"
		// Consult a database and build a list of other potential breakfasts based on this
		Message "Maybe you want some ideas? IDEAS HERE Do these sound any good?"
		Options "Yes"
			Message "Here's a link to our website"
		Options "No"
			Message "You must be kidding! Everybody loves breakfast!"
EndMessage: 
```

With this DSL, you're able to write the dialog flow in a fashion that very closely ressembles your flow diagrams :grinning:

```csharp
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
```

Using these data structures you can form conditions, loops, and take action to successfully interact with your user through a bot.

This library uses the Microsoft Bot Framework. See "BreakFastBot" project as an example.

### Development

1. Install Visual studio community edition 2015
2. Open FacebookBotDialogFlow.sln
3. To run a sample, set it as a startup project and hit "run"
4. You can use the Microsoft bot framework bot emulator to interact with the bot