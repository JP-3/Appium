using Appium_Demo.TestProvider;
using PageModels.AppiumUtilities.Config;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Appium_Demo
{
    [Collection("AppiumStartup")]
    public class AppiumFixture : IDisposable
    {
        private readonly ITestProvider testProvider;

        public AppiumFixture()
        {
            testProvider = CreateTestProvider();
            testProvider.OneTimeSetup();

            TestInfo.TestProvider = testProvider;
        }

        public void Dispose() => testProvider.OneTimeTearDown();

        private static ITestProvider CreateTestProvider()
        {
            var myTestProvider = AppiumConfig.GetTestProvider();

            if (myTestProvider.ToString().ToUpper() == "KOBITON")
            {
                return new KobitonTestProvider();
            }

            return new GridProvider();
        }
    }
}
