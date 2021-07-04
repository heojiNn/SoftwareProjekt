using System;
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

        [OneTimeSetUp]
        public void GetSut()
        {
            sut = GetProjectService();
        }



        [Test]
        public void FieldUpdateTest()
        {
            var project = sut.ShowAllProjects().First();
            var firstId = project.Id;

            Assert.False(project.Field == "Automobil");
            project.Field = "Automobil";
            sut.Update(project);

            var newProject = sut.ShowProject(firstId);
            Assert.NotNull(newProject);
            Assert.True(newProject.Field == "Automobil");
        }


        [Test]
        public void FieldUpdateFail()
        {
            var project = sut.ShowAllProjects().First();
            var firstId = project.Id;

            project.Field = "Autompil";
            sut.Update(project);

            var newProject = sut.ShowProject(firstId);
            Assert.False(newProject.Field == "Automobil");
        }




        [Test]
        [Order(1)]
        public void CreateTest()
        {
            // PreCondition
            var noVal = sut.ShowAllProjects().FirstOrDefault(x => x.Title == "TestTitle");
            Assert.Null(noVal);

            //Assert
            sut.Create("TestTitle");
            var requestAgain = sut.ShowAllProjects().FirstOrDefault(x => x.Title == "TestTitle");
            Assert.NotNull(requestAgain);
        }

        [Test]
        [Order(1)]
        public void Add()
        {
            var toChange = sut.ShowAllProjects().First(x => x.Title == "TestTitle");
            var e = GetEmployeeService().ShowAllProfiles().First();

            sut.Add(toChange, "secActi");
            sut.Add(toChange, e);
            sut.Add(toChange, e, "secActi");

            //Assert
            var requestAgain = sut.ShowAllProjects().First(x => x.Title == "TestTitle");
            Assert.Contains(e.PersoNumber, requestAgain.Activities["secActi"].persNr);
            e = GetEmployeeService().ShowAllProfiles().First();
            Console.WriteLine(e);

        }

    }

}

