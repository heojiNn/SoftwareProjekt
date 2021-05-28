using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using NUnit.Framework;
using OpenQA.Selenium.Support.UI;

//die neuen Skills sollten noch aus Datanbank gelöscht, bzw. gar nicht erst auf echte Datenbank hinzugefügt werden

namespace Tests
{
    [TestFixture]
    class TestNewSkillFirefox
    {
        IWebDriver firefoxDriver;

        [OneTimeSetUp]
        public void startBrowser()
        {
            firefoxDriver = new FirefoxDriver(".");
            firefoxDriver.Navigate().GoToUrl("http://localhost:5000/");


            sleep();
            firefoxDriver.FindElement(By.LinkText("Skills")).Click();
            sleep();
        }


        [Test]
        public void TestNewSkillValidSoftskillName()
        {
            string validName = "valider Name";
            firefoxDriver.FindElement(By.LinkText("erstellen")).Click();
            sleep();
            IWebElement inputName = firefoxDriver.FindElement(By.Id("name"));
            sleep();
            inputName.Clear();
            inputName.SendKeys(validName);
            sleep();

            SelectElement dropdown = new SelectElement(firefoxDriver.FindElement(By.Id("skilltype")));
            dropdown.SelectByText("Softskill");
            sleep();
            firefoxDriver.FindElement(By.Id("submit")).Click();
            sleep();
            Assert.IsTrue(firefoxDriver.Url.ToString().EndsWith("/Skill"));
            System.Threading.Thread.Sleep(3000);

        }

        [Test]
        public void TestNewSkillInvalidSoftskillName()
        {
            string invalidName = "Softskillname!";
            sleep();
            firefoxDriver.FindElement(By.LinkText("erstellen")).Click();
            sleep();

            IWebElement inputName = firefoxDriver.FindElement(By.Id("name"));
            sleep();
            inputName.Clear();
            inputName.SendKeys(invalidName);


            sleep();
            SelectElement dropdown = new SelectElement(firefoxDriver.FindElement(By.Id("skilltype")));
            dropdown.SelectByText("Softskill");
            sleep();

            firefoxDriver.FindElement(By.Id("submit")).Click();
            IWebElement context = firefoxDriver.FindElement(By.Id("submit"));
            var message = new Microsoft.AspNetCore.Components.Forms.EditContext(context).GetValidationMessages();
            foreach (string m in message)
            {
                if (m.Equals("Es sind keine Sonderzeichen erlaubt."))
                    Assert.IsTrue(m.Equals("Es sind keine Sonderzeichen erlaubt."));

            }
            Assert.IsTrue(firefoxDriver.Url.ToString().EndsWith("/skill/edit/new"));
            firefoxDriver.FindElement(By.LinkText("Skills")).Click();
            System.Threading.Thread.Sleep(4000);
        }

        [Test]
        public void TestNewSkillValidHardskillName()
        {
            string validName = "Hardskill-Name-ä!";
            sleep();
            firefoxDriver.FindElement(By.LinkText("erstellen")).Click();
            sleep();

            IWebElement inputName = firefoxDriver.FindElement(By.Id("name"));
            sleep();
            inputName.Clear();
            inputName.SendKeys(validName);

            sleep();
            SelectElement dropdown = new SelectElement(firefoxDriver.FindElement(By.Id("skilltype")));
            dropdown.SelectByText("Hardskill");
            sleep();
            firefoxDriver.FindElement(By.Id("submit")).Click();
            sleep();
            Assert.IsTrue(firefoxDriver.Url.ToString().EndsWith("/Skill"));
            System.Threading.Thread.Sleep(3000);
        }

        [Test]
        public void TestNewSkillInvalidHardskillName()
        {
            string invalidName = "Ein viel zu langer Name: 0123456789012345678901234567890123456789";
            sleep();
            firefoxDriver.FindElement(By.LinkText("erstellen")).Click();
            sleep();

            IWebElement inputName = firefoxDriver.FindElement(By.Id("name"));
            sleep();
            inputName.Clear();
            inputName.SendKeys(invalidName);

            sleep();
            SelectElement dropdown = new SelectElement(firefoxDriver.FindElement(By.Id("skilltype")));
            dropdown.SelectByText("Hardskill");
            sleep();

            firefoxDriver.FindElement(By.Id("submit")).Click();
            IWebElement context = firefoxDriver.FindElement(By.Id("submit"));
            var message = new Microsoft.AspNetCore.Components.Forms.EditContext(context).GetValidationMessages();
            foreach (string m in message)
            {
                if (m.Equals("Der Name ist zu lang."))
                    Assert.IsTrue(m.Equals("Der Name ist zu lang."));

            }
            sleep();
            Assert.IsTrue(firefoxDriver.Url.ToString().EndsWith("/skill/edit/new"));
            firefoxDriver.FindElement(By.LinkText("Skills")).Click();

            System.Threading.Thread.Sleep(4000);
        }

        [Test]
        public void TestNewSkillCancel()
        {
            string name = "nicht vorhanden";
            sleep();
            firefoxDriver.FindElement(By.LinkText("erstellen")).Click();
            sleep();

            IWebElement inputName = firefoxDriver.FindElement(By.Id("name"));
            sleep();
            inputName.Clear();
            inputName.SendKeys(name);
            sleep();

            SelectElement dropdown = new SelectElement(firefoxDriver.FindElement(By.Id("skilltype")));
            dropdown.SelectByText("Hardskill");
            sleep();
            firefoxDriver.FindElement(By.LinkText("abbrechen")).Click();
            sleep();
            Assert.IsTrue(firefoxDriver.Url.ToString().EndsWith("/skill"));

            System.Threading.Thread.Sleep(4000);
        }

        public void sleep()
        {
            System.Threading.Thread.Sleep(2000);
        }

        [OneTimeTearDown]
        public void killBrowser()
        {
            firefoxDriver.Close();
        }

    }
}
