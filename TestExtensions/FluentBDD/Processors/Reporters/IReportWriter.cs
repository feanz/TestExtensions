namespace TestExtensions.FluentBDD.Processors.Reporters
{
	public interface IReportWriter
	{
		void OutputReport(string reportData, string reportName, string outputDirectory = null);
	}
}