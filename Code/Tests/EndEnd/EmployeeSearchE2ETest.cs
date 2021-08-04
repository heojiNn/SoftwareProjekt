using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using System.Collections.Generic;
using System;

namespace Tests.EndEnd
{
    [TestFixture]
    public class EmployeeSearchE2ETest
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
        //Nach unten aufgeführten Hardskills, Softskills & Branchen suchen
        [TestCase("",                                                                           //Name (keine Eingabe)
                  new string[] { "" },                                                          //Rollen (keine Eingabe)
                  new string[] { "Cypress", "C#" },                                             //Hardskills
                  new string[] { "Problemlösungsfähigkeit" },                                   //Softskills
                  new string[] { "Elektro/Elektronik" },                                        //Branchen
                  new string[] { "" })]                                                         //Sprachen (keine Eingabe)

        //Nach unten aufgeführten Rollen, Hardskills, Softskills, Branchen & Sprachen suchen
        [TestCase("",                                                                           //Name (keine Eingabe)
                  new string[] { "Agile Coach", "Consultant" },                                 //Rollen
                  new string[] { "Ubuntu", "MongoDB", "Automapper" },                           //Hardskills
                  new string[] { "Akquisitionsstärke", "Beratungsfähigkeit" },                  //Softskills
                  new string[] { "Automobil", "IT" },                                           //Branchen
                  new string[] { "Arabisch", "Türkisch" })]                                     //Sprachen

        //Nur nach Name suchen (sobald nach Name gesucht wird, werden alle anderen Eingaben ignoriert)
        [TestCase("Arnold",                                                                     //Name 
                  new string[] { "" },                                                          //Rollen (keine Eingabe)
                  new string[] { "" },                                                          //Hardskills (keine Eingabe)
                  new string[] { "" },                                                          //Softskills (keine Eingabe)
                  new string[] { "" },                                                          //Branchen (keine Eingabe)
                  new string[] { "" })]                                                         //Sprachen (keine Eingabe)

        public void ValidEmployeeSearch(string name, string[] role, string[] hardskills, string[] softskills, string[] fields, string[] languages)
        {
            foreach (IWebDriver driver in drivers)
            {
                driver.FindElement(By.LinkText("Mitarbeitersuche")).Click();
                Assert.AreEqual("http://localhost:5005/employee-search", driver.Url, "Could not reach EmployeeSearch page");

                driver.Navigate().Refresh();

                //Namen (man kann nur nach einem Namen gleichzeitig suchen)
                driver.FindElement(By.XPath("/html/body/div[1]/div[2]/div/html/body/div[2]/div/div/div[1]/div/input[@placeholder='Name']")).SendKeys(name);
                if (name.Length > 1)
                {
                    driver.FindElement(By.XPath("/ html / body / div[1] / div[2] / div / html / body / div[2] / div / div / div[1] / div[2] / div")).Click();
                }

                //Rollen
                if (role[0] != "" || role.Length > 1)
                {
                    for (int i = 0; i < role.Length; i++)
                    {
                        driver.FindElement(By.XPath("//input[@placeholder='Rollen']")).SendKeys(role[i]);
                        driver.FindElement(By.XPath("/html/body/div[1]/div[2]/div/html/body/div[2]/div/div/div[2]/div[2]/div")).Click();
                    }
                }

                //Hardskills
                if (hardskills[0] != "" || hardskills.Length > 1)
                {
                    for (int i = 0; i < hardskills.Length; i++)
                    {
                        driver.FindElement(By.XPath("//input[@placeholder='Hardskills']")).SendKeys(hardskills[i]);
                        driver.FindElement(By.XPath("/html/body/div[1]/div[2]/div/html/body/div[2]/div/div/div[3]/div[2]/div[1]")).Click();
                    }
                }

                //Sofskills
                if (softskills[0] != "" || softskills.Length > 1)
                {
                    for (int i = 0; i < softskills.Length; i++)
                    {
                        driver.FindElement(By.XPath("//input[@placeholder='Softskills']")).SendKeys(softskills[i]);
                        driver.FindElement(By.XPath("/html/body/div[1]/div[2]/div/html/body/div[2]/div/div/div[4]/div[2]/div[1]")).Click();
                    }
                }

                //Branchen
                if (fields[0] != "" || fields.Length > 1)
                {
                    for (int i = 0; i < fields.Length; i++)
                    {
                        driver.FindElement(By.XPath("//input[@placeholder='Branchen']")).SendKeys(fields[i]);
                        driver.FindElement(By.XPath("/html/body/div[1]/div[2]/div/html/body/div[2]/div/div/div[5]/div[2]/div[1]")).Click();
                    }
                }

                //Sprachen
                if (languages[0] != "" || languages.Length > 1)
                {
                    for (int i = 0; i < languages.Length; i++)
                    {
                        driver.FindElement(By.XPath("//input[@placeholder='Sprachen']")).SendKeys(languages[i]);
                        driver.FindElement(By.XPath("/html/body/div[1]/div[2]/div/html/body/div[2]/div/div/div[6]/div[2]/div[1]")).Click();
                    }
                }


                //Suchen Button
                driver.FindElement(By.XPath("/html/body/div[1]/div[2]/div/html/body/div[2]/div/input[1]")).Click();

                //Sicherstellen, dass die Tabelle mit den Suchergebnissen angezeigt wird
                //'Arnold schwarzenegger' erfüllt bei allen 3 Suchanfragen alle Kriterien (also steht er ganz oben)
                driver.FindElement(By.XPath("/ html / body / div[1] / div[2] / div / html / body / div[3]"));
                IWebElement tableCell = driver.FindElement(By.XPath("/html/body/div[1]/div[2]/div/html/body/div[3]/table/tbody/tr/td[1]/a"));
                string searchResult = tableCell.Text;
                Assert.AreEqual("Arnold schwarzenegger", searchResult);

                //Neue Suche
                driver.FindElement(By.XPath("/ html / body / div[1] / div[2] / div / html / body / div[3] / div / input")).Click();
            }
        }

        [Test]
        [Order(2)]
        public void ExtendedEmployeeSearch()
        {
            foreach (IWebDriver driver in drivers)
            {
                driver.FindElement(By.LinkText("Mitarbeitersuche")).Click();
                Assert.AreEqual("http://localhost:5005/employee-search", driver.Url, "Could not reach EmployeeSearch page");

                //Hardskill
                driver.FindElement(By.XPath("//input[@placeholder='Hardskills']")).SendKeys("C");
                driver.FindElement(By.XPath("/html/body/div[1]/div[2]/div/html/body/div[2]/div/div/div[3]/div[2]/div[1]")).Click();

                //Softskill
                driver.FindElement(By.XPath("//input[@placeholder='Softskills']")).SendKeys("Akquisi");
                driver.FindElement(By.XPath("/html/body/div[1]/div[2]/div/html/body/div[2]/div/div/div[4]/div[2]/div[1]")).Click();

                //Erweiter Button
                driver.FindElement(By.XPath("/html/body/div[1]/div[2]/div/html/body/div[2]/div/input[2]")).Click();

                //Erweitert Hardskill
                driver.FindElement(By.XPath("/html/body/div[1]/div[2]/div/html/body/div[2]/div[2]/div/div[2]/div/div[1]/input")).SendKeys("C");
                driver.FindElement(By.XPath("/html/body/div[1]/div[2]/div/html/body/div[2]/div[2]/div/div[2]/div[2]/div[1]")).Click();

                //Suchen Button
                driver.FindElement(By.XPath("/html/body/div[1]/div[2]/div/html/body/div[2]/div/input[1]")).Click();

                //Sicherstellen, dass die Tabelle mit den Suchergebnissen angezeigt wird
                //'Arnold schwarzenegger' erfüllt bei allen 3 Suchanfragen alle Kriterien (also steht er ganz oben)
                driver.FindElement(By.XPath("/html/body/div[1]/div[2]/div/html/body/div[3]"));
                IWebElement tableCell = driver.FindElement(By.XPath("/html/body/div[1]/div[2]/div/html/body/div[3]/table/tbody/tr/td[1]/a"));
                string searchResult = tableCell.Text;
                Assert.AreEqual("Daniel Craig", searchResult);

                //Neue Suche
                driver.FindElement(By.XPath("/ html / body / div[1] / div[2] / div / html / body / div[3] / div / input")).Click();
            }
        }


        [Test]
        [Order(3)]
        //Typ 1 - niemand erfüllt alle Kriterien
        [TestCase(new Object[] { "Agile Coach", "Consultant", "EntwicklerIn", "ProjektmanagerIn", "UI/UX ExpertIn" })]

        //Typ 2 - niemand erfüllt mindestens 1 Kriterium
        [TestCase(new Object[] { "Testskill" })]
        public void IncompleteEmployeeSearch(Object[] role)
        {
            foreach (IWebDriver driver in drivers)
            {
                //Hinzufügen eines neuen Skills den kein Mitarbeiter besitzt
                if (role.Length == 1)
                {
                    driver.FindElement(By.LinkText("Datenbasis ändern")).Click();
                    driver.FindElement(By.XPath("/html/body/div[1]/div[2]/div/div[3]/div[4]/div/div[2]/div/div/label")).Click();
                    driver.FindElement(By.XPath("/html/body/div[1]/div[2]/div/div[3]/div[4]/div/div[2]/div/div/div/div/div/div[1]/input")).SendKeys("Testskill");
                    driver.FindElement(By.XPath("/html/body/div[1]/div[2]/div/div[3]/div[4]/div/div[2]/div/div/div/div/div/div[3]/select")).Click();
                    driver.FindElement(By.XPath("/html/body/div[1]/div[2]/div/div[3]/div[4]/div/div[2]/div/div/div/div/div/div[3]/select")).SendKeys(Keys.ArrowUp);
                    driver.FindElement(By.XPath("/ html / body / div[1] / div[2] / div / div[3] / div[4] / div / div[2] / div / div / div / div / div / div[4] / button")).Click();
                }


                driver.FindElement(By.LinkText("Mitarbeitersuche")).Click();
                Assert.AreEqual("http://localhost:5005/employee-search", driver.Url, "Could not reach EmployeeSearch page");

                driver.Navigate().Refresh();

                //Rollen (= Suchkriterien) eingeben 
                for (int i = 0; i < role.Length; i++)
                {
                    driver.FindElement(By.XPath("//input[@placeholder='Rollen']")).SendKeys((string)role[i]);
                    driver.FindElement(By.XPath("/html/body/div[1]/div[2]/div/html/body/div[2]/div/div/div[2]/div[2]/div")).Click();
                }

                //Suchen Button
                driver.FindElement(By.XPath("/html/body/div[1]/div[2]/div/html/body/div[2]/div/input[1]")).Click();

                //Typ 2
                if (role.Length == 1)
                {
                    //Sicherstellen, dass richtige Fehlermeldung angezeigt wird
                    IWebElement infoBox = driver.FindElement(By.XPath("/html/body/div[1]/div[2]/div/html/body/div[3]/div[1]"));
                    string infoText = infoBox.Text;
                    Assert.AreEqual("Kein/Keine MitarbeiterIn erfüllt mindestens ein gesuchtes Kriterium.", infoText);
                    //Löschen des neuen Skills
                    driver.FindElement(By.LinkText("Datenbasis ändern")).Click();
                    driver.FindElement(By.XPath("/html/body/div[1]/div[2]/div/div[3]/div[4]/div/div[2]/input[1]")).SendKeys("Testskill");
                    driver.FindElement(By.XPath("/html/body/div[1]/div[2]/div/div[3]/div[4]/div/div[2]/span[1]/button[2]")).Click();


                }
                //Typ 1
                else
                {
                    //Sicherstellen, dass richtige Fehlermeldung angezeigt wird
                    IWebElement infoBox = driver.FindElement(By.XPath("/html/body/div[1]/div[2]/div/html/body/div[3]/div[1]"));
                    string infoText = infoBox.Text;
                    Assert.AreEqual("Kein/Keine MitarbeiterIn erfüllt alle Kriterien.", infoText);

                    //'Arnold schwarzenegger' erfüllt trotzdem die meisten Kriterien (also steht er ganz oben)
                    driver.FindElement(By.XPath("/html/body/div[1]/div[2]/div/html/body/div[3]/table/tbody/tr/td[1]/a"));
                    IWebElement tableCell = driver.FindElement(By.XPath("/html/body/div[1]/div[2]/div/html/body/div[3]/table/tbody/tr[1]/td[1]/a"));
                    string searchResult = tableCell.Text;
                    Assert.AreEqual("Arnold schwarzenegger", searchResult);
                }
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
