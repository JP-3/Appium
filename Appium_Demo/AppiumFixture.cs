using Appium_Demo.TestProvider;
using PageModels.AppiumUtilities.Config;
using System;
using System.Collections.Generic;
using System.Text;

namespace Appium_Demo
{
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

            return new KobitonTestProvider();


            // return new GridTestProvider();
        }
    }
}
