using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Appium_Demo
{
    [CollectionDefinition("AppiumStartup")]
    public class AppiumCollection : ICollectionFixture<AppiumFixture>
    {
    }
}
