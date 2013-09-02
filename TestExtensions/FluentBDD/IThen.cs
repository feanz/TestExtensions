using System;
using System.Linq.Expressions;

namespace TestExtensions.FluentBDD
{
	public interface IThen<TScenario>
	{
		IAndThen<TScenario> And(Expression<Action<TScenario>> andThenStep, bool includeInputsInStepTitle);

		IAndThen<TScenario> And(Expression<Action<TScenario>> andThenStep, string stepTextTemplate = null);
	}
}