using System.Collections.Generic;
using TestExtensions.FluentBDD.Processors;
using TestExtensions.FluentBDD.Processors.Reporters;

namespace TestExtensions.FluentBDD.Configuration
{
	/// <summary>
	/// This class holds our processors.  At the moment its just hard coded this could be updated to load from factories or configuration for additional execution steps or reporters.
	/// </summary>
	public class Processors
	{
		public static IEnumerable<IBatchProcessor> GetBatchProcessors()
		{
			yield return new HtmlReporter();
		}

		public static IEnumerable<IProcessor> GetProcessors()
		{
			yield return new TestRunner();
			yield return new ConsoleReporter();
			yield return new ExceptionProcessor();
			yield return new StoryCache();
		}
	}
}