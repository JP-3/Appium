using KobitonAPI;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Android;
using OpenQA.Selenium.Remote;
using PageModels.AppiumUtilities.Appium;
using PageModels.AppiumUtilities.Config;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Appium_Demo.TestProvider
{
    public class KobitonTestProvider : ITestProvider
    {
        private readonly KobitonApi kobitonApi = new KobitonApi();
        private string standAloneServerURI = "https://api.kobiton.com/wd/hub";
        private TimeSpan serverWait = AppiumConfig.GetServerWaitValue();
        private List<MobileDriver> mobileDrivers = new List<MobileDriver>();

        public List<MobileDriver> MobileDrivers => mobileDrivers;

        private int RunAppId { get; set; }

        /// <inheritdoc />
        /// <summary>
        /// Ones the time setup.  Called once per test run to upload application to Kobiton server
        /// </summary>
        public void OneTimeSetup()
        {
            Console.WriteLine($"{DateTime.UtcNow:HH:mm:ss:fff} KobitonTestProvider: OneTimeSetup");

            var file = new FileInfo(GetAppPath());

            RunAppId = kobitonApi.PushApp(file);
        }

        /// <summary>
        /// Ones the time tear down.  called once per test run to remove the application from Kobiton
        /// </summary>
        public void OneTimeTearDown()
        {
            Console.WriteLine($"{DateTime.UtcNow:HH:mm:ss:fff} KobitonTestProvider:  OneTimeTearDown");
            kobitonApi.DeleteApp(RunAppId);
        }

        /// <summary>
        /// Setup.  Called once per individual test to setup initial driver and user information for the test.
        /// </summary>
        public List<MobileDriver> Setup(int driverCount, MethodInfo methodInfo)
        {
            Console.WriteLine($"{DateTime.UtcNow:HH:mm:ss:fff} Setup: creating mobile drivers");

            mobileDrivers = CreateDrivers(driverCount, methodInfo);

            Console.WriteLine($"{DateTime.UtcNow:HH:mm:ss:fff} Setup: created {mobileDrivers.Count} mobile driver(s).");

            return mobileDrivers;
        }

        /// <inheritdoc />
        /// <summary>
        /// Tears down.  Called once per test execution to clean up test data.
        /// </summary>
        public void TearDown(MethodInfo methodInfo) => CleanUp(methodInfo);

        /// <summary>
        /// Creation of the drivers for the tests to use
        /// </summary>
        /// <param name="driverCount"> number of appium drivers to spin up for the test</param>
        /// <param name="methodInfo"></param>
        //private List<MobileDriver> CreateDrivers(int driverCount, MethodInfo methodInfo)
        //{
        //    List<Device> onlineDevices;

        //    var mydevices = kobitonApi.GetDevices();

        //    var devicePlatform = AppiumConfig.GetDevicePlatform();

        //    // TODO: refine better how to select appropriate devices for test execution
        //    if (devicePlatform == DevicePlatform.Android)
        //    {
        //        onlineDevices = mydevices.Where(
        //            d => d.PlatformName == "Android"
        //            && d.PlatformVersion[0] >= '5'
        //            && !d.IsBooked
        //            && !d.IsHidden
        //            && !d.AppiumDisabled
        //            && d.IsOnline).ToList();
        //    }
        //    else
        //    {
        //        onlineDevices = mydevices.Where(
        //            d => d.PlatformName == "iOS"
        //            && d.DeviceName.Contains("iPhone")
        //            && (d.PlatformVersion.Contains("10.3") || d.PlatformVersion.Contains("11."))
        //            && !d.IsBooked
        //            && !d.IsHidden
        //            && !d.AppiumDisabled
        //            && d.IsOnline).ToList();
        //    }

        //    var drivers = new List<MobileDriver>();
        //    Console.WriteLine($"{DateTime.UtcNow:HH:mm:ss:fff} KobitonAPI: {onlineDevices.Count()} {devicePlatform} devices found");

        //    for (var i = 0; i < driverCount; i++)
        //    {
        //        drivers.Add(CreateDriver(onlineDevices[i], methodInfo));
        //    }

        //    return drivers;
        //}

        private List<MobileDriver> CreateDrivers(int driverCount, MethodInfo methodInfo)
        {
            //List<Device> onlineDevices;

            Device device = new Device();
            var drivers = new List<MobileDriver>();
            //Console.WriteLine($"{DateTime.UtcNow:HH:mm:ss:fff} KobitonAPI: {onlineDevices.Count()} {devicePlatform} devices found");

            for (var i = 0; i < driverCount; i++)
            {
                drivers.Add(CreateDriver(device, methodInfo));
            }

            return drivers;
        }

        /// <summary>
        /// Creates a mobile driver based on device information
        /// </summary>
        /// <returns>The driver.</returns>
        /// <param name="device">Device.</param>
        /// <param name="methodInfo"></param>
        private MobileDriver CreateDriver(Device device, MethodInfo methodInfo)
        {
            var driver = new MobileDriver();

            var capabilities = CreateDesiredCapabilities(device, methodInfo);

            //if (capabilities.GetCapability("platformName").ToString() == "Android")
            //{
            try
            {
                driver.AppiumDriver = AndroidDriver(capabilities);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            //}
            //else
            //{
            //    driver.AppiumDriver = IOSDriver(capabilities);
            //}

            return driver;
        }

        /// <summary>
        /// Creates DesiredCababilities object based on Device information.
        /// </summary>
        /// <returns>The desired capabilities.</returns>
        /// <param name="device">Device.</param>
        /// <param name="methodInfo"></param>
        private DesiredCapabilities CreateDesiredCapabilities(Device device, MethodInfo methodInfo)
        {
            DesiredCapabilities capabilities = new DesiredCapabilities();

            //capabilities.SetCapability("username", AppiumConfig.GetKobitonUserName());
            //capabilities.SetCapability("accessKey", AppiumConfig.GetKobitonAccessKey());
            //capabilities.SetCapability("newCommandTimeout", "400");

            ////capabilities.SetCapability("appWaitActivity", "md5a28fba6e093d468832e1157c64fa578d.MainActivity");
            //capabilities.SetCapability("unicodeKeyboard", true);
            //capabilities.SetCapability("resetKeyboard", true);
            //capabilities.SetCapability("browserName", "Kobiton TestProvider");
            //capabilities.SetCapability("browserTimeout", 120);
            //capabilities.SetCapability("app", $"kobiton-store:{RunAppId}");

            //var className = methodInfo.ReflectedType.ToString().Split('.').Last();
            //var buildName = Environment.GetEnvironmentVariable("JOB_BASE_NAME");
            //capabilities.SetCapability("sessionName", $" {buildName} - {className} - {methodInfo.Name}");
            //capabilities.SetCapability("sessionDescription", "Automation Run");

            //capabilities.SetCapability("deviceOrientation", "portrait");
            //capabilities.SetCapability("captureScreenshots", true);
            //capabilities.SetCapability("deviceGroup", "KOBITON");
            //capabilities.SetCapability("deviceName", "i");
            //capabilities.SetCapability("platformVersion", "11.");
            //capabilities.SetCapability("version", "11.");

            capabilities.SetCapability("username", AppiumConfig.GetKobitonUserName());
            capabilities.SetCapability("accessKey", AppiumConfig.GetKobitonAccessKey());
            capabilities.SetCapability("newCommandTimeout", "400");

            //capabilities.SetCapability("appWaitActivity", "md5a28fba6e093d468832e1157c64fa578d.MainActivity");
            capabilities.SetCapability("unicodeKeyboard", true);
            capabilities.SetCapability("resetKeyboard", true);
            capabilities.SetCapability("browserName", "Kobiton TestProvider");
            capabilities.SetCapability("browserTimeout", 120);
            capabilities.SetCapability("app", $"kobiton-store:{RunAppId}");

            var className = methodInfo.ReflectedType.ToString().Split('.').Last();
            var buildName = Environment.GetEnvironmentVariable("JOB_BASE_NAME");
            capabilities.SetCapability("sessionName", $" {buildName} - {className} - {methodInfo.Name}");
            capabilities.SetCapability("sessionDescription", "Automation Run");

            capabilities.SetCapability("deviceOrientation", "portrait");
            capabilities.SetCapability("captureScreenshots", true);
            capabilities.SetCapability("deviceGroup", "KOBITON");
            capabilities.SetCapability("deviceName", "Galaxy");
            //capabilities.SetCapability("platformVersion", "11.");
            //capabilities.SetCapability("version", "11.");

            //var devicePlatform = AppiumConfig.GetDevicePlatform();
            //if (devicePlatform == DevicePlatform.IOS)
            //{
            //    // HACK: converting iOS to MAC, watch out for when Appium switches back to iOS
            //    capabilities.SetCapability("platformName", "MAC");
            //}
            //else
            //{
            capabilities.SetCapability("platformName", device.PlatformName);
            //}

            return capabilities;
        }

        /// <summary>
        /// Returns an Android driver for use in tests
        /// </summary>
        /// <returns>The driver.</returns>
        /// <param name="desiredCapabilities">Desired capabilities.</param>
        private AppiumDriver<AppiumWebElement> AndroidDriver(DesiredCapabilities desiredCapabilities)
        {
            var driver = new AndroidDriver<AppiumWebElement>(
                new Uri(standAloneServerURI),
                desiredCapabilities,
                serverWait);

            return driver;
        }

        /// <summary>
        /// Returns an IOS driver for use in tests.
        /// </summary>
        /// <returns>The river.</returns>
        /// <param name="desiredCapabilities">Desired capabilities.</param>
        private AppiumDriver<AppiumWebElement> IOSDriver(DesiredCapabilities desiredCapabilities)
        {
            Console.WriteLine($"{DateTime.UtcNow:HH:mm:ss:fff} Creating iOS Driver");

            var driver = new OpenQA.Selenium.Appium.iOS.IOSDriver<AppiumWebElement>(
                new Uri(standAloneServerURI),
                desiredCapabilities,
                serverWait);

            return driver;
        }

        /// <summary>
        /// Disposing drivers for test cleanup
        /// </summary>
        private void DisposeDrivers()
        {
            Console.WriteLine("Disposing of drivers");

            foreach (var driver in mobileDrivers)
            {
                driver.DisposeAppiumDriver();
            }
        }

        /// <summary>
        /// Test cleanup, disposing of drivers and users
        /// </summary>
        private void CleanUp(MethodInfo methodInfo)
        {
            Console.WriteLine($"{DateTime.UtcNow:HH:mm:ss:fff} Performing Cleanup");

            DisposeDrivers();
        }

        /// <summary>
        /// Gets the app path for the application being tested
        /// </summary>
        /// <returns>The app path.</returns>
        private string GetAppPath()
        {
            var devicePlatform = AppiumConfig.GetDevicePlatform();
            var appName = devicePlatform == DevicePlatform.Android ? AppiumConfig.GetAndroidRealApp() : AppiumConfig.GetiOSRealApp();

            return Path.Combine(AppiumConfig.GetAppPath(), appName);
        }
    }
}
