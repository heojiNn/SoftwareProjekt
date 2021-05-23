using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using NUnit.Framework;


namespace Test
{
    [TestFixture]
    class TestNewSkill
    {
        IWebDriver chromeDriver;
        // IWebDriver firefoxDriver = new FirefoxDriver();
       // ChromeOptions options = new ChromeOptions();


        [SetUp]
        public void startBrowser()
        {
           // options.AddArgument("ignore-certificate-errors");
            //options.AddExcludedArgument("enable-automation" );
            chromeDriver = new ChromeDriver("C:/Users/giddi/Desktop/ChromeDriver");
        }
        

        [Test]
        public void TestNewSkills()
        {
            chromeDriver.Navigate().GoToUrl("localhost:5001");
            chromeDriver.FindElement(By.LinkText("Skills")).Click();
            System.Threading.Thread.Sleep(3000);
            chromeDriver.FindElement(By.LinkText("erstellen")).Click();
            System.Threading.Thread.Sleep(3000);
            IWebElement input = chromeDriver.FindElement(By.Id("name"));
            System.Threading.Thread.Sleep(3000);
            input.SendKeys("Test");

        }


        [TearDown]
        public void killBrowser()
        {
            chromeDriver.Close();
        }

    }
}
