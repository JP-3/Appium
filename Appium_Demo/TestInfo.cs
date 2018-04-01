using Appium_Demo.TestProvider;
using PageModels.AppiumUtilities.Appium;
using Polly;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;

namespace Appium_Demo
{
    public class TestInfo
    {
        private static Dictionary<string, TestInfo> uiTestData = new Dictionary<string, TestInfo>();
       // private ICreateUsers generateUsers;

        public TestInfo()
        {
            Drivers = new List<MobileDriver>();
           // Users = new List<User>();
            WebDriverCount = 0;
            UserCount = 0;
        }

        public static ITestProvider TestProvider { get; set; }

        public static Dictionary<string, TestInfo> UiTestData
        {
            get => uiTestData;
            set => uiTestData = value;
        }

        public List<MobileDriver> Drivers { get; set; }

       // public List<User> Users { get; set; }

        public int WebDriverCount { get; set; }

        public int UserCount { get; set; }

        public static TestInfo GetTestInfo([CallerMemberName] string testName = null)
        {
            TestInfo testInfo;

            try
            {
                testInfo = uiTestData[testName];
            }
            catch (KeyNotFoundException)
            {
                throw new KeyNotFoundException($"TestInfo was not created for test {testName}");
            }

            if (testInfo.Drivers.Count != testInfo.WebDriverCount)
            {
                throw new ArgumentException($"Not all webdrivers were created.\nOnly {testInfo.Drivers.Count} were created when {testInfo.WebDriverCount} was expected");
            }
            //else if (testInfo.Users.Count != testInfo.UserCount)
            //{
            //    throw new ArgumentException($"Not all users were created.\nOnly {testInfo.Users.Count} were created when {testInfo.UserCount} was expected");
            //}

            return testInfo;
        }

        ///// <summary>
        ///// Create selenium drivers
        ///// </summary>
        ///// <param name="driverCount"></param>
        public void CreateDrivers(int driverCount, MethodInfo methodInfo)
        {
            var retryPolicy = Policy
                .Handle<Exception>()
                .WaitAndRetry(
                    4,
                    retryAttempt => TimeSpan.FromSeconds(10),
                    (exception, timespan, retryCount, context) =>
                    {
                        Console.WriteLine($"Failed to create Driver: {exception}");
                        TestProvider.TearDown(methodInfo);
                        Console.WriteLine($"Retry creating driver: {retryCount}");
                    });

            Drivers = new List<MobileDriver>();

            // Use the Retry Policy to try and spin up a driver on a device, if it fails then try one more time
            retryPolicy.Execute(() => Drivers = TestProvider.Setup(driverCount, methodInfo));
        }

        /// <summary>
        /// Dispose of selenium drivers
        /// </summary>
        /// <param name="methodInfo">Method Information</param>
        public void DisposeDrivers(MethodInfo methodInfo) => TestProvider.TearDown(methodInfo);

        /// <summary>
        /// Generates users based off of which environment variable is set, userCount is the number of users to be used in the test
        /// </summary>
        /// <param name="userCount">How many users to make</param>
        public void GenerateUsers(int userCount)
        {

        }

        /// <summary>
        /// GenerateUsers
        /// </summary>
        public void DisposeUsers()
        {

        }
    }
}
