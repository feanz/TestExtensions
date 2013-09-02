using System;
using TestExtensions.FluentBDD.Processors.Reporters.HTML;

namespace TestExtensions.FluentBDD.Processors.Reporters
{
	internal class HtmlReportTag : IDisposable
	{
		private readonly HtmlTag _tagName;
		private readonly Action<HtmlTag> _closeTagAction;

		public HtmlReportTag(HtmlTag tag, Action<HtmlTag> closeTagAction)
		{
			_tagName = tag;
			_closeTagAction = closeTagAction;
		}

		public void Dispose()
		{
			_closeTagAction(_tagName);
		}
	}
}