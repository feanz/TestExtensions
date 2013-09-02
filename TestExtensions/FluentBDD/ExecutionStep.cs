using System;
using System.Diagnostics;

namespace TestExtensions.FluentBDD
{
	public class ExecutionStep
	{
		public ExecutionStep(
			Action<object> stepAction,
			string stepTitle,
			bool asserts,
			ExecutionOrder executionOrder,
			bool shouldReport)
		{
			Asserts = asserts;
			ExecutionOrder = executionOrder;
			ShouldReport = shouldReport;
			Result = StepExecutionResult.NotExecuted;
			Id = Guid.NewGuid();
			StepTitle = stepTitle;
			StepAction = stepAction;
		}

		public bool Asserts { get; private set; }
		public TimeSpan Duration { get; set; }
		public Exception Exception { get; set; }
		public ExecutionOrder ExecutionOrder { get; private set; }
		public int ExecutionSubOrder { get; set; }
		public Guid Id { get; private set; }
		public StepExecutionResult Result { get; set; }
		public bool ShouldReport { get; private set; }
		public string StepTitle { get; private set; }
		private Action<object> StepAction { get; set; }

		public void Execute(object testObject)
		{
			Stopwatch sw = Stopwatch.StartNew();
			try
			{
				StepAction(testObject);
				sw.Stop();
				Duration = sw.Elapsed;
			}
			finally
			{
				sw.Stop();
				Duration = sw.Elapsed;
			}
		}
	}
}