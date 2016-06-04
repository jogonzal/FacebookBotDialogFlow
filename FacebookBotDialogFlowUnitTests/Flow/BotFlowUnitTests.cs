using FacebookBotDialogFlow.Flow;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FacebookBotDialogFlowUnitTests.Flow
{
	[TestClass]
	public class BotFlowUnitTests
	{
		[TestMethod]
		public void BotFlow_BuildSimpleDataStructure_Verify()
		{
			var myBotFlow =
				BotFlow.DisplayMessage("Hello! Do you want milk?", "http://www.mysite.com/milk.png")
					.WithOption("Yes",
								BotFlow.DisplayMessage("Here's your milk."))
					.WithOption("No",
								BotFlow.DisplayMessage("Well, then what do you want?")
								.WithOption("Cookies",
									BotFlow.DisplayMessage("Here are your cookies"))
								.WithOption("Waffles",
									BotFlow.DisplayMessage("Here are your waffles"))
								.WithOption("Nothing",
									BotFlow.DisplayMessage("Sorry, I don't have anything else for breakfast!")));

			myBotFlow.Message.Should().Be("Hello! Do you want milk?");
			myBotFlow.ImageUrl.Should().Be("http://www.mysite.com/milk.png");
			myBotFlow.Options.Count.Should().Be(2);
			myBotFlow.Options[0].Item1.OptionString.Should().Be("Yes");
			myBotFlow.Options[1].Item1.OptionString.Should().Be("No");
			myBotFlow.Options[0].Item2.Options.Should().BeEmpty();
			myBotFlow.Options[0].Item2.Message.Should().Be("Here's your milk.");
			myBotFlow.Options[1].Item2.Options.Should().HaveCount(3);
		}
	}
}
