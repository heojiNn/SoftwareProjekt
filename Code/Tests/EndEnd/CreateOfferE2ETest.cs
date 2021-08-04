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
    public class CreateOfferE2ETest
    {
        List<IWebDriver> drivers = new List<IWebDriver>() { /*new FirefoxDriver(),*/ new ChromeDriver() };
        string url = "http://localhost:5005";

        [OneTimeSetUp]
        public void Setup()
        {
            //Login
            foreach (IWebDriver driver in drivers)
            {
                driver.Navigate().GoToUrl(url + "/login");
                driver.Manage().Window.Maximize();
                driver.FindElement(By.XPath("/html/body/div[1]/div[2]/div/div/div[2]/div[1]/div/div[1]/input")).SendKeys("001");
                driver.FindElement(By.XPath("/html/body/div[1]/div[2]/div/div/div[2]/div[1]/div/div[2]/input")).SendKeys("001");
                driver.FindElement(By.XPath("/html/body/div[1]/div[2]/div/div/div[2]/div[1]/div/button[1]")).Click();
                driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(2);
            }
        }

        [Test]
        [Order(1)]
        [TestCase("Angebot-Test", "Microsoft SQL Einbindung", "15102021", "17102021", "IT", "MS SQL")]
        public void CreateOffer(string title, string description, string startDate, string endDate, string field, string hardskill)
        {
            foreach (IWebDriver driver in drivers)
            {
                //ZUr Angebotsübersicht Seite
                driver.FindElement(By.LinkText("Angebotsübersicht")).Click();
                Assert.AreEqual("http://localhost:5005/offers", driver.Url, "Could not reach offer overview page");

                //Neues Angebot erstellen
                driver.FindElement(By.LinkText("Angebot erstellen")).Click();
                Assert.AreEqual("http://localhost:5005/offer-create", driver.Url, "Could not reach offer-create page");

                //Titel angeben
                driver.FindElement(By.XPath("/html/body/div[1]/div[2]/div/div[2]/div/div/div/div[2]/h3/input")).SendKeys(title);
                //Beschreibung
                driver.FindElement(By.XPath("/html/body/div[1]/div[2]/div/div[3]/div[1]/div/textarea")).SendKeys(description);
                //Zeitraum
                driver.FindElement(By.XPath("/html/body/div[1]/div[2]/div/div[3]/div[2]/div/div/div[1]/input")).SendKeys(startDate);
                driver.FindElement(By.XPath("/html/body/div[1]/div[2]/div/div[3]/div[2]/div/div/div[2]/input")).SendKeys(endDate);
                //Branchen
                driver.FindElement(By.XPath("/html/body/div[1]/div[2]/div/div[4]/div[1]/h4")).Click();
                driver.FindElement(By.XPath("/html/body/div[1]/div[2]/div/div[4]/div[2]/input")).SendKeys(field);
                driver.FindElement(By.XPath("/html/body/div[1]/div[2]/div/div[4]/div[2]/div/div/div/label/input")).Click();
                //Hardskills
                driver.FindElement(By.XPath("/ html / body / div[1] / div[2] / div / div[5] / div[1] / div / div / h4")).Click();
                driver.FindElement(By.XPath("/ html / body / div[1] / div[2] / div / div[5] / div[1] / div / div[2] / input[@placeholder='suche']")).SendKeys(hardskill);
                driver.FindElement(By.XPath("/ html / body / div[1] / div[2] / div / div[5] / div[1] / div / div[2] / div / div[1] / div[1] / label / input")).Click();

                //Hardskill range
                IWebElement slider = driver.FindElement(By.XPath("/html/body/div[1]/div[2]/div/div[5]/div[1]/div/div[2]/div/div[1]/div[2]/input"));
                Actions actions = new Actions(driver);
                actions.ClickAndHold(slider);
                actions.SendKeys(Keys.ArrowRight).Build().Perform();

                driver.FindElement(By.XPath("/ html / body / div[1] / div[2] / div / div[5] / div[2] / div / div[1] / h4")).Click();
                driver.FindElement(By.XPath("/ html / body / div[1] / div[2] / div / div[5] / div[2] / div / div[2] / div / div / div[3] / label / input")).Click();

                //Mitarbeiter hinzufügen
                driver.FindElement(By.XPath("/html/body/div[1]/div[2]/div/div[6]/div[2]/a")).Click();
                Assert.AreEqual("http://localhost:5005/offer-search-add", driver.Url, "Could not reach employee-search page");

                //Name
                driver.FindElement(By.XPath("/html/body/div[1]/div[2]/div/html/body/div[2]/div/div/div[1]/div/input")).SendKeys("Daniel Craig");
                driver.FindElement(By.XPath("/html/body/div[1]/div[2]/div/html/body/div[2]/div/div/div[1]/div[2]/div")).Click();
                //Suche
                driver.FindElement(By.XPath("/ html / body / div[1] / div[2] / div / html / body / div[2] / div / input[1]")).Click();

                //Sicherstellen, dass Suchergebnisse angezeigt werden
                driver.FindElement(By.Id("myTable"));

                //Zum Angebot hinzufügen
                driver.FindElement(By.XPath("/html/body/div[1]/div[2]/div/html/body/div[3]/table/tbody/tr/td[2]/button")).Click();

                //Zurück zur Angebotserstellung
                driver.FindElement(By.XPath("/html/body/div[1]/div[2]/div/html/body/a")).Click();
                Assert.AreEqual("http://localhost:5005/offer-create", driver.Url, "Could not go back to offer-create page");

                //Sicherstellen, dass Mitarbeiter hinzugefügt wurde
                IWebElement employeeCard = driver.FindElement(By.XPath("/html/body/div[1]/div[2]/div/div[6]/div[2]/div/div/div[1]"));
                string employeeName = employeeCard.Text;
                Assert.IsTrue(employeeName.StartsWith("Daniel C."));

                //Angebot erstellen
                driver.FindElement(By.XPath("/html/body/div[1]/div[2]/div/button")).Click();

                //Sicherstellen, dass Infobox-Modal angezeigt wird
                IWebElement infoBox = driver.FindElement(By.XPath("/html/body/div[1]/div[2]/div/div[7]/div/div/div[2]/pre"));
                string infoText = infoBox.Text;
                Assert.AreEqual("Das Angebot kann erfolgreich erstellt werden.", infoText);

                //Erstellen im Modal bestätigen
                driver.FindElement(By.XPath("/html/body/div[1]/div[2]/div/div[7]/div/div/div[3]/a")).Click();
                Assert.AreEqual("http://localhost:5005/offers", driver.Url, "Could not reach offer overview page after save");

                //Sicherstellen, dass erstelltes Angebot in Angebotsliste angezeigt wird
                //driver.FindElement(By.XPath("/html/body/div[1]/div[2]/div/div[2]/div/div/div[1]/input")).SendKeys(title);
                //driver.FindElement(By.XPath("/ html / body / div[1] / div[2] / div / div[2] / div / div[2] / div")).Click();

                //driver.FindElement(By.XPath("/html/body/div[1]/div[2]/div/button")).Click();

                IWebElement offer = driver.FindElement(By.LinkText(title));

                string offerName = offer.Text;
                Assert.AreEqual(title, offerName);

                //Angebot löschen
                driver.FindElement(By.XPath("/html/body/div[1]/div[2]/div/div[6]/div/div[1]/h4/div/button")).Click();
                driver.FindElement(By.XPath("/html/body/div[1]/div[2]/div/div[6]/div/div[1]/h4/div/div/a[4]/a")).Click();

            }
        }

        [Test]
        [Order(2)]
        [TestCase("")]
        [TestCase("Angebot?")]
        [TestCase("AngebottitelAngebottitelAngebottitelAngebottitelAngebottitelAngebottitelAngebottitel")]

        public void InvalidTitleCreateOffer(string title)
        {
            foreach (IWebDriver driver in drivers)
            {
                driver.FindElement(By.LinkText("Angebotsübersicht")).Click();
                Assert.AreEqual("http://localhost:5005/offers", driver.Url, "Could not reach offer overview page");

                driver.FindElement(By.LinkText("Angebot erstellen")).Click();
                Assert.AreEqual("http://localhost:5005/offer-create", driver.Url, "Could not reach offer-create page");

                //Titel angeben
                driver.FindElement(By.XPath("/html/body/div[1]/div[2]/div/div[2]/div/div/div/div[2]/h3/input")).SendKeys(title);
                //Angebot erstellen
                driver.FindElement(By.XPath("/html/body/div[1]/div[2]/div/button")).Click();

                //Sicherstellen, dass richtige Fehlermeldung angezeigt wird
                if (title == "")
                {
                    IWebElement errorBox = driver.FindElement(By.XPath("/html/body/div[1]/div[2]/div/div[7]/div/div/div[2]/ul/li/pre"));
                    string error = errorBox.Text;
                    Assert.AreEqual(" Der Title sollte aus mindestens einem Zeichen bestehen.", error);
                }
                else if (title == "Angebot?" || title == "AngebottitelAngebottitelAngebottitelAngebottitelAngebottitelAngebottitelAngebottitel")
                {
                    IWebElement errorBox = driver.FindElement(By.XPath("/html/body/div[1]/div[2]/div/div[7]/div/div/div[2]/ul/li/pre"));
                    string error = errorBox.Text;
                    Assert.AreEqual("darf nur Buchstaben Zahlen oder -_,. enthalten", error);
                }
                //Gleiche Fehlermeldung bei zu langem Titel und Sonderzeichen im Titel
                //else if (title == "AngebottitelAngebottitelAngebottitelAngebottitelAngebottitelAngebottitelAngebottitel?")
                //{
                //    IWebElement errorBox = driver.FindElement(By.XPath("/html/body/div[1]/div[2]/div/div[7]/div/div/div[2]/ul/li/pre"));
                //    string error = errorBox.Text;
                //    Assert.AreEqual("Der Titel darf nicht länger als 40 Zeichen sein.", error);
                //}

                //Modal schließen
                driver.FindElement(By.XPath("/ html / body / div[1] / div[2] / div / div[7] / div / div / div[3] / button")).Click();

                //Zurück zur Angebotsübersicht
                driver.FindElement(By.XPath("/ html / body / div[1] / div[2] / div / a")).Click();
                Assert.AreEqual("http://localhost:5005/offers", driver.Url, "Could not reach offer-overview page");
            }
        }

        [Test]
        [Order(3)]
        [TestCase("Angebot-Apple", "Beschreibung")]
        public void InvalidDescriptionCreateOffer(string title, string description)
        {
            foreach (IWebDriver driver in drivers)
            {
                driver.FindElement(By.LinkText("Angebotsübersicht")).Click();
                Assert.AreEqual("http://localhost:5005/offers", driver.Url, "Could not reach offer overview page");

                driver.FindElement(By.LinkText("Angebot erstellen")).Click();
                Assert.AreEqual("http://localhost:5005/offer-create", driver.Url, "Could not reach offer-create page");

                //Titel angeben
                driver.FindElement(By.XPath("/html/body/div[1]/div[2]/div/div[2]/div/div/div/div[2]/h3/input")).SendKeys(title);

                //Beschreibung angeben (40-mal, damit zu lang)
                for (int i = 0; i < 40; i++)
                {
                    driver.FindElement(By.XPath("/html/body/div[1]/div[2]/div/div[3]/div[1]/div/textarea")).SendKeys(description);
                }

                //Angebot erstellen
                driver.FindElement(By.XPath("/html/body/div[1]/div[2]/div/button")).Click();

                //Sicherstellen, dass richtige Fehlermeldung angezeigt wird
                IWebElement errorBox = driver.FindElement(By.XPath("/html/body/div[1]/div[2]/div/div[7]/div/div/div[2]/ul/li/pre"));
                string error = errorBox.Text;
                Assert.AreEqual("Beschreibung nicht über 140 Zeichen", error);

                //Modal schließen
                driver.FindElement(By.XPath("/ html / body / div[1] / div[2] / div / div[7] / div / div / div[3] / button")).Click();
                //Zurück zur Angebotsübersicht
                driver.FindElement(By.XPath("/ html / body / div[1] / div[2] / div / a")).Click();
                Assert.AreEqual("http://localhost:5005/offers", driver.Url, "Could not reach offer-overview page");
            }
        }

        [Test]
        [Order(4)]
        [TestCase("Angebot-Test2", "100000", "25", "34", "500")]
        public void InvalidEmployeeCreateOffer(string title, string stundensatz, string stundenprotag, string laufzeit, string rabatt)
        {
            foreach (IWebDriver driver in drivers)
            {
                driver.FindElement(By.LinkText("Angebotsübersicht")).Click();
                Assert.AreEqual("http://localhost:5005/offers", driver.Url, "Could not reach offer overview page");

                driver.FindElement(By.LinkText("Angebot erstellen")).Click();
                Assert.AreEqual("http://localhost:5005/offer-create", driver.Url, "Could not reach offer-create page");

                //Titel angeben
                driver.FindElement(By.XPath("/html/body/div[1]/div[2]/div/div[2]/div/div/div/div[2]/h3/input")).SendKeys(title);
                //Mitarbeiter hinzufügen
                driver.FindElement(By.XPath("/html/body/div[1]/div[2]/div/div[6]/div[2]/a")).Click();
                //Name
                driver.FindElement(By.XPath("/html/body/div[1]/div[2]/div/html/body/div[2]/div/div/div[1]/div/input")).SendKeys("Daniel Craig");
                driver.FindElement(By.XPath("/html/body/div[1]/div[2]/div/html/body/div[2]/div/div/div[1]/div[2]/div")).Click();
                //Suche
                driver.FindElement(By.XPath("/ html / body / div[1] / div[2] / div / html / body / div[2] / div / input[1]")).Click();
                //Zum Angebot hinzufügen
                driver.FindElement(By.XPath("/html/body/div[1]/div[2]/div/html/body/div[3]/table/tbody/tr/td[2]/button")).Click();

                //Zurück zur Angebotserstellung
                driver.FindElement(By.XPath("/html/body/div[1]/div[2]/div/html/body/a")).Click();

                //Stundensatz
                driver.FindElement(By.XPath("/html/body/div[1]/div[2]/div/div[6]/div[2]/div/div/div[2]/div[1]/div[1]/input")).SendKeys(stundensatz);
                //Angebot erstellen
                driver.FindElement(By.XPath("/html/body/div[1]/div[2]/div/button")).Click();
                //Sicherstellen, dass richtige Fehlermeldung angezeigt wird
                IWebElement errorBox1 = driver.FindElement(By.XPath("/html/body/div[1]/div[2]/div/div[7]/div/div/div[2]/ul/li/pre"));
                string error1 = errorBox1.Text;
                Assert.AreEqual("Der Stundenlohn ist momentan auf 9999.99 begrenzt.", error1);
                //Modal schließen
                driver.FindElement(By.XPath("/ html / body / div[1] / div[2] / div / div[7] / div / div / div[3] / button")).Click();
                //Stundensatz ausbessern
                driver.FindElement(By.XPath("/html/body/div[1]/div[2]/div/div[6]/div[2]/div/div/div[2]/div[1]/div[1]/input")).Clear();
                driver.FindElement(By.XPath("/html/body/div[1]/div[2]/div/div[6]/div[2]/div/div/div[2]/div[1]/div[1]/input")).SendKeys("8");

                //Std/Tag
                driver.FindElement(By.XPath("/html/body/div[1]/div[2]/div/div[6]/div[2]/div/div/div[2]/div[1]/div[2]/input")).Clear();
                driver.FindElement(By.XPath("/html/body/div[1]/div[2]/div/div[6]/div[2]/div/div/div[2]/div[1]/div[2]/input")).SendKeys(stundenprotag);
                //Angebot erstellen
                driver.FindElement(By.XPath("/html/body/div[1]/div[2]/div/button")).Click();
                //Sicherstellen, dass richtige Fehlermeldung angezeigt wird
                IWebElement errorBox2 = driver.FindElement(By.XPath("/html/body/div[1]/div[2]/div/div[7]/div/div/div[2]/ul/li/pre"));
                string error2 = errorBox2.Text;
                Assert.AreEqual("Die maximale Anzahl an Arbeitsstunden pro Tag ist überschritten.", error2);
                //Modal schließen
                driver.FindElement(By.XPath("/ html / body / div[1] / div[2] / div / div[7] / div / div / div[3] / button")).Click();
                //Std/Tag ausbessern
                driver.FindElement(By.XPath("/html/body/div[1]/div[2]/div/div[6]/div[2]/div/div/div[2]/div[1]/div[2]/input")).Clear();
                driver.FindElement(By.XPath("/html/body/div[1]/div[2]/div/div[6]/div[2]/div/div/div[2]/div[1]/div[2]/input")).SendKeys("8");

                //Tage/Laufzeit
                driver.FindElement(By.XPath("/html/body/div[1]/div[2]/div/div[6]/div[2]/div/div/div[2]/div[2]/div[1]/input")).Clear();
                driver.FindElement(By.XPath("/html/body/div[1]/div[2]/div/div[6]/div[2]/div/div/div[2]/div[2]/div[1]/input")).SendKeys(laufzeit);
                //Angebot erstellen
                driver.FindElement(By.XPath("/html/body/div[1]/div[2]/div/button")).Click();
                //Sicherstellen, dass richtige Fehlermeldung angezeigt wird
                IWebElement errorBox3 = driver.FindElement(By.XPath("/html/body/div[1]/div[2]/div/div[7]/div/div/div[2]/ul/li/pre"));
                string error3 = errorBox3.Text;
                Assert.AreEqual("EinE MitarbeiterIn hat mehr Arbeitstage als die Projektgesamtlaufzeit besitzt.", error3);
                //Modal schließen
                driver.FindElement(By.XPath("/ html / body / div[1] / div[2] / div / div[7] / div / div / div[3] / button")).Click();
                //Tage/Laufzeit ausbessern
                driver.FindElement(By.XPath("/html/body/div[1]/div[2]/div/div[6]/div[2]/div/div/div[2]/div[2]/div[1]/input")).Clear();
                driver.FindElement(By.XPath("/html/body/div[1]/div[2]/div/div[6]/div[2]/div/div/div[2]/div[2]/div[1]/input")).SendKeys("0");

                //Rabatt
                driver.FindElement(By.XPath("/html/body/div[1]/div[2]/div/div[6]/div[2]/div/div/div[2]/div[2]/div[2]/input")).Clear();
                driver.FindElement(By.XPath("/html/body/div[1]/div[2]/div/div[6]/div[2]/div/div/div[2]/div[2]/div[2]/input")).SendKeys(rabatt);
                //Angebot erstellen
                driver.FindElement(By.XPath("/html/body/div[1]/div[2]/div/button")).Click();
                //Sicherstellen, dass richtige Fehlermeldung angezeigt wird
                IWebElement errorBox4 = driver.FindElement(By.XPath("/html/body/div[1]/div[2]/div/div[7]/div/div/div[2]/ul/li/pre"));
                string error4 = errorBox4.Text;
                Assert.AreEqual("Die Rabattangabe bitte als ganze Zahl zwischen 0 - 100 (%), ohne das Prozentsymbol", error4);
                //Modal schließen
                driver.FindElement(By.XPath("/ html / body / div[1] / div[2] / div / div[7] / div / div / div[3] / button")).Click();
                //Rabatt ausbessern
                driver.FindElement(By.XPath("/html/body/div[1]/div[2]/div/div[6]/div[2]/div/div/div[2]/div[2]/div[2]/input")).Clear();
                driver.FindElement(By.XPath("/html/body/div[1]/div[2]/div/div[6]/div[2]/div/div/div[2]/div[2]/div[2]/input")).SendKeys("5");

                //Angebot erstellen
                driver.FindElement(By.XPath("/html/body/div[1]/div[2]/div/button")).Click();

                //Sicherstellen, dass Infobox-Modal angezeigt wird
                IWebElement infoBox = driver.FindElement(By.XPath("/ html / body / div[1] / div[2] / div / div[7] / div / div / div[2] / pre"));
                string infoText = infoBox.Text;
                Assert.AreEqual("Das Angebot kann erfolgreich erstellt werden.", infoText);

                //Erstellen im Modal bestätigen
                driver.FindElement(By.XPath("/html/body/div[1]/div[2]/div/div[7]/div/div/div[3]/a")).Click();
                Assert.AreEqual("http://localhost:5005/offers", driver.Url, "Could not reach offer overview page after save");

                //Sicherstellen, dass erstelltes Angebot in Angebotsliste angezeigt wird
                //driver.FindElement(By.XPath("/html/body/div[1]/div[2]/div/div[2]/div/div/div[1]/input")).SendKeys(title);
                //driver.FindElement(By.XPath("/ html / body / div[1] / div[2] / div / div[2] / div / div[2] / div")).Click();
                //driver.FindElement(By.XPath("/html/body/div[1]/div[2]/div/button")).Click();
                IWebElement offer = driver.FindElement(By.LinkText(title));
                string offerName = offer.Text;
                Assert.AreEqual(title, offerName);

                //Angebot löschen
                driver.FindElement(By.XPath("/html/body/div[1]/div[2]/div/div[6]/div/div[1]/h4/div/button")).Click();
                driver.FindElement(By.XPath("/html/body/div[1]/div[2]/div/div[6]/div/div[1]/h4/div/div/a[4]/a")).Click();
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