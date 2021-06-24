
using System.Collections.Generic;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using XCV.Data;


namespace Tests
{
    public class Initializer
    {
        private readonly string testConnection = "Server=(localdb)\\MSSQLLocalDB; Integrated Security=true; Database=IntegrationTests;";
        private readonly string testConnectionWhithout = "Server=(localdb)\\MSSQLLocalDB; Integrated Security=true;";

        [OneTimeSetUp]
        public void Before()
        {
            var logger = Mock.Of<ILogger<DatabaseUtils>>();

            var dbUtils = new DatabaseUtils(GetConfigMoq(), logger);
            dbUtils.CreateDatabase();
            dbUtils.CreateTables();
            dbUtils.Populate();
        }


        [OneTimeTearDown]
        public void After()
        {
            var logger = Mock.Of<ILogger<DatabaseUtils>>();

            var sut = new DatabaseUtils(GetConfigMoq(), logger);
            sut.DropTables();
        }






        public IConfiguration GetConfigMoq()
        {
            using var con = new SqlConnection(testConnectionWhithout);

            var inMemoryConfiguration = new Dictionary<string, string> {
                { "ConnectionStrings:MyLocalConnection", testConnection },
                { "ConnectionStrings:MyLocalWhithoutDB", testConnectionWhithout },
                { "ConnectionStrings:DatabaseName", "IntegrationTests" } };

            return new ConfigurationBuilder().AddInMemoryCollection(inMemoryConfiguration).Build();
        }
        public static IWebHostEnvironment GetEnvMoq()
        {
            var mockEnvironment = new Mock<IWebHostEnvironment>();
            mockEnvironment
                .Setup(m => m.WebRootPath)
                .Returns(Directory.GetCurrentDirectory());
            return mockEnvironment.Object;
        }


    }
}
