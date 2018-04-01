using System;
using Xunit;

namespace Appium_Demo
{
    public class UnitTest1
    {
        [Fact]
        [BaseAppiumTest.SeleniumAttribute(1, 1)]
        public void Test1()
        {
            TestInfo testInfo = TestInfo.GetTestInfo();

            var v = testInfo.Drivers[0].AppiumDriver;
        }
    }
}
