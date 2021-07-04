using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using XCV.Data;


namespace Tests.Integration
{
    //----------------------Integration Tests for    IFieldService
    [TestFixture]
    public class IFieldService_Test : Initializer
    {
        private IFieldService sut; //=System under Test
        private ChangeResult changeRes;
        private void OnEventReturn(object sender, ChangeResult e) => changeRes = e;
        [SetUp]
        public void ResetResult() => changeRes = new();

        [OneTimeSetUp]
        public void GetSut()
        {
            sut = GetFieldService();
            sut.ChangeEventHandel += OnEventReturn;
        }



        //--------------------------UpdateAllFields(fields)--------------------------
        //
        // Vorbedingungen:  keine einziger (.Name.Length>50)
        // Nachbedingungen: die Brachen die in der Persistenz waren, aber nicht in fields wurden gelöscht.
        //                   alle Brachen die in fields waren wurden inserted wenn nötig und sind nun abrufbar
        [TestCase(10)]
        [TestCase(50)]  // 50 random strings of length==36
        public void UpdateAll(int size)
        {
            HashSet<Field> newFields = new();                   //input
            for (int i = 0; i < size; i++)
                newFields.Add(new Field() { Name = Guid.NewGuid().ToString() });

            var oldFieldsCount = sut.GetAllFields().Count();    //old Value


            // Act
            (var addedRows, var remoRows) = sut.UpdateAllFields(newFields);

            // Assert
            var requestAgain = sut.GetAllFields();

            Assert.AreEqual(newFields.Count, addedRows);
            Assert.AreEqual(oldFieldsCount, remoRows, "not all old Fields were removed");
            Assert.True(newFields.SetEquals(requestAgain), "new Fields weren't stored correctly");
        }
        //-----------------------------------------------------------------------------------------
        //     bei invaliden EingabeParametern : nichts in der Persistenz ändert sich
        [TestCase("")]
        [TestCase("123456789-123456789-123456789-123456789-123456789-field")]
        public void Invalid_UpdateAll(string name)
        {
            var field = new Field() { Name = name };            //input

            var oldFields = sut.GetAllFields().ToHashSet();     //old Value


            // Act
            (var addedRows, var remoRows) = sut.UpdateAllFields(new[] { field });

            // Assert
            var requestAgain = sut.GetAllFields();

            Assert.AreEqual(0, addedRows);
            Assert.AreEqual(0, remoRows);
            Assert.True(oldFields.SetEquals(requestAgain), "old Fields did change");
        }


        //---------------------------CreateField(field)--------------------------
        //
        // Vorbedingungen:  .Name existiert noch nicht Persistenz && Name.Length < 50
        // Nachbedingungen: neue Brache gespeichert und abrufbar
        //                    die anderen Brache(und ihren Referenzen) wurden nicht verändert
        [TestCase("Brache 1")]
        [TestCase("Brache 2")]
        [TestCase("Brache 3")]
        [Order(1)]
        public void CreateField(string name)
        {
            var field = new Field() { Name = name };                //input

            var combindFields = sut.GetAllFields().Append(field).ToHashSet();   //old Value


            // Act
            sut.CreateField(field);

            // Assert
            var requestAgain = sut.GetAllFields();

            Assert.AreEqual(combindFields.Count, requestAgain.Count());
            Assert.True(combindFields.SetEquals(requestAgain), "invalid change");
            Assert.AreNotEqual("", changeRes.SuccesMessage, "User didn't got a SuccesMessage");
        }
        //-----------------------------------------------------------------------------------------
        //   bei invaliden EingabeParametern : nichts in der Persistenz ändert sich
        [TestCase("")]
        [TestCase("123456789-123456789-123456789-123456789-123456789-field")]
        [TestCase("Brache 1")]
        [Order(2)]
        public void Invalid_CreateField(string name)
        {
            var field = new Field() { Name = name };                //input

            var oldFields = sut.GetAllFields().ToHashSet();         //old Value


            // Act
            sut.CreateField(field);

            // Assert
            var requestAgain = sut.GetAllFields();

            Assert.True(oldFields.SetEquals(requestAgain), "old Fields where changed");
            Assert.AreEqual(1, changeRes.ErrorMessages.Count(), "User didn't got an ErrorMessages");
        }

    }
}

