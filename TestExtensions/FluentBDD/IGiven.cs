using System;
using System.Linq.Expressions;

namespace TestExtensions.FluentBDD
{
	public interface IGiven<TScenario>
	{
		IAndGiven<TScenario> And(Expression<Action<TScenario>> andGivenStep, bool includeInputsInStepTitle);
		IAndGiven<TScenario> And(Expression<Action<TScenario>> andGivenStep, string stepTextTemplate = null);
		
		IWhen<TScenario> When(Expression<Action<TScenario>> whenStep, bool includeInputsInStepTitle);
		IWhen<TScenario> When(Expression<Action<TScenario>> whenStep, string stepTextTemplate = null);

		IThen<TScenario> Then(Expression<Action<TScenario>> thenStep, bool includeInputsInStepTitle);
		IThen<TScenario> Then(Expression<Action<TScenario>> thenStep, string stepTextTemplate = null);
	}
}