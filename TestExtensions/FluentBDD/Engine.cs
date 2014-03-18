using System;
using System.Linq;
using TestExtensions.FluentBDD.Processors;

namespace TestExtensions.FluentBDD
{
	public class Engine
	{
		private readonly Scenario scenario;
		private readonly Type explicitStoryType;

		static Engine()
		{
			AppDomain.CurrentDomain.DomainUnload += CurrentDomain_DomainUnload;
		}

		private static void CurrentDomain_DomainUnload(object sender, EventArgs e)
		{
			foreach (var batchProcessor in Configuration.Processors.GetBatchProcessors())
			{
				batchProcessor.Process(StoryCache.Stories);
			}
		}

		public Engine(Scenario scenario, Type explicitStoryType)
		{
			this.scenario = scenario;
			this.explicitStoryType = explicitStoryType;
		}

		public Story Run()
		{
			var metaData = new StoryAttributeMetaDataScanner().Scan(scenario.TestObject, explicitStoryType);

			Story = new Story(metaData, scenario);

			var processors = Configuration.Processors.GetProcessors().ToList();

			try
			{
				//run processors in the right order regardless of the order they are provided 
				foreach (var processor in processors.Where(p => p.ProcessType != ProcessType.Finally).OrderBy(p => (int)p.ProcessType))
					processor.Process(Story);
			}
			finally
			{
				foreach (var finallyProcessor in processors.Where(p => p.ProcessType == ProcessType.Finally))
					finallyProcessor.Process(Story);
			}

			return Story;
		}

		public Story Story { get; private set; }
	}
}