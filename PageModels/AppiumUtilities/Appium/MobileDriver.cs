using OpenQA.Selenium.Appium;
using PageModels.AppiumUtilities.Support;
using System;
using System.Collections.Generic;
using System.Text;

namespace PageModels.AppiumUtilities.Appium
{
    public class MobileDriver
    {
        public AppiumDriver<AppiumWebElement> AppiumDriver { get; set; }

        public void CreateAppiumDriver(string deviceName)
        {
            MobileDriverFactory mobileDriverFactory = new MobileDriverFactory();
            AppiumDriver = mobileDriverFactory.CreateDriver(deviceName);
        }

        public void DisposeAppiumDriver() => AppiumDriver.Quit();
    }
}
