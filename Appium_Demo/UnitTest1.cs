using PageModels.HomePage;
using System;
using Xunit;

namespace Appium_Demo
{
    [Collection("AppiumStartup")]
    public class UnitTest1
    {
        public UnitTest1(AppiumFixture fixture)
        {
            Fixture = fixture;
        }

        private AppiumFixture Fixture { get; }

        [Fact]
        [BaseAppiumTest.SeleniumAttribute(1, 1)]
        public void Test1()
        {
            TestInfo testInfo = TestInfo.GetTestInfo();

            HomePage homePage = new HomePage(testInfo.Drivers[0].AppiumDriver);
            homePage.EnterText();
        }
    }
}
