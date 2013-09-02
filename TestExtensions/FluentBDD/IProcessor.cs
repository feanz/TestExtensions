namespace TestExtensions.FluentBDD
{
	public interface IProcessor
	{
		ProcessType ProcessType { get; }
		void Process(Story story);
	}
}