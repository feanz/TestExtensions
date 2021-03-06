﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace TestExtensions.FluentBDD
{
	public class Scenario
	{
		private readonly List<ExecutionStep> _steps;

		public Scenario(object testObject, IEnumerable<ExecutionStep> steps, string scenarioText)
		{
			TestObject = testObject;
			_steps = steps.OrderBy(o => o.ExecutionOrder).ToList();
			Id = Guid.NewGuid();

			Title = scenarioText;
		}

		public TimeSpan Duration
		{
			get { return new TimeSpan(_steps.Sum(x => x.Duration.Ticks)); }
		}

		public Guid Id { get; private set; }

		public StepExecutionResult Result
		{
			get
			{
				if (!Steps.Any())
					return StepExecutionResult.NotExecuted;

				return (StepExecutionResult) Steps.Max(s => (int) s.Result);
			}
		}

		public List<ExecutionStep> Steps
		{
			get { return _steps; }
		}

		public object TestObject { get; private set; }

		public string Title { get; private set; }

		// ToDo: this method does not really belong to this class
		public StepExecutionResult ExecuteStep(ExecutionStep executionStep)
		{
			try
			{
				executionStep.Execute(TestObject);
				executionStep.Result = StepExecutionResult.Passed;
			}
			catch (Exception ex)
			{
				var exception = ex;
				if (exception is TargetInvocationException)
				{
					exception = ex.InnerException ?? ex;
				}

				if (exception is NotImplementedException)
				{
					executionStep.Result = StepExecutionResult.NotImplemented;
					executionStep.Exception = exception;
				}
				else if (IsInconclusive(exception))
				{
					executionStep.Result = StepExecutionResult.Inconclusive;
					executionStep.Exception = exception;
				}
				else
				{
					executionStep.Exception = exception;
					executionStep.Result = StepExecutionResult.Failed;
				}
			}

			return executionStep.Result;
		}

		private static bool IsInconclusive(Exception exception)
		{
			return exception.GetType().Name.Contains("InconclusiveException");
		}
	}
}