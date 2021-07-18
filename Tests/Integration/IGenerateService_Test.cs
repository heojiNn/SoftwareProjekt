using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using XCV.Data;

namespace Tests.Integration
{
    //----------------------Integration Tests for    IGenerateService
    [TestFixture]
    class IGenerateService_Test : Initializer
    {
   
        private GenerateService sut;

        [OneTimeSetUp]
        public void GetSut()
        {
            sut = GetGenerateService();
        }








    }
}
