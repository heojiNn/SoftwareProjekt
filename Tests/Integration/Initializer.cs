using System;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using XCV.Data;



namespace Tests.Integration
{
    /// <summary>
    ///         Parrent of all Integration Tests
    /// </summary>
    /// <remarks>
    ///         everthing will be reseted before the Test,  so the IntegrationTests database can be evaluated afterwards if needed
    /// </remarks>
    [TestFixture]
    public class Initializer
    {
        /// <summary>
        ///         will run  before(at the beginnig) of each testClass, that inherits Initializer
        /// </summary>
        [OneTimeSetUp]
        public void Before()
        {
            var dbUtils = GetBasicDBUtils();    // can be put in commets
            dbUtils.CreateDatabase();           // after setup (local or pipeline)
            dbUtils.CreateTables();             // to improve test speed

            DoJsonUpdate();
            Assert.AreEqual(360, GetSkillService().GetAllSkills().Count()); // every Test has the fresh Set of 360 Skills+Fields....

            var eService = GetEmployeeService();
            foreach (var e in eService.ShowAllProfiles())
                eService.DeleteAccount(e.PersoNumber);
            eService.CreateAccount(new Employee() { PersoNumber = "000", FirstName = "admin", LastName = "admin", EmployedSince = DateTime.Now });

            Assert.AreEqual(1, eService.ShowAllProfiles().Count());       // and one fresh account
            var pService = GetProjectService();
            foreach (var p in pService.ShowAllProjects())
                pService.Delete(p);
        }

        /// <summary>
        ///         will run a fter(at the end) of each testClass
        /// </summary>
        [OneTimeTearDown]
        public void After()
        {
            //
        }




        public static IConfiguration GetTestConfig()
        {
            var currentParent = Directory.GetParent(Directory.GetCurrentDirectory()).FullName;
            currentParent = Directory.GetParent(currentParent).FullName;
            currentParent = Directory.GetParent(currentParent).FullName;
            var settingsPath = Path.Combine(currentParent, "testsettings.json");

            return new ConfigurationBuilder().AddJsonFile(settingsPath).Build();
        }
        public static IWebHostEnvironment GetEnvMoq()
        {
            var mockEnvironment = new Mock<IWebHostEnvironment>();
            mockEnvironment
                .Setup(m => m.WebRootPath)
                .Returns(Directory.GetCurrentDirectory());
            return mockEnvironment.Object;
        }


        public static DatabaseUtils GetBasicDBUtils()
        {
            var dLogger = Mock.Of<ILogger<DatabaseUtils>>();
            var sut = new DatabaseUtils(GetTestConfig(), dLogger);
            return sut;
        }
        public static EmployeeService GetEmployeeService()
        {
            var eLogger = Mock.Of<ILogger<EmployeeService>>();
            var sut = new EmployeeService(GetTestConfig(), eLogger, GetSkillService(), GetLangService(), GetEnvMoq());
            return sut;
        }
        public static ProjectService GetProjectService()
        {
            var pLogger = Mock.Of<ILogger<ProjectService>>();
            var sut = new ProjectService(GetTestConfig(), pLogger);
            return sut;
        }
        public static BasicDataSetService GetBDSService()
        {
            var sut = new BasicDataSetService(GetFieldService(), GetRoleService(), GetLangService(), GetSkillService());
            return sut;
        }

        public static FieldService GetFieldService()
        {
            var fLogger = Mock.Of<ILogger<FieldService>>();
            var sut = new FieldService(GetTestConfig(), fLogger);
            return sut;
        }
        public static RoleService GetRoleService()
        {
            var rLogger = Mock.Of<ILogger<RoleService>>();
            var sut = new RoleService(GetTestConfig(), rLogger);
            return sut;
        }
        public static LanguageService GetLangService()
        {
            var lLogger = Mock.Of<ILogger<LanguageService>>();
            var sut = new LanguageService(GetTestConfig(), lLogger);
            return sut;
        }
        public static SkillService GetSkillService()
        {
            var sLogger = Mock.Of<ILogger<SkillService>>();
            var sut = new SkillService(GetTestConfig(), sLogger);
            return sut;
        }
        public static OfferService GetOfferService()
        {
            var sLogger = Mock.Of<ILogger<OfferService>>();
            var sut = new OfferService(GetTestConfig(), sLogger, GetSkillService(), GetEmployeeService());
            return sut;
        }

        public static void DoJsonUpdate()
        {
            var bdsService = GetBDSService();
            var content = File.ReadAllText("datenbasis.json");
            bdsService.JsonUpdate(content, false);
        }

    }
}
