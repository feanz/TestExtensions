using System;
using System.Linq.Expressions;

namespace TestExtensions.FluentBDD
{
	public interface IWhen<TScenario>
	{
		IAndWhen<TScenario> And(Expression<Action<TScenario>> andWhenStep, bool includeInputsInStepTitle);

		IAndWhen<TScenario> And(Expression<Action<TScenario>> andWhenStep, string stepTextTemplate = null);

		IThen<TScenario> Then(Expression<Action<TScenario>> thenStep, bool includeInputsInStepTitle);

		IThen<TScenario> Then(Expression<Action<TScenario>> thenStep, string stepTextTemplate = null);
	}
}