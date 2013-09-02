namespace TestExtensions.FluentBDD
{
	internal interface ITestContainer
	{
		Scenario GetScenario(string explicitScenarioTitle);
		object TestObject { get; }
	}
}