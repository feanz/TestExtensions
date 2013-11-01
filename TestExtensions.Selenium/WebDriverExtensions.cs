using System;
using System.Threading;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;

namespace TestExtensions.Selenium
{
	public static class Extensions
	{
		/// <summary>
		/// Drag and Drop Item
		/// </summary>
		/// <param name="webDriver"></param>
		/// <param name="item">Item being dragged</param>
		/// <param name="target">Target</param>
		public static void DragDropItem(this IWebDriver webDriver, IWebElement item, IWebElement target)
		{
			var builder = new Actions(webDriver);
			var dragAndDrop = builder.ClickAndHold(item)
				.MoveToElement(target)
				.Release(target);
			dragAndDrop.Perform();
		}

		/// <summary>
		/// Drop and Drop Item By Offset - allows users to drag and drop to an offset of the current target.
		/// </summary>
		/// <param name="webDriver">Current Driver</param>
		/// <param name="item">source item</param>
		/// <param name="target">target item</param>
		/// <param name="offSet">offset to current target object</param>
		public static void DragDropItem(this IWebDriver webDriver, IWebElement item, IWebElement target, int offSet)
		{
			var builder = new Actions(webDriver).DragAndDropToOffset(item, offSet + item.Location.X - target.Location.X, offSet + target.Location.Y - item.Location.Y);
			builder.Perform();
		}

		///  <summary>
		/// Hover and Click Element on Page
		///  </summary>
		/// <param name="webDriver">Current Driver</param>
		/// <param name="targetElement"></param>
		///  <returns></returns>
		public static void HoverAndClickItem(this IWebDriver webDriver, IWebElement targetElement)
		{
			var builder = new Actions(webDriver);
			var hoverClick = builder.MoveToElement(targetElement).Click();
			hoverClick.Perform();
		}
		
		/// <summary>
		///     Click any element on the page By the id provided
		/// </summary>
		/// <param name="webDriver">
		///     A <see cref="IWebDriver" /> instance.
		/// </param>
		/// <param name="id">id of the element to click</param>
		/// <exception cref="NoSuchElementException">No element was found.</exception>
		public static void Click(this IWebDriver webDriver, string id)
		{
			webDriver.WaitFor(driver => driver.FindElement(By.Id(id))).Click();
		}

		/// <summary>
		///     Clicks a button By the id ending By the provided value.
		/// </summary>
		/// <param name="idEndsWith">A CSS id.</param>
		/// <param name="webDriver">
		///     A <see cref="IWebDriver" /> instance.
		/// </param>
		/// <exception cref="NoSuchElementException">No element was found.</exception>
		public static void ClickButtonWithId(this IWebDriver webDriver, string idEndsWith)
		{
			webDriver.WaitFor(driver => driver.FindElement(By.CssSelector("input[id$='" + idEndsWith + "']"))).Click();
		}

		/// <summary>
		///     Clicks a button that has the given value.
		/// </summary>
		/// <param name="buttonValue">The button's value (input[value=])</param>
		/// <param name="webDriver">
		///     A <see cref="IWebDriver" /> instance.
		/// </param>
		/// <exception cref="NoSuchElementException">No element was found.</exception>
		public static void ClickButtonWithValue(this IWebDriver webDriver, string buttonValue)
		{
			webDriver.WaitFor(driver => driver.FindElement(By.CssSelector("input[value='" + buttonValue + "']"))).Click();
		}

		/// <summary>
		///     Click on element By the xpath provided
		/// </summary>
		/// <param name="webDriver">
		///     A <see cref="IWebDriver" /> instance.
		/// </param>
		/// <param name="xpath">The xpath used to find element</param>
		/// <exception cref="NoSuchElementException">No element was found.</exception>
		public static void ClickElementByXpath(this IWebDriver webDriver, string xpath)
		{
			webDriver.WaitFor(driver => driver.FindElement(By.XPath(xpath))).Click();
		}

		/// <summary>
		///     Click image By the alt text provided
		/// </summary>
		/// <param name="webDriver">
		///     A <see cref="IWebDriver" /> instance.
		/// </param>
		/// <param name="altText">The alt text of the image</param>
		/// <exception cref="NoSuchElementException">No element was found.</exception>
		public static void ClickImageWithAltText(this IWebDriver webDriver, string altText)
		{
			webDriver.WaitFor(driver => driver.FindElement(By.CssSelector("img[alt=\"Friends\"]"))).Click();
		}

		/// <summary>
		///     Clicks a link By the text provided. This is case sensitive and searches using an Xpath contains() search.
		/// </summary>
		/// <param name="linkContainsText">The link text to search for.</param>
		/// <param name="webDriver">
		///     A <see cref="IWebDriver" /> instance.
		/// </param>
		/// <exception cref="NoSuchElementException">No element was found.</exception>
		public static void ClickLinkWithText(this IWebDriver webDriver, string linkContainsText)
		{
			webDriver.WaitFor(driver => driver.FindElement(By.XPath("//a[contains(text(),'" + linkContainsText + "')]"))).Click();
		}

		/// <summary>
		///  If this element exists return true
		/// </summary>
		/// <param name="webElement"></param>
		/// <returns></returns>
		public static bool Exists(this IWebElement webElement)
		{
			var result = false;
			try
			{
				if (webElement.Enabled)
					result = true;
			}
			catch (NoSuchElementException)
			{
				Exists(webElement);
			}

			return result;
		}

		/// <summary>
		///  If this element exists i want an action performed on it. 
		/// </summary>
		/// <param name="webElement"></param>
		/// <param name="action"></param>
		public static bool DoIfExists(this IWebElement webElement, Action<IWebElement> action)
		{
			var result = false;
			try
			{
				action.Invoke(webElement);
				result = true;
			}
			catch (NoSuchElementException)
			{ }

			return result;
		}

		/// <summary>
		/// Checks if control has default error state
		/// </summary>
		/// <param name="webElement"></param>
		/// <returns></returns>
		public static bool IsInErrorState(this IWebElement webElement)
		{
			return webElement.GetClass().Contains("input-validation-error");
		}

		/// <summary>
		///     Check that an element is on the page
		/// </summary>
		/// <param name="webDriver">
		///     A <see cref="IWebDriver" /> instance.
		/// </param>
		/// <param name="selector">The selector to use to find the element</param>
		/// <returns></returns>
		public static bool Exists(this IWebDriver webDriver, By selector)
		{
			try
			{
				webDriver.FindElement(selector);
				return true;
			}
			catch (NoSuchElementException)
			{
				return false;
			}
		}

		/// <summary>
		///     Gets an element's text using the given selector.
		/// </summary>
		/// <param name="selector">A valid selector.</param>
		/// <param name="webDriver">
		///     A <see cref="IWebDriver" /> instance.
		/// </param>
		/// <returns>The element's text.</returns>
		/// <exception cref="NoSuchElementException">No element was found.</exception>
		public static string ElementText(this IWebDriver webDriver, By selector)
		{
			return webDriver.WaitFor(driver => driver.FindElement(selector)).Text;
		}
		
		/// <summary>
		///     Select all the text on the page
		/// </summary>
		/// <param name="webDriver">
		///     A <see cref="IWebDriver" /> instance.
		/// </param>
		/// <param name="textToFind"></param>
		/// <returns></returns>
		public static IWebElement FindTextOnPage(this IWebDriver webDriver, string textToFind)
		{
			var locator = By.XPath(string.Format("//*[contains(.,'{0}')]", textToFind));
			return webDriver.WaitFor(driver => driver.FindElement(locator));
		}

		/// <summary>
		///     Get the class of an Element
		/// </summary>
		/// <param name="e">The element</param>
		/// <returns>Class string</returns>
		public static string GetClass(this IWebElement e)
		{
			return e.GetAttribute("class");
		}

		/// <summary>
		///     Find the parent of the element
		/// </summary>
		/// <param name="e"></param>
		/// <returns>Elements parent </returns>
		public static IWebElement GetParent(this IWebElement e)
		{
			return e.FindElement(By.XPath(".."));
		}

		/// <summary>
		///     Keep performing an action By the webdriver until some webdriver call returns true
		/// </summary>
		/// <param name="webDriver">
		///     A <see cref="IWebDriver" /> instance.
		/// </param>
		/// <param name="action">The action to retry</param>
		/// <param name="until">The function to check every time we retry</param>
		/// <param name="maxRetry">The umber of times to retry before we throw error default 5</param>
		public static void RetryUntil(this IWebDriver webDriver, Action<IWebDriver> action, Func<IWebDriver, bool> until, int maxRetry = 5)
		{
			var retryCount = 1;
			do
			{
				action(webDriver);
				retryCount++;

				if (retryCount > maxRetry )
					throw new ApplicationException("Tried to execute an action more than the set max retry rate");
				if (retryCount > maxRetry)
					return;
			} while (!until.Invoke(webDriver));
		}

		/// <summary>
		///     Returns a script object for the webdriver provided that allows you to execute scripts
		/// </summary>
		/// <param name="webdriver"></param>
		/// <returns></returns>
		public static Script Script(this IWebDriver webdriver)
		{
			return new Script(webdriver);
		}

		/// <summary>
		///     Select the item in a drop down list by the text value
		/// </summary>
		/// <param name="element">the select element</param>
		/// <param name="text">the text of the item to select</param>
		public static void SelectByText(this IWebElement element, string text)
		{
			var selectElement = new SelectElement(element);
			selectElement.SelectByText(text);
		}

		/// <summary>
		///     Sets an element's (an input field) value to the provided text, using the given selector and using SendKeys().
		/// </summary>
		/// <param name="selector">A valid selector.</param>
		/// <param name="value">The text to type.</param>
		/// <param name="webDriver">
		///     A <see cref="IWebDriver" /> instance.
		/// </param>
		/// <exception cref="NoSuchElementException">No element was found.</exception>
		public static void SetElementValue(this IWebDriver webDriver, By selector, string value)
		{
			webDriver.WaitFor(driver => driver.FindElement(selector)).Clear();
			webDriver.WaitFor(driver => driver.FindElement(selector)).SendKeys(value);
		}

		/// <summary>
		///     Sets an element's (an input field) value to the provided text by using SendKeys().
		/// </summary>
		/// <param name="value">The text to type.</param>
		/// <param name="element">
		///     A <see cref="IWebElement" /> instance.
		/// </param>
		/// <exception cref="NoSuchElementException">No element was found.</exception>
		public static void SetValue(this IWebElement element, string value)
		{
			element.Clear();
			element.SendKeys(value);
		}

		/// <summary>
		///     Submit form By the id provided
		/// </summary>
		/// <param name="webDriver">
		///     A <see cref="IWebElement" /> instance.
		/// </param>
		/// <param name="id">Id of button to submit</param>
		public static void SubmitFormWithID(this IWebDriver webDriver, string id)
		{
			webDriver.WaitFor(driver => driver.FindElement(By.Id(id))).Submit();
		}

		/// <summary>
		/// Check if the element is visible this checks both the dispaly property 
		/// and if the element is placed of the screen
		/// </summary>
		/// <param name="element">The element to inspect</param>
		/// <returns></returns>
		public static bool Visible(this IWebElement element)
		{
			return element.Displayed && !element.GetCssValue("top").Contains("-10000");
		}

		/// <summary>
		///     Wait default 1 second
		/// </summary>
		/// <param name="webDriver">
		///     A <see cref="IWebElement" /> instance.
		/// </param>
		public static void Wait(this IWebDriver webDriver)
		{
			webDriver.Wait(TimeSpan.FromSeconds(1));
		}

		/// <summary>
		///     Wait a provided length of time
		/// </summary>
		/// <param name="webDriver">
		///     A <see cref="IWebElement" /> instance.
		/// </param>
		/// <param name="wait">Time to wait</param>
		public static void Wait(this IWebDriver webDriver, TimeSpan wait)
		{
			Thread.Sleep(wait);
		}

		/// <summary>
		///  Wait a given number of seconds
		/// </summary>
		/// <param name="webDriver">
		///     A <see cref="IWebElement" /> instance.
		/// </param>
		/// <param name="seconds">number of seconds to wait</param>
		public static void Wait(this IWebDriver webDriver, int seconds)
		{
			Thread.Sleep(TimeSpan.FromSeconds(seconds));
		}

		/// <summary>
		///  Wait a given number of seconds
		/// </summary>
		/// <param name="webDriver">
		///     A <see cref="IWebElement" /> instance.
		/// </param>
		/// <param name="milliseconds"></param>
		public static void WaitMs(this IWebDriver webDriver, int milliseconds)
		{
			Thread.Sleep(milliseconds);
		}


		/// <summary>
		///     Explicit wait for a given action
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="webDriver">
		///     A <see cref="IWebDriver" /> instance.
		/// </param>
		/// <param name="action">The action to wait for</param>
		/// <param name="timeout">The time to wait for action</param>
		/// <returns>Action result</returns>
		public static T WaitFor<T>(this IWebDriver webDriver, Func<IWebDriver, T> action, TimeSpan timeout)
		{
			var wait = new WebDriverWait(webDriver, timeout);

			return wait.Until(action);
		}

		/// <summary>
		///     Explicit wait for a given action USING DEFAULT TIMEOUT
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="webDriver">
		///     A <see cref="IWebDriver" /> instance.
		/// </param>
		/// <param name="action">The action to wait for</param>
		/// <returns>Action result</returns>
		public static T WaitFor<T>(this IWebDriver webDriver, Func<IWebDriver, T> action)
		{
			return webDriver.WaitFor(action, AutomatedTestConfiguration.DefaultPageTimeOut);
		}
	}
}