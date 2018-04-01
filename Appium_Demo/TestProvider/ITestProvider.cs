using System;
using System.Collections.Generic;
using System.Reflection;
using PageModels.AppiumUtilities.Appium;

namespace Appium_Demo.TestProvider
{
    public interface ITestProvider
    {
        List<MobileDriver> MobileDrivers { get; }

        /// <summary>
        /// Used Managing devices across the entire test run on local and uploading the app for Kobiton
        /// </summary>
        void OneTimeSetup();

        /// <summary>
        /// Does nothing for local, but on Kobiton it removes the app after the entire run has finished
        /// </summary>
        void OneTimeTearDown();

        /// <summary>
        /// Creates a customer, users and driver for each individual test, on kobiton it will find a device that is available to push the driver to
        /// </summary>
        /// <returns>The setup.</returns>
        /// <param name="driverCount">Driver count.</param>
        /// <param name="methodInfo">Method info.</param>
        List<MobileDriver> Setup(int driverCount, MethodInfo methodInfo);

        /// <summary>
        /// Removes the application from teh device after the test run and deletes any customers or users that were created
        /// </summary>
        /// <param name="methodInfo">Method info.</param>
        void TearDown(MethodInfo methodInfo);
    }
}
