using System;
using System.Linq;
using NUnit.Framework;
using XCV.Data;


namespace Tests.Integration
{
    //----------------------Integration Tests for    IAccountService
    [TestFixture]
    public class IAccountService_Test : Initializer
    {
        private EmployeeService sut;  //=System under Test

        [OneTimeSetUp]
        public void GetSut()
        {
            sut = GetEmployeeService();
        }




        //aus Kontrakt:   CreateAccount(newAccount)
        //
        // Vorbedingungen: .PersoNumber==(noch frei)   .PersoNumber==(2-6 Zeichen   [a-zA-Z0-9_\-,.]*)
        //                   .FirstName==(1-20 Zeichen)   .LastName==(1-20 Zeichen)
        //
        //Nachbedingungen: .Password,  .FirstName, .LastName,  .AcRoles
        //                  wurde alles  1-zu-1 gespeichert
        //                  und kann über die e.PersoNumber auch wieder abgerufen werden
        //
        [TestCase("0A-1", "Test", "Tester", ExpectedResult = true)]
        [TestCase("0 1", "Test", "Tester", ExpectedResult = false)]
        [TestCase("01", "aaaaaaaaaaaaaaaaaaaaaaaaaaaaa", "Tester", ExpectedResult = true)]
        [TestCase("01", "Test", "", ExpectedResult = false)]
        [TestCase("0''1", "Test", "Tester", ExpectedResult = false)]
        [Order(1)]
        public bool CreateAccount(string nr, string fName, string lName)
        {
            var newOne = new Employee() { PersoNumber = nr, FirstName = fName, LastName = lName, EmployedSince = DateTime.Now };
            newOne.AcRoles.Add(AccessRole.Admin);
            newOne.AcRoles.Add(AccessRole.Sales);

            // Act
            sut.CreateAccount(newOne);

            // Assert
            var requestAgain = sut.ShowProfile(nr);

            if (requestAgain == null)
            {
                Console.WriteLine("nothing created");
                return false;
            }
            if (requestAgain.PersoNumber != nr || requestAgain.Password != nr)
            {
                Console.WriteLine("PersoNumber/Password created incorrectly");
                return false;
            }
            if (requestAgain.FirstName != fName || requestAgain.LastName != lName)
            {
                Console.WriteLine("Name created incorrectly");
                return false;
            }
            Assert.Contains(AccessRole.Admin, requestAgain.AcRoles.ToList<AccessRole>(), "Roles created incorrectly ");
            Assert.Contains(AccessRole.Sales, requestAgain.AcRoles.ToList<AccessRole>(), "Roles created incorrectly ");
            return true;
        }



        //aus Kontrakt:   DeleteAccount(IdToDelete)
        //
        //   Vorbedingungen: PersoNumber-ToDelete==(existiert in Persistenz)
        //
        //   Nachbedingungen: Persistenz liefer keine Ergebnisse mehr, für Employyes mit dieser PersoNumber,
        //                    auch nicht als Referenz in anderen Bereichen
        //
        [TestCase("0A-1")]
        [Order(2)]
        public void DeleteAccount(string persoNumber)
        {
            // PreCondition
            Assert.NotNull(sut.ShowProfile(persoNumber), "should have been created by the previous Test");


            // Act
            sut.DeleteAccount(persoNumber);

            // Assert
            var requestAgain = sut.ShowProfile(persoNumber);
            Assert.Null(requestAgain, "failed to delete");
        }

    }
}
