namespace TestExtensions.FluentBDD.Processors.Reporters
{
	public interface IReportBuilder
	{
		string CreateReport(FileReportModel model);
	}
}