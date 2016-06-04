using System.Threading.Tasks;

namespace BreakfastBot.GoogleLookup
{
	public static class DatabaseLookup
	{
		public static Task<string> GetTop10Breakfasts()
		{
			// This simulates an I/O request to a database that returns top 10 breakfasts
			return Task.FromResult("toast, cereal, granola, milk, bacon, fruit, eggs");
		}
	}
}