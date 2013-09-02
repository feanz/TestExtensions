using System;

namespace TestExtensions.FluentBDD.Processors.Reporters
{
	public interface IHtmlReportConfiguration
	{
		string ReportHeader { get; set; }
		string ReportDescription { get; set; }
		string OutputPath { get; }
		string OutputFileName { get; }

		void InitReportConfig(Type testObjectType);
	}
}