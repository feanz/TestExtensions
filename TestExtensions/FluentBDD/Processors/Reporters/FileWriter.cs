using System;
using System.IO;

namespace TestExtensions.FluentBDD.Processors.Reporters
{
	public class FileWriter : IReportWriter
	{
		public void OutputReport(string reportData, string reportName, string outputDirectory = null)
		{
			var directory = outputDirectory ?? GetDefaultOutputDirectory;
			var path = Path.Combine(directory, reportName);

			if (File.Exists(path))
				File.Delete(path);
			File.WriteAllText(path, reportData);
		}

		private static string GetDefaultOutputDirectory
		{
			get
			{
				string codeBase = typeof(HtmlReporter).Assembly.CodeBase;
				var uri = new UriBuilder(codeBase);
				string path = Uri.UnescapeDataString(uri.Path);
				return Path.GetDirectoryName(path);
			}
		}
	}
}