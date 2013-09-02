using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using TestExtensions.FluentBDD.Processors.Reporters.HTML;

namespace TestExtensions.FluentBDD.Processors.Reporters
{
	public class HtmlReporter : IBatchProcessor
	{
		private readonly IReportBuilder _builder;
		private readonly IReportWriter _writer;
		readonly IHtmlReportConfiguration _configuration;

		public HtmlReporter() : this(new DefaultHtmlReportConfiguration(), new HtmlReportBuilder(), new FileWriter()) { }

		public HtmlReporter(IHtmlReportConfiguration configuration, IReportBuilder builder, IReportWriter writer)
		{
			_configuration = configuration;
			_builder = builder;
			_writer = writer;
		}

		public void Process(IEnumerable<Story> stories)
		{
			SetupConfiguration(stories);

			WriteOutScriptFiles();

			WriteOutHtmlReport(stories);
		}

		private void SetupConfiguration(IEnumerable<Story> stories)
		{
			var scenario = stories.First().Scenarios.FirstOrDefault();

			if (scenario != null) _configuration.InitReportConfig(scenario.TestObject.GetType());
		}

		private void WriteOutHtmlReport(IEnumerable<Story> stories)
		{
			const string error = "There was an error compiling the html report: ";
			var viewModel = new HtmlReportViewModel(_configuration, stories);
			ShouldTheReportUseCustomization(viewModel);
			string report;

			try
			{
				report = _builder.CreateReport(viewModel);
			}
			catch (Exception ex)
			{
				report = error + ex.Message;
			}

			_writer.OutputReport(report, _configuration.OutputFileName, _configuration.OutputPath);
		}

		private void ShouldTheReportUseCustomization(HtmlReportViewModel viewModel)
		{
			var customStylesheet = Path.Combine(_configuration.OutputPath, "BDDCustom.css");
			viewModel.UseCustomStylesheet = File.Exists(customStylesheet);

			var customJavascript = Path.Combine(_configuration.OutputPath, "BDDCustom.js");
			viewModel.UseCustomJavascript = File.Exists(customJavascript);
		}

		void WriteOutScriptFiles()
		{
			_writer.OutputReport(HtmlResources.BDD_css, "BDD.css", _configuration.OutputPath);
			_writer.OutputReport(HtmlResources.jquery_1_7_1_min, "jquery-1.7.1.min.js", _configuration.OutputPath);
			_writer.OutputReport(HtmlResources.BDD_js, "BDD.js", _configuration.OutputPath);
		}
	}
}