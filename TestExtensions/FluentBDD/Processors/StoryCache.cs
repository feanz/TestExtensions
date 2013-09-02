using System.Collections.Generic;

namespace TestExtensions.FluentBDD.Processors
{
	public class StoryCache : IProcessor
	{
		private static readonly List<Story> _cache = new List<Story>();

		public void Process(Story story)
		{
			_cache.Add(story);
		}

		public ProcessType ProcessType
		{
			get { return ProcessType.Finally; }
		}

		public static IEnumerable<Story> Stories
		{
			get { return _cache; }
		}
	}
}