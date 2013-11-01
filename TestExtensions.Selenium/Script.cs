using OpenQA.Selenium;

namespace TestExtensions.Selenium
{
	public class Script
	{
		private readonly IWebDriver webdriver;

		public Script(IWebDriver webdriver)
		{
			this.webdriver = webdriver;
		}

		public void Execute(string script)
		{
			((IJavaScriptExecutor)webdriver).ExecuteScript(script);
		}
	}
}