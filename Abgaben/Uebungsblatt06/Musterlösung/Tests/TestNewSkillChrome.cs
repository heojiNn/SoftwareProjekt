
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using NUnit.Framework;
using OpenQA.Selenium.Support.UI;

//die neuen Skills sollten noch aus Datanbank gelöscht, bzw. gar nicht erst auf echte Datenbank hinzugefügt werden

namespace Tests
{
    [TestFixture]
    class TestNewSkillChrome
    {
        IWebDriver chromeDriver;

        [OneTimeSetUp]
        public void startBrowser()
        {
            chromeDriver = new ChromeDriver(".");
            chromeDriver.Navigate().GoToUrl("https://localhost:5001/");
            sleep();
            chromeDriver.FindElement(By.LinkText("Skills")).Click();
            sleep();
        }


        [Test]
        public void TestNewSkillValidSoftskillName()
        {
            string validName = "valider Name";
            chromeDriver.FindElement(By.LinkText("erstellen")).Click();
            sleep();
            IWebElement inputName = chromeDriver.FindElement(By.Id("name"));
            sleep();
            inputName.Clear();
            inputName.SendKeys(validName);
            sleep();

            SelectElement dropdown = new SelectElement(chromeDriver.FindElement(By.Id("skilltype")));
            dropdown.SelectByText("Softskill");
            sleep();
            chromeDriver.FindElement(By.Id("submit")).Click();
            sleep();
            Assert.IsTrue(chromeDriver.Url.ToString().EndsWith("/Skill"));
            System.Threading.Thread.Sleep(3000);
        }

        [Test]
        public void TestNewSkillInvalidSoftskillName()
        {
            string invalidName = "Softskillname!";
            sleep();
            chromeDriver.FindElement(By.LinkText("erstellen")).Click();
            sleep();

            IWebElement inputName = chromeDriver.FindElement(By.Id("name"));
            sleep();
            inputName.Clear();
            inputName.SendKeys(invalidName);


            sleep();
            SelectElement dropdown = new SelectElement(chromeDriver.FindElement(By.Id("skilltype")));
            dropdown.SelectByText("Softskill");
            sleep();

            chromeDriver.FindElement(By.Id("submit")).Click();
            IWebElement context = chromeDriver.FindElement(By.Id("submit"));
            var message = new Microsoft.AspNetCore.Components.Forms.EditContext(context).GetValidationMessages();
            foreach (string m in message)
            {
                if (m.Equals("Es sind keine Sonderzeichen erlaubt."))
                    Assert.IsTrue(m.Equals("Es sind keine Sonderzeichen erlaubt."));

            }
            Assert.IsTrue(chromeDriver.Url.ToString().EndsWith("/skill/edit/new"));
            chromeDriver.FindElement(By.LinkText("Skills")).Click();
            System.Threading.Thread.Sleep(4000);
        }

        [Test]
        public void TestNewSkillValidHardskillName()
        {
            string validName = "Hardskill-Name-ä!";
            sleep();
            chromeDriver.FindElement(By.LinkText("erstellen")).Click();
            sleep();

            IWebElement inputName = chromeDriver.FindElement(By.Id("name"));
            sleep();
            inputName.Clear();
            inputName.SendKeys(validName);

            sleep();
            SelectElement dropdown = new SelectElement(chromeDriver.FindElement(By.Id("skilltype")));
            dropdown.SelectByText("Hardskill");
            sleep();
            chromeDriver.FindElement(By.Id("submit")).Click();
            sleep();
            Assert.IsTrue(chromeDriver.Url.ToString().EndsWith("/Skill"));
            System.Threading.Thread.Sleep(3000);
        }

        [Test]
        public void TestNewSkillInvalidHardskillName()
        {
            string invalidName = "Ein viel zu langer Name: 0123456789012345678901234567890123456789";
            sleep();
            chromeDriver.FindElement(By.LinkText("erstellen")).Click();
            sleep();

            IWebElement inputName = chromeDriver.FindElement(By.Id("name"));
            sleep();
            inputName.Clear();
            inputName.SendKeys(invalidName);

            sleep();
            SelectElement dropdown = new SelectElement(chromeDriver.FindElement(By.Id("skilltype")));
            dropdown.SelectByText("Hardskill");
            sleep();

            chromeDriver.FindElement(By.Id("submit")).Click();
            IWebElement context = chromeDriver.FindElement(By.Id("submit"));
            var message = new Microsoft.AspNetCore.Components.Forms.EditContext(context).GetValidationMessages();
            foreach (string m in message)
            {
                if (m.Equals("Der Name ist zu lang."))
                    Assert.IsTrue(m.Equals("Der Name ist zu lang."));

            }
            sleep();
            Assert.IsTrue(chromeDriver.Url.ToString().EndsWith("/skill/edit/new"));
            chromeDriver.FindElement(By.LinkText("Skills")).Click();

            System.Threading.Thread.Sleep(4000);
        }

        [Test]
        public void TestNewSkillCancel()
        {
            string name = "nicht vorhanden";
            sleep();
            chromeDriver.FindElement(By.LinkText("erstellen")).Click();
            sleep();

            IWebElement inputName = chromeDriver.FindElement(By.Id("name"));
            sleep();
            inputName.Clear();
            inputName.SendKeys(name);
            sleep();

            SelectElement dropdown = new SelectElement(chromeDriver.FindElement(By.Id("skilltype")));
            dropdown.SelectByText("Hardskill");
            sleep();
            chromeDriver.FindElement(By.LinkText("abbrechen")).Click();
            sleep();
            Assert.IsTrue(chromeDriver.Url.ToString().EndsWith("/skill"));

            System.Threading.Thread.Sleep(4000);
        }

        public void sleep()
        {
            System.Threading.Thread.Sleep(2000);
        }

        [OneTimeTearDown]
        public void killBrowser()
        {
            chromeDriver.Close();
        }

    }
}
