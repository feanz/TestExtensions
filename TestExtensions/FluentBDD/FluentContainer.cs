using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace TestExtensions.FluentBDD
{
	/// <summary>
	/// Holds the test object on which we are working and the current set of execution steps
	/// </summary>
	/// <typeparam name="TScenario"></typeparam>
	internal class FluentContainer<TScenario> : IInitialStep<TScenario>, IAndGiven<TScenario>, IAndWhen<TScenario>, IAndThen<TScenario>, ITestContainer
	{
		private readonly List<ExecutionStep> _steps = new List<ExecutionStep>();
		private readonly object _testObject;

		public FluentContainer(object testObject)
		{
			this._testObject = testObject;
		}

		private void AddStep(Expression<Action<TScenario>> stepAction, string stepTextTemplate, bool asserts, ExecutionOrder executionOrder, bool reports = true, bool includeInputsInStepTitle = true)
		{
			var methodInfo = GetMethodInfo(stepAction);
			var inputArguments = new object[0];
			if (includeInputsInStepTitle)
			{
				inputArguments = stepAction.ExtractConstants().ToArray();
			}

			var flatInputArray = inputArguments.FlattenArrays();
			var stepTitle = StringUtil.CodeNameToNiceFormat(methodInfo.Name);

			if (!string.IsNullOrEmpty(stepTextTemplate))
				stepTitle = string.Format(stepTextTemplate, flatInputArray);
			else if (includeInputsInStepTitle)
			{
				var stringFlatInputs = flatInputArray.Select(i => i.ToString()).ToArray();
				stepTitle = stepTitle + " " + string.Join(", ", stringFlatInputs);
			}

			stepTitle = stepTitle.Trim();
			var action = stepAction.Compile();
			_steps.Add(new ExecutionStep(o => action((TScenario) o), stepTitle, asserts, executionOrder, reports));
		}

		private static MethodInfo GetMethodInfo(Expression<Action<TScenario>> stepAction)
		{
			var methodCall = (MethodCallExpression)stepAction.Body;
			return methodCall.Method;
		}

		public Scenario GetScenario(string explicitScenarioTitle = null)
		{
			var scenarioText = explicitScenarioTitle ?? GetTitleFromMethodNameInStackTrace(_testObject);
			return new Scenario(_testObject, _steps, scenarioText);	
		}

		public object TestObject 
		{
			get
			{
				return _testObject;
			} 
		}

		internal static string GetTitleFromMethodNameInStackTrace(object testObject)
		{
			var trace = new StackTrace();
			var frames = trace.GetFrames();
			if (frames == null)
				return null;

			var initiatingFrame = frames.LastOrDefault(s => s.GetMethod().DeclaringType == testObject.GetType());
			if (initiatingFrame == null)
				return null;

			return StringUtil.CodeNameToNiceFormat(initiatingFrame.GetMethod().Name);
		}

		public IGiven<TScenario> Given(Expression<Action<TScenario>> givenStep, string stepTextTemplate = null)
		{
			AddStep(givenStep, stepTextTemplate, false, ExecutionOrder.SetupState);
			return this;
		}

		IWhen<TScenario> IInitialStep<TScenario>.When(Expression<Action<TScenario>> whenStep, string stepTextTemplate)
		{
			AddStep(whenStep, stepTextTemplate, false, ExecutionOrder.Transition);
			return this;
		}

		IGiven<TScenario> IInitialStep<TScenario>.Given(Expression<Action<TScenario>> givenStep, bool includeInputsInStepTitle)
		{
			AddStep(givenStep, null, false, ExecutionOrder.SetupState, includeInputsInStepTitle: includeInputsInStepTitle);
			return this;
		}

		IWhen<TScenario> IInitialStep<TScenario>.When(Expression<Action<TScenario>> whenStep, bool includeInputsInStepTitle)
		{
			AddStep(whenStep, null, false, ExecutionOrder.Transition, includeInputsInStepTitle: includeInputsInStepTitle);
			return this;
		}

		IAndGiven<TScenario> IGiven<TScenario>.And(Expression<Action<TScenario>> andGivenStep, bool includeInputsInStepTitle)
		{
			AddStep(andGivenStep, null, false, ExecutionOrder.ConsecutiveSetupState, includeInputsInStepTitle: includeInputsInStepTitle);
			return this;
		}

		IThen<TScenario> IWhen<TScenario>.Then(Expression<Action<TScenario>> thenStep, bool includeInputsInStepTitle)
		{
			AddStep(thenStep, null, true, ExecutionOrder.Assertion, includeInputsInStepTitle: includeInputsInStepTitle);
			return this;
		}

		IAndWhen<TScenario> IWhen<TScenario>.And(Expression<Action<TScenario>> andWhenStep, bool includeInputsInStepTitle)
		{
			AddStep(andWhenStep, null, false, ExecutionOrder.ConsecutiveTransition, includeInputsInStepTitle: includeInputsInStepTitle);
			return this;
		}

		IThen<TScenario> IGiven<TScenario>.Then(Expression<Action<TScenario>> thenStep, bool includeInputsInStepTitle)
		{
			AddStep(thenStep, null, true, ExecutionOrder.Assertion, includeInputsInStepTitle: includeInputsInStepTitle);
			return this;
		}

		IAndGiven<TScenario> IGiven<TScenario>.And(Expression<Action<TScenario>> andGivenStep, string stepTextTemplate)
		{
			AddStep(andGivenStep, stepTextTemplate, false, ExecutionOrder.ConsecutiveSetupState);
			return this;
		}

		IAndThen<TScenario> IThen<TScenario>.And(Expression<Action<TScenario>> andThenStep, bool includeInputsInStepTitle)
		{
			AddStep(andThenStep, null, true, ExecutionOrder.ConsecutiveAssertion, includeInputsInStepTitle: includeInputsInStepTitle);
			return this;
		}

		IThen<TScenario> IWhen<TScenario>.Then(Expression<Action<TScenario>> thenStep, string stepTextTemplate)
		{
			AddStep(thenStep, stepTextTemplate, true, ExecutionOrder.Assertion);
			return this;
		}

		IWhen<TScenario> IGiven<TScenario>.When(Expression<Action<TScenario>> whenStep, bool includeInputsInStepTitle)
		{
			AddStep(whenStep, null, false, ExecutionOrder.Transition, includeInputsInStepTitle: includeInputsInStepTitle);
			return this;
		}

		IAndWhen<TScenario> IWhen<TScenario>.And(Expression<Action<TScenario>> andWhenStep, string stepTextTemplate)
		{
			AddStep(andWhenStep, stepTextTemplate, false, ExecutionOrder.ConsecutiveTransition);
			return this;
		}

		IThen<TScenario> IGiven<TScenario>.Then(Expression<Action<TScenario>> thenStep, string stepTextTemplate)
		{
			AddStep(thenStep, stepTextTemplate, true, ExecutionOrder.Assertion);
			return this;
		}

		IWhen<TScenario> IGiven<TScenario>.When(Expression<Action<TScenario>> whenStep, string stepTextTemplate)
		{
			AddStep(whenStep, stepTextTemplate, false, ExecutionOrder.Transition);
			return this;
		}

		IAndThen<TScenario> IThen<TScenario>.And(Expression<Action<TScenario>> andThenStep, string stepTextTemplate)
		{
			AddStep(andThenStep, stepTextTemplate, true, ExecutionOrder.ConsecutiveAssertion);
			return this;
		}
	}
}