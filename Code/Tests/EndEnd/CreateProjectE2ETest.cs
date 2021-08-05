using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Support.UI;
using System.Collections.Generic;
using System;

namespace Tests.EndEnd
{
    [TestFixture]
    public class CreateProjectE2ETest
    {
        List<IWebDriver> drivers = new List<IWebDriver>() { new FirefoxDriver(), new ChromeDriver() };
        string url = "http://localhost:5005";

        [OneTimeSetUp]
        public void Setup()
        {
            //Login
            foreach (IWebDriver driver in drivers)
            {
                driver.Navigate().GoToUrl(url + "/login");
                driver.Manage().Window.Maximize();
                driver.FindElement(By.XPath("/html/body/div[1]/div[2]/div/div/div[2]/div[1]/div/div[1]/input")).SendKeys("000");
                driver.FindElement(By.XPath("/html/body/div[1]/div[2]/div/div/div[2]/div[1]/div/div[2]/input")).SendKeys("000");
                driver.FindElement(By.XPath("/html/body/div[1]/div[2]/div/div/div[2]/div[1]/div/button[1]")).Click();
                driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(2);
            }
        }

        [Test]
        [Order(1)]
        [TestCase("ProjektTest", "12102011", "14122013", "Automobil", "Beschreibug Test",
           new string[] { "TestZweck1", "TestZweck2" },                                     //Zwecke
           new string[] { "TestTaetigkeit1", "TestTaetigkeit2", "TestTaetigkeit3" },        //Tätigkeiten
           new string[] { "Admin admin", "Daniel Craig" })]                                 //Mitarbeiter

        public void ValidCreateProject(string title, string startDate, string endDate, string field,
           string description, string[] purpose, string[] activity, string[] employees)
        {
            foreach (IWebDriver driver in drivers)
            {
                //Auf Projektübersicht seite gehen
                driver.FindElement(By.LinkText("Projektübersicht")).Click();
                Assert.AreEqual("http://localhost:5005/projects", driver.Url, "Could not reach projects page");

                //Auf Projekt erstellen Seite gehen
                driver.FindElement(By.LinkText("Projekt erstellen")).Click();
                Assert.AreEqual("http://localhost:5005/project-create", driver.Url, "Could not reach project-create page");

                ////Neues Projekt mit den übergebenen Daten erstellen

                //Titel
                driver.FindElement(By.XPath("/html/body/div[1]/div[2]/div/div[1]/div/div/div/div[2]/h3/input")).SendKeys(title);
                //Anfangsdatum
                driver.FindElement(By.Id("start")).SendKeys(startDate);
                //Enddatum
                driver.FindElement(By.Id("end")).SendKeys(endDate);
                //Branche
                var BrancheSelect = driver.FindElement(By.Id("field"));
                var selectElement = new SelectElement(BrancheSelect);
                selectElement.SelectByValue(field);

                //Beschreibung
                driver.FindElement(By.Id("description")).SendKeys(description);

                //Zwecke
                for (int i = 0; i < purpose.Length; i++)
                {
                    driver.FindElement(By.Id("purpose " + i)).SendKeys(purpose[i]);
                    driver.FindElement(By.Id("AddPurpose")).Click();
                }

                //Tätigkeiten
                for (int i = 0; i < activity.Length; i++)
                {
                    driver.FindElement(By.Id("activity " + i)).SendKeys(activity[i]);
                    driver.FindElement(By.XPath("/html/body/div[1]/div[2]/div/div[3]/div[2]/div/div[2]/button")).Click();
                }

                //Mitarbeiter hinzufügen (jeder Mitarbeiter wird beispielhaft jeder Aktivität hinzugefügt)
                for (int i = 0; i < employees.Length; i++)
                {
                    for (int j = 0; j < activity.Length; j++)
                    {
                        driver.FindElement(By.Id(activity[j])).Click();

                        //Sicherstellen, dass das Popup Fester geöffnet wird
                        IWebElement popUp = driver.FindElement(By.XPath("/html/body/div[1]/div[2]/div/div[5]/div/div/div[1]/h5"));
                        string popUpHeader = popUp.Text;
                        Assert.AreEqual("Fügen Sie der Tätigkeit Mitarbeiter und Skills hinzu", popUpHeader);

                        driver.FindElement(By.XPath("/html/body/div/div/div/div/div/div/div/div/div/div/input[@placeholder='Name']")).SendKeys(employees[i]);
                        driver.FindElement(By.XPath("/html/body/div[1]/div[2]/div/div[5]/div/div/div[2]/div[1]/div[2]/div")).Click();
                        driver.FindElement(By.XPath("//button[text()='speichern']")).Click();
                    }
                }

                //Projekt speichern
                driver.FindElement(By.XPath("//button[text()=' Speichern ']")).Click();

                //Sicherstellen, dass das Projekt erstellt wurde (man ist automatisch auf Detailansicht, wenn es geklappt hat)
                IWebElement projectHeader = driver.FindElement(By.XPath("/html/body/div[1]/div[2]/div/div[1]/div/h3"));
                string projectHeaderText = projectHeader.Text;
                Assert.AreEqual(title, projectHeaderText);

                //Zurück zur Projektübersicht
                driver.FindElement(By.XPath("/ html / body / div[1] / div[2] / div / a[3]")).Click();
                Assert.AreEqual("http://localhost:5005/projects", driver.Url, "Could not reach projects page");
            }
        }

        [Test]
        [Order(2)]
        [TestCase("ProjektTest")]
        [TestCase("projekttest")]
        public void InvalidDuplicateCreateProject(string title)
        {
            foreach (IWebDriver driver in drivers)
            {
                driver.FindElement(By.LinkText("Projekt erstellen")).Click();
                Assert.AreEqual("http://localhost:5005/project-create", driver.Url, "Could not reach project-create page");

                //Titel
                driver.FindElement(By.XPath("/html/body/div[1]/div[2]/div/div[1]/div/div/div/div[2]/h3/input")).SendKeys(title);

                //Projekt speichern
                driver.FindElement(By.XPath("//button[text()=' Speichern ']")).Click();

                //Sicherstellen, dass richtige Fehlermeldung angezeigt wird
                IWebElement errorBox = driver.FindElement(By.XPath("/html/body/div[1]/div[2]/div/div[6]/div/div/div[2]/ul/li/pre"));
                string error = errorBox.Text;
                Assert.AreEqual("Es existiert schon ein Projekt mit diesem Titel.", error);

                //Schließen
                driver.FindElement(By.XPath("/html/body/div[1]/div[2]/div/div[6]/div/div/div[3]/button")).Click();

                //Zurück zur Projektübersicht
                driver.FindElement(By.XPath("/html/body/div[1]/div[2]/div/a")).Click();
                Assert.AreEqual("http://localhost:5005/projects", driver.Url, "Could not reach projects page");

            }
        }


        [Test]
        [Order(3)]
        [TestCase("")]
        [TestCase("InvalidLongTitleInvalidLongTitleInvalidLongTitleInvalidLongTitleInvalidLongTitle")]

        public void InvalidTitleCreateProject(string title)
        {
            foreach (IWebDriver driver in drivers)
            {
                driver.FindElement(By.LinkText("Projekt erstellen")).Click();
                Assert.AreEqual("http://localhost:5005/project-create", driver.Url, "Could not reach project-create page");

                //Titel
                driver.FindElement(By.XPath("/html/body/div[1]/div[2]/div/div[1]/div/div/div/div[2]/h3/input")).SendKeys(title);

                //Projekt speichern
                driver.FindElement(By.XPath("//button[text()=' Speichern ']")).Click();

                //Sicherstellen, dass richtige Fehlermeldung angezeigt wird
                if (title == "")
                {
                    IWebElement errorBox = driver.FindElement(By.XPath("/html/body/div[1]/div[2]/div/div[6]/div/div/div[2]/ul/li/pre"));
                    string error = errorBox.Text;
                    Assert.AreEqual("Der Titel darf nicht leer sein.", error);
                }
                else if (title == "InvalidLongTitleInvalidLongTitleInvalidLongTitleInvalidLongTitleInvalidLongTitle")
                {
                    IWebElement errorBox = driver.FindElement(By.XPath("/html/body/div[1]/div[2]/div/div[6]/div/div/div[2]/ul/li/pre"));
                    string error = errorBox.Text;
                    Assert.AreEqual("Der Titel darf nicht länger als 40 Zeichen sein.", error);
                }

                //Schließen
                driver.FindElement(By.XPath("/html/body/div[1]/div[2]/div/div[6]/div/div/div[3]/button")).Click();

                //Zurück zur Projektübersicht
                driver.FindElement(By.XPath("/html/body/div[1]/div[2]/div/a")).Click();
                Assert.AreEqual("http://localhost:5005/projects", driver.Url, "Could not reach projects page");
            }
        }

        [Test]
        [Order(3)]
        [TestCase("ProjektDatumTest", "22122001", "22122004")]
        [TestCase("ProjektDatumTest", "22122015", "22122013")]

        public void InvalidDateCreateProject(string title, string startdate, string enddate)
        {
            foreach (IWebDriver driver in drivers)
            {
                driver.FindElement(By.LinkText("Projekt erstellen")).Click();
                Assert.AreEqual("http://localhost:5005/project-create", driver.Url, "Could not reach project-create page");

                //Titel
                driver.FindElement(By.XPath("/html/body/div[1]/div[2]/div/div[1]/div/div/div/div[2]/h3/input")).SendKeys(title);
                //Datum
                driver.FindElement(By.Id("start")).SendKeys(startdate);
                driver.FindElement(By.Id("end")).SendKeys(enddate);

                //Projekt speichern
                driver.FindElement(By.XPath("//button[text()=' Speichern ']")).Click();

                //Sicherstellen, dass richtige Fehlermeldung angezeigt wird
                if (startdate == "22122001")
                {
                    IWebElement errorBox = driver.FindElement(By.XPath("/html/body/div[1]/div[2]/div/div[6]/div/div/div[2]/ul/li/pre"));
                    string error = errorBox.Text;
                    Assert.AreEqual("Der Projektanfang kann nicht vor dem Jahr 2011 liegen.", error);
                }
                else if (startdate == "22122015")
                {
                    IWebElement errorBox = driver.FindElement(By.XPath("/html/body/div[1]/div[2]/div/div[6]/div/div/div[2]/ul/li/pre"));
                    string error = errorBox.Text;
                    Assert.AreEqual("Das Enddatum muss hinter dem Beginn liegen.", error);
                }

                //Schließen
                driver.FindElement(By.XPath("/html/body/div[1]/div[2]/div/div[6]/div/div/div[3]/button")).Click();

                //Zurück zur Projektübersicht
                driver.FindElement(By.XPath("/html/body/div[1]/div[2]/div/a")).Click();
                Assert.AreEqual("http://localhost:5005/projects", driver.Url, "Could not reach projects page");
            }
        }

        [Test]
        [Order(4)]
        [TestCase("ProjektDatumTest", "InvalidDescriptionistolong")]

        public void InvalidDescriptionCreateProject(string title, string description)
        {
            foreach (IWebDriver driver in drivers)
            {
                driver.FindElement(By.LinkText("Projekt erstellen")).Click();
                Assert.AreEqual("http://localhost:5005/project-create", driver.Url, "Could not reach project-create page");

                //Titel
                driver.FindElement(By.XPath("/html/body/div[1]/div[2]/div/div[1]/div/div/div/div[2]/h3/input")).SendKeys(title);
                //Description
                for (int i = 0; i < 40; i++)
                {
                    driver.FindElement(By.Id("description")).SendKeys(description);
                }

                //Projekt speichern
                driver.FindElement(By.XPath("//button[text()=' Speichern ']")).Click();

                //Sicherstellen, dass richtige Fehlermeldung angezeigt wird
                IWebElement errorBox = driver.FindElement(By.XPath("/html/body/div[1]/div[2]/div/div[6]/div/div/div[2]/ul/li/pre"));
                string error = errorBox.Text;
                Assert.AreEqual("Die Beschreibung darf nicht länger als 400 Zeichen sein.", error);

                //Schließen
                driver.FindElement(By.XPath("/html/body/div[1]/div[2]/div/div[6]/div/div/div[3]/button")).Click();

                //Zurück zur Projektübersicht
                driver.FindElement(By.XPath("/html/body/div[1]/div[2]/div/a")).Click();
                Assert.AreEqual("http://localhost:5005/projects", driver.Url, "Could not reach projects page");
            }
        }

        [OneTimeTearDown]
        public void CleanUp()
        {
            foreach (IWebDriver driver in drivers)
            {
                //Erstelltes Projekt löschen
                driver.FindElement(By.XPath("/ html / body / div[1] / div[2] / div / html / body / input")).SendKeys("ProjektTest");
                driver.FindElement(By.XPath("/ html / body / div[1] / div[2] / div / html / body / table / tr[2] / td[3] / div / button")).Click();
                driver.FindElement(By.XPath("/ html / body / div[1] / div[2] / div / html / body / table / tr[2] / td[3] / div / div / a[3] / a")).Click();
                driver.Quit();
            }
        }
    }
}

