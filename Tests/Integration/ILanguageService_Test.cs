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
        }


        //--------------------------UpdateAllLanguages(languages)--------------------------
        //
        // Vorbeding.: keine einziger (.Name.Length>50)
        // Nachbedin.: alle Sprachen die nicht mehr in languages enthalten sind,  wurden mit ihren refs. gelöscht
        //               alle neuen Sprachen wurden in der Persistenz gespeichert
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
            Assert.AreEqual(oldCount, remoRows, "not all old Languages weren't removed");
            Assert.True(newLanguages.SetEquals(requestAgain), "Languages weren't stored correctly");
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
            var lvl = sut.GetAllLevel().Append("extra").ToArray();

            // Act
            (var addedRows, var remoRows) = sut.UpdateAllLevels(lvl);

            // Assert
            var requestAgain = sut.GetAllLevel();
            Assert.AreEqual(1, addedRows);
            Assert.AreEqual(0, remoRows);
            Assert.AreEqual(lvl, requestAgain);
        }
        [Test]
        public void ChangeLevelOrder()
        {
            var lvl = sut.GetAllLevel().Reverse().ToArray();

            // Act
            (var addedRows, var remoRows) = sut.UpdateAllLevels(lvl);

            // Assert
            var requestAgain = sut.GetAllLevel();
            Assert.AreEqual(0, addedRows);
            Assert.AreEqual(0, remoRows);
            Assert.AreEqual(lvl, requestAgain);
        }

    }
}
