using System.Linq;
using NUnit.Framework;
using XCV.Data;


namespace Tests.Integration
{
    //----------------------Integration Tests for    IRoleService
    [TestFixture]
    public class IRoleService_Test : Initializer
    {
        private IRoleService sut;

        [OneTimeSetUp]
        public void GetSut()
        {
            sut = GetRoleService();
        }


        //--------------------------UpdateAllRoles(roles)--------------------------
        //
        // Vorbeding.: .Name.Length <50 && <2 && unique
        // Nachbedin.:  alle Rollen die nicht mehr im EingabeParameter(roles) enthalten sind,
        ///             wurden mit ihren refs. gelöscht  alle neuen wurden in Persistenz geschrieben
        [Test]
        public void UpdateAll()
        {
            var r1 = new Role() { Name = "Coder", RCL = 1, Wage = 0.10F };
            var r2 = new Role() { Name = "Coder", RCL = 2, Wage = 100 };
            var r3 = new Role() { Name = "Coder", RCL = 3, Wage = 500 };
            var newRoles = (new[] { r1, r2, r3 }).ToHashSet(new RoleComparerAnything()); //input for exchange

            var oldCount = sut.GetAllRoles().Count();                   //old count

            // Act
            (var addedRows, _, var remoRows) = sut.UpdateAllRoles(newRoles);

            // Assert
            var requestAgain = sut.GetAllRoles();

            Assert.AreEqual(3, addedRows);
            Assert.AreEqual(oldCount, remoRows, "old Fields weren't removed");
            Assert.True(newRoles.SetEquals(requestAgain), "Values weren't correctlly stored");
        }
        [Test]
        public void UpdateWages()
        {
            var r1 = new Role() { Name = "Coder", RCL = 1, Wage = 1 };
            var r2 = new Role() { Name = "Coder", RCL = 2, Wage = 10 };
            var r3 = new Role() { Name = "Coder", RCL = 3, Wage = 100 };
            var rolesToUpdate = (new[] { r1, r2, r3 }).ToHashSet(new RoleComparerAnything()); //input for exchange


            // Act
            (int addedRows, int chang, int remoRows) = sut.UpdateAllRoles(rolesToUpdate);

            // Assert
            var requestAgain = sut.GetAllRoles();
            Assert.AreEqual(0, addedRows);
            Assert.AreEqual(3, chang);
            Assert.AreEqual(0, remoRows);
            Assert.True(rolesToUpdate.SetEquals(requestAgain), "Values weren't correctlly stored");
        }
        //-----------------------------------------------------------------------------------------
        //     bei invaliden EingabeParametern : nichts in der Persistenz ändert sich
        [TestCase("")]
        [TestCase("123456789-123456789-123456789-123456789-123456789-abc")]
        public void Invalid_UpdateAll(string name)
        {
            var roles = new[] { new Role() { Name = name } };   //input

            var oldRoles = sut.GetAllRoles().ToHashSet();       //old Value


            // Act
            (int addedRows, int chang, int remoRows) = sut.UpdateAllRoles(roles);

            // Assert
            var requestAgain = sut.GetAllRoles();
            Assert.AreEqual(0, addedRows);
            Assert.AreEqual(0, chang);
            Assert.AreEqual(0, remoRows);
            Assert.True(oldRoles.SetEquals(requestAgain), "old Roles did change");
        }

    }
}

