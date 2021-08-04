using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using System.Collections.Generic;
using System;

namespace Tests.EndEnd
{
    [TestFixture]
    public class LoginE2ETest
    {
        string url = "http://localhost:5005";
        List<IWebDriver> drivers = new List<IWebDriver>() { /*new FirefoxDriver(),*/ new ChromeDriver() };

        [OneTimeSetUp]
        public void Setup()
        {
            foreach (IWebDriver driver in drivers)
            {
                driver.Navigate().GoToUrl(url + "/login");
                driver.Manage().Window.Maximize();
            }
        }

        [Test]
        [Order(1)]
        [TestCase("001", "001")]
        public void Login(string persnr, string password)
        {
            foreach (IWebDriver driver in drivers)
            {
                driver.FindElement(By.XPath("/html/body/div[1]/div[2]/div/div/div[2]/div[1]/div/div[1]/input")).SendKeys(persnr);
                driver.FindElement(By.XPath("/html/body/div[1]/div[2]/div/div/div[2]/div[1]/div/div[2]/input")).SendKeys(password);
                driver.FindElement(By.XPath("/html/body/div[1]/div[2]/div/div/div[2]/div[1]/div/button[1]")).Click();

                System.Threading.Thread.Sleep(2000);
                Assert.AreEqual("http://localhost:5005/my-profile", driver.Url, "Login e2e test failed");
            }

        }

        [Test]
        [Order(2)]
        public void Logout()
        {
            foreach (IWebDriver driver in drivers)
            {
                Assert.AreNotEqual(url + "/login", driver.Url, "Logout e2e test failed, must be logged in");
                driver.FindElement(By.LinkText("Logout")).Click();

                System.Threading.Thread.Sleep(500);
                Assert.AreEqual(driver.Url, "http://localhost:5005/login", "Logout test failed, account cannot be logged out");
            }
        }

        [OneTimeTearDown]
        public void CleanUp()
        {
            foreach (IWebDriver driver in drivers)
            {
                driver.Quit();
            }
        }
    }
}

