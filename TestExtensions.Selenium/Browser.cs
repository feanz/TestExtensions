using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Drawing;
using System.Threading;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Interactions;

namespace TestExtensions.Selenium
{
	public static class Browser
	{
		private static bool disableJavaScript;

		private static IWebDriver webDriver;

		public static string Title
		{
			get { return WebDriver.Title; }
		}

		public static string Url
		{
			get { return webDriver.Url; }
		}

		/// <summary>
		///     internal access to driver only test project should not access this value
		/// </summary>
		internal static IWebDriver Driver
		{
			get { return WebDriver; }
		}

		private static IWebDriver WebDriver
		{
			get { return webDriver ?? (webDriver = CreateWebDriver()); }
		}

		public static void Back()
		{
			WebDriver.Navigate().Back();
		}

		/// <summary>
		///     Clear Browser Cache
		/// </summary>
		public static void ClearCache()
		{
			// Use Ctrl + F5
			Actions builder = new Actions(Driver);
			builder.KeyDown(Keys.Control).SendKeys(Keys.F5).KeyUp(Keys.Control).Perform();
		}

		public static void Close()
		{
			if (webDriver != null)
				webDriver.Quit();
			webDriver = null;
		}

		/// <summary>
		///     Delete all Browser Cookies
		/// </summary>
		public static void DeleteCookies(bool refresh = true)
		{
			Driver.Manage().Cookies.DeleteAllCookies();
			if (refresh)
			{
				RefreshPage();
			}
		}

		public static void DisableJavaScript()
		{
			disableJavaScript = true;
		}

		/// <summary>
		///     Manipulate hover over effects during testing
		/// </summary>
		/// <param name="targetElement"></param>
		/// <returns></returns>
		public static bool ElementMouseOver(IWebElement targetElement)
		{
			var currentWinSize = Driver.Manage().Window.Size;
			Driver.Manage().Window.Maximize();
			var builder = new Actions(Driver);
			try
			{
				builder.MoveToElement(targetElement).Build().Perform();
				Thread.Sleep(2000); // pause otherwise mouse hover can happen for fraction of seconds and then disappear.                
				Driver.Manage().Window.Size = currentWinSize;
			}
			catch (Exception e)
			{
				throw new InvalidOperationException(e.Message);
			}
			return true;
		}

		public static IWebElement FindElement(By selector)
		{
			return Driver.FindElement(selector);
		}

		public static ReadOnlyCollection<IWebElement> FindElements(By selector)
		{
			return Driver.FindElements(selector);
		}

		public static void Goto(string url)
		{
			if (!url.Contains(AutomatedTestConfiguration.EnvironmentBaseUrl))
				url = AutomatedTestConfiguration.BaseUrl + url;

			Trace.WriteLine("Goto: " + url);

			WebDriver.Url = url;
		}

		/// <summary>
		///     Refresh Current Page
		/// </summary>
		public static void RefreshPage()
		{
			Driver.Navigate().Refresh();
		}

		private static IWebDriver CreateWebDriver()
		{
			IWebDriver driver;
			if (disableJavaScript)
			{
				switch (AutomatedTestConfiguration.BrowserMake)
				{
					case BrowserMake.Firefox:
						{
							var profile = new FirefoxProfile();
							profile.SetPreference("javascript.enabled", false);
							driver = new FirefoxDriver(profile);
						}
						break;

					default:
						throw new InvalidOperationException(string.Format("{0} Browser does not support javascript disabled"));
				}
			}
			else
			{
				driver = (IWebDriver)Activator.CreateInstance(AutomatedTestConfiguration.DriverType);                
			}

			driver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(5));

			//reset to default of false here
			disableJavaScript = false;

			if (AutomatedTestConfiguration.MaximiseBrowser)
			{
				driver.Manage().Window.Maximize();
			}
			else
			{
				driver.Manage().Window.Size = new Size(AutomatedTestConfiguration.BrowserWidth, AutomatedTestConfiguration.BrowserHeight);
			}

			return driver;
		}
	}
}