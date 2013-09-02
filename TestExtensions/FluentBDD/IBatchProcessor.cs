using System.Collections.Generic;

namespace TestExtensions.FluentBDD
{
	public interface IBatchProcessor
	{
		void Process(IEnumerable<Story> stories);
	}
}