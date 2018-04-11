using OpenQA.Selenium.Appium;
using PageModels.AppiumUtilities.Appium;
using System;
using System.Collections.Generic;
using System.Text;

namespace PageModels.HomePage
{
    public class HomePage
    {
        internal IHomePage Home { get; set; }
        protected AppiumWait Wait { get; set; }

        public HomePage(AppiumDriver<AppiumWebElement> driver)
        {
            Home = new AndroidHome();
        }

        public void EnterText()
        {
            Wait.ForClickableElement(Home.NineButton).Click();
        }
    }
}

