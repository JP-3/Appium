using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using PageModels.AppiumUtilities.Config;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace PageModels.AppiumUtilities.Appium
{
    public class AppiumWait
    {
        private WebDriverWait webDriverWait;

        /// <summary>
        /// Initializes a new instance of the <see cref="AppiumWait"/> class.
        /// </summary>
        /// <param name="driver">IWebDriver instance</param>
        public AppiumWait(IWebDriver driver)
            : this(driver, AppiumConfig.GetWaitValue())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AppiumWait"/> class.
        /// </summary>
        /// <param name="driver">IWebDriver instance</param>
        /// <param name="timeOut">int value of seconds to wait before timeout</param>
        public AppiumWait(IWebDriver driver, TimeSpan timeOut)
        {
            this.Driver = driver;
            webDriverWait = new WebDriverWait(driver, timeOut);
        }

        public IWebDriver Driver { get; set; }

        /// <summary>
        /// Wait for the element to be visible
        /// </summary>
        /// <param name="selector">By selector to wait for</param>
        /// <returns>true if the element is found and visible, else false</returns>
        public bool UntilVisibleElement(By selector) => ForVisibleElement(selector, false) != null;

        /// <summary>
        /// Wait for the element to be visible
        /// </summary>
        /// <param name="selector">By selector to wait for</param>
        /// <param name="assertVisible">true to assert the element is Visible</param>
        /// <returns>IWebElement found for the selector, null if not found and Assert == false</returns>
        public IWebElement ForVisibleElement(By selector, bool assertVisible = true)
        {
            try
            {
                return webDriverWait.Until(ExpectedConditions.ElementIsVisible(selector));
            }
            catch
            {
                string error = string.Format(
                    " on 'Wait.ForVisibleElement'" + Environment.NewLine +
                    "Element for selector [{0}] isn't visible." + Environment.NewLine, selector.ToString());
                if (assertVisible)
                {
                    throw new Exception(error);
                }

                Console.WriteLine(error);
                return null;
            }
        }

        /// <summary>
        /// Wait until element is clickable
        /// </summary>
        /// <param name="selector">By selector for the element to wait for clickable</param>
        /// <returns>true if it is clickable, false if not after timeout</returns>
        public bool UntilClickableElement(By selector) => ForClickableElement(selector, false) != null;

        /// <summary>
        /// Waits for element to be clickable
        /// </summary>
        /// <param name="selector">By selector to wait for clickable</param>
        /// <param name="assertFound">true to assert element is found. Null is returned if false and not found</param>
        /// <returns>IWebElement of clickable element or null if AssertFound == false</returns>
        public IWebElement ForClickableElement(By selector, bool assertFound = true)
        {
            try
            {
                return webDriverWait.Until(ExpectedConditions.ElementToBeClickable(selector));
            }
            catch
            {
                string error = string.Format(
                    " on 'Wait.ForClickableElement'" + Environment.NewLine +
                    "Element for selector [{0}] isn't clickable." + Environment.NewLine, selector.ToString());
                if (assertFound)
                {
                    throw new Exception(error);
                }

                Console.WriteLine(error);
                return null;
            }
        }

        /// <summary>
        /// Wait until the element exists
        /// </summary>
        /// <param name="by">By selector to wait for</param>
        /// <returns>true if the element is found, false if not</returns>
        public bool UntilElementExists(By by) => ForElementExists(by, false) != null;

        /// <summary>
        /// Wait for the element to exist
        /// </summary>
        /// <param name="selector">By selector to wait for existence</param>
        /// <param name="assertFound">true to assert the element is found. Exception is thrown if it isn't</param>
        /// <returns>IWebElement of By selector. Null if not found (and AssertFound == false)</returns>
        public IWebElement ForElementExists(By selector, bool assertFound = true)
        {
            try
            {
                return webDriverWait.Until(ExpectedConditions.ElementExists(selector));
            }
            catch
            {
                string error = string.Format(" on 'Wait.ForElementExists' \nElement for selector [{0}] didn't exist.", selector.ToString());
                if (assertFound)
                {
                    throw new Exception(error);
                }

                Console.WriteLine(error);
                return null;
            }
        }

        /// <summary>
        /// Wait for an element to exist with an overloaded timeout (in seconds).  Returns null if not found
        /// </summary>
        /// <param name="selector">Selector to look for</param>
        /// <param name="timeOutSeconds">New timeout to wait for in seconds</param>
        /// <returns>IWebElement found. Else null</returns>
        public IWebElement ForElementExists(By selector, int timeOutSeconds)
        {
            TimeSpan originalTimeOut = webDriverWait.Timeout;
            webDriverWait.Timeout = TimeSpan.FromSeconds(timeOutSeconds);
            IWebElement element = ForElementExists(selector, false);
            webDriverWait.Timeout = originalTimeOut;
            return element;
        }

        /// <summary>
        /// Wait for specific text to be presnet
        /// </summary>
        /// <param name="by">selector to wait for text</param>
        /// <param name="text">string to wait for existence</param>
        /// <returns>True if element contains text</returns>
        public bool UntilTextInElement(By by, string text) => ForTextInElement(by, text, false) != null;

        /// <summary>
        /// Waits for specifc text to be present in element
        /// </summary>
        /// <param name="by">selector to wait for text</param>
        /// <param name="text">string to wait for</param>
        /// <returns>Element if element contains text</returns>
        public IWebElement ForTextInElement(By by, string text) => webDriverWait.Until(ElementContainsText(by, text));

        /// <summary>
        /// Waits for specific text to be present in element
        /// </summary>
        /// <param name="selector">selector to wait for</param>
        /// <param name="text">string to wait to exist in web element</param>
        /// <param name="assertFound">true to assert it's found</param>
        /// <returns>IWebElement if it's found. null if not (and assert == false)</returns>
        public IWebElement ForTextInElement(By selector, string text, bool assertFound)
        {
            try
            {
                return webDriverWait.Until(ElementContainsText(selector, text));
            }
            catch
            {
                string error = string.Format(" on 'Wait.ForTextInElement'\n Text for selector [{0}] didn't contain [{1}].", selector.ToString(), text);
                if (assertFound)
                {
                    throw new Exception(error);
                }

                Console.WriteLine(error);
                return null;
            }
        }

        /// <summary>
        /// Wait for specific exact text to be present in element
        /// </summary>
        /// <param name="selector">selector to wait for</param>
        /// <param name="text">text to exist in the selector</param>
        /// <returns>true if it's found</returns>
        public bool UntilExactTextInElement(By selector, string text) => ForExactTextInElement(selector, text, false) != null;

        /// <summary>
        /// Wait for specific exact text to be present in element
        /// </summary>
        /// <param name="selector">selector to wait for</param>
        /// <param name="text">text to exist in the selector</param>
        /// <param name="assertFound">true to assert it's found</param>
        /// <returns>IWebElement found, null if not found and assert == false</returns>
        public IWebElement ForExactTextInElement(By selector, string text, bool assertFound = true)
        {
            try
            {
                return webDriverWait.Until(ElementHasExactText(selector, text));
            }
            catch
            {
                string error = string.Format(" on 'Wait.ForExactTextInElement'\nText for selector [{0}] didn't match [{1}].", selector.ToString(), text);
                if (assertFound)
                {
                    throw new Exception(error);
                }

                Console.WriteLine(error);
                return null;
            }
        }

        /// <summary>
        /// Waits for title of page to be present for specific value
        /// </summary>
        /// <param name="title">string value of the page title</param>
        /// <param name="assertFound">true to assert the page title is found</param>
        /// <returns>true if found, false if not (and assert is false)</returns>
        public bool ForTitleOfPage(string title, bool assertFound = true)
        {
            try
            {
                return webDriverWait.Until(ExpectedConditions.TitleIs(title));
            }
            catch
            {
                string error = string.Format(" on 'Wait.ForTitleOfPage'\n Title of page doesn't match [{0}]", title);
                if (assertFound)
                {
                    throw new Exception(error);
                }

                Console.WriteLine(error);
                return false;
            }
        }

        /// <summary>
        /// Waits for Element to not exist on page
        /// </summary>
        /// <param name="by">selector to not exist on page</param>
        /// <returns>true if element doesn't exist, else true</returns>
        public bool ForElementDoesNotExistOnPage(By by) => ElementDoesNotExist(by, Driver);

        /// <summary>
        /// Waits for Page to laod
        /// </summary>
        /// <returns>Did Page Load</returns>
        public bool ForPageLoad() => PageLoad();

        /// <summary>
        /// Checks if page has loaded
        /// </summary>
        /// <returns>Did Page Load</returns>
        private bool PageLoad()
        {
            DateTime startTime = DateTime.Now;
            string source = null;

            while (DateTime.Now - startTime < webDriverWait.Timeout)
            {
                Thread.Sleep(500);
                try
                {
                    string newSource = Driver.PageSource;

                    if (!string.IsNullOrEmpty(source) && source.Equals(newSource))
                    {
                        return true;
                    }

                    source = newSource;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }

            return false;
        }

        /// <summary>
        /// Check if an element does not exist on page
        /// </summary>
        /// <param name="by">Selector to interact with</param>
        /// <param name="driver">IWebDriver instance</param>
        /// <returns>Does the element exist</returns>
        private bool ElementDoesNotExist(By by, IWebDriver driver)
        {
            double timeLeft = webDriverWait.Timeout.Seconds;
            while (timeLeft != 0)
            {
                Thread.Sleep(500);
                try
                {
                    driver.FindElement(by);
                }
                catch (NoSuchElementException)
                {
                    return true;
                }
                catch (InvalidElementStateException)
                {
                    return true;
                }

                timeLeft = timeLeft - .5;
            }

            return false;
        }

        /// <summary>
        /// Check if an element contains expected text
        /// </summary>
        /// <param name="by">Selector to interact with</param>
        /// <param name="text">IWebDriver instance</param>
        /// <returns>The driver and element</returns>
        private Func<IWebDriver, IWebElement> ElementContainsText(By by, string text)
        {
            return driver =>
            {
                IWebElement element = driver.FindElement(by);
                return (element != null && element.Text.Contains(text)) ? element : null;
            };
        }

        /// <summary>
        /// Check if an element has exacted text
        /// </summary>
        /// <param name="by">Selector to interact with</param>
        /// <param name="text">IWebDriver instance</param>
        /// <returns>Driver and Element</returns>
        private Func<IWebDriver, IWebElement> ElementHasExactText(By by, string text)
        {
            return driver =>
            {
                IWebElement element = driver.FindElement(by);
                return (element != null && element.Text.Equals(text)) ? element : null;
            };
        }
    }
}
