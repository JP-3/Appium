using OpenQA.Selenium.Appium;
using PageModels.AppiumUtilities.Appium;
using System;
using System.Collections.Generic;
using System.Text;

namespace PageModels.HomePage
{
    public class HomePage
    {
        private IHomePage Home { get; set; }
        private AppiumWait Wait { get; set; }

        public HomePage(AppiumDriver<AppiumWebElement> driver)
        {
            Home = new AndroidHome();
        }

        public void EnterText()
        {
            var t = Home.NineButton;
            Wait.ForClickableElement(Home.NineButton).Click();
        }
    }
}

