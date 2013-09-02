using System;

namespace TestExtensions.FluentBDD
{
	public class StoryMetaData
	{
		public StoryMetaData(Type storyType, StoryAttribute storyAttribute)
		{
			var title = storyAttribute.Title;
			if (string.IsNullOrEmpty(title))
				title = StringUtil.CodeNameToNiceFormat(storyType.Name);

			Type = storyType;
			Title = title;

			AsA = storyAttribute.AsA;
			IWant = storyAttribute.IWant;
			SoThat = storyAttribute.SoThat;
		}

		public StoryMetaData(Type storyType, string asA, string iWant, string soThat, string storyTitle = null)
		{
			Title = storyTitle ?? StringUtil.CodeNameToNiceFormat(storyType.Name);
			Type = storyType;

			AsA = asA;
			IWant = iWant;
			SoThat = soThat;
		}

		public Type Type { get; private set; }
		public string Title { get; private set; }
		public string AsA { get; private set; }
		public string IWant { get; private set; }
		public string SoThat { get; private set; }
	}
}