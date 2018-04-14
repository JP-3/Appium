using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Android;
using OpenQA.Selenium.Appium.iOS;
using OpenQA.Selenium.Remote;
using PageModels.AppiumUtilities.Config;
using System;
using System.Collections.Generic;
using System.Text;

namespace PageModels.AppiumUtilities.Support
{
    public class MobileDriverFactory
    {
        // Device queue for checking out and checking in devices to allow multiple tests to run without conflicting against the devices
        private static DeviceLibrary que = new DeviceLibrary();

        private string standAloneServerURI = AppiumConfig.GetStandAloneServerUri();
        private TimeSpan serverWait = AppiumConfig.GetServerWaitValue();

        public MobileDriverFactory()
        {
        }

        public static DeviceLibrary Que
        {
            get => que;
            set => que = value;
        }

        public AppiumDriver<AppiumWebElement> CreateDriver(string deviceName)
        {
            AppiumConfig appiumConfig = new AppiumConfig(deviceName);
            Dictionary<string, object> deviceMap = appiumConfig.DeviceMap;
            DesiredCapabilities desiredCapabilities = new DesiredCapabilities(deviceMap);

            string platform = desiredCapabilities.GetCapability("platformName").ToString().ToUpper();
            switch (platform)
            {
                case "ANDROID":
                    return AndroidDriver(desiredCapabilities);
                case "MAC":
                    return IOSDriver(desiredCapabilities);
                default:
                    throw new Exception($"No driver logic for: #{platform}");
            }
        }

        public AppiumDriver<AppiumWebElement> AndroidDriver(DesiredCapabilities desiredCapabilities)
        {
            var driver = new AndroidDriver<AppiumWebElement>(
                new Uri(standAloneServerURI),
                desiredCapabilities,
                serverWait);

            return driver;
        }

        public AppiumDriver<AppiumWebElement> IOSDriver(DesiredCapabilities desiredCapabilities)
        {
            var driver = new IOSDriver<AppiumWebElement>(
                new Uri(standAloneServerURI),
                desiredCapabilities,
                serverWait);

            return driver;
        }
    }
}