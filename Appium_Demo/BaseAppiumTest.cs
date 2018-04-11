using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using Xunit;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace Appium_Demo
{
    /// <summary>
    /// Base class for appium to setup necessary data for test runs and test cleanup
    /// </summary>
    public class BaseAppiumTest
    {
        private readonly ITestOutputHelper output;

        public BaseAppiumTest(ITestOutputHelper output)
        {
            this.output = output;

            output.WriteLine("Starting up new test");
        }

        public void Dispose()
        {
            output.WriteLine("Clean up test");
        }

        /// <summary>
        /// Assert no errors were found. if any were, fail and print
        /// </summary>
        /// <param name="errors">StringBuilder that tracked any errors</param>
        protected void PrintErrors(StringBuilder errors)
        {
            if (errors.Length > 0)
            {
                Assert.True("Failed" == "True", errors.ToString());
            }
        }

        public class SeleniumAttribute : BeforeAfterTestAttribute
        {
            private int userCount;
            private int driverCount;

            public SeleniumAttribute(int driverCount, int userCount)
            {
                this.userCount = userCount;
                this.driverCount = driverCount;
            }

            public override void Before(MethodInfo methodUnderTest)
            {
                TestInfo testInfo = new TestInfo();
                testInfo.WebDriverCount = driverCount;

                try
                {
                    testInfo.CreateDrivers(driverCount, methodUnderTest);
                    Console.WriteLine("\r\n--------" + "Starting test: " + methodUnderTest.Name);
                    Console.WriteLine("Created drivers");
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Failed to create drivers, {e}");
                }

                try
                {
                    if (userCount > 0)
                    {
                        testInfo.GenerateUsers(userCount);
                        Console.WriteLine("Created users");
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Failed to create users, {e}");
                }

                TestInfo.UiTestData.Add(methodUnderTest.Name, testInfo);
            }

            public override void After(MethodInfo methodUnderTest)
            {
                TestInfo testInfo = TestInfo.UiTestData[methodUnderTest.Name];
                try
                {
                    testInfo.DisposeDrivers(methodUnderTest);
                    Console.WriteLine("Cleaned up drivers");
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Failed to clean up drivers, {e}");
                }

                try
                {
                    if (userCount > 0)
                    {
                        testInfo.DisposeUsers();
                        Console.WriteLine("Cleaned up users");
                        Console.WriteLine("--------" + "Finished test: " + methodUnderTest.Name + "\r\n");
                    }
                    else
                    {
                        Console.WriteLine("--------" + "Finished test: " + methodUnderTest.Name + "\r\n");
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Failed to clean up users, {e}");
                }
            }
        }
    }
}
