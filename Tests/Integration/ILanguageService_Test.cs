using System;
using System.Linq;
using NUnit.Framework;
using XCV.Data;


namespace Tests.Integration
{
    //----------------------Integration Tests for    ILanguageService
    [TestFixture]
    public class ILanguageService_Test : Initializer
    {
        private ILanguageService sut;
        private ChangeResult changeRes;
        private void OnEventReturn(object sender, ChangeResult e) => changeRes = e;
        [SetUp]
        public void ResetResult() => changeRes = new();

        [OneTimeSetUp]
        public void GetSut()
        {
            sut = GetLangService();
            sut.ChangeEventHandel += OnEventReturn;

            // Assert
            var content = sut.GetAllLanguages();
            Assert.True(content.Count() > 3, "Not enough Languages for testing");
        }


        //--------------------------UpdateAllLanguages(languages)--------------------------
        //
        // Vorbeding.: .Name.Length <50 && <1 && unique
        // Nachbedin.: alle Sprachen die nicht mehr in EingabeParameter enthalten sind,  wurden mit ihren refs. gelöscht
        //             alle neuen Sprachen wurden in der Persistenz gespeichert
        [Test]
        public void UpdateAll()
        {
            var l1 = new Language() { Name = "Quenya" };
            var l2 = new Language() { Name = "Klingon" };
            var newLanguages = (new[] { l1, l2 }).ToHashSet();  //input to exchange

            var oldCount = sut.GetAllLanguages().Count();       //old Count


            // Act
            (var addedRows, var remoRows) = sut.UpdateAllLanguages(newLanguages);

            // Assert
            var requestAgain = sut.GetAllLanguages();

            Assert.AreEqual(2, addedRows);
            Assert.AreEqual(oldCount, remoRows, "not all old Languages were removed");
            Assert.True(newLanguages.SetEquals(requestAgain), "Languages weren't stored correctly");

            Assert.False(changeRes.ErrorMessages.Any(), "User got a ErrorMessages");
            Assert.AreNotEqual("", changeRes.SuccesMessage, "User didn't got a SuccesMessage");
        }
        //-----------------------------------------------------------------------------------------
        //     bei invaliden EingabeParametern : nichts in der Persistenz ändert sich
        [TestCase("")]
        [TestCase("123456789-123456789-123456789-123456789-123456789-Lang")]
        public void Invalid_UpdateAll(string name)
        {
            var languages = new[] { new Language() { Name = name } };   //input to exchange

            var oldLangs = sut.GetAllLanguages().ToHashSet();           //old Value

            // Act
            (var addedRows, var remoRows) = sut.UpdateAllLanguages(languages);

            // Assert
            var requestAgain = sut.GetAllLanguages();
            Assert.AreEqual(0, addedRows);
            Assert.AreEqual(0, remoRows);
            Assert.True(oldLangs.SetEquals(requestAgain), "old Languages did change");
            Assert.AreEqual(1, changeRes.ErrorMessages.Count(), "User didn't got an ErrorMessages");
        }

        //--------------------------UpdateAllLevels(levels)--------------------------
        [Test]
        public void UpdateAllLevels()
        {
            var lvl = sut.GetAllLevel().Reverse().ToArray();

            // Act
            var rows = sut.UpdateAllLevels(lvl);

            // Assert
            var requestAgain = sut.GetAllLevel();

            Assert.AreEqual(6, rows);
            Assert.AreEqual(lvl, requestAgain, "Values not updated correctly");

            // Act
            lvl[4] = "supergut";
            sut.UpdateAllLevels(lvl);

            // Assert
            requestAgain = sut.GetAllLevel();
            Assert.AreEqual("supergut", requestAgain[4]);
        }

        [Test]
        public void Invalid_UpdateAllLevels()
        {
            var old = sut.GetAllLevel();
            var lvl = sut.GetAllLevel();
            lvl[2] = "super";
            lvl[4] = "Super ";

            // Act
            var rows = sut.UpdateAllLevels(lvl);

            // Assert
            var requestAgain = sut.GetAllLevel();
            Assert.AreEqual(0, rows);
            Assert.AreEqual(old, requestAgain);
        }

    }
}
