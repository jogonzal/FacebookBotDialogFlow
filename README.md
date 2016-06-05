# FacebookBotDialogFlow
A framework for building simple Facebook bots that communicate based on dialogs that use buttons like these:

![Alt text](/dialog.PNG?raw=true "Facebook dialog example")

### Code structure
1. Implementation of DSL is in "FacebookBotDialogFlow"
2. Implementation of sample bots is described above and in SampleBots/BreakfastBot and SampleBots/QuizBot

### Development

1. Install Visual studio community edition 2015
2. Open FacebookBotDialogFlow.sln
3. To run a sample, set it as a startup project and hit "run" (Start with "BreakfastBot")
4. You can use the Microsoft bot framework bot emulator to interact with the bot

## Design rationale

The facebook bot framework provides APIs that allow us to build bots - we can interact with them by writing text or having them display options as buttons and clicking/tapping them. This DSL focuses on the latter.

These types of bots tend to be designed based on a flow chart that determines the things they will say to the user and how they are going to react based on user input.

One flowchart we can use as an example is the following for "Breakfastbot":

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
EndMessage: "Have a nice day!"
```

Keeping state for the flowchart options, knowing how to build the payload to send to the facebook bot framework and interpreting user input are the main tasks of this bot. One could imagine this type of bot could be useful for providing an interface for ordering and customizing a particular item, learning tools (quiz) among other interactive bot scenarios.

With the Microsoft Bot Framework (or with other bot frameworks), it is possible to build such a "flowchart" bot. However, structuring dialog classes and implementing them to provide support for this sort of bot is not as intuitive, and the code that is produced at the end will likely not look as much like the flowchart above. If this is the case, the code will be hard to write, maintain, and it will be more prone to have bugs.

The FacebookBotDialogFlow is a DSL implemented in C# that allows us to write code that looks like the flowchart above and produces a bot with such functionality.

Here's a working example for the flowchart described above:

```csharp

// This code goes in your WebApi controller

BotFlow myBotFlow =
		BotFlow.DisplayMessage("Hello I am breakfastbot and will do my best to serve you breakfast! Do you want milk?")
			.WithThumbnail("http://www.mysite.com/milk.png")
			.WithOption("Yes",
				BotFlow.DisplayMessage("Here's your milk.")
				.Do(() => Ordering.OrderMilk()))
			.WithOption("No",
				BotFlow.DisplayMessage("Well, then what do you want? ;)")
				.WithOption("Cookies",
					BotFlow.DisplayMessage("Here are your cookies")
					.Do(() => Ordering.OrderCookies()))
				.WithOption("Waffles",
					BotFlow.DisplayMessage("Here are your waffles")
					.Do(() => Ordering.OrderWaffles()))
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

Using these data structures you can form conditions, loops, and a flow to successfully interact with your user through a bot - the code closely ressembles the flowchart described above and will be easier to write and maintain.

## Example 2: Quizbot

```csharp
// Notice how we report scores on every wrong answer or every completed quiz
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
```