using System;
using System.Diagnostics;
using System.Linq;

namespace TestExtensions.FluentBDD
{
	public class StoryAttributeMetaDataScanner
	{
		public virtual StoryMetaData Scan(object testObject, Type explicitStoryType = null)
		{
			return GetStoryMetaData(testObject, explicitStoryType) ?? GetStoryMetaDataFromScenario(testObject);
		}

		protected virtual Type GetCandidateStory(object testObject, Type explicitStoryType)
		{
			if (explicitStoryType != null)
				return explicitStoryType;

			var stackTrace = new StackTrace();
			var frames = stackTrace.GetFrames();
			if (frames == null)
				return null;

			var scenarioType = testObject.GetType();
			// This is assuming scenario and story live in the same assembly
			var firstFrame = frames.LastOrDefault(f => f.GetMethod().DeclaringType.Assembly == scenarioType.Assembly);
			if (firstFrame == null)
				return null;

			return firstFrame.GetMethod().DeclaringType;
		}

		private static StoryAttribute GetStoryAttribute(Type candidateStoryType)
		{
			return (StoryAttribute) candidateStoryType.GetCustomAttributes(typeof (StoryAttribute), true).FirstOrDefault();
		}

		private static StoryMetaData GetStoryMetaDataFromScenario(object testObject)
		{
			var scenarioType = testObject.GetType();
			var storyAttribute = GetStoryAttribute(scenarioType);

			return storyAttribute == null ? null : new StoryMetaData(scenarioType, storyAttribute);
		}

		private StoryMetaData GetStoryMetaData(object testObject, Type explicateStoryType)
		{
			var candidateStoryType = GetCandidateStory(testObject, explicateStoryType);
			if (candidateStoryType == null)
				return null;

			var storyAttribute = GetStoryAttribute(candidateStoryType);
			if (storyAttribute == null)
				return null;

			return new StoryMetaData(candidateStoryType, storyAttribute);
		}
	}
}