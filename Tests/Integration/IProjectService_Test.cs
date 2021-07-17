using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using XCV.Data;


namespace Tests.Integration
{
    //---------------------- Integration Tests for the   IProjectService
    [TestFixture]
    public class IProjectService_Test : Initializer
    {
        private IProjectService sut;
        private ChangeResult changeRes;
        private void OnEventReturn(object sender, ChangeResult e) => changeRes = e;


        [OneTimeSetUp]
        public void GetSut()
        {
            sut = GetProjectService();
            sut.ChangeEventHandel += OnEventReturn;

        }

        //Vorbedingung:

        //Es liegt kein Projekt mit diesem Namen vor
        //es wurde ein Titel mit passendem Format(bestehend aus Buchstaben und Zahlen und den erlaubten Zusatzzeichen: '-', '/', '&', '_', '#', '+') eingegeben
        //Die Beschreibung ist höchstens 400 Zeichen lang
        //Das Startdatum liegt vor dem Enddatum, beide liegen zwischen dem 01.01.2011 und dem 01.01.2100
        //die Projektzwecke bestehen aus mindestens zwei und höchstens 200 Zeichen
        //die Projekttätigkeiten bestehen aus mindestens zwei und höchstens 50 Zeichen

        //Nachbedingung:

        //Alle eingegeben Daten wurden übernommen und mit den zugehörigen Feldern (Project.Title-> Projekt.Titel) im (aktualisierten) Projekt assoziiert
        [Test]
        [Order(1)]
        public void CreateTest()
        {
            // PreCondition
            var noVal = sut.ShowAllProjects().FirstOrDefault(x => x.Title == "TestTitle");
            Assert.Null(noVal);

            //Assert
            Project newP = new Project()
            {
                Title = "TestTitle",
                Description = "TestDescription",
                Start = new DateTime(2020, 01, 01),
                End = new DateTime(2020, 04, 04),
                Field = "IT",
                Purpose = { "purp", "purp2" },
                Activities = new()
                {
                    { "act", (new List<string>() { GetEmployeeService().ShowAllProfiles().First().PersoNumber }, new List<Skill>() { GetSkillService().GetAllSkills().First() }) }
                }
            };
            sut.Create(newP);
            var requestAgain = sut.ShowAllProjects().FirstOrDefault(x => x.Title == "TestTitle");
            Assert.NotNull(requestAgain);
            Assert.AreEqual(newP.Title, requestAgain.Title, "Title wasn't created");
            Assert.AreEqual(newP.Description, requestAgain.Description, "Description wasn't created");
            Assert.AreEqual(newP.Start, requestAgain.Start, "Start wasn't created");
            Assert.AreEqual(newP.End, requestAgain.End, "End wasn't created");
            Assert.AreEqual(newP.Field, requestAgain.Field, "Field wasn't created");
            Assert.True(newP.Purpose.SequenceEqual(requestAgain.Purpose), "Purposes were not created");
            Assert.That(newP.Activities, Is .EqualTo(requestAgain.Activities), "Activities were not created");
            Assert.False(changeRes.ErrorMessages.Any());
            Assert.AreNotEqual("", changeRes.SuccesMessage, "User has not received a success message");
        }

        [Test]
        [Order(3)]
        public void InvalidCreateTest()
        {
            changeRes = new();
            sut.Create(new Project() { Title = "TestTitle" });
            Assert.True(changeRes.ErrorMessages.Contains("Es existiert schon ein Projekt mit diesem Titel."));
            sut.Create(new Project() { Title = "" });
            Assert.True(changeRes.ErrorMessages.Contains("Der Titel darf nicht leer sein."));

            sut.Create(new Project() { Title = "valid", Start = new DateTime(2009, 01, 01) });
            Assert.True(changeRes.ErrorMessages.Contains("Der Projektanfang kann nicht vor dem Jahr 2011 liegen."));
            sut.Create(new Project() { Title = "valid", End = new DateTime(2101, 01, 01) });
            Assert.True(changeRes.ErrorMessages.Contains("Das Projekt muss vor dem Jahr 2100 beendet werden."));
            sut.Create(new Project() { Title = "valid", Start = new DateTime(2022, 03, 01),End = new DateTime(2022, 01, 01) });
            Assert.True(changeRes.ErrorMessages.Contains("Das Enddatum muss hinter dem Beginn liegen."));

            sut.Create(new Project() { Title = "valid", Purpose = { "a" } });
            Assert.True(changeRes.ErrorMessages.Contains("Die Beschreibung des Zwecks:(a) ist zu kurz."));

            sut.Create(new Project() { Title = "valid", Activities = { {"a", new() } } });
            Assert.True(changeRes.ErrorMessages.Contains("Die Beschreibung der Tätigkeit:(a) ist zu kurz."));
        }

        // Vorbedingung:
        // Nutzer hat Rechte zum bearbeiten eines Projekts
        //Die Daten wurden an das Modell mit dessen Validierung Notizen gebunden
        //es existiert ein Projekt mit angegebenem Namen
        //Eingabe entsprechend passendem Format(bestehend aus Buchstaben und Zahlen und den erlaubten Zusatzzeichen: '-', '/', '&', '_', '#', '+')

        //Nachbedingung:

        //Alle eingegeben Daten wurden übernommen und mit den zugehörigen Feldern (Project.Title-> Projekt.Titel) im (aktualisierten) Projekt assoziiert
        [Test]
        [Order(4)]
        public void UpdateTest()
        {
            changeRes = new();
            var project = sut.ShowAllProjects().First();
            project.Title = "UpdateTitle";
            project.Description = "UpdateDescription";
            project.Start = new DateTime(2021, 01, 01);
            project.End = new DateTime(2021, 05, 01);
            project.Field = "Chemie";
            project.Purpose.Add("updated Purpose");
            project.Activities.Add("new Key", (new List<string>() { GetEmployeeService().ShowAllProfiles().First().PersoNumber }, new List<Skill>(){ GetSkillService().GetAllSkills().First()} ));

            sut.ValidateUpdate(project);
            Assert.False(changeRes.ErrorMessages.Any());
            Assert.True(changeRes.InfoMessages.Any());
            sut.Update(project);
            var requestAgain = sut.ShowProject(project.Id);
            Assert.NotNull(requestAgain);
            Assert.AreEqual(project.Title, requestAgain.Title, "Title wasn't created");
            Assert.AreEqual(project.Description, requestAgain.Description, "Description wasn't created");
            Assert.AreEqual(project.Start, requestAgain.Start, "Start wasn't created");
            Assert.AreEqual(project.End, requestAgain.End, "End wasn't created");
            Assert.AreEqual(project.Field, requestAgain.Field, "Field wasn't created");
            Assert.True(project.Purpose.SequenceEqual(requestAgain.Purpose), "Purposes were not created");
            Assert.That(project.Activities, Is.EqualTo(requestAgain.Activities), "Activities were not created");
            Assert.AreNotEqual("", changeRes.SuccesMessage, "User has not received a success message");
        }

        [Test]
        [Order(4)]
        public void InvalidUpdateTest()
        {
            changeRes = new();
            sut.Create(new Project { Title = "only for testing the title" });

            var project = sut.ShowAllProjects().First();
            project.Title = "only for testing the title";
            project.Start = new DateTime(2010, 01, 01);
            project.End = new DateTime(2009, 01, 01);
            project.Purpose.Add("a");
            project.Activities.Add("a", (new List<string>(), new List<Skill>()));
            project.Activities.Add("aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa", (new List<string>(), new List<Skill>()));

            sut.ValidateUpdate(project);
            Assert.True(changeRes.ErrorMessages.Contains("Es existiert schon ein Projekt mit diesem Titel."));
            Assert.True(changeRes.ErrorMessages.Contains("Der Projektanfang kann nicht vor dem Jahr 2011 liegen."));
            Assert.True(changeRes.ErrorMessages.Contains("Das Enddatum muss hinter den Beginn liegen."));
            Assert.True(changeRes.ErrorMessages.Contains("Die Beschreibung des Zwecks:(a) ist zu kurz."));
            Assert.True(changeRes.ErrorMessages.Contains("Die Beschreibung der Tätigkeit:(a) ist zu kurz"));
            Assert.True(changeRes.ErrorMessages.Contains("Die Beschreibung der Tätigkeit:(aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa) ist zu lang"));

            changeRes = new();
            sut.Update(project); 
            Assert.AreEqual("", changeRes.SuccesMessage, "User has not received a success message");
        }


        [Test]
        [Order(2)]
        public void Add()
        {
            var toChange = sut.ShowAllProjects().First(x => x.Title == "TestTitle");
            var e = GetEmployeeService().ShowAllProfiles().First();

            toChange.Activities.Add("secActi", (new List<string>(), new List<Skill>()));

            sut.Update(toChange);
            sut.Add(toChange.Id, e.PersoNumber, "secActi");

            //Assert
            var requestAgain = sut.ShowAllProjects().First(x => x.Title == "TestTitle");
            Assert.Contains(e.PersoNumber, requestAgain.Activities["secActi"].persNr);
            e = GetEmployeeService().ShowAllProfiles().First();
            Console.WriteLine(e);

        }

        // Vorbedingung:
        //Nutzer hat Vertriebs-/Adminrechte
        //es existiert ein Projekt mit angegebenem Namen

        //Nachbedingung:
        //Projekt liegt nicht mehr vor

        [Test]
        [Order(5)]
        public void DeleteTest()
        {
            var project = sut.ShowAllProjects().First();
            var id = project.Id;
            sut.Delete(project);
            var requestAgain = sut.ShowProject(id);
            Assert.Null(requestAgain, "failed to delete");
        }

    }

}

