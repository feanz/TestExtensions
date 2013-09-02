using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;

namespace TestExtensions.FluentBDD.Processors.Reporters
{
	public class DefaultHtmlReportConfiguration : IHtmlReportConfiguration
	{
		private const string _outputFileName = "BDD_TestResults.html";
		private bool _init;

		private void SetReportDescription(Type testObjectType)
		{
			var descriptionAttribute = Assembly.GetAssembly(testObjectType)
				.GetCustomAttributes(typeof (AssemblyDescriptionAttribute), false)
				.OfType<AssemblyDescriptionAttribute>().FirstOrDefault();

			ReportDescription = (descriptionAttribute != null && !string.IsNullOrEmpty(descriptionAttribute.Description)) ? descriptionAttribute.Description : "Test Report";
		}

		private void SetReportHeader(Type testObjectType)
		{
			var assemblyName = StringUtil.CodeNameToNiceFormat(Assembly.GetAssembly(testObjectType).GetName().Name);

			if (assemblyName.EndsWith("tests", StringComparison.InvariantCultureIgnoreCase))
			{
				assemblyName = assemblyName.Substring(0, (assemblyName.Length - 5));
			}

			ReportHeader = Thread.CurrentThread.CurrentCulture.TextInfo.
				ToTitleCase(assemblyName.Replace(".", ""));
		}

		public void InitReportConfig(Type testObjectType)
		{
			if (!_init)
			{
				SetReportHeader(testObjectType);
				SetReportDescription(testObjectType);

				_init = true;
			}
		}

		public virtual string OutputFileName
		{
			get { return _outputFileName; }
		}

		public virtual string OutputPath
		{
			get { return AssemblyDirectory; }
		}

		public virtual string ReportDescription { get; set; }
		public virtual string ReportHeader { get; set; }

		// http://stackoverflow.com/questions/52797/c-how-do-i-get-the-path-of-the-assembly-the-code-is-in#answer-283917
		private static string AssemblyDirectory
		{
			get
			{
				var codeBase = Assembly.GetExecutingAssembly().CodeBase;
				var uri = new UriBuilder(codeBase);
				var path = Uri.UnescapeDataString(uri.Path);
				return Path.GetDirectoryName(path);
			}
		}
	}
}