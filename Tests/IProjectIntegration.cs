using System.Linq;
using System.Threading;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using XCV.Data;


namespace Tests
{
    //---------------------- Integration Tests for the   IProjectService
    [TestFixture]
    public class IProjectIntegration : Initializer
    {
        private ProjectService sut;


        [OneTimeSetUp]
        public void GetSut()
        {
            var pLogger = Mock.Of<ILogger<ProjectService>>();
            var fLogger = Mock.Of<ILogger<FieldService>>();
            var fieldService = new FieldService(GetConfigMoq(), fLogger);

            sut = new ProjectService(GetConfigMoq(), pLogger, fieldService);
        }





        [Test]
        public void FieldUpdateTest()
        {
            var project = sut.ShowAllProjects().First();
            var firstId = project.Id;

            Assert.False(project.Field == "Automobil");
            project.Field = "Automobil";
            sut.Update(project);

            Thread.Sleep(200);
            var newProject = sut.ShowProject(firstId);
            Assert.True(newProject.Field == "Automobil");
        }
        [Test]
        public void FieldUpdateFail()
        {
            var project = sut.ShowAllProjects().First();
            var firstId = project.Id;

            project.Field = "Autompil";
            sut.Update(project);

            Thread.Sleep(200);
            var newProject = sut.ShowProject(firstId);
            Assert.False(newProject.Field == "Automobil");
        }




        [Test]
        public void CreateTest()
        {
            sut.Create("TestTitle");

            Assert.True(sut.ShowAllProjects().FirstOrDefault(x => x.Title == "TestTitle") != null);
        }

    }

}

