using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Text;

namespace PageModels.HomePage
{
    public class BaseHomePage
    {
        //number9
        public virtual By NineButton => By.XPath("//android.widget.Button[@content-desc='number9']");
    }
}
