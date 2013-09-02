using System;
using System.Collections.Generic;
using System.Linq;

namespace TestExtensions.FluentBDD.Processors.Reporters
{
	public class ConsoleReporter : IProcessor
	{
		private readonly List<Exception> _exceptions = new List<Exception>();
		private int _longestStepSentence;

		private void ReportOnStep(Scenario scenario, ExecutionStep step)
		{
			var message =
				string.Format
					("\t{0}  [{1}] ",
						PrefixWithSpaceIfRequired(step).PadRight(_longestStepSentence + 5),
						StringUtil.CodeNameToNiceFormat(step.Result.ToString()));

			// if all the steps have passed, there is no reason to make noise
			if (scenario.Result == StepExecutionResult.Passed)
				message = "\t" + PrefixWithSpaceIfRequired(step);

			if (step.Exception != null)
			{
				_exceptions.Add(step.Exception);

				var exceptionReference = string.Format("[Details at {0} below]", _exceptions.Count);
				if (!string.IsNullOrEmpty(step.Exception.Message))
					message += string.Format("[{0}] {1}", FlattenExceptionMessage(step.Exception.Message), exceptionReference);
				else
					message += string.Format("{0}", exceptionReference);
			}

			if (step.Result == StepExecutionResult.Inconclusive || step.Result == StepExecutionResult.NotImplemented)
				Console.ForegroundColor = ConsoleColor.Yellow;
			else if (step.Result == StepExecutionResult.Failed)
				Console.ForegroundColor = ConsoleColor.Red;
			else if (step.Result == StepExecutionResult.NotExecuted)
				Console.ForegroundColor = ConsoleColor.Gray;

			Console.WriteLine(message);
			Console.ForegroundColor = ConsoleColor.White;
		}

		public void Process(Story story)
		{
			ReportStoryHeader(story);

			var allSteps = story.Scenarios.SelectMany(s => s.Steps).ToList();
			if (allSteps.Any())
				_longestStepSentence = allSteps.Max(s => PrefixWithSpaceIfRequired(s).Length);

			foreach (var scenario in story.Scenarios)
			{
				Report(scenario);

				if (scenario.Steps.Any())
				{
					foreach (var step in scenario.Steps.Where(s => s.ShouldReport))
						ReportOnStep(scenario, step);
				}
			}
		}

		public ProcessType ProcessType
		{
			get { return ProcessType.Report; }
		}

		private static string FlattenExceptionMessage(string message)
		{
			return message
				.Replace("\t", " ") // replace tab with one space
				.Replace(Environment.NewLine, ", ") // replace new line with one space
				.Trim() // trim starting and trailing spaces
				.Replace("  ", " ")
				.TrimEnd(','); // chop any , from the end
		}

		private static string PrefixWithSpaceIfRequired(ExecutionStep step)
		{
			var stepTitle = step.StepTitle;
			var executionOrder = step.ExecutionOrder;

			if (executionOrder == ExecutionOrder.ConsecutiveAssertion ||
			    executionOrder == ExecutionOrder.ConsecutiveSetupState ||
			    executionOrder == ExecutionOrder.ConsecutiveTransition)
				stepTitle = "  " + stepTitle; // add two spaces in the front for indentation.

			return stepTitle.Replace(Environment.NewLine, Environment.NewLine + "\t\t");
		}

		private static void Report(Scenario scenario)
		{
			Console.ForegroundColor = ConsoleColor.White;
			Console.WriteLine();
			Console.WriteLine("Scenario: " + scenario.Title);
		}

		private static void ReportStoryHeader(Story story)
		{
			if (story.MetaData == null || story.MetaData.Type == null)
				return;

			Console.WriteLine("Story: " + story.MetaData.Title);
			Console.WriteLine("\t" + story.MetaData.AsA);
			Console.WriteLine("\t" + story.MetaData.IWant);
			Console.WriteLine("\t" + story.MetaData.SoThat);
		}
	}
}