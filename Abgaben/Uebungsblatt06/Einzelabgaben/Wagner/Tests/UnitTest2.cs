using System;
using System.Linq;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Support.UI;

namespace Tests
{

    public class UnitTest2
    {

        IWebDriver driver;

        [SetUp]
        public void startBrowser()
        {
            driver = new FirefoxDriver(".");
        }
        // works in 1 of 10 trys if the time is right by chance
        [Test]
        public void test()
        {
            driver.Navigate().GoToUrl("http://localhost:5000");
            driver.FindElement(By.LinkText("Skills")).Click();
            driver.FindElement(By.LinkText("erstellen")).Click();
            IWebElement inpuField = driver.FindElement(By.Id("name"));
            inpuField = driver.FindElement(By.Id("name"));
            inpuField.Clear();
            inpuField = driver.FindElement(By.Id("name"));
            inpuField.SendKeys("ZweiterTest");


            Click(By.Id("submit"));


            var result = driver.FindElement(By.XPath("//*[contains(., 'ZweiterTest')]"));

        }

        [TearDown]
        public void closeBrowser()
        {
            driver.Close();
        }
        public bool Click(By by)
        {
            bool result = false;
            int attempts = 0;
            while (attempts < 4)
            {
                try
                {
                    driver.FindElement(by).Click();
                    result = true;
                    break;
                }
                catch (StaleElementReferenceException) { }
                attempts++;
            }
            return result;
        }

    }
}


