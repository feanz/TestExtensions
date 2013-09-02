using System;
using System.Linq.Expressions;

namespace TestExtensions.FluentBDD
{
	/// <summary>
	/// Extensions that implement a fluent BDD approach.  That allows you to specify tests in the format below
	/// </summary>
	/// <remarks>
	/// </remarks>
	/// <typeparam name="TScenario"></typeparam>
	/// <example>
	/// <code>
	/// [Fact]
	/// public void AccountHasSufficientFund()
	/// {
	///     this.Given(s => s.GivenTheAccountBalanceIs(100), GivenTheAccountBalanceIsTitleTemplate)
	///             .And(s => s.AndTheCardIsValid())
	///             .And(s => s.AndTheMachineContains(100), AndTheMachineContainsEnoughMoneyTitleTemplate)
	///         .When(s => s.WhenTheAccountHolderRequests(20), WhenTheAccountHolderRequestsTitleTemplate)
	///         .Then(s => s.TheAtmShouldDispense(20), "Then the ATM should dispense $20")
	///             .And(s => s.AndTheAccountBalanceShouldBe(80), "And the account balance should be $80")
	///             .And(s => s.ThenCardIsRetained(false), AndTheCardShouldBeReturnedTitleTemplate)
	///         .Run();
	/// }
	/// </code>
	/// </example>
	public static class FluentBDDExtensions
	{
		private static IInitialStep<TScenario> Scan<TScenario>(this TScenario testObject) where TScenario : class
		{
			return new FluentContainer<TScenario>(testObject);
		}

		public static IGiven<TScenario> Given<TScenario>(this TScenario testObject, Expression<Action<TScenario>> givenStep, string stepTextTemplate)
			where TScenario : class
		{
			return testObject.Scan().Given(givenStep, stepTextTemplate);
		}

		public static IGiven<TScenario> Given<TScenario>(this TScenario testObject, Expression<Action<TScenario>> givenStep, bool includeInputsInStepTitle)
			where TScenario : class
		{
			return testObject.Scan().Given(givenStep, includeInputsInStepTitle);
		}

		public static IWhen<TScenario> When<TScenario>(this TScenario testObject, Expression<Action<TScenario>> whenStep, string stepTextTemplate)
			where TScenario : class
		{
			return testObject.Scan().When(whenStep, stepTextTemplate);
		}

		public static IWhen<TScenario> When<TScenario>(this TScenario testObject, Expression<Action<TScenario>> whenStep, bool includeInputsInStepTitle)
			where TScenario : class
		{
			return testObject.Scan().When(whenStep, includeInputsInStepTitle);
		}

		public static IGiven<TScenario> Given<TScenario>(this TScenario testObject, Expression<Action<TScenario>> givenStep)
			where TScenario : class
		{
			return testObject.Given(givenStep, null);
		}

		public static IWhen<TScenario> When<TScenario>(this TScenario testObject, Expression<Action<TScenario>> whenStep)
			where TScenario : class
		{
			return testObject.When(whenStep, null);
		}

		public static Story Run(this object testObject)
		{
			return Run(testObject, null);
		}

		public static Story Run(this object testObject, string explicitScenarioTitle)
		{
			return testObject.LazyRun(explicitScenarioTitle).Run();
		}

		public static Engine LazyRun(this object testObject, string explicitScenarioTitle = null)
		{
			return InternalLazyBDDfy(testObject, explicitScenarioTitle);
		}

		static Engine InternalLazyBDDfy(
			object testObject,
			string explicitScenarioTitle)
		{
			var testContainer = testObject as ITestContainer;
			if (testContainer != null)
			{
				var scenario = testContainer.GetScenario(explicitScenarioTitle);
				return new Engine(scenario);
			}

			throw new ArgumentException("Not a vlaid TestConatiner");
		}
	}
}