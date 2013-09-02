using System;
using System.Linq.Expressions;

namespace TestExtensions.FluentBDD
{
	public interface IInitialStep<TScenario>
	{
		IGiven<TScenario> Given(Expression<Action<TScenario>> givenStep, bool includeInputsInStepTitle);
		IGiven<TScenario> Given(Expression<Action<TScenario>> givenStep, string stepTextTemplate = null);

		IWhen<TScenario> When(Expression<Action<TScenario>> whenStep, bool includeInputsInStepTitle);
		IWhen<TScenario> When(Expression<Action<TScenario>> whenStep, string stepTextTemplate = null);
	}
}