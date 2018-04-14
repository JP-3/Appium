using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using PageModels.AppiumUtilities.Appium;
using PageModels.AppiumUtilities.Config;
using PageModels.AppiumUtilities.Support;

namespace Appium_Demo.TestProvider
{
    public class GridProvider : ITestProvider
    {
        private string[] android = { "local" };
        public List<MobileDriver> MobileDrivers { get; set;  }

        public void OneTimeSetup()
        {
            MobileDriverFactory.Que.SetDeviceLibrary(android);
        }

        public void OneTimeTearDown()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Setup.  Called once per individual test to setup initial driver and user information for the test.
        /// </summary>
        /// <param name="deviceCount">Number of devices</param>
        /// <param name="methodInfo">The Method information</param>
        /// <returns>List of mobile devices</returns>
        public List<MobileDriver> Setup(int deviceCount, MethodInfo methodInfo)
        {
            return CreateDrivers(deviceCount, methodInfo);
        }

        /// <summary>
        /// Tears down.  Called once per test execution to clean up test data.
        /// </summary>
        /// <param name="methodInfo">method information</param>
        public void TearDown(MethodInfo methodInfo) => CleanUp(methodInfo);

        /// <summary>
        /// Creation of the drivers for the tests to use
        /// </summary>
        /// <param name="driverCount"> number of appium drivers to spin up for the test</param>
        /// <param name="methodInfo">Method Information</param>
        /// <returns>Mobile Driver List</returns>
        public List<MobileDriver> CreateDrivers(int driverCount, MethodInfo methodInfo)
        {
            MobileDrivers = new List<MobileDriver>();

             MobileDriver mobileDriver = new MobileDriver();
             mobileDriver.CreateAppiumDriver(MobileDriverFactory.Que.CheckoutDevice(methodInfo.Name));
             MobileDrivers.Add(mobileDriver);

            return MobileDrivers;
        }

        /// <summary>
        /// Disposing drivers for test cleanup
        /// </summary>
        /// <param name="methodInfo">Method Information</param>
        public void DisposeDrivers(MethodInfo methodInfo)
        {
            try
            {
                // Parallel loop to dispose of all the drivers at once speeding up cleanup
                Parallel.ForEach(MobileDrivers, device =>
                {
                    if (device.AppiumDriver.Capabilities.GetCapability("platformName").ToString().ToLower().Equals("ios"))
                    {
                        device.AppiumDriver.CloseApp();
                        device.AppiumDriver.RemoveApp(AppiumConfig.GetAppId());
                    }

                    device.DisposeAppiumDriver();
                });
            }
            catch (Exception e)
            {
                Console.WriteLine("IF YOU SEE ME YOU ARE NOT CLEANING UP DEVICES PROPERLY.");
                Console.WriteLine(e);
            }

            MobileDriverFactory.Que.CheckinDevice(methodInfo.Name);
        }

        /// <summary>
        /// Test cleanup, disposing of drivers and users
        /// </summary>
        /// <param name="methodInfo">Method Information</param>
        public void CleanUp(MethodInfo methodInfo) => DisposeDrivers(methodInfo);
    }
}