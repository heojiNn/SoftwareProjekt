using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using XCV.Data;

namespace Tests.Integration
{

    //----------------------Integration Tests for    IOfferService
    [TestFixture]
    class IOfferService_Test : Initializer
    {

        private OfferService sut;
        private ChangeResult changeRes;
        private void OnEventReturn(object sender, ChangeResult e) => changeRes = e;

        [OneTimeSetUp]
        public void GetSut()
        {
            sut = GetOfferService();
            sut.ChangeEventHandel += OnEventReturn;

        }


        [Test]
        [Order(1)]
        public void CreateTest()
        {
            if(sut.ShowAllOffers().Where(x => x.Title == "TestTitle").Any())
            {
                foreach (var o in sut.ShowAllOffers().Where(x => x.Title == "TestTitle"))
                    sut.Delete(o);
            }
            string title = "TestTitle";
            string description = "Test";
            DateTime start = new DateTime(2020, 01, 01);
            DateTime end = new DateTime(2020, 04, 04);
           
            sut.Create(title, description, start, end);
            var requestAgain = sut.ShowAllOffers().FirstOrDefault(x => x.Title == "TestTitle");
            Assert.NotNull(requestAgain);
            Assert.AreEqual(title, requestAgain.Title, "Title wasn't created");
            Assert.AreEqual(description, requestAgain.Description, "Description wasn't created");
            Assert.AreEqual(start, requestAgain.Start, "Start wasn't created");
            Assert.AreEqual(end, requestAgain.End, "End wasn't created");
            Assert.False(changeRes.ErrorMessages.Any());
            Assert.AreNotEqual("", changeRes.SuccesMessage, "User has not received a success message");
        }

        [Test]
        [Order(2)]
        public void ValidateCreateTest()
        {
            changeRes = new();
            Offer offer = new Offer() { Title = "ValidateTitle", Description = "Test Description", Start = new DateTime(2020, 01, 01), End = new DateTime(2021, 01, 01),
                Requirements = new HashSet<Skill>(),
                Fields = new HashSet<Field>(),
                participants = new List<Employee>()
            };

            sut.ValidateCreate(offer);
            Assert.False(changeRes.ErrorMessages.Any());
            Assert.True(changeRes.InfoMessages.Contains("Das Angebot kann erfolgreich erstellt werden."));

        }
        [Test]
        [Order(3)]
        public void InvalidCreateTest()
        {
            changeRes = new();
            sut.ValidateCreate(new Offer { Title = "invalid Title!" });
            Assert.True(changeRes.ErrorMessages.Contains("darf nur Buchstaben Zahlen oder -_,. enthalten"));

            sut.ValidateCreate(new Offer { Title = "Valid", Description = new string('a', 141) });
            Assert.True(changeRes.ErrorMessages.Contains("Beschreibung nicht über 140 Zeichen"));

            sut.ValidateCreate(new Offer { Title = "Valid", End = new DateTime(1999, 01, 03) });
            Assert.True(changeRes.ErrorMessages.Contains(1999 + ": Das Enddatum sollte in der Zukunft liegen."));
            sut.ValidateCreate(new Offer { Title = "valid", End = new DateTime(3020, 01, 03) });
            Assert.True(changeRes.ErrorMessages.Contains(3020 +": Das Enddatum sollte noch in diesem Jahrtausend liegen."));

            sut.ValidateCreate(new Offer { Title = "valid", Start = new DateTime(3020, 01, 01) });
            Assert.True(changeRes.ErrorMessages.Contains(3020 + ": Das Startdatum sollte noch in diesem Jahrtausend liegen."));
            sut.ValidateCreate(new Offer { Title = "valid",Start =  new DateTime(1920, 01, 03)});
            Assert.True(changeRes.ErrorMessages.Contains(1920 + ": Das Startdatum sollte nicht zu weit in der Vergangenheit liegen."));

        }

        [Test]
        [Order(4)]
        public void UpdateTest()
        {
            changeRes = new();
            var offer = sut.ShowAllOffers().Where(x => x.Title == "TestTitle").First();
            offer.Title = "UpdateTitle";
            offer.Description = "UpdateDescription";
            offer.Start = new DateTime(2021, 01, 01);
            offer.End = new DateTime(2021, 05, 01);
            offer.Fields = new HashSet<Field> { GetFieldService().GetAllFields().First() };

            sut.ValidateUpdate(offer);
            foreach (var s in changeRes.ErrorMessages) Console.WriteLine(s);
            Assert.False(changeRes.ErrorMessages.Any());
            Assert.True(changeRes.InfoMessages.Any());
            sut.Update(offer);
            var requestAgain = sut.ShowOffer(offer.Id);
            Assert.NotNull(requestAgain);
            Assert.AreEqual(offer.Title, requestAgain.Title, "Title wasn't created");
            Assert.AreEqual(offer.Description, requestAgain.Description, "Description wasn't created");
            Assert.AreEqual(offer.Start, requestAgain.Start, "Start wasn't created");
            Assert.AreEqual(offer.End, requestAgain.End, "End wasn't created");
            Assert.True(offer.Fields.SequenceEqual(requestAgain.Fields), "Fields were not created");
        }

        [Test]
        [Order(5)]
        public void InvalidUpdateTest()
        {
            changeRes = new();
            var offer = sut.ShowAllOffers().First();

            offer.Title = "UpdateTitle Incorrect!!!!";
            sut.ValidateUpdate(offer);
            Assert.True(changeRes.ErrorMessages.Any());
            changeRes = new();

            offer.Description = new string('a', 141);
            sut.ValidateUpdate(offer);
            Assert.True(changeRes.ErrorMessages.Any());
            changeRes = new();

            offer.Start = new DateTime(1921, 01, 01);
            sut.ValidateUpdate(offer);
            Assert.True(changeRes.ErrorMessages.Any());
            changeRes = new();
            offer.Start = new DateTime(3001, 01, 01);
            sut.ValidateUpdate(offer);
            Assert.True(changeRes.ErrorMessages.Any());
            changeRes = new();

            offer.End = new DateTime(1921, 01, 01);
            sut.ValidateUpdate(offer);
            Assert.True(changeRes.ErrorMessages.Any());
            changeRes = new();
            offer.End = new DateTime(3001, 01, 01);
            sut.ValidateUpdate(offer);
            Assert.True(changeRes.ErrorMessages.Any());
            changeRes = new();

            offer.Requirements = new HashSet<Skill> { GetSkillService().GetAllSkills().First() };
            sut.ValidateUpdate(offer);
            Assert.True(changeRes.ErrorMessages.Any());
            changeRes = new();

            offer.Fields = new HashSet<Field> { GetFieldService().GetAllFields().First(), GetFieldService().GetAllFields().First() };
            sut.ValidateUpdate(offer);
            Assert.True(changeRes.ErrorMessages.Any());
            changeRes = new();

        }

        [Test]
        [Order(6)]
        public void DeleteTest()
        {
            var offer = sut.ShowAllOffers().First();
            var id = offer.Id;
            sut.Delete(offer);
            var requestAgain = sut.ShowOffer(id);
            Assert.Null(requestAgain, "failed to delete");
        }



    }
}

