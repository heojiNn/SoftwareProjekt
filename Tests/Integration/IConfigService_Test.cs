using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using XCV.Data;

namespace Tests.Integration
{
    //---------------------- Integration Tests for the   IConfigService
    [TestFixture]
    class IConfigService_Test : Initializer
    {

        private IConfigService sut;
        private IOfferService offerService;
        private ChangeResult changeRes;
        private void OnEventReturn(object sender, ChangeResult e) => changeRes = e;

        [OneTimeSetUp]
        public void GetSut()
        {
            sut = GetConfigService();
            sut.ChangeEventHandel += OnEventReturn;
            offerService = GetOfferService();
        }

        //Vorbedingung:
        //Es liegt keine Konfiguration mit diesem Namen vor
        //es wurde ein Namen mit passendem Format(Länge 30) eingegeben
        //Das übergebene Angebot enthält Mitarbeiterprofile

        //Nachbedingung:
        //Eine neue Dokumentenkonfiguration wurde in der Datenbasis angelegt und wird mit einem Angebot assoziiert
        [Test]
        [Order(1)]
        public void CreateTest()
        {

            Offer o = offerService.ShowAllOffers().FirstOrDefault();
            // PreCondition
            var noVal = sut.GetDocumentConfig(o, "TestTitle");
            Assert.Null(noVal);

            //Assert
            IList<EmployeeConfig> ecfgSamples = new List<EmployeeConfig>();
            foreach (Employee e in o.participants)
            {
                ecfgSamples.Add(
                    new EmployeeConfig()
                    {
                        PersNr = e.PersoNumber,
                        FirstName = e.FirstName ?? null,
                        LastName = e.LastName ?? null,
                        Description = e.Description ?? null,
                        Image = e.Image ?? null,
                        Experience = e.Experience ?? null,
                        EmployedSince = e.EmployedSince,
                        selectedFields = e.Fields,
                        selectedSoftSkills = e.Abilities.Where(s => s.Type == SkillGroup.Softskill).ToHashSet(),
                        selectedHardSkills = e.Abilities.Where(s => s.Type == SkillGroup.Hardskill).ToHashSet(),
                        selectedProjects = e.Projects,
                        order = new int[] { 1, 2, 3, 4, 5 }
                    });
            }
            DocumentConfig newCfg = new DocumentConfig()
            {
                Name = "TestTitle",
                employeeConfigs = ecfgSamples
            };

            sut.CreateDefaultDocumentConfig(o, newCfg.Name);
            var requestAgain = sut.GetAllDocumentConfigs(o).FirstOrDefault(x => x.Name == "TestTitle");
            Assert.NotNull(requestAgain);
            Assert.AreEqual(newCfg.Name, requestAgain.Name, "Name wasn't created");
            Assert.AreEqual(newCfg.employeeConfigs.Count, requestAgain.employeeConfigs.Count, "Amount of EmployeeConfigs is incorrect");
            foreach (EmployeeConfig ecfg in newCfg.employeeConfigs)
            {
                foreach (EmployeeConfig ecfgToTEst in requestAgain.employeeConfigs)
                {
                    Assert.AreEqual(ecfg.PersNr, ecfgToTEst.PersNr, "PersNr wasn't created");
                    Assert.AreEqual(ecfg.FirstName, ecfgToTEst.FirstName, "FirstName wasn't created");
                    Assert.AreEqual(ecfg.LastName, ecfgToTEst.LastName, "LastName wasn't created");
                    Assert.AreEqual(ecfg.Description, ecfgToTEst.Description, "Description wasn't created");
                    Assert.AreEqual(ecfg.Image, ecfgToTEst.Image, "Image wasn't created");
                    Assert.AreEqual(ecfg.Experience, ecfgToTEst.Experience, "Experience wasn't created");
                    Assert.AreEqual(ecfg.EmployedSince, ecfgToTEst.EmployedSince, "EmployedSince wasn't created");
                    Assert.AreEqual(ecfg.selectedFields, ecfgToTEst.selectedFields, "selectedFields wasn't created");
                    Assert.AreEqual(ecfg.selectedSoftSkills, ecfgToTEst.selectedSoftSkills, "selectedSoftSkills wasn't created");
                    Assert.AreEqual(ecfg.selectedHardSkills, ecfgToTEst.selectedHardSkills, "selectedHardSkills wasn't created");
                    Assert.AreEqual(ecfg.selectedProjects, ecfgToTEst.selectedProjects, "selectedProjects wasn't created");
                    Assert.AreEqual(ecfg.order, ecfgToTEst.order, "order wasn't created");
                }
            }
            Assert.False(changeRes.ErrorMessages.Any());
        }








    }
}
